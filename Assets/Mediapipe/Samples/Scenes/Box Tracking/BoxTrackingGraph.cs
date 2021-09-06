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
      var trackedDetections = FetchNextVector<Detection>(trackedDetectionsStreamPoller, trackedDetectionsPacket, trackedDetectionsStreamName);
      OnTrackedDetectionsOutput.Invoke(trackedDetections);
      return trackedDetections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr TrackedDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new DetectionVectorPacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as BoxTrackingGraph).OnTrackedDetectionsOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    protected override void PrepareDependentAssets() {
      AssetLoader.PrepareAsset("ssdlite_object_detection_labelmap.txt");
      AssetLoader.PrepareAsset("ssdlite_object_detection.bytes");
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
