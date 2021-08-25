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

    string TAG { get { return this.GetType().Name; } }

    [SerializeField] TextAsset cpuConfig = null;
    [SerializeField] TextAsset gpuConfig = null;
    [SerializeField] TextAsset openGlEsConfig = null;

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
      return packet.Get();
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

    protected static bool TryGetGraphRunner(IntPtr graphPtr, out GraphRunner graphRunner) {
      var isInstanceIdFound = nameTable.TryGetValue(graphPtr, out var instanceId);

      if (isInstanceIdFound) {
        return instanceTable.TryGetValue(instanceId, out graphRunner);
      }
      graphRunner = null;
      return false;
    }

    protected Timestamp GetCurrentTimestamp() {
      if (stopwatch == null || !stopwatch.IsRunning) {
        return Timestamp.Unset();
      }
      var microseconds = (stopwatch.ElapsedTicks) / (TimeSpan.TicksPerMillisecond / 1000);
      return new Timestamp(microseconds);
    }

    protected virtual Status InitializeCalculatorGraph() {
      calculatorGraph = new CalculatorGraph(config.text);

      if (inferenceMode == InferenceMode.CPU) {
        return Status.Ok();
      }

      return calculatorGraph.SetGpuResources(GpuManager.gpuResources);
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
