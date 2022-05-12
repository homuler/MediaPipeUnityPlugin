// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Unity.BoxTracking
{
  public class BoxTrackingGraph : GraphRunner
  {
    public event EventHandler<OutputEventArgs<List<Detection>>> OnTrackedDetectionsOutput
    {
      add => _trackedDetectionsStream.AddListener(value);
      remove => _trackedDetectionsStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _TrackedDetectionsStreamName = "tracked_detections";
    private OutputStream<DetectionVectorPacket, List<Detection>> _trackedDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _trackedDetectionsStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _trackedDetectionsStream?.Close();
      _trackedDetectionsStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> trackedDetections, bool allowBlock = true)
    {
      return TryGetNext(_trackedDetectionsStream, out trackedDetections, allowBlock, GetCurrentTimestampMicrosec());
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("ssdlite_object_detection_labelmap.txt"),
        WaitForAsset("ssdlite_object_detection.bytes"),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _trackedDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(
            calculatorGraph, _TrackedDetectionsStreamName, config.AddPacketPresenceCalculator(_TrackedDetectionsStreamName), timeoutMicrosec);
      }
      else
      {
        _trackedDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _TrackedDetectionsStreamName, true, timeoutMicrosec);
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
