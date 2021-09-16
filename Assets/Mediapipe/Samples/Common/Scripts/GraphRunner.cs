using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity {
  public abstract class GraphRunner : MonoBehaviour {
    public enum ConfigType {
      None,
      CPU,
      GPU,
      OpenGLES,
    }

    protected string TAG { get { return this.GetType().Name; } }

    [SerializeField] TextAsset cpuConfig = null;
    [SerializeField] TextAsset gpuConfig = null;
    [SerializeField] TextAsset openGlEsConfig = null;
    [SerializeField] long _timeoutMicrosec = 0;

    static readonly GlobalInstanceTable<int, GraphRunner> instanceTable = new GlobalInstanceTable<int, GraphRunner>(5);
    static readonly Dictionary<IntPtr, int> nameTable = new Dictionary<IntPtr, int>();

    public InferenceMode inferenceMode {
      get { return configType == ConfigType.CPU ? InferenceMode.CPU : InferenceMode.GPU; }
    }
    public ConfigType configType { get; private set; }
    public TextAsset config {
      get {
        switch (configType) {
          case ConfigType.CPU: return cpuConfig;
          case ConfigType.GPU: return gpuConfig;
          case ConfigType.OpenGLES: return openGlEsConfig;
          default: return null;
        }
      }
    }

    public long timeoutMicrosec {
      get { return _timeoutMicrosec; }
      private set { _timeoutMicrosec = value; }
    }
    public long timeoutMillisec { get { return timeoutMicrosec / 1000; } }

    Stopwatch stopwatch;
    protected CalculatorGraph calculatorGraph { get; private set; }
    protected Timestamp currentTimestamp;

#if UNITY_ANDROID
    // For OpenGL ES
    readonly object frameLock = new object();
    protected TextureFrame currentTextureFrame;
#endif

    protected virtual void Start() {
      instanceTable.Add(GetInstanceID(), this);
    }

    protected virtual void OnDestroy() {
      Stop();
    }

    public WaitForResult WaitForInit() {
      return new WaitForResult(this, Initialize());
    }

    public virtual IEnumerator Initialize() {
      configType = DetectConfigType();
      Logger.LogInfo(TAG, $"Using {configType} config");

      if (configType == ConfigType.None) {
        throw new InvalidOperationException("Failed to detect config. Check if config is set to GraphRunner");
      }

      InitializeCalculatorGraph().AssertOk();
      stopwatch = new Stopwatch();
      stopwatch.Start();

      Logger.LogInfo(TAG, "Loading dependent assets...");
      var assetRequests = RequestDependentAssets();
      yield return new WaitWhile(() => assetRequests.Any((request) => request.keepWaiting));

      var errors = assetRequests.Where((request) => request.isError).Select((request) => request.error).ToList();
      if (errors.Count > 0) {
        foreach (var error in errors) {
          Logger.LogError(TAG, error);
        }
        throw new InternalException("Failed to prepare dependent assets");
      }
    }

    public abstract Status StartRun(ImageSource imageSource);

    public virtual void Stop() {
      if (calculatorGraph == null) { return; }

      // TODO: not to call CloseAllPacketSources if calculatorGraph has not started.
      using (var status = calculatorGraph.CloseAllPacketSources()) {
        if (!status.ok) {
          Logger.LogError(TAG, status.ToString());
        }
      }

      using (var status = calculatorGraph.WaitUntilDone()) {
        if (!status.ok) {
          Logger.LogError(TAG, status.ToString());
        }
      }

      nameTable.Remove(calculatorGraph.mpPtr);
      calculatorGraph.Dispose();
      calculatorGraph = null;

      if (stopwatch != null && stopwatch.IsRunning) {
        stopwatch.Stop();
      }
    }

    public Status AddPacketToInputStream<T>(string streamName, Packet<T> packet) {
      return calculatorGraph.AddPacketToInputStream(streamName, packet);
    }

    public Status AddTextureFrameToInputStream(string streamName, TextureFrame textureFrame) {
      currentTimestamp = GetCurrentTimestamp();

      if (configType == ConfigType.OpenGLES) {
        var gpuBuffer = textureFrame.BuildGpuBuffer(GpuManager.glCalculatorHelper.GetGlContext());
        return calculatorGraph.AddPacketToInputStream(streamName, new GpuBufferPacket(gpuBuffer, currentTimestamp));
      }

      var imageFrame = textureFrame.BuildImageFrame();
      textureFrame.Release();

      return AddPacketToInputStream(streamName, new ImageFramePacket(imageFrame, currentTimestamp));
    }

    /// <summary>
    ///   Fetch next value from <paramref name="poller" />.
    ///   Note that this method blocks the thread till the next value is fetched.
    /// </summary>
    /// <remarks>
    ///   If there's no next value, this method never returns.
    /// </remarks>
    public bool FetchNext<T>(OutputStreamPoller<T> poller, Packet<T> packet, out T value, string streamName = null) {
      if (poller.Next(packet)) {
        if (!packet.IsEmpty()) {
          value = packet.Get();
          return true;
        }
      } else if (streamName != null) {
        Logger.LogWarning(TAG, $"Failed to fetch next packet from {streamName}");
      }
      // failed or packet is empty
      value = default(T);
      return false;
    }

    public IEnumerator FetchNext<T>(OutputStreamPoller<T> poller, Packet<T> packet, string streamName = null) {
      while (true) {
        if (FetchNext(poller, packet, out var value, streamName)) {
          yield return value;
          break;
        }
        if (packet.Timestamp().Microseconds() >= currentTimestamp.Microseconds()) {
          // The latest input packet has already been processed
          yield return default(T);
          break;
        }
        yield return new WaitForEndOfFrame();
      }
    }

    public WaitForResult<T> WaitForNext<T>(OutputStreamPoller<T> poller, Packet<T> packet, string streamName = null) {
      return new WaitForResult<T>(this, FetchNext(poller, packet, streamName), timeoutMicrosec);
    }

    public bool FetchLatest<T>(OutputStreamPoller<T> poller, Packet<T> packet, out T value, string streamName = null) {
      while (true) {
        if (FetchNext(poller, packet, out value, streamName)) {
          Logger.Log($"Fetched next value, {value}");
          return true;
        }
        Logger.LogDebug("Failed to fetch next");
        if (packet.Timestamp().Microseconds() >= currentTimestamp.Microseconds()) {
          // The latest input packet has already been processed
          Logger.LogDebug($"Latest packet: {packet.Timestamp().Microseconds()}, {currentTimestamp.Microseconds()}");
          value = default(T);
          return false;
        }
        Logger.LogDebug("Retry");
      }
    }

    public void SetTimeoutMicrosec(long timeoutMicrosec) {
      this.timeoutMicrosec = (long)Mathf.Max(0, timeoutMicrosec);
    }

    public void SetTimeoutMillisec(long timeoutMillisec) {
      SetTimeoutMicrosec(1000 * timeoutMillisec);
    }

    protected static bool TryGetGraphRunner(IntPtr graphPtr, out GraphRunner graphRunner) {
      var isInstanceIdFound = nameTable.TryGetValue(graphPtr, out var instanceId);

      if (isInstanceIdFound) {
        return instanceTable.TryGetValue(instanceId, out graphRunner);
      }
      graphRunner = null;
      return false;
    }

    protected static Status InvokeIfGraphRunnerFound<T>(IntPtr graphPtr, IntPtr packetPtr, Action<T, IntPtr> action) where T : GraphRunner {
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found");
        }
        var graph = (T)graphRunner;
        action(graph, packetPtr);
        return Status.Ok();
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString());
      }
    }

    protected static Status InvokeIfGraphRunnerFound<T>(IntPtr graphPtr, Action<T> action) where T : GraphRunner {
      return InvokeIfGraphRunnerFound<T>(graphPtr, IntPtr.Zero, (graph, ptr) => { action(graph); });
    }

    protected bool TryGetPacketValue<T>(Packet<T> packet, ref long prevMicrosec, out T value) where T : class {
      long currentMicrosec = 0;
      using (var timestamp = packet.Timestamp()) {
        currentMicrosec = timestamp.Microseconds();
      }

      if (!packet.IsEmpty()) {
        prevMicrosec = currentMicrosec;
        value = packet.Get();
        return true;
      }

      value = null;
      return currentMicrosec - prevMicrosec > timeoutMicrosec;
    }

    protected bool TryConsumePacketValue<T>(Packet<T> packet, ref long prevMicrosec, out T value) where T : class {
      long currentMicrosec = 0;
      using (var timestamp = packet.Timestamp()) {
        currentMicrosec = timestamp.Microseconds();
      }

      if (!packet.IsEmpty()) {
        prevMicrosec = currentMicrosec;
        var statusOrValue = packet.Consume();

        value = statusOrValue.ValueOr();
        return true;
      }

      value = null;
      return currentMicrosec - prevMicrosec > timeoutMicrosec;
    }

    protected Timestamp GetCurrentTimestamp() {
      if (stopwatch == null || !stopwatch.IsRunning) {
        return Timestamp.Unset();
      }
      var microseconds = (stopwatch.ElapsedTicks) / (TimeSpan.TicksPerMillisecond / 1000);
      return new Timestamp(microseconds);
    }

    protected Status InitializeCalculatorGraph() {
      calculatorGraph = new CalculatorGraph();
      nameTable.Add(calculatorGraph.mpPtr, GetInstanceID());

      // NOTE: There's a simpler way to initialize CalculatorGraph.
      //
      //     calculatorGraph = new CalculatorGraph(config.text);
      //
      //   However, if the config format is invalid, this code does not initialize CalculatorGraph and does not throw exceptions either.
      //   The problem is that if you call ObserveStreamOutput in this state, the program will crash.
      //   The following code is not very efficient, but it will return Non-OK status when an invalid configuration is given.
      try {
        var calculatorGraphConfig = GetCalculatorGraphConfig();
        var status = calculatorGraph.Initialize(calculatorGraphConfig);

        if (!status.ok || inferenceMode == InferenceMode.CPU) {
          return status;
        }

        return calculatorGraph.SetGpuResources(GpuManager.gpuResources);
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString());
      }
    }

    protected virtual CalculatorGraphConfig GetCalculatorGraphConfig() {
      return CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);
    }

    protected virtual ConfigType DetectConfigType() {
      if (GpuManager.isInitialized) {
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && openGlEsConfig != null) {
          return ConfigType.OpenGLES;
        }
        if (gpuConfig != null) {
          return ConfigType.GPU;
        }
      }
      if (cpuConfig != null) {
        return ConfigType.CPU;
      }
      return ConfigType.None;
    }

    protected WaitForResult WaitForAsset(string assetName, string uniqueKey, long timeoutMillisec, bool overwrite = false) {
      return new WaitForResult(this, AssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite), timeoutMillisec);
    }

    protected WaitForResult WaitForAsset(string assetName, long timeoutMillisec, bool overwrite = false) {
      return WaitForAsset(assetName, assetName, timeoutMillisec, overwrite);
    }

    protected WaitForResult WaitForAsset(string assetName, string uniqueKey, bool overwrite = false) {
      return new WaitForResult(this, AssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite));
    }

    protected WaitForResult WaitForAsset(string assetName, bool overwrite = false) {
      return WaitForAsset(assetName, assetName, overwrite);
    }

    protected abstract IList<WaitForResult> RequestDependentAssets();

    protected class OutputStream<TPacket, TValue> where TPacket : Packet<TValue>, new() {
      readonly CalculatorGraph calculatorGraph;

      readonly string streamName;
      OutputStreamPoller<TValue> poller;
      TPacket outputPacket;

      string presenceStreamName;
      OutputStreamPoller<bool> presencePoller;
      BoolPacket presencePacket;

      bool canFreeze { get { return presenceStreamName != null; } }

      public OutputStream(CalculatorGraph calculatorGraph, string streamName) {
        this.calculatorGraph = calculatorGraph;
        this.streamName = streamName;
      }

      public Status StartPolling(bool observeTimestampBounds = false) {
        this.outputPacket = new TPacket();

        var statusOrPoller = calculatorGraph.AddOutputStreamPoller<TValue>(streamName, observeTimestampBounds);
        var status = statusOrPoller.status;
        if (status.ok) {
          this.poller = statusOrPoller.Value();
        }
        return status;
      }

      public Status StartPolling(string presenceStreamName) {
        this.presenceStreamName = presenceStreamName;
        var status = this.StartPolling(false);

        if (status.ok) {
          this.presencePacket = new BoolPacket();

          var statusOrPresencePoller = calculatorGraph.AddOutputStreamPoller<bool>(presenceStreamName);
          status = statusOrPresencePoller.status;
          if (status.ok) {
            this.presencePoller = statusOrPresencePoller.Value();
          }
        }
        return status;
      }

      public Status AddListener(CalculatorGraph.NativePacketCallback callback, bool observeTimestampBounds = false) {
        return calculatorGraph.ObserveOutputStream(streamName, callback, observeTimestampBounds);
      }

      public bool TryGetNext(out TValue value) {
        if (HasNextValue()) {
          value = outputPacket.Get();
          return true;
        }
        value = default(TValue);
        return false;
      }

      public bool TryGetLatest(out TValue value) {
        if (HasNextValue()) {
          var queueSize = poller.QueueSize();

          // Assume that queue size will not be reduced from another thread.
          while (queueSize-- > 0) {
            if (!Next()) {
              value = default(TValue);
              return false;
            }
          }
          value = outputPacket.Get();
          return true;
        }
        value = default(TValue);
        return false;
      }

      bool HasNextValue() {
        if (canFreeze) {
          if (!NextPresence() || presencePacket.IsEmpty() || !presencePacket.Get()) {
            // NOTE: IsEmpty() should always return false
            return false;
          }
        }
        return Next() && !outputPacket.IsEmpty();
      }

      bool NextPresence() {
        return Next(presencePoller, presencePacket, presenceStreamName);
      }

      bool Next() {
        return Next(poller, outputPacket, streamName);
      }

      static bool Next<T>(OutputStreamPoller<T> poller, Packet<T> packet, string streamName) {
        if (!poller.Next(packet)) {
          Logger.LogWarning($"Failed to get next value from {streamName}, so there may be errors inside the calculatorGraph. See logs for more details");
          return false;
        }
        return true;
      }
    }
  }
}
