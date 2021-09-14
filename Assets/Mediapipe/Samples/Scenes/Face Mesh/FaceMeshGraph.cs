using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.FaceMesh {
  public class FaceMeshGraph : GraphRunner {
    public int maxNumFaces = 1;
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnMultiFaceLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
    public UnityEvent<List<NormalizedRect>> OnFaceRectsFromLandmarksOutput = new UnityEvent<List<NormalizedRect>>();

    const string inputStreamName = "input_video";

    const string faceDetectionsStreamName = "face_detections";
    OutputStreamPoller<List<Detection>> faceDetectionsStreamPoller;
    DetectionVectorPacket faceDetectionsPacket;
    protected long prevFaceDetectionsMicrosec = 0;

    const string multiFaceLandmarksStreamName = "multi_face_landmarks";
    OutputStreamPoller<List<NormalizedLandmarkList>> multiFaceLandmarksStreamPoller;
    NormalizedLandmarkListVectorPacket multiFaceLandmarksPacket;
    protected long prevMultiFaceLandmarksMicrosec = 0;

    const string faceRectsFromLandmarksStreamName = "face_rects_from_landmarks";
    OutputStreamPoller<List<NormalizedRect>> faceRectsFromLandmarksStreamPoller;
    NormalizedRectVectorPacket faceRectsFromLandmarksPacket;
    protected long prevFaceRectsFromLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      faceDetectionsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<Detection>>(faceDetectionsStreamName, true).Value();
      faceDetectionsPacket = new DetectionVectorPacket();

      multiFaceLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(multiFaceLandmarksStreamName, true).Value();
      multiFaceLandmarksPacket = new NormalizedLandmarkListVectorPacket();

      faceRectsFromLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedRect>>(faceRectsFromLandmarksStreamName, true).Value();
      faceRectsFromLandmarksPacket = new NormalizedRectVectorPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(faceDetectionsStreamName, FaceDetectionsCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(multiFaceLandmarksStreamName, MultiFaceLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(faceRectsFromLandmarksStreamName, FaceRectsFromLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnMultiFaceLandmarksOutput.RemoveAllListeners();
      OnFaceRectsFromLandmarksOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public FaceMeshValue FetchNextValue() {
      var faceDetections = FetchNextVector<Detection>(faceDetectionsStreamPoller, faceDetectionsPacket, faceDetectionsStreamName);
      var multiFaceLandmarks = FetchNextVector<NormalizedLandmarkList>(multiFaceLandmarksStreamPoller, multiFaceLandmarksPacket, multiFaceLandmarksStreamName);
      var faceRectsFromLandmarks = FetchNextVector<NormalizedRect>(faceRectsFromLandmarksStreamPoller, faceRectsFromLandmarksPacket, faceRectsFromLandmarksStreamName);

      OnFaceDetectionsOutput.Invoke(faceDetections);
      OnMultiFaceLandmarksOutput.Invoke(multiFaceLandmarks);
      OnFaceRectsFromLandmarksOutput.Invoke(faceRectsFromLandmarks);

      return new FaceMeshValue(faceDetections, multiFaceLandmarks, faceRectsFromLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) => {
        using (var packet = new DetectionVectorPacket(ptr, false)) {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevFaceDetectionsMicrosec, out var value)) {
            faceMeshGraph.OnFaceDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr MultiFaceLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false)) {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevMultiFaceLandmarksMicrosec, out var value)) {
            faceMeshGraph.OnMultiFaceLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceRectsFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) => {
        using (var packet = new NormalizedRectVectorPacket(ptr, false)) {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevFaceRectsFromLandmarksMicrosec, out var value)) {
            faceMeshGraph.OnFaceRectsFromLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_landmark.bytes"),
      };
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("num_faces", new IntPacket(maxNumFaces));

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
