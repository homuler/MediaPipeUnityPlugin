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
    const string faceRectStreamName = "face_rect";
    const string faceLandmarksWithIrisStreamName = "face_landmarks_with_iris";

    OutputStream<DetectionVectorPacket, List<Detection>> faceDetectionsStream;
    OutputStream<NormalizedRectPacket, NormalizedRect> faceRectStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> faceLandmarksWithIrisStream;

    protected long prevFaceDetectionsMicrosec = 0;
    protected long prevFaceRectMicrosec = 0;
    protected long prevFaceLandmarksWithIrisMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();

      faceDetectionsStream.StartPolling(true).AssertOk();
      faceRectStream.StartPolling(true).AssertOk();
      faceLandmarksWithIrisStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();

      faceDetectionsStream.AddListener(FaceDetectionsCallback, true).AssertOk();
      faceRectStream.AddListener(FaceRectCallback, true).AssertOk();
      faceLandmarksWithIrisStream.AddListener(FaceLandmarksWithIrisCallback, true).AssertOk();

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
      faceDetectionsStream.TryGetNext(out var faceDetections);
      faceRectStream.TryGetNext(out var faceRect);
      faceLandmarksWithIrisStream.TryGetNext(out var faceLandmarksWithIris);

      OnFaceDetectionsOutput.Invoke(faceDetections);
      OnFaceRectOutput.Invoke(faceRect);
      OnFaceLandmarksWithIrisOutput.Invoke(faceLandmarksWithIris);

      return new IrisTrackingValue(faceDetections, faceRect, faceLandmarksWithIris);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) => {
        using (var packet = new DetectionVectorPacket(ptr, false)) {
          if (irisTrackingGraph.TryGetPacketValue(packet, ref irisTrackingGraph.prevFaceDetectionsMicrosec, out var value)) {
            irisTrackingGraph.OnFaceDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceRectCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) => {
        using (var packet = new NormalizedRectPacket(ptr, false)) {
          if (irisTrackingGraph.TryGetPacketValue(packet, ref irisTrackingGraph.prevFaceRectMicrosec, out var value)) {
            irisTrackingGraph.OnFaceRectOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceLandmarksWithIrisCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (irisTrackingGraph.TryGetPacketValue(packet, ref irisTrackingGraph.prevFaceLandmarksWithIrisMicrosec, out var value)) {
            irisTrackingGraph.OnFaceLandmarksWithIrisOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_landmark.bytes"),
        WaitForAsset("iris_landmark.bytes"),
      };
    }

    protected void InitializeOutputStreams() {
      faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, faceDetectionsStreamName);
      faceRectStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, faceRectStreamName);
      faceLandmarksWithIrisStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, faceLandmarksWithIrisStreamName);
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
