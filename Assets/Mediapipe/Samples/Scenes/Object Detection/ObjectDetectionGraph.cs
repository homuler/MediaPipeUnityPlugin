// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.ObjectDetection
{
  public class ObjectDetectionGraph : GraphRunner
  {
#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<List<Detection>> OnOutputDetectionsOutput = new UnityEvent<List<Detection>>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private const string _OutputDetectionsStreamName = "output_detections";
    private OutputStream<DetectionVectorPacket, List<Detection>> _outputDetectionsStream;
    protected long prevOutputDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _outputDetectionsStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _outputDetectionsStream.AddListener(OutputDetectionsCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnOutputDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public List<Detection> FetchNextDetections()
    {
      var _ = _outputDetectionsStream.TryGetNext(out var outputDetections);
      OnOutputDetectionsOutput.Invoke(outputDetections);
      return outputDetections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr OutputDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectDetectionGraph>(graphPtr, packetPtr, (objectDetectionGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (objectDetectionGraph.TryGetPacketValue(packet, ref objectDetectionGraph.prevOutputDetectionsMicrosec, out var value))
          {
            objectDetectionGraph.OnOutputDetectionsOutput.Invoke(value);
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
      _outputDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _OutputDetectionsStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
