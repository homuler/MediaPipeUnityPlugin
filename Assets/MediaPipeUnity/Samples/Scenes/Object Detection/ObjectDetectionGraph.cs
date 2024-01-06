// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediapipe.Unity.Sample.ObjectDetection
{
  public class ObjectDetectionGraph : GraphRunner
  {
    public event EventHandler<OutputStream.OutputEventArgs> OnOutputDetectionsOutput
    {
      add => _outputDetectionsStream.AddListener(value, timeoutMicrosec);
      remove => _outputDetectionsStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private const string _OutputDetectionsStreamName = "output_detections";
    private OutputStream _outputDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _outputDetectionsStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      _outputDetectionsStream?.Dispose();
      _outputDetectionsStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public async Task<List<Detection>> WaitNextAsync()
    {
      var result = await _outputDetectionsStream.WaitNextAsync();
      AssertResult(result);

      _ = TryGetValue(result.packet, out var outputDetections, (packet) =>
      {
        return packet.GetProtoList(Detection.Parser);
      });

      return outputDetections;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("ssdlite_object_detection_labelmap.txt"),
        WaitForAsset("ssdlite_object_detection.bytes"),
      };
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      _outputDetectionsStream = new OutputStream(calculatorGraph, _OutputDetectionsStreamName, true);
      calculatorGraph.Initialize(config);
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
