using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.HairSegmentation {
  public class HairSegmentationGraph : GraphRunner {
    public UnityEvent<ImageFrame> OnHairMaskOutput = new UnityEvent<ImageFrame>();

    const string inputStreamName = "input_video";

    const string hairMaskStreamName = "hair_mask";
    OutputStream<ImageFramePacket, ImageFrame> hairMaskStream;
    protected long prevHairMaskMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();
      hairMaskStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();
      hairMaskStream.AddListener(HairMaskCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnHairMaskOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public ImageFrame FetchNextValue() {
      hairMaskStream.TryGetNext(out var hairMask);
      OnHairMaskOutput.Invoke(hairMask);
      return hairMask;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr HairMaskCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<HairSegmentationGraph>(graphPtr, packetPtr, (hairSegmentationGraph, ptr) => {
        using (var packet = new ImageFramePacket(ptr, false)) {
          if (hairSegmentationGraph.TryGetPacketValue(packet, ref hairSegmentationGraph.prevHairMaskMicrosec, out var value)) {
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

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("hair_segmentation.bytes"),
      };
    }

    protected void InitializeOutputStreams() {
      hairMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, hairMaskStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
