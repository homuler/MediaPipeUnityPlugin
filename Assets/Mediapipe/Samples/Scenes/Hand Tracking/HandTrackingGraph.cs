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
    const string handRectsFromPalmDetectionsStreamName = "hand_rects_from_palm_detections";
    const string handLandmarksStreamName = "hand_landmarks";
    const string handRectsFromLandmarksStreamName = "hand_rects_from_landmarks";
    const string handednessStreamName = "handedness";
  
    OutputStream<DetectionVectorPacket, List<Detection>> palmDetectionsStream;
    OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> handRectsFromPalmDetectionsStream;
    OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> handLandmarksStream;
    OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> handRectsFromLandmarksStream;
    OutputStream<ClassificationListVectorPacket, List<ClassificationList>> handednessStream;
  
    protected long prevPalmDetectionMicrosec = 0;
    protected long prevHandRectsFromPalmDetectionsMicrosec = 0;
    protected long prevHandLandmarksMicrosec = 0;
    protected long prevHandRectsFromLandmarksMicrosec = 0;
    protected long prevHandednessMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();

      palmDetectionsStream.StartPolling(true).AssertOk();
      handRectsFromPalmDetectionsStream.StartPolling(true).AssertOk();
      handLandmarksStream.StartPolling(true).AssertOk();
      handRectsFromLandmarksStream.StartPolling(true).AssertOk();
      handednessStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();

      palmDetectionsStream.AddListener(PalmDetectionsCallback, true).AssertOk();
      handRectsFromPalmDetectionsStream.AddListener(HandRectsFromPalmDetectionsCallback, true).AssertOk();
      handLandmarksStream.AddListener(HandLandmarksCallback, true).AssertOk();
      handRectsFromLandmarksStream.AddListener(HandRectsFromLandmarksCallback, true).AssertOk();
      handednessStream.AddListener(HandednessCallback, true).AssertOk();

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
      palmDetectionsStream.TryGetNext(out var palmDetections);
      handRectsFromPalmDetectionsStream.TryGetNext(out var handRectsFromPalmDetections);
      handLandmarksStream.TryGetNext(out var handLandmarks);
      handRectsFromLandmarksStream.TryGetNext(out var handRectsFromLandmarks);
      handednessStream.TryGetNext(out var handedness);

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

    protected void InitializeOutputStreams() {
      palmDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, palmDetectionsStreamName);
      handRectsFromPalmDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, handRectsFromPalmDetectionsStreamName);
      handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, handLandmarksStreamName);
      handRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, handRectsFromLandmarksStreamName);
      handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(calculatorGraph, handednessStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

      return sidePacket;
    }
  }
}
