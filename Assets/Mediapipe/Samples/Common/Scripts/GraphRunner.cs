using System;
using System.Collections.Generic;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity {
  public abstract class GraphRunner : MonoBehaviour {
    public enum ConfigType {
      CPU,
      GPU,
      OpenGLES,
    }

    [SerializeField] TextAsset cpuConfig = null;
    [SerializeField] TextAsset gpuConfig = null;
    [SerializeField] TextAsset openGlEsConfig = null;

    static readonly InstanceCacheTable<IntPtr, GraphRunner> instanceCacheTable = new InstanceCacheTable<IntPtr, GraphRunner>(10);

    public InferenceMode inferenceMode { get; private set; }
    public ConfigType configType { get; private set; }
    public TextAsset config {
      get {
        switch (configType) {
          case ConfigType.CPU: return cpuConfig;
          case ConfigType.OpenGLES: return openGlEsConfig;
          default: return gpuConfig;
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
      inferenceMode = GpuManager.isInitialized ? InferenceMode.GPU : InferenceMode.CPU;
      Debug.Log($"Inference mode: {inferenceMode}");

      if (inferenceMode == InferenceMode.CPU) {
        configType = ConfigType.CPU;
#if UNITY_ANDROID
      } else if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3) {
        configType = ConfigType.OpenGLES;
#endif
      } else {
        configType = ConfigType.GPU;
      }
      Debug.Log($"Config type: {configType}");

      Debug.Log($"Loading dependent assets...");
      PrepareDependentAssets();
    }

    protected virtual void OnDestroy() {
      Stop();

      if (calculatorGraph != null) {
        calculatorGraph.Dispose();
        calculatorGraph = null;
      }

      if (stopwatch != null && stopwatch.IsRunning) {
        stopwatch.Stop();
      }
    }

    public virtual Status Initialize() {
      var status = InitializeCalculatorGraph();
      instanceCacheTable.Add(calculatorGraph.mpPtr, this);
      stopwatch = new Stopwatch();
      stopwatch.Start();

      return status;
    }

    public abstract Status StartRun();

    public virtual void Stop() {
      if (calculatorGraph == null) { return; }

      using (var status = calculatorGraph.CloseAllPacketSources()) {
        if (!status.ok) {
          Debug.LogError(status.ToString());
        }
      }

      using (var status = calculatorGraph.WaitUntilDone()) {
        if (!status.ok) {
          Debug.LogError(status.ToString());
        }
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
          Debug.LogWarning($"Failed to fetch next packet from {streamName}");
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
      return instanceCacheTable.TryGetValue(graphPtr, out graphRunner);
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

    protected abstract void PrepareDependentAssets();
  }
}
