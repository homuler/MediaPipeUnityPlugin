using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.BoxTracking {
  public class BoxTrackingGraph : GraphRunner {
    public UnityEvent<List<Detection>> OnTrackedDetectionsOutput = new UnityEvent<List<Detection>>();

    const string inputStreamName = "input_video";

    const string trackedDetectionsStreamName = "tracked_detections";
    OutputStreamPoller<List<Detection>> trackedDetectionsStreamPoller;
    DetectionVectorPacket trackedDetectionsPacket;
    protected long prevTrackedDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      trackedDetectionsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<Detection>>(trackedDetectionsStreamName, true).Value();
      trackedDetectionsPacket = new DetectionVectorPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(trackedDetectionsStreamName, TrackedDetectionsCallback, true).AssertOk();
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
      FetchNext(trackedDetectionsStreamPoller, trackedDetectionsPacket, out var trackedDetections, trackedDetectionsStreamName);
      OnTrackedDetectionsOutput.Invoke(trackedDetections);
      return trackedDetections;
    }

    public List<Detection> FetchLatestValue() {
      FetchLatest(trackedDetectionsStreamPoller, trackedDetectionsPacket, out var trackedDetections, trackedDetectionsStreamName);
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
