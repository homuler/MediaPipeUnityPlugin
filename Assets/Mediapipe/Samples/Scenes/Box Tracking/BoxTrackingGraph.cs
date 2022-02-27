// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.BoxTracking
{
  public class BoxTrackingGraph : GraphRunner
  {
#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<List<Detection>> OnTrackedDetectionsOutput = new UnityEvent<List<Detection>>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";
    private const string _TrackedDetectionsStreamName = "tracked_detections";
    private OutputStream<DetectionVectorPacket, List<Detection>> _trackedDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _trackedDetectionsStream.StartPolling().AssertOk();
      }
      else
      {
        _trackedDetectionsStream.AddListener(TrackedDetectionsCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnTrackedDetectionsOutput.RemoveAllListeners();
      _trackedDetectionsStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> trackedDetections, bool allowBlock = true)
    {
      if (TryGetNext(_trackedDetectionsStream, out trackedDetections, allowBlock, GetCurrentTimestampMicrosec()))
      {
        OnTrackedDetectionsOutput.Invoke(trackedDetections);
        return true;
      }
      return false;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr TrackedDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<BoxTrackingGraph>(graphPtr, packetPtr, (boxTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (boxTrackingGraph._trackedDetectionsStream.TryGetPacketValue(packet, out var value, boxTrackingGraph.timeoutMicrosec))
          {
            boxTrackingGraph.OnTrackedDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
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
        _trackedDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _TrackedDetectionsStreamName, config.AddPacketPresenceCalculator(_TrackedDetectionsStreamName));
      }
      else
      {
        _trackedDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _TrackedDetectionsStreamName, true);
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
