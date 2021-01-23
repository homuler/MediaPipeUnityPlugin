using Mediapipe;
using System;
using System.Collections.Generic;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

public abstract class DemoGraph : MonoBehaviour, IDemoGraph<TextureFrame> {
  [SerializeField] protected TextAsset config = null;

  protected const string inputStream = "input_video";
  protected Stopwatch stopwatch;
  protected static CalculatorGraph graph;
  protected static GlCalculatorHelper gpuHelper;

#if UNITY_ANDROID
  static readonly object frameLock = new object();
  static Timestamp currentTimestamp;
  static TextureFrame currentTextureFrame;
  static IntPtr currentTextureName;
#endif

  protected virtual void OnDestroy() {
    Stop();

    if (stopwatch != null && stopwatch.IsRunning) {
      stopwatch.Stop();
    }
  }

  public virtual void Initialize() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    graph = new CalculatorGraph(config.text);
    stopwatch = new Stopwatch();
  }

  public void Initialize(GpuResources gpuResources, GlCalculatorHelper gpuHelper) {
    this.Initialize();

    graph.SetGpuResources(gpuResources).AssertOk();
    DemoGraph.gpuHelper = gpuHelper;
  }

  public abstract Status StartRun();
  public virtual Status StartRun(Texture texture) {
    stopwatch.Start();
    return StartRun();
  }

  public Status PushInput(TextureFrame textureFrame) {
    var timestamp = GetCurrentTimestamp();

#if !UNITY_ANDROID
    var imageFrame = new ImageFrame(
      ImageFormat.Format.SRGBA, textureFrame.width, textureFrame.height, 4 * textureFrame.width, textureFrame.GetRawNativeByteArray());
    textureFrame.Release();
    var packet = new ImageFramePacket(imageFrame, timestamp);

    return graph.AddPacketToInputStream(inputStream, packet);
#else
    lock (frameLock) {
      currentTimestamp = timestamp;
      currentTextureFrame = textureFrame;
      currentTextureName = textureFrame.GetNativeTexturePtr();

      return gpuHelper.RunInGlContext(PushInputInGlContext);
    }
#endif
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

  Timestamp GetCurrentTimestamp() {
    if (stopwatch == null || !stopwatch.IsRunning) {
      return Timestamp.Unset();
    }

    var microseconds = (stopwatch.ElapsedTicks) / (TimeSpan.TicksPerMillisecond / 1000);
    return new Timestamp(microseconds);
  }
}
