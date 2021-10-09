// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.InstantMotionTracking
{
  public class RegionTrackingGraph : GraphRunner
  {
    private bool _isTracking = false;
    private int _currentStickerSentinelId = -1;
    private readonly Anchor3d[] _anchors = new Anchor3d[1];

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<List<Anchor3d>> OnTrackedAnchorDataOutput = new UnityEvent<List<Anchor3d>>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";
    private const string _StickerSentinelStreamName = "sticker_sentinel";
    private const string _InitialAnchorDataStreamName = "initial_anchor_data";

    private const string _TrackedAnchorDataStreamName = "tracked_anchor_data";
    private OutputStream<Anchor3dVectorPacket, List<Anchor3d>> _trackedAnchorDataStream;
    protected long prevTrackedAnchorDataMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _trackedAnchorDataStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _trackedAnchorDataStream.AddListener(TrackedAnchorDataCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnTrackedAnchorDataOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      var status = AddTextureFrameToInputStream(_InputStreamName, textureFrame);
      if (!status.Ok())
      {
        return status;
      }

      var stickerSentinelId = _isTracking ? -1 : _currentStickerSentinelId;
      status = AddPacketToInputStream(_StickerSentinelStreamName, new IntPacket(stickerSentinelId, currentTimestamp));

      if (!status.Ok())
      {
        return status;
      }

      _isTracking = true;
      return AddPacketToInputStream(_InitialAnchorDataStreamName, new Anchor3dVectorPacket(_anchors, currentTimestamp));
    }

    public List<Anchor3d> FetchNextValue()
    {
      var _ = _trackedAnchorDataStream.TryGetNext(out var trackedAnchorData);
      OnTrackedAnchorDataOutput.Invoke(trackedAnchorData);
      return trackedAnchorData;
    }

    public void ResetAnchor(float normalizedX = 0.5f, float normalizedY = 0.5f)
    {
      _anchors[0].stickerId = ++_currentStickerSentinelId;
      _isTracking = false;
      _anchors[0].x = normalizedX;
      _anchors[0].y = normalizedY;
      Logger.LogInfo(TAG, $"New anchor = {_anchors[0]}");
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr TrackedAnchorDataCallback(IntPtr graphPtr, IntPtr packetPtr)
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
      _trackedAnchorDataStream = new OutputStream<Anchor3dVectorPacket, List<Anchor3d>>(calculatorGraph, _TrackedAnchorDataStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
