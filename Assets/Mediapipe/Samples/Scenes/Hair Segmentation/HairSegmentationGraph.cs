// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.HairSegmentation
{
  public class HairSegmentationGraph : GraphRunner
  {
#pragma warning disable IDE1006
    public UnityEvent<ImageFrame> OnHairMaskOutput = new UnityEvent<ImageFrame>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private const string _HairMaskStreamName = "hair_mask";
    private OutputStream<ImageFramePacket, ImageFrame> _hairMaskStream;
    protected long prevHairMaskMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _hairMaskStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();
      _hairMaskStream.AddListener(HairMaskCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnHairMaskOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public ImageFrame FetchNextValue()
    {
      var _ = _hairMaskStream.TryGetNext(out var hairMask);
      OnHairMaskOutput.Invoke(hairMask);
      return hairMask;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HairMaskCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HairSegmentationGraph>(graphPtr, packetPtr, (hairSegmentationGraph, ptr) =>
      {
        using (var packet = new ImageFramePacket(ptr, false))
        {
          if (hairSegmentationGraph.TryGetPacketValue(packet, ref hairSegmentationGraph.prevHairMaskMicrosec, out var value))
          {
            hairSegmentationGraph.OnHairMaskOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }


#if UNITY_IOS
    protected override ConfigType DetectConfigType() {
      return ConfigType.CPU;
    }
#endif

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("hair_segmentation.bytes"),
      };
    }

    protected void InitializeOutputStreams()
    {
      _hairMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _HairMaskStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      var outputRotation = imageSource.isHorizontallyFlipped ? imageSource.rotation.Reverse() : imageSource.rotation;
      sidePacket.Emplace("output_rotation", new IntPacket((int)outputRotation));

      return sidePacket;
    }
  }
}
