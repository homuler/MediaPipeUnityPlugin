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
    protected long prevTrackedDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _trackedDetectionsStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _trackedDetectionsStream.AddListener(TrackedDetectionsCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnTrackedDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public List<Detection> FetchNextValue()
    {
      var _ = _trackedDetectionsStream.TryGetNext(out var trackedDetections);
      OnTrackedDetectionsOutput.Invoke(trackedDetections);
      return trackedDetections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr TrackedDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<BoxTrackingGraph>(graphPtr, packetPtr, (boxTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (boxTrackingGraph.TryGetPacketValue(packet, ref boxTrackingGraph.prevTrackedDetectionsMicrosec, out var value))
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

    protected void InitializeOutputStreams()
    {
      _trackedDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _TrackedDetectionsStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
