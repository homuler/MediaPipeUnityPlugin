// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace Mediapipe.Unity.MediaPipeVideo
{
  public class MediaPipeVideoGraph : GraphRunner
  {
    public int maxNumHands = 2;
#pragma warning disable IDE1006
    public UnityEvent<ImageFrame> OnOutput = new UnityEvent<ImageFrame>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private GpuBufferPacket _outputGpuBufferPacket;
    private string _destinationBufferName;
    private TextureFrame _destinationTexture;

    private const string _OutputVideoStreamName = "output_video";
    private OutputStream<ImageFramePacket, ImageFrame> _outputVideoStream;
    protected long prevOutputVideoMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      if (configType != ConfigType.OpenGLES)
      {
        InitializeOutputStreams();
        _outputVideoStream.StartPolling(true).AssertOk();
      }
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      if (configType != ConfigType.OpenGLES)
      {
        InitializeOutputStreams();
        _outputVideoStream.AddListener(OutputVideoCallback, true).AssertOk();
      }
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnOutput.RemoveAllListeners();
    }

    public void SetupOutputPacket(TextureFrame textureFrame)
    {
      if (configType != ConfigType.OpenGLES)
      {
        throw new InvalidOperationException("This method is only supported for OpenGL ES");
      }
      _destinationTexture = textureFrame;
      _outputGpuBufferPacket = new GpuBufferPacket(_destinationTexture.BuildGpuBuffer(GpuManager.GlCalculatorHelper.GetGlContext()));
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public ImageFrame FetchNextValue()
    {
      var _ = _outputVideoStream.TryGetNext(out var outputVideo);
      OnOutput.Invoke(outputVideo);
      return outputVideo;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr OutputVideoCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<MediaPipeVideoGraph>(graphPtr, packetPtr, (mediaPipeVideoGraph, ptr) =>
      {
        using (var packet = new ImageFramePacket(ptr, false))
        {
          if (mediaPipeVideoGraph.TryConsumePacketValue(packet, ref mediaPipeVideoGraph.prevOutputVideoMicrosec, out var value))
          {
            mediaPipeVideoGraph.OnOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override CalculatorGraphConfig GetCalculatorGraphConfig()
    {
      var calculatorGraphConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);

      if (configType == ConfigType.OpenGLES)
      {
        var sinkNode = calculatorGraphConfig.Node.Last((node) => node.Calculator == "GlScalerCalculator");
        _destinationBufferName = Tool.GetUnusedSidePacketName(calculatorGraphConfig, "destination_buffer");

        sinkNode.InputSidePacket.Add($"DESTINATION:{_destinationBufferName}");
      }

      return calculatorGraphConfig;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("hand_landmark_full.bytes"),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForAsset("palm_detection_full.bytes"),
      };
    }

    protected void InitializeOutputStreams()
    {
      _outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _OutputVideoStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("output_rotation", new IntPacket((int)imageSource.rotation));
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));

      if (configType == ConfigType.OpenGLES)
      {
        sidePacket.Emplace(_destinationBufferName, _outputGpuBufferPacket);
      }

      return sidePacket;
    }
  }
}
