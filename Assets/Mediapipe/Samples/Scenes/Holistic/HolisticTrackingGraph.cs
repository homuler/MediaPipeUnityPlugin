using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.Holistic {
  public class HolisticTrackingGraph : GraphRunner {
    public enum ModelComplexity {
      Lite = 0,
      Full = 1,
      Heavy = 2,
    }

    public bool detectIris = false;
    public ModelComplexity modelComplexity = ModelComplexity.Lite;
    public bool smoothLandmarks = true;

    public UnityEvent<Detection> OnPoseDetectionOutput = new UnityEvent<Detection>();
    public UnityEvent<NormalizedLandmarkList> OnPoseLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnFaceLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnLeftHandLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnRightHandLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnLeftIrisLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<NormalizedLandmarkList> OnRightIrisLandmarksOutput = new UnityEvent<NormalizedLandmarkList>();
    public UnityEvent<LandmarkList> OnPoseWorldLandmarksOutput = new UnityEvent<LandmarkList>();
    public UnityEvent<NormalizedRect> OnPoseRoiOutput = new UnityEvent<NormalizedRect>();

    const string inputStreamName = "input_video";

    const string poseDetectionStreamName = "pose_detection";
    const string poseLandmarksStreamName = "pose_landmarks";
    const string faceLandmarksStreamName = "face_landmarks";
    const string leftHandLandmarksStreamName = "left_hand_landmarks";
    const string rightHandLandmarksStreamName = "right_hand_landmarks";
    const string leftIrisLandmarksStreamName = "left_iris_landmarks";
    const string rightIrisLandmarksStreamName = "right_iris_landmarks";
    const string poseWorldLandmarksStreamName = "pose_world_landmarks";
    const string poseRoiStreamName = "pose_roi";

    OutputStream<DetectionPacket, Detection> poseDetectionStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> poseLandmarksStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> faceLandmarksStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> leftHandLandmarksStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> rightHandLandmarksStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> leftIrisLandmarksStream;
    OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> rightIrisLandmarksStream;
    OutputStream<LandmarkListPacket, LandmarkList> poseWorldLandmarksStream;
    OutputStream<NormalizedRectPacket, NormalizedRect> poseRoiStream;

    protected long prevPoseDetectionMicrosec = 0;
    protected long prevPoseLandmarksMicrosec = 0;
    protected long prevFaceLandmarksMicrosec = 0;
    protected long prevLeftHandLandmarksMicrosec = 0;
    protected long prevRightHandLandmarksMicrosec = 0;
    protected long prevLeftIrisLandmarksMicrosec = 0;
    protected long prevRightIrisLandmarksMicrosec = 0;
    protected long prevPoseWorldLandmarksMicrosec = 0;
    protected long prevPoseRoiMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();

      poseDetectionStream.StartPolling(true).AssertOk();
      poseLandmarksStream.StartPolling(true).AssertOk();
      faceLandmarksStream.StartPolling(true).AssertOk();
      leftHandLandmarksStream.StartPolling(true).AssertOk();
      rightHandLandmarksStream.StartPolling(true).AssertOk();
      leftIrisLandmarksStream.StartPolling(true).AssertOk();
      rightIrisLandmarksStream.StartPolling(true).AssertOk();
      poseWorldLandmarksStream.StartPolling(true).AssertOk();
      poseRoiStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();

      poseDetectionStream.AddListener(PoseDetectionCallback, true).AssertOk();
      poseLandmarksStream.AddListener(PoseLandmarksCallback, true).AssertOk();
      faceLandmarksStream.AddListener(FaceLandmarksCallback, true).AssertOk();
      leftHandLandmarksStream.AddListener(LeftHandLandmarksCallback, true).AssertOk();
      rightHandLandmarksStream.AddListener(RightHandLandmarksCallback, true).AssertOk();
      leftIrisLandmarksStream.AddListener(LeftIrisLandmarksCallback, true).AssertOk();
      rightIrisLandmarksStream.AddListener(RightIrisLandmarksCallback, true).AssertOk();
      poseWorldLandmarksStream.AddListener(PoseWorldLandmarksCallback, true).AssertOk();
      poseRoiStream.AddListener(PoseRoiCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnPoseDetectionOutput.RemoveAllListeners();
      OnPoseLandmarksOutput.RemoveAllListeners();
      OnFaceLandmarksOutput.RemoveAllListeners();
      OnLeftHandLandmarksOutput.RemoveAllListeners();
      OnRightHandLandmarksOutput.RemoveAllListeners();
      OnLeftIrisLandmarksOutput.RemoveAllListeners();
      OnRightIrisLandmarksOutput.RemoveAllListeners();
      OnPoseWorldLandmarksOutput.RemoveAllListeners();
      OnPoseRoiOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public HolisticTrackingValue FetchNextValue() {
      poseDetectionStream.TryGetNext(out var poseDetection);
      poseLandmarksStream.TryGetNext(out var poseLandmarks);
      faceLandmarksStream.TryGetNext(out var faceLandmarks);
      leftHandLandmarksStream.TryGetNext(out var leftHandLandmarks);
      rightHandLandmarksStream.TryGetNext(out var rightHandLandmarks);
      leftIrisLandmarksStream.TryGetNext(out var leftIrisLandmarks);
      rightIrisLandmarksStream.TryGetNext(out var rightIrisLandmarks);
      poseWorldLandmarksStream.TryGetNext(out var poseWorldLandmarks);
      poseRoiStream.TryGetNext(out var poseRoi);

      OnPoseDetectionOutput.Invoke(poseDetection);
      OnPoseLandmarksOutput.Invoke(poseLandmarks);
      OnFaceLandmarksOutput.Invoke(faceLandmarks);
      OnLeftHandLandmarksOutput.Invoke(leftHandLandmarks);
      OnRightHandLandmarksOutput.Invoke(rightHandLandmarks);
      OnLeftIrisLandmarksOutput.Invoke(leftIrisLandmarks);
      OnRightIrisLandmarksOutput.Invoke(rightIrisLandmarks);
      OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks);
      OnPoseRoiOutput.Invoke(poseRoi);

      return new HolisticTrackingValue(
        poseDetection, poseLandmarks, faceLandmarks, leftHandLandmarks, rightHandLandmarks, leftIrisLandmarks, rightIrisLandmarks, poseWorldLandmarks, poseRoi
      );
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new DetectionPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseDetectionMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseDetectionOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevFaceLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnFaceLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr LeftHandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevLeftHandLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnLeftHandLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr RightHandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevRightHandLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnRightHandLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr LeftIrisLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevLeftIrisLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnLeftIrisLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr RightIrisLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevRightIrisLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnRightIrisLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new LandmarkListPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseWorldLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseWorldLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseRoiCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HolisticTrackingGraph>(graphPtr, packetPtr, (holisticTrackingGraph, ptr) => {
        using (var packet = new NormalizedRectPacket(ptr, false)) {
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseRoiMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseRoiOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_landmark.bytes"),
        WaitForAsset("iris_landmark.bytes"),
        WaitForAsset("hand_landmark.bytes"),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForAsset("palm_detection.bytes"),
        WaitForAsset("pose_detection.bytes"),
        WaitForPoseLandmarkModel(),
      };
    }

    WaitForResult WaitForPoseLandmarkModel() {
      if (modelComplexity == ModelComplexity.Lite) {
        return WaitForAsset("pose_landmark_lite.bytes");
      } else if (modelComplexity == ModelComplexity.Full) {
        return WaitForAsset("pose_landmark_full.bytes");
      } else {
        return WaitForAsset("pose_landmark_heavy.bytes");
      }
    }

    protected void InitializeOutputStreams() {
      poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, poseDetectionStreamName);
      poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, poseLandmarksStreamName);
      faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, faceLandmarksStreamName);
      leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, leftHandLandmarksStreamName);
      rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, rightHandLandmarksStreamName);
      leftIrisLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, leftIrisLandmarksStreamName);
      rightIrisLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, rightIrisLandmarksStreamName);
      poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, poseWorldLandmarksStreamName);
      poseRoiStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, poseRoiStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("enable_iris_detection", new BoolPacket(detectIris));
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

      return sidePacket;
    }
  }
}
