using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Mediapipe.Unity.MediaPipeVideo {
  public class MediaPipeVideoGraph : GraphRunner {
    public int maxNumHands = 2;

    public UnityEvent<ImageFrame> OnOutput = new UnityEvent<ImageFrame>();

    const string inputStreamName = "input_video";

    GpuBufferPacket outputGpuBufferPacket;
    string destinationBufferName;
    TextureFrame destinationTexture;

    const string outputStreamName = "output_video";
    OutputStreamPoller<ImageFrame> outputStreamPoller;
    ImageFramePacket outputImageFramePacket;
    protected long prevOutputMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      if (configType != ConfigType.OpenGLES) {
        outputStreamPoller = calculatorGraph.AddOutputStreamPoller<ImageFrame>(outputStreamName, true).Value();
        outputImageFramePacket = new ImageFramePacket();
      }

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      if (configType != ConfigType.OpenGLES) {
        calculatorGraph.ObserveOutputStream(outputStreamName, OutputCallback, true).AssertOk();
      }

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnOutput.RemoveAllListeners();
    }

    public void SetupOutputPacket(TextureFrame textureFrame) {
      if (configType != ConfigType.OpenGLES) {
        throw new InvalidOperationException("This method is only supported for OpenGL ES");
      }
      destinationTexture = textureFrame;
      outputGpuBufferPacket = new GpuBufferPacket(destinationTexture.BuildGpuBuffer(GpuManager.glCalculatorHelper.GetGlContext()));
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public ImageFrame FetchNextValue() {
      FetchNext(outputStreamPoller, outputImageFramePacket, out var outputVideo, outputStreamName);
      OnOutput.Invoke(outputVideo);
      return outputVideo;
    }

    public ImageFrame FetchLatestValue() {
      FetchLatest(outputStreamPoller, outputImageFramePacket, out var outputVideo, outputStreamName);
      OnOutput.Invoke(outputVideo);
      return outputVideo;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr OutputCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<MediaPipeVideoGraph>(graphPtr, packetPtr, (mediaPipeVideoGraph, ptr) => {
        using (var packet = new ImageFramePacket(ptr, false)) {
          if (mediaPipeVideoGraph.TryConsumePacketValue(packet, ref mediaPipeVideoGraph.prevOutputMicrosec, out var value)) {
            mediaPipeVideoGraph.OnOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override CalculatorGraphConfig GetCalculatorGraphConfig() {
      var calculatorGraphConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);

      if (configType == ConfigType.OpenGLES) {
        var sinkNode = calculatorGraphConfig.Node.Last((node) => node.Calculator == "GlScalerCalculator");
        destinationBufferName = Tool.GetUnusedSidePacketName(calculatorGraphConfig, "destination_buffer");

        sinkNode.InputSidePacket.Add($"DESTINATION:{destinationBufferName}");
      }

      return calculatorGraphConfig;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("hand_landmark.bytes"),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForAsset("palm_detection.bytes"),
      };
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

      if (configType == ConfigType.OpenGLES) {
        sidePacket.Emplace(destinationBufferName, outputGpuBufferPacket);
      }

      // Coordinate transformation from Unity to MediaPipe
      // Filps the input image if it's **not** mirrored, because MediaPipe assumes that the the input is vertically flipped,
      if (imageSource.isMirrored) {
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      } else {
        sidePacket.Emplace("input_rotation", new IntPacket(180));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(false));
      }

      return sidePacket;
    }
  }
}
