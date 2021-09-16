using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.InstantMotionTracking {
  public class RegionTrackingGraph : GraphRunner {
    bool isTracking = false;
    int currentStickerSentinelId = -1;
    readonly Anchor3d[] anchors = new Anchor3d[1];

    public UnityEvent<List<Anchor3d>> OnTrackedAnchorDataOutput = new UnityEvent<List<Anchor3d>>();

    const string inputStreamName = "input_video";
    const string stickerSentinelStreamName = "sticker_sentinel";
    const string initialAnchorDataStreamName = "initial_anchor_data";

    const string trackedAnchorDataStreamName = "tracked_anchor_data";
    OutputStreamPoller<List<Anchor3d>> trackedAnchorDataStreamPoller;
    Anchor3dVectorPacket trackedAnchorDataPacket;
    protected long prevTrackedAnchorDataMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      ResetAnchor();
      trackedAnchorDataStreamPoller = calculatorGraph.AddOutputStreamPoller<List<Anchor3d>>(trackedAnchorDataStreamName, true).Value();
      trackedAnchorDataPacket = new Anchor3dVectorPacket();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      ResetAnchor();
      calculatorGraph.ObserveOutputStream(trackedAnchorDataStreamName, TrackedAnchorDataCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnTrackedAnchorDataOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      var status = AddTextureFrameToInputStream(inputStreamName, textureFrame);
      if (!status.ok) {
        return status;
      }

      var stickerSentinelId = isTracking ? -1 : currentStickerSentinelId;
      status = AddPacketToInputStream(stickerSentinelStreamName, new IntPacket(stickerSentinelId, currentTimestamp));

      if (!status.ok) {
        return status;
      }

      isTracking = true;
      return AddPacketToInputStream(initialAnchorDataStreamName, new Anchor3dVectorPacket(anchors, currentTimestamp));
    }

    public List<Anchor3d> FetchNextValue() {
      FetchNext(trackedAnchorDataStreamPoller, trackedAnchorDataPacket, out var trackedAnchorData, trackedAnchorDataStreamName);
      OnTrackedAnchorDataOutput.Invoke(trackedAnchorData);
      return trackedAnchorData;
    }

    public List<Anchor3d> FetchLatestValue() {
      FetchLatest(trackedAnchorDataStreamPoller, trackedAnchorDataPacket, out var trackedAnchorData, trackedAnchorDataStreamName);
      OnTrackedAnchorDataOutput.Invoke(trackedAnchorData);
      return trackedAnchorData;
    }

    public void ResetAnchor(float normalizedX = 0.5f, float normalizedY = 0.5f) {
      anchors[0].StickerId = ++currentStickerSentinelId;
      isTracking = false;
      anchors[0].X = normalizedX;
      anchors[0].Y = normalizedY;
      Logger.LogInfo(TAG, $"New anchor = {anchors[0]}");
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr TrackedAnchorDataCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<RegionTrackingGraph>(graphPtr, packetPtr, (regionTrackingGraph, ptr) => {
        using (var packet = new Anchor3dVectorPacket(ptr, false)) {
          if (regionTrackingGraph.TryGetPacketValue(packet, ref regionTrackingGraph.prevTrackedAnchorDataMicrosec, out var value)) {
            regionTrackingGraph.OnTrackedAnchorDataOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
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
