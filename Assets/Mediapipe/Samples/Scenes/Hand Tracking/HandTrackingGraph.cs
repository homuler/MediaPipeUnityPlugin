using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.HandTracking {
  public class HandTrackingGraph : GraphRunner {
    public int maxNumHands = 2;

    public UnityEvent<List<Detection>> OnPalmDetectectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<List<NormalizedRect>> OnHandRectsFromPalmDetectionsOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnHandLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
    public UnityEvent<List<NormalizedRect>> OnHandRectsFromLandmarksOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<ClassificationList>> OnHandednessOutput = new UnityEvent<List<ClassificationList>>();

    const string inputStreamName = "input_video";

    const string palmDetectionsStreamName = "palm_detections";
    OutputStreamPoller<List<Detection>> palmDetectionsStreamPoller;
    DetectionVectorPacket palmDetectionsPacket;
    protected long prevPalmDetectionMicrosec = 0;

    const string handRectsFromPalmDetectionsStreamName = "hand_rects_from_palm_detections";
    OutputStreamPoller<List<NormalizedRect>> handRectsFromPalmDetectionsStreamPoller;
    NormalizedRectVectorPacket handRectsFromPalmDetectionsPacket;
    protected long prevHandRectsFromPalmDetectionsMicrosec = 0;

    const string handLandmarksStreamName = "hand_landmarks";
    OutputStreamPoller<List<NormalizedLandmarkList>> handLandmarksStreamPoller;
    NormalizedLandmarkListVectorPacket handLandmarksPacket;
    protected long prevHandLandmarksMicrosec = 0;

    const string handRectsFromLandmarksStreamName = "hand_rects_from_landmarks";
    OutputStreamPoller<List<NormalizedRect>> handRectsFromLandmarksStreamPoller;
    NormalizedRectVectorPacket handRectsFromLandmarksPacket;
    protected long prevHandRectsFromLandmarksMicrosec;

    const string handednessStreamName = "handedness";
    OutputStreamPoller<List<ClassificationList>> handednessStreamPoller;
    ClassificationListVectorPacket handednessPacket;
    protected long prevHandednessMicrosec;

    public override Status StartRun(ImageSource imageSource) {
      palmDetectionsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<Detection>>(palmDetectionsStreamName, true).Value();
      palmDetectionsPacket = new DetectionVectorPacket();

      handRectsFromPalmDetectionsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedRect>>(handRectsFromPalmDetectionsStreamName, true).Value();
      handRectsFromPalmDetectionsPacket = new NormalizedRectVectorPacket();

      handLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(handLandmarksStreamName, true).Value();
      handLandmarksPacket = new NormalizedLandmarkListVectorPacket();

      handRectsFromLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedRect>>(handRectsFromLandmarksStreamName, true).Value();
      handRectsFromLandmarksPacket = new NormalizedRectVectorPacket();

      handednessStreamPoller = calculatorGraph.AddOutputStreamPoller<List<ClassificationList>>(handednessStreamName, true).Value();
      handednessPacket = new ClassificationListVectorPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(palmDetectionsStreamName, PalmDetectionsCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(handRectsFromPalmDetectionsStreamName, HandRectsFromPalmDetectionsCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(handLandmarksStreamName, HandLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(handRectsFromLandmarksStreamName, HandRectsFromLandmarksCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(handednessStreamName, HandednessCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnPalmDetectectionsOutput.RemoveAllListeners();
      OnHandRectsFromPalmDetectionsOutput.RemoveAllListeners();
      OnHandLandmarksOutput.RemoveAllListeners();
      OnHandRectsFromLandmarksOutput.RemoveAllListeners();
      OnHandednessOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public HandTrackingValue FetchNextValue() {
      var palmDetections = FetchNextVector(palmDetectionsStreamPoller, palmDetectionsPacket, palmDetectionsStreamName);
      var handRectsFromPalmDetections = FetchNextVector(handRectsFromPalmDetectionsStreamPoller, handRectsFromPalmDetectionsPacket, handRectsFromPalmDetectionsStreamName);
      var handLandmarks = FetchNextVector(handLandmarksStreamPoller, handLandmarksPacket, handLandmarksStreamName);
      var handRectsFromLandmarks = FetchNextVector(handRectsFromLandmarksStreamPoller, handRectsFromLandmarksPacket, handRectsFromLandmarksStreamName);
      var handedness = FetchNextVector(handednessStreamPoller, handednessPacket, handednessStreamName);

      OnPalmDetectectionsOutput.Invoke(palmDetections);
      OnHandRectsFromPalmDetectionsOutput.Invoke(handRectsFromPalmDetections);
      OnHandLandmarksOutput.Invoke(handLandmarks);
      OnHandRectsFromLandmarksOutput.Invoke(handRectsFromLandmarks);
      OnHandednessOutput.Invoke(handedness);

      return new HandTrackingValue(palmDetections, handRectsFromPalmDetections, handLandmarks, handRectsFromLandmarks, handedness);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr PalmDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) => {
        using (var packet = new DetectionVectorPacket(ptr, false)) {
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevPalmDetectionMicrosec, out var value)) {
            handTrackingGraph.OnPalmDetectectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr HandRectsFromPalmDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) => {
        using (var packet = new NormalizedRectVectorPacket(ptr, false)) {
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandRectsFromPalmDetectionsMicrosec, out var value)) {
            handTrackingGraph.OnHandRectsFromPalmDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr HandLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false)) {
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandLandmarksMicrosec, out var value)) {
            handTrackingGraph.OnHandLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr HandRectsFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) => {
        using (var packet = new NormalizedRectVectorPacket(ptr, false)) {
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandRectsFromLandmarksMicrosec, out var value)) {
            handTrackingGraph.OnHandRectsFromLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr HandednessCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HandTrackingGraph>(graphPtr, packetPtr, (handTrackingGraph, ptr) => {
        using (var packet = new ClassificationListVectorPacket(ptr, false)) {
          if (handTrackingGraph.TryGetPacketValue(packet, ref handTrackingGraph.prevHandednessMicrosec, out var value)) {
            handTrackingGraph.OnHandednessOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("hand_landmark.bytes"),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForAsset("palm_detection.bytes"),
      };
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

      // Coordinate transformation from Unity to MediaPipe
      // Filps the input image if it's **not** mirrored, because MediaPipe assumes that the the input is vertically flipped,
      if (imageSource.isMirrored) {
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      } else {
        sidePacket.Emplace("input_rotation", new IntPacket(180));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(false));
      }

      return sidePacket;
    }
  }
}
