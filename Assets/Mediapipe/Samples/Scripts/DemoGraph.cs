using Mediapipe;
using System;
using System.Collections.Generic;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

public abstract class DemoGraph : MonoBehaviour, IDemoGraph<TextureFrame> {
  [SerializeField] protected TextAsset gpuConfig = null;
  [SerializeField] protected TextAsset cpuConfig = null;
  [SerializeField] protected TextAsset androidConfig = null;

  GameObject resourceManager;
  protected const string inputStream = "input_video";
  protected Stopwatch stopwatch;
  protected static CalculatorGraph graph;
  protected static GlCalculatorHelper gpuHelper;
  protected static Timestamp currentTimestamp;

#if UNITY_ANDROID
  static readonly object frameLock = new object();
  static TextureFrame currentTextureFrame;
  static IntPtr currentTextureName;
#endif

  void OnEnable() {
    resourceManager = GameObject.Find("ResourceManager");
  }

  protected virtual void OnDestroy() {
    Stop();

    if (graph != null) {
      graph.Dispose();
      graph = null;
    }

    gpuHelper = null;

    if (stopwatch != null && stopwatch.IsRunning) {
      stopwatch.Stop();
    }
  }

  public virtual void Initialize() {
    PrepareDependentAssets();
    Debug.Log("Loaded dependent assets");

    var config = GetConfig();

    if (config == null) {
      Debug.LogError("config is missing");
      return;
    }

    graph = new CalculatorGraph(config.text);
    stopwatch = new Stopwatch();
  }

  public void Initialize(GpuResources gpuResources, GlCalculatorHelper gpuHelper) {
    DemoGraph.gpuHelper = gpuHelper;

    this.Initialize();
    graph?.SetGpuResources(gpuResources).AssertOk();
  }

  public abstract Status StartRun();
  public virtual Status StartRun(Texture texture) {
    stopwatch.Start();
    return StartRun();
  }

  public virtual Status PushInput(TextureFrame textureFrame) {
    currentTimestamp = GetCurrentTimestamp();

#if UNITY_ANDROID && !UNITY_EDITOR
    if (IsGpuEnabled()) {
      lock (frameLock) {
        currentTextureFrame = textureFrame;
        currentTextureName = textureFrame.GetNativeTexturePtr();

        return gpuHelper.RunInGlContext(PushInputInGlContext);
      }
    }
#endif

    var imageFrame = new ImageFrame(
      ImageFormat.Format.SRGBA, textureFrame.width, textureFrame.height, 4 * textureFrame.width, textureFrame.GetRawNativeByteArray());
    textureFrame.Release();
    var packet = new ImageFramePacket(imageFrame, currentTimestamp);

    return graph.AddPacketToInputStream(inputStream, packet);
  }

#if UNITY_ANDROID
  /// <remarks>
  ///    <see cref="currentTimestamp" />, <see cref="currentTextureFrame" /> and <see cref="currentTextureName" /> must be set before calling.
  /// </remarks>
  [AOT.MonoPInvokeCallback(typeof(GlCalculatorHelper.NativeGlStatusFunction))]
  static IntPtr PushInputInGlContext() {
    try {
      var glContext = GlContext.GetCurrent();
      var glTextureBuffer = new GlTextureBuffer((UInt32)currentTextureName, currentTextureFrame.width, currentTextureFrame.height,
                                                currentTextureFrame.gpuBufferformat, currentTextureFrame.OnRelease, glContext);
      var gpuBuffer = new GpuBuffer(glTextureBuffer);

      // TODO: ensure the returned status won't be garbage collected prematurely.
      return graph.AddPacketToInputStream(inputStream, new GpuBufferPacket(gpuBuffer, currentTimestamp)).mpPtr;
    } catch (Exception e) {
      return Status.FailedPrecondition(e.ToString()).mpPtr;
    }
  }
#endif

  public abstract void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame);

  public void Stop() {
    if (graph == null) { return; }

    using (var status = graph.CloseAllPacketSources()) {
      if (!status.ok) {
        Debug.LogError(status.ToString());
      }
    }

    using (var status = graph.WaitUntilDone()) {
      if (!status.ok) {
        Debug.LogError(status.ToString());
      }
    }
  }

  /// <summary>
  ///   Fetch next value from <paramref name="poller" />.
  ///   Note that this method blocks the thread till the next value is fetched.
  ///   If the next value is empty, this method never returns.
  /// </summary>
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
  /// <returns>
  ///   Fetched vector or an empty List when failed.
  /// </returns>
  /// <seealso cref="FetchNext" />
  public List<T> FetchNextVector<T>(OutputStreamPoller<List<T>> poller, Packet<List<T>> packet, string streamName = null) {
    var nextValue = FetchNext<List<T>>(poller, packet, streamName);

    return nextValue == null ? new List<T>() : nextValue;
  }

  protected bool IsGpuEnabled() {
    return gpuHelper != null;
  }

  protected TextAsset GetConfig() {
    if (!IsGpuEnabled()) {
      return cpuConfig;
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    if (androidConfig != null) {
      return androidConfig;
    }
#endif
    return gpuConfig;
  }

  protected Timestamp GetCurrentTimestamp() {
    if (stopwatch == null || !stopwatch.IsRunning) {
      return Timestamp.Unset();
    }

    var microseconds = (stopwatch.ElapsedTicks) / (TimeSpan.TicksPerMillisecond / 1000);
    return new Timestamp(microseconds);
  }

  protected virtual void PrepareDependentAssets() {}

  protected void PrepareDependentAsset(string assetName, string uniqueKey, bool overwrite = false) {
    resourceManager.GetComponent<AssetLoader>().PrepareAsset(assetName, uniqueKey, overwrite);
  }

  protected void PrepareDependentAsset(string assetName, bool overwrite = false) {
    PrepareDependentAsset(assetName, assetName, overwrite);
  }
}
