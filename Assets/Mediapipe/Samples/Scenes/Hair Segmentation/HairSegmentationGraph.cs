using System;
using UnityEngine.Events;

namespace Mediapipe.Unity.HairSegmentation {
  public class HairSegmentationGraph : GraphRunner {
    public UnityEvent<ImageFrame> OnHairMaskOutput = new UnityEvent<ImageFrame>();

    const string inputStreamName = "input_video";

    const string hairMaskStreamName = "hair_mask_cpu";
    OutputStreamPoller<ImageFrame> hairMaskStreamPoller;
    ImageFramePacket hairMaskPacket;

    public override Status StartRun(ImageSource imageSource) {
      hairMaskStreamPoller = calculatorGraph.AddOutputStreamPoller<ImageFrame>(hairMaskStreamName).Value();
      hairMaskPacket = new ImageFramePacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(hairMaskStreamName, HairMaskCallback, true).AssertOk();
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
      var hairMask = FetchNext(hairMaskStreamPoller, hairMaskPacket, hairMaskStreamName);
      OnHairMaskOutput.Invoke(hairMask);
      return hairMask;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr HairMaskCallback(IntPtr graphPtr, IntPtr packetPtr){
      try {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound) {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new ImageFramePacket(packetPtr, false)) {
          var value = packet.IsEmpty() ? null : packet.Get();
          (graphRunner as HairSegmentationGraph).OnHairMaskOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      } catch (Exception e) {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    protected override void PrepareDependentAssets() {
      AssetLoader.PrepareAsset("hair_segmentation.bytes");
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();

      // Coordinate transformation from Unity to MediaPipe
      if (imageSource.isMirrored) {
        sidePacket.Emplace("input_rotation", new IntPacket(180));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(false));
      } else {
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      }

      return sidePacket;
    }
  }
}
