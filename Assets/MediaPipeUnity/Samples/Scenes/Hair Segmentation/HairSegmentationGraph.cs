// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Mediapipe.Unity.Sample.HairSegmentation
{
  public class HairSegmentationGraph : GraphRunner
  {
    public event EventHandler<OutputStream<ImageFrame>.OutputEventArgs> OnHairMaskOutput
    {
      add => _hairMaskStream.AddListener(value, timeoutMicrosec);
      remove => _hairMaskStream.RemoveListener(value);
    }

#if UNITY_IOS
    public override ConfigType configType => ConfigType.CPU;
#endif

    private const string _InputStreamName = "input_video";
    private const string _HairMaskStreamName = "hair_mask";
    private OutputStream<ImageFrame> _hairMaskStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _hairMaskStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }


    public override void Stop()
    {
      base.Stop();
      _hairMaskStream?.Dispose();
      _hairMaskStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public async Task<ImageFrame> WaitNext()
    {
      var result = await _hairMaskStream.WaitNextAsync();
      AssertResult(result);

      _ = TryGetValue(result.packet, out var hairMask, (packet) =>
      {
        return packet.Get();
      });
      return hairMask;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("hair_segmentation.bytes"),
      };
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      _hairMaskStream = new OutputStream<ImageFrame>(calculatorGraph, _HairMaskStreamName, true);
      calculatorGraph.Initialize(config);
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();

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

      sidePacket.Emplace("output_rotation", Packet.CreateInt((int)outputRotation));
      sidePacket.Emplace("output_horizontally_flipped", Packet.CreateBool(outputHorizontallyFlipped));
      sidePacket.Emplace("output_vertically_flipped", Packet.CreateBool(outputVerticallyFlipped));

      Debug.Log($"output_rotation = {outputRotation}, output_horizontally_flipped = {outputHorizontallyFlipped}, output_vertically_flipped = {outputVerticallyFlipped}");
      return sidePacket;
    }
  }
}
