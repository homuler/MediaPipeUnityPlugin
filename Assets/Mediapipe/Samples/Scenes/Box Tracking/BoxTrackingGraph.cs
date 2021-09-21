using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.BoxTracking {
  public class BoxTrackingGraph : GraphRunner {
    public UnityEvent<List<Detection>> OnTrackedDetectionsOutput = new UnityEvent<List<Detection>>();

    const string inputStreamName = "input_video";

    const string trackedDetectionsStreamName = "tracked_detections";
    OutputStream<DetectionVectorPacket, List<Detection>> trackedDetectionsStream;
    protected long prevTrackedDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();
      trackedDetectionsStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();
      trackedDetectionsStream.AddListener(TrackedDetectionsCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnTrackedDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public List<Detection> FetchNextValue() {
      trackedDetectionsStream.TryGetNext(out var trackedDetections);
      OnTrackedDetectionsOutput.Invoke(trackedDetections);
      return trackedDetections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr TrackedDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<BoxTrackingGraph>(graphPtr, packetPtr, (boxTrackingGraph, ptr) => {
        using (var packet = new DetectionVectorPacket(ptr, false)) {
          if (boxTrackingGraph.TryGetPacketValue(packet, ref boxTrackingGraph.prevTrackedDetectionsMicrosec, out var value)) {
            boxTrackingGraph.OnTrackedDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("ssdlite_object_detection_labelmap.txt"),
        WaitForAsset("ssdlite_object_detection.bytes"),
      };
    }

    protected void InitializeOutputStreams() {
      trackedDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, trackedDetectionsStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
