// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Unity.InstantMotionTracking
{
  public class RegionTrackingGraph : GraphRunner
  {
    private bool _isTracking = false;
    private int _currentStickerSentinelId = -1;
    private readonly Anchor3d[] _anchors = new Anchor3d[1];

    public event EventHandler<OutputEventArgs<List<Anchor3d>>> OnTrackedAnchorDataOutput
    {
      add => _trackedAnchorDataStream.AddListener(value);
      remove => _trackedAnchorDataStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _StickerSentinelStreamName = "sticker_sentinel";
    private const string _InitialAnchorDataStreamName = "initial_anchor_data";

    private const string _TrackedAnchorDataStreamName = "tracked_anchor_data";
    private OutputStream<Anchor3dVectorPacket, List<Anchor3d>> _trackedAnchorDataStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _trackedAnchorDataStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _trackedAnchorDataStream?.Close();
      _trackedAnchorDataStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);

      var stickerSentinelId = _isTracking ? -1 : _currentStickerSentinelId;
      AddPacketToInputStream(_StickerSentinelStreamName, new IntPacket(stickerSentinelId, latestTimestamp));

      _isTracking = true;
      AddPacketToInputStream(_InitialAnchorDataStreamName, new Anchor3dVectorPacket(_anchors, latestTimestamp));
    }

    public bool TryGetNext(out List<Anchor3d> trackedAnchorData, bool allowBlock = true)
    {
      return TryGetNext(_trackedAnchorDataStream, out trackedAnchorData, allowBlock, GetCurrentTimestampMicrosec());
    }

    public void ResetAnchor(float normalizedX = 0.5f, float normalizedY = 0.5f)
    {
      _anchors[0].stickerId = ++_currentStickerSentinelId;
      _isTracking = false;
      _anchors[0].x = normalizedX;
      _anchors[0].y = normalizedY;
      Logger.LogInfo(TAG, $"New anchor = {_anchors[0]}");
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("ssdlite_object_detection.bytes"),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _trackedAnchorDataStream = new OutputStream<Anchor3dVectorPacket, List<Anchor3d>>(
            calculatorGraph, _TrackedAnchorDataStreamName, config.AddPacketPresenceCalculator(_TrackedAnchorDataStreamName), timeoutMicrosec);
      }
      else
      {
        _trackedAnchorDataStream = new OutputStream<Anchor3dVectorPacket, List<Anchor3d>>(calculatorGraph, _TrackedAnchorDataStreamName, true, timeoutMicrosec);
      }
      return calculatorGraph.Initialize(config);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
