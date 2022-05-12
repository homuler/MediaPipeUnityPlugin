// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Unity.HairSegmentation
{
  public class HairSegmentationGraph : GraphRunner
  {
    public event EventHandler<OutputEventArgs<ImageFrame>> OnHairMaskOutput
    {
      add => _hairMaskStream.AddListener(value);
      remove => _hairMaskStream.RemoveListener(value);
    }

#if UNITY_IOS
    public override ConfigType configType => ConfigType.CPU;
#endif

    private const string _InputStreamName = "input_video";
    private const string _HairMaskStreamName = "hair_mask";
    private OutputStream<ImageFramePacket, ImageFrame> _hairMaskStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _hairMaskStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }


    public override void Stop()
    {
      _hairMaskStream?.Close();
      _hairMaskStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out ImageFrame hairMask, bool allowBlock = true)
    {
      return TryGetNext(_hairMaskStream, out hairMask, allowBlock, GetCurrentTimestampMicrosec());
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("hair_segmentation.bytes"),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _hairMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _HairMaskStreamName, config.AddPacketPresenceCalculator(_HairMaskStreamName), timeoutMicrosec);
      }
      else
      {
        _hairMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _HairMaskStreamName, true, timeoutMicrosec);
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
