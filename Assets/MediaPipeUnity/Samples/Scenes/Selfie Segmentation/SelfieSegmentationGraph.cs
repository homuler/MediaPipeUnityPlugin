// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Unity.SelfieSegmentation
{
  public class SelfieSegmentationGraph : GraphRunner
  {
    public event EventHandler<OutputEventArgs<ImageFrame>> OnSegmentationMaskOutput
    {
      add => _segmentationMaskStream.AddListener(value);
      remove => _segmentationMaskStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _SegmentationMaskStreamName = "segmentation_mask";
    private OutputStream<ImageFramePacket, ImageFrame> _segmentationMaskStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _segmentationMaskStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }


    public override void Stop()
    {
      _segmentationMaskStream?.Close();
      _segmentationMaskStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out ImageFrame segmentationMask, bool allowBlock = true)
    {
      return TryGetNext(_segmentationMaskStream, out segmentationMask, allowBlock, GetCurrentTimestampMicrosec());
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("selfie_segmentation.bytes"),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _segmentationMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _SegmentationMaskStreamName, config.AddPacketPresenceCalculator(_SegmentationMaskStreamName), timeoutMicrosec);
      }
      else
      {
        _segmentationMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _SegmentationMaskStreamName, true, timeoutMicrosec);
      }
      return calculatorGraph.Initialize(config);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);

      // TODO: refactoring
      // The orientation of the output image must match that of the input image.
      var isInverted = CoordinateSystem.ImageCoordinate.IsInverted(imageSource.rotation);
      var outputRotation = imageSource.rotation;
      var outputHorizontallyFlipped = !isInverted && imageSource.isHorizontallyFlipped;
      var outputVerticallyFlipped = (!runningMode.IsSynchronous() && imageSource.isVerticallyFlipped) ^ (isInverted && imageSource.isHorizontallyFlipped);

      if ((outputHorizontallyFlipped && outputVerticallyFlipped) || outputRotation == RotationAngle.Rotation180)
      {
        outputRotation = outputRotation.Add(RotationAngle.Rotation180);
        outputHorizontallyFlipped = !outputHorizontallyFlipped;
        outputVerticallyFlipped = !outputVerticallyFlipped;
      }

      sidePacket.Emplace("output_rotation", new IntPacket((int)outputRotation));
      sidePacket.Emplace("output_horizontally_flipped", new BoolPacket(outputHorizontallyFlipped));
      sidePacket.Emplace("output_vertically_flipped", new BoolPacket(outputVerticallyFlipped));

      Logger.LogDebug($"output_rotation = {outputRotation}, output_horizontally_flipped = {outputHorizontallyFlipped}, output_vertically_flipped = {outputVerticallyFlipped}");
      return sidePacket;
    }
  }
}
