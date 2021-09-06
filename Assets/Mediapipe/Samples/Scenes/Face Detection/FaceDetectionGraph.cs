using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.FaceDetection {
  public class FaceDetectionGraph : GraphRunner {
    public enum ModelType {
      ShortRange = 0,
      FullRangeSparse = 1,
    }
    public ModelType modelType = ModelType.ShortRange;
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();

    const string inputStreamName = "input_video";
    const string faceDetectionsStreamName = "face_detections";
    OutputStreamPoller<List<Detection>> faceDetectionsStreamPoller;
    DetectionVectorPacket faceDetectionsPacket;

    public override Status StartRun(ImageSource imageSource) {
      faceDetectionsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStreamName, true).Value();
      faceDetectionsPacket = new DetectionVectorPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(faceDetectionsStreamName, FaceDetectionsCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public List<Detection> FetchNextDetections() {
      var detections = FetchNextVector<Detection>(faceDetectionsStreamPoller, faceDetectionsPacket, faceDetectionsStreamName);
      OnFaceDetectionsOutput.Invoke(detections);
      return detections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new DetectionVectorPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as FaceDetectionGraph).OnFaceDetectionsOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    protected override void PrepareDependentAssets() {
      AssetLoader.PrepareAsset("face_detection_short_range.bytes");
      AssetLoader.PrepareAsset("face_detection_full_range_sparse.bytes");
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("model_type", new IntPacket((int)modelType));

      // Coordinate transformation from Unity to MediaPipe
      if (imageSource.isMirrored) {
        sidePacket.Emplace("input_rotation", new IntPacket(180));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(false));
      } else {
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      }

      return sidePacket;
    }
  }
}
