using Mediapipe;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DemoGraph : MonoBehaviour, IDemoGraph<PixelData> {
  [SerializeField] protected TextAsset config = null;

  protected const string inputStream = "input_video";
  protected CalculatorGraph graph;
  protected GlCalculatorHelper gpuHelper;

  void OnDestroy() {
    Stop();
  }

  public void Initialize() {
    if (config == null) {
      throw new InvalidOperationException("config is missing");
    }

    graph = new CalculatorGraph(config.text);
  }

  public void Initialize(GpuResources gpuResources, GlCalculatorHelper gpuHelper) {
    this.Initialize();

    graph.SetGpuResources(gpuResources).AssertOk();
    this.gpuHelper = gpuHelper;
  }

  public abstract Status StartRun();

  /// <summary>
  ///   Convert <paramref name="colors" /> to a packet and send it to the input stream.
  /// </summary>
  public Status PushInput(PixelData pixelData) {
    var timestamp = new Timestamp(System.Environment.TickCount & System.Int32.MaxValue);
    var imageFrame = ImageFrame.FromPixels32(pixelData.Colors, pixelData.Width, pixelData.Height, true);

    if (!IsGpuEnabled()) {
      var packet = new ImageFramePacket(imageFrame, timestamp);

      return graph.AddPacketToInputStream(inputStream, packet);
    }

    var status = gpuHelper.RunInGlContext(() => {
      var texture = gpuHelper.CreateSourceTexture(imageFrame);
      var gpuFrame = texture.GetGpuBufferFrame();

      Gl.Flush();
      texture.Release();

      return graph.AddPacketToInputStream(inputStream, new GpuBufferPacket(gpuFrame, timestamp));
    });

    imageFrame.Dispose();

    return status;
  }

  public abstract void RenderOutput(WebCamScreenController screenController, PixelData pixelData);

  public void Stop() {
    if (graph != null) {
      graph.CloseInputStream(inputStream).AssertOk();
      graph.WaitUntilDone().AssertOk();
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
}
