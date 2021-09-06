using System;
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
    public long timeoutMicrosec = 50000; // 30 millisec

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
    OutputStreamPoller<Detection> poseDetectionStreamPoller;
    DetectionPacket poseDetectionPacket;
    protected long prevPoseDetectionMicrosec = 0;

    const string poseLandmarksStreamName = "pose_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
    NormalizedLandmarkListPacket poseLandmarksPacket;
    protected long prevPoseLandmarksMicrosec = 0;

    const string faceLandmarksStreamName = "face_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> faceLandmarksStreamPoller;
    NormalizedLandmarkListPacket faceLandmarksPacket;
    protected long prevFaceLandmarksMicrosec = 0;

    const string leftHandLandmarksStreamName = "left_hand_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> leftHandLandmarksStreamPoller;
    NormalizedLandmarkListPacket leftHandLandmarksPacket;
    protected long prevLeftHandLandmarksMicrosec = 0;

    const string rightHandLandmarksStreamName = "right_hand_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> rightHandLandmarksStreamPoller;
    NormalizedLandmarkListPacket rightHandLandmarksPacket;
    protected long prevRightHandLandmarksMicrosec = 0;

    const string leftIrisLandmarksStreamName = "left_iris_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> leftIrisLandmarksStreamPoller;
    NormalizedLandmarkListPacket leftIrisLandmarksPacket;
    protected long prevLeftIrisLandmarksMicrosec = 0;

    const string rightIrisLandmarksStreamName = "right_iris_landmarks";
    OutputStreamPoller<NormalizedLandmarkList> rightIrisLandmarksStreamPoller;
    NormalizedLandmarkListPacket rightIrisLandmarksPacket;
    protected long prevRightIrisLandmarksMicrosec = 0;

    const string poseWorldLandmarksStreamName = "pose_world_landmarks";
    OutputStreamPoller<LandmarkList> poseWorldLandmarksStreamPoller;
    LandmarkListPacket poseWorldLandmarksPacket;
    protected long prevPoseWorldLandmarksMicrosec = 0;

    const string poseRoiStreamName = "pose_roi";
    OutputStreamPoller<NormalizedRect> poseRoiStreamPoller;
    NormalizedRectPacket poseRoiPacket;
    protected long prevPoseRoiMicrosec = 0;



    public override Status StartRun(ImageSource imageSource) {
      poseDetectionStreamPoller = calculatorGraph.AddOutputStreamPoller<Detection>(poseDetectionStreamName, true).Value();
      poseDetectionPacket = new DetectionPacket();

      poseLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStreamName, true).Value();
      poseLandmarksPacket = new NormalizedLandmarkListPacket();

      faceLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(faceLandmarksStreamName, true).Value();
      faceLandmarksPacket = new NormalizedLandmarkListPacket();

      leftHandLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(leftHandLandmarksStreamName, true).Value();
      leftHandLandmarksPacket = new NormalizedLandmarkListPacket();

      rightHandLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(rightHandLandmarksStreamName, true).Value();
      rightHandLandmarksPacket = new NormalizedLandmarkListPacket();

      leftIrisLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(leftIrisLandmarksStreamName, true).Value();
      leftIrisLandmarksPacket = new NormalizedLandmarkListPacket();

      rightIrisLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedLandmarkList>(rightIrisLandmarksStreamName, true).Value();
      rightIrisLandmarksPacket = new NormalizedLandmarkListPacket();

      poseWorldLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<LandmarkList>(poseWorldLandmarksStreamName, true).Value();
      poseWorldLandmarksPacket = new LandmarkListPacket();

      poseRoiStreamPoller = calculatorGraph.AddOutputStreamPoller<NormalizedRect>(poseRoiStreamName, true).Value();
      poseRoiPacket = new NormalizedRectPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(poseDetectionStreamName, PoseDetectionCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseLandmarksStreamName, PoseLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseWorldLandmarksStreamName, PoseWorldLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(poseRoiStreamName, PoseRoiCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(faceLandmarksStreamName, FaceLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(leftHandLandmarksStreamName, LeftHandLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(rightHandLandmarksStreamName, RightHandLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(leftIrisLandmarksStreamName, LeftIrisLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(rightIrisLandmarksStreamName, RightIrisLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public HolisticTrackingValue FetchNextValue() {
      var poseDetection = FetchNext(poseDetectionStreamPoller, poseDetectionPacket, poseDetectionStreamName);
      var poseLandmarks = FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStreamName);
      var poseWorldLandmarks = FetchNext(poseWorldLandmarksStreamPoller, poseWorldLandmarksPacket, poseWorldLandmarksStreamName);
      var roiFromLandmarks = FetchNext(poseRoiStreamPoller, poseRoiPacket, poseRoiStreamName);
      var faceLandmarks = FetchNext(faceLandmarksStreamPoller, faceLandmarksPacket, faceLandmarksStreamName);
      var leftHandLandmarks = FetchNext(leftHandLandmarksStreamPoller, leftHandLandmarksPacket, leftHandLandmarksStreamName);
      var rightHandLandmarks = FetchNext(rightHandLandmarksStreamPoller, rightHandLandmarksPacket, rightHandLandmarksStreamName);
      var leftIrisLandmarks = FetchNext(leftIrisLandmarksStreamPoller, leftIrisLandmarksPacket, leftIrisLandmarksStreamName);
      var rightIrisLandmarks = FetchNext(rightIrisLandmarksStreamPoller, rightIrisLandmarksPacket, rightIrisLandmarksStreamName);

      OnPoseDetectionOutput.Invoke(poseDetection);
      OnPoseLandmarksOutput.Invoke(poseLandmarks);
      OnPoseWorldLandmarksOutput.Invoke(poseWorldLandmarks);
      OnPoseRoiOutput.Invoke(roiFromLandmarks);
      OnFaceLandmarksOutput.Invoke(faceLandmarks);
      OnLeftHandLandmarksOutput.Invoke(leftHandLandmarks);
      OnRightHandLandmarksOutput.Invoke(rightHandLandmarks);
      OnLeftIrisLandmarksOutput.Invoke(leftIrisLandmarks);
      OnRightIrisLandmarksOutput.Invoke(rightIrisLandmarks);

      return new HolisticTrackingValue(
        poseDetection, poseLandmarks, poseWorldLandmarks, roiFromLandmarks, faceLandmarks, leftHandLandmarks, rightHandLandmarks, leftIrisLandmarks, rightIrisLandmarks
      );
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseDetectionCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new DetectionPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseDetectionMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseDetectionOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevFaceLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnFaceLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr LeftHandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevLeftHandLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnLeftHandLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr RightHandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevRightHandLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnRightHandLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr LeftIrisLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevLeftIrisLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnLeftIrisLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr RightIrisLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedLandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevRightIrisLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnRightIrisLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseWorldLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new LandmarkListPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseWorldLandmarksMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseWorldLandmarksOutput.Invoke(value);
          }
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PoseRoiCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new NormalizedRectPacket(packetPtr, false)) {
          var holisticTrackingGraph = (HolisticTrackingGraph)graphRunner;
          if (holisticTrackingGraph.TryGetPacketValue(packet, ref holisticTrackingGraph.prevPoseRoiMicrosec, out var value)) {
            holisticTrackingGraph.OnPoseRoiOutput.Invoke(value);
          }
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
      AssetLoader.PrepareAsset("hand_landmark.bytes");
      AssetLoader.PrepareAsset("hand_recrop.bytes");
      AssetLoader.PrepareAsset("handedness.txt");
      AssetLoader.PrepareAsset("palm_detection.bytes");
      AssetLoader.PrepareAsset("pose_detection.bytes");

      if (modelComplexity == ModelComplexity.Lite) {
        AssetLoader.PrepareAsset("pose_landmark_lite.bytes");
      } else if (modelComplexity == ModelComplexity.Full) {
        AssetLoader.PrepareAsset("pose_landmark_full.bytes");
      } else {
        AssetLoader.PrepareAsset("pose_landmark_heavy.bytes");
      }
    }

    protected bool TryGetPacketValue<T>(Packet<T> packet, ref long prevMicrosec, out T value) where T : class {
      return TryGetPacketValue(packet, timeoutMicrosec, ref prevMicrosec, out value);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("enable_iris_detection", new BoolPacket(detectIris));
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));

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