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

    const string outputVideoStreamName = "output_video";
    OutputStream<ImageFramePacket, ImageFrame> outputVideoStream;
    protected long prevOutputVideoMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      if (configType != ConfigType.OpenGLES) {
        InitializeOutputStreams();
        outputVideoStream.StartPolling(true).AssertOk();
      }
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      if (configType != ConfigType.OpenGLES) {
        InitializeOutputStreams();
        outputVideoStream.AddListener(OutputVideoCallback, true).AssertOk();
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
      outputVideoStream.TryGetNext(out var outputVideo);
      OnOutput.Invoke(outputVideo);
      return outputVideo;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr OutputVideoCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<MediaPipeVideoGraph>(graphPtr, packetPtr, (mediaPipeVideoGraph, ptr) => {
        using (var packet = new ImageFramePacket(ptr, false)) {
          if (mediaPipeVideoGraph.TryConsumePacketValue(packet, ref mediaPipeVideoGraph.prevOutputVideoMicrosec, out var value)) {
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

    protected void InitializeOutputStreams() {
      outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, outputVideoStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("output_rotation", new IntPacket((int)imageSource.rotation));
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

      if (configType == ConfigType.OpenGLES) {
        sidePacket.Emplace(destinationBufferName, outputGpuBufferPacket);
      }

      return sidePacket;
    }
  }
}
