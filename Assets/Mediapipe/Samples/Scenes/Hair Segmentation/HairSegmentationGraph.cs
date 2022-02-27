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
      else
      {
        _hairMaskStream.AddListener(HairMaskCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }


    public override void Stop()
    {
      base.Stop();
      OnHairMaskOutput.RemoveAllListeners();
      _hairMaskStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out ImageFrame hairMask, bool allowBlock = true)
    {
      if (TryGetNext(_hairMaskStream, out hairMask, allowBlock, GetCurrentTimestampMicrosec()))
      {
        OnHairMaskOutput.Invoke(hairMask);
        return true;
      }
      return false;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr HairMaskCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<HairSegmentationGraph>(graphPtr, packetPtr, (hairSegmentationGraph, ptr) =>
      {
        using (var packet = new ImageFramePacket(ptr, false))
        {
          if (hairSegmentationGraph._hairMaskStream.TryGetPacketValue(packet, out var value, hairSegmentationGraph.timeoutMicrosec))
          {
            hairSegmentationGraph.OnHairMaskOutput.Invoke(value);
          }
        }
      }).mpPtr;
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
        _hairMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _HairMaskStreamName, config.AddPacketPresenceCalculator(_HairMaskStreamName));
      }
      else
      {
        _hairMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _HairMaskStreamName);
      }
      return calculatorGraph.Initialize(config);
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
