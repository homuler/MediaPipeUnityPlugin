using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.InstantMotionTracking
{
  public class RegionTrackingGraph : GraphRunner
  {
    bool isTracking = false;
    int currentStickerSentinelId = -1;
    readonly Anchor3d[] anchors = new Anchor3d[1];

    public UnityEvent<List<Anchor3d>> OnTrackedAnchorDataOutput = new UnityEvent<List<Anchor3d>>();

    const string inputStreamName = "input_video";
    const string stickerSentinelStreamName = "sticker_sentinel";
    const string initialAnchorDataStreamName = "initial_anchor_data";

    const string trackedAnchorDataStreamName = "tracked_anchor_data";
    OutputStream<Anchor3dVectorPacket, List<Anchor3d>> trackedAnchorDataStream;
    protected long prevTrackedAnchorDataMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();
      trackedAnchorDataStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();
      trackedAnchorDataStream.AddListener(TrackedAnchorDataCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnTrackedAnchorDataOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      var status = AddTextureFrameToInputStream(inputStreamName, textureFrame);
      if (!status.Ok())
      {
        return status;
      }

      var stickerSentinelId = isTracking ? -1 : currentStickerSentinelId;
      status = AddPacketToInputStream(stickerSentinelStreamName, new IntPacket(stickerSentinelId, currentTimestamp));

      if (!status.Ok())
      {
        return status;
      }

      isTracking = true;
      return AddPacketToInputStream(initialAnchorDataStreamName, new Anchor3dVectorPacket(anchors, currentTimestamp));
    }

    public List<Anchor3d> FetchNextValue()
    {
      trackedAnchorDataStream.TryGetNext(out var trackedAnchorData);
      OnTrackedAnchorDataOutput.Invoke(trackedAnchorData);
      return trackedAnchorData;
    }

    public void ResetAnchor(float normalizedX = 0.5f, float normalizedY = 0.5f)
    {
      anchors[0].stickerId = ++currentStickerSentinelId;
      isTracking = false;
      anchors[0].x = normalizedX;
      anchors[0].y = normalizedY;
      Logger.LogInfo(TAG, $"New anchor = {anchors[0]}");
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr TrackedAnchorDataCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<RegionTrackingGraph>(graphPtr, packetPtr, (regionTrackingGraph, ptr) =>
      {
        using (var packet = new Anchor3dVectorPacket(ptr, false))
        {
          if (regionTrackingGraph.TryGetPacketValue(packet, ref regionTrackingGraph.prevTrackedAnchorDataMicrosec, out var value))
          {
            regionTrackingGraph.OnTrackedAnchorDataOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("ssdlite_object_detection.bytes"),
      };
    }

    protected void InitializeOutputStreams()
    {
      trackedAnchorDataStream = new OutputStream<Anchor3dVectorPacket, List<Anchor3d>>(calculatorGraph, trackedAnchorDataStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
