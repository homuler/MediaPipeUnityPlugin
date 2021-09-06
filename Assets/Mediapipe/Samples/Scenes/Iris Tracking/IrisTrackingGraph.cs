using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.IrisTracking {
  public class IrisTrackingGraph : GraphRunner {
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<NormalizedRect> OnFaceRectOutput = new UnityEvent<NormalizedRect>();
    public UnityEvent<NormalizedLandmarkList> OnFaceLandmarksWithIrisOutput = new UnityEvent<NormalizedLandmarkList>();

    const string inputStreamName = "input_video";

    const string faceDetectionsStreamName = "face_detections";
    OutputStreamPoller<List<Detection>> faceDetectionsStreamPoller;
    DetectionVectorPacket faceDetectionsPacket;

    const string faceRectStreamName = "face_rect";
    OutputStreamPoller<NormalizedRect> faceRectStreamPoller;
    NormalizedRectPacket faceRectPacket;

    const string faceLandmarksWithIrisStreamName = "face_landmarks_with_iris";
    OutputStreamPoller<NormalizedLandmarkList> faceLandmarksWithIrisStreamPoller;
    NormalizedLandmarkListPacket faceLandmarksWithIrisPacket;

    public override Status StartRun(ImageSource imageSource) {
      faceDetectionsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStreamName, true).Value();
      faceDetectionsPacket = new DetectionVectorPacket();

      faceRectStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedRect>(faceRectStreamName, true).Value();
      faceRectPacket = new NormalizedRectPacket();

      faceLandmarksWithIrisStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(faceLandmarksWithIrisStreamName, true).Value();
      faceLandmarksWithIrisPacket = new NormalizedLandmarkListPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(faceDetectionsStreamName, FaceDetectionsCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(faceRectStreamName, FaceRectCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(faceLandmarksWithIrisStreamName, FaceLandmarksWithIrisCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnFaceRectOutput.RemoveAllListeners();
      OnFaceLandmarksWithIrisOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public IrisTrackingValue FetchNextValue() {
      var faceDetections = FetchNextVector(faceDetectionsStreamPoller, faceDetectionsPacket, faceDetectionsStreamName);
      var faceRect = FetchNext(faceRectStreamPoller, faceRectPacket, faceRectStreamName);
      var faceLandmarksWithIris = FetchNext(faceLandmarksWithIrisStreamPoller, faceLandmarksWithIrisPacket, faceLandmarksWithIrisStreamName);

      OnFaceDetectionsOutput.Invoke(faceDetections);
      OnFaceRectOutput.Invoke(faceRect);
      OnFaceLandmarksWithIrisOutput.Invoke(faceLandmarksWithIris);

      return new IrisTrackingValue(faceDetections, faceRect, faceLandmarksWithIris);
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
          (graphRunner as IrisTrackingGraph).OnFaceDetectionsOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceRectCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedRectPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as IrisTrackingGraph).OnFaceRectOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceLandmarksWithIrisCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as IrisTrackingGraph).OnFaceLandmarksWithIrisOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    protected override void PrepareDependentAssets() {
      AssetLoader.PrepareAsset("face_detection_short_range.bytes");
      AssetLoader.PrepareAsset("face_landmark.bytes");
      AssetLoader.PrepareAsset("iris_landmark.bytes");
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();

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
