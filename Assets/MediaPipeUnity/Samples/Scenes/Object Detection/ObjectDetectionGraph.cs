// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Unity.ObjectDetection
{
  public class ObjectDetectionGraph : GraphRunner
  {
    public event EventHandler<OutputEventArgs<List<Detection>>> OnOutputDetectionsOutput
    {
      add => _outputDetectionsStream.AddListener(value);
      remove => _outputDetectionsStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private const string _OutputDetectionsStreamName = "output_detections";
    private OutputStream<DetectionVectorPacket, List<Detection>> _outputDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _outputDetectionsStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _outputDetectionsStream?.Close();
      _outputDetectionsStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> outputDetections, bool allowBlock = true)
    {
      return TryGetNext(_outputDetectionsStream, out outputDetections, allowBlock, GetCurrentTimestampMicrosec());
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
        _outputDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(
            calculatorGraph, _OutputDetectionsStreamName, config.AddPacketPresenceCalculator(_OutputDetectionsStreamName), timeoutMicrosec);
      }
      else
      {
        _outputDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _OutputDetectionsStreamName, true, timeoutMicrosec);
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
