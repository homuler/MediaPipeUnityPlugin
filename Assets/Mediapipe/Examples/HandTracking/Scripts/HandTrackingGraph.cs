using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingGraph : DemoGraph {
  [SerializeField] private bool useGPU = true;

  private const string handednessStream = "handedness";
  private const string handPresenceStream = "hand_presence";
  private const string handRectStream = "hand_rect";
  private const string handLandmarksStream = "hand_landmarks";
  private const string palmDetectionsStream = "palm_detections";
  private const string palmDetectionsPresenceStream = "palm_detections_presence";
  private const string outputStream = "output_video";

  private OutputStreamPoller<ClassificationList> handednessStreamPoller;
  private OutputStreamPoller<bool> handPresenceStreamPoller;
  private OutputStreamPoller<NormalizedRect> handRectStreamPoller;
  private OutputStreamPoller<NormalizedLandmarkList> handLandmarksStreamPoller;
  private OutputStreamPoller<List<Detection>> palmDetectionsStreamPoller;
  private OutputStreamPoller<bool> palmDetectionsPresenceStreamPoller;
  private OutputStreamPoller<GpuBuffer> outputStreamPoller;
  private ClassificationListPacket handednessPacket;
  private BoolPacket handPresencePacket;
  private NormalizedRectPacket handRectPacket;
  private NormalizedLandmarkListPacket handLandmarkListPacket;
  private DetectionVectorPacket palmDetectionsPacket;
  private BoolPacket palmDetectionsPresencePacket;
  private GpuBufferPacket outputPacket;

  public override Status StartRun(SidePacket sidePacket) {
    handednessStreamPoller = graph.AddOutputStreamPoller<ClassificationList>(handednessStream).ConsumeValue();
    handPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(handPresenceStream).ConsumeValue();
    handRectStreamPoller = graph.AddOutputStreamPoller<NormalizedRect>(handRectStream).ConsumeValue();
    handLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(handLandmarksStream).ConsumeValue();
    palmDetectionsStreamPoller = graph.AddOutputStreamPoller<List<Detection>>(palmDetectionsStream).ConsumeValue();
    palmDetectionsPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(palmDetectionsPresenceStream).ConsumeValue();
    outputStreamPoller = graph.AddOutputStreamPoller<GpuBuffer>(outputStream).ConsumeValue();

    handednessPacket = new ClassificationListPacket();
    handPresencePacket = new BoolPacket();
    handRectPacket = new NormalizedRectPacket();
    handLandmarkListPacket = new NormalizedLandmarkListPacket();
    palmDetectionsPacket = new DetectionVectorPacket();
    palmDetectionsPresencePacket = new BoolPacket();
    outputPacket = new GpuBufferPacket();

    return graph.StartRun(sidePacket);
  }

  public override void RenderOutput(Texture2D texture, Color32[] pixelData) {
    if (!outputStreamPoller.Next(outputPacket)) { // blocks
      Debug.LogWarning("Failed to fetch an output packet, rendering the input image");
      texture.SetPixels32(pixelData);
      texture.Apply();
      return;
    }

    ImageFrame outputFrame = null;

    var status = gpuHelper.RunInGlContext(() => {
      var gpuFrame = outputPacket.GetValue();
      var gpuFrameFormat = gpuFrame.Format();
      var sourceTexture = gpuHelper.CreateSourceTexture(gpuFrame);

      outputFrame = new ImageFrame(
        gpuFrameFormat.ImageFormatFor(), gpuFrame.Width(), gpuFrame.Height(), ImageFrame.kGlDefaultAlignmentBoundary);

      gpuHelper.BindFramebuffer(sourceTexture);
      var info = gpuFrameFormat.GlTextureInfoFor(0);

      UnsafeNativeMethods.GlReadPixels(0, 0, sourceTexture.Width(), sourceTexture.Height(), info.GlFormat(), info.GlType(), outputFrame.PixelDataPtr());
      UnsafeNativeMethods.GlFlush();

      sourceTexture.Release();

      return Status.Ok(false);
    });

    if (status.IsOk()) {
      texture.SetPixels32(outputFrame.GetColor32s());
    } else {
      Debug.LogError(status.ToString());
      texture.SetPixels32(pixelData);
    }

    texture.Apply();

    // Fetch other outputs
    var isHandPresent = FetchNextHandPresence();
    var handedness = FetchNextHandedness();
    var rect = FetchNextRect();
    var landmarks = FetchNextHandLandmarkList();
    var palmDetections = FetchNextPalmDetections();
  }

  private bool FetchNextHandPresence() {
    if (!handPresenceStreamPoller.Next(handPresencePacket)) { // blocks
      Debug.LogWarning($"Failed to fetch next packet from {handPresenceStream}");
      return false;
    }
    
    return handPresencePacket.GetValue();
  }

  private ClassificationList FetchNextHandedness() {
    if (!handednessStreamPoller.Next(handednessPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handednessStream}");
      return null;
    }

    return handednessPacket.GetValue();
  }

  private NormalizedRect FetchNextRect() {
    if (!handRectStreamPoller.Next(handRectPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handRectStream}");
      return null;
    }

    return handRectPacket.GetValue();
  }

  private NormalizedLandmarkList FetchNextHandLandmarkList() {
    if (!handLandmarksStreamPoller.Next(handLandmarkListPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {handLandmarksStream}");
      return null;
    }

    return handLandmarkListPacket.GetValue();
  }

  private List<Detection> FetchNextPalmDetections() {
    if (!palmDetectionsPresenceStreamPoller.Next(palmDetectionsPresencePacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {palmDetectionsPresenceStream}");
      return null;
    }

    if (!palmDetectionsPresencePacket.GetValue()) {
      return null;
    }

    if (!palmDetectionsStreamPoller.Next(palmDetectionsPacket)) {
      Debug.LogWarning($"Failed to fetch next packet from {palmDetectionsStream}");
      return null;
    }

    return palmDetectionsPacket.GetValue();
  }

  public override bool shouldUseGPU() {
    return useGPU;
  }
}
