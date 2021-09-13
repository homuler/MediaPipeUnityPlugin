using System;
using System.Collections.Generic;
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
      Logger.LogInfo(TAG, $"Loading dependent assets...");
      PrepareDependentAssets();
      instanceTable.Add(GetInstanceID(), this);
    }

    protected virtual void OnDestroy() {
      Stop();
    }

    public virtual Status Initialize() {
      configType = DetectConfigType();
      Logger.LogInfo(TAG, $"Using {configType} config");

      if (configType == ConfigType.None) {
        return Status.FailedPrecondition("Failed to detect config. Check if config is set to GraphRunner");
      }

      var status = InitializeCalculatorGraph();
      nameTable.Add(calculatorGraph.mpPtr, GetInstanceID());
      stopwatch = new Stopwatch();
      stopwatch.Start();

      return status;
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
    ///   If the next value is empty, this method never returns.
    /// </remarks>
    public T FetchNext<T>(OutputStreamPoller<T> poller, Packet<T> packet, string streamName = null, T failedValue = default(T)) {
      if (!poller.Next(packet)) { // blocks
        if (streamName != null) {
          Logger.LogWarning(TAG, $"Failed to fetch next packet from {streamName}");
        }
        return failedValue;
      }
      return packet.IsEmpty() ? failedValue : packet.Get();
    }

    /// <summary>
    ///   Fetch next vector value from <paramref name="poller" />.
    /// </summary>
    /// <remarks>
    ///   If the next value is empty, this method never returns.
    /// </remarks>
    /// <returns>
    ///   Fetched vector or an empty List when failed.
    /// </returns>
    /// <seealso cref="FetchNext" />
    public List<T> FetchNextVector<T>(OutputStreamPoller<List<T>> poller, Packet<List<T>> packet, string streamName = null) {
      var nextValue = FetchNext<List<T>>(poller, packet, streamName);
      return nextValue == null ? new List<T>() : nextValue;
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

    protected abstract void PrepareDependentAssets();
  }
}
