// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediapipe.Unity.Sample.MediaPipeVideo
{
  public class MediaPipeVideoGraph : GraphRunner
  {
    public int maxNumHands = 2;

    private const string _InputStreamName = "input_video";

    private Packet<GpuBuffer> _outputGpuBufferPacket;
    private string _destinationBufferName;
    private Experimental.TextureFrame _destinationTexture;

    private const string _OutputVideoStreamName = "output_video";
    private OutputStream<ImageFrame> _outputVideoStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (configType != ConfigType.OpenGLES)
      {
        _outputVideoStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      _outputVideoStream?.Dispose();
      _outputVideoStream = null;
    }


    public override IEnumerator Initialize(RunningMode runningMode)
    {
      if (runningMode == RunningMode.Async)
      {
        throw new ArgumentException("Asynchronous mode is not supported");
      }
      return base.Initialize(runningMode);
    }

    public void SetupOutputPacket(Experimental.TextureFrame textureFrame, GlContext glContext)
    {
      if (configType != ConfigType.OpenGLES)
      {
        throw new InvalidOperationException("This method is only supported for OpenGL ES");
      }
      _destinationTexture = textureFrame;
      _outputGpuBufferPacket = Packet.CreateGpuBuffer(_destinationTexture.BuildGpuBuffer(glContext));
    }

    public void AddTextureFrameToInputStream(Experimental.TextureFrame textureFrame, GlContext glContext = null)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame, glContext);
    }

    public async Task<ImageFrame> WaitNextAsync()
    {
      var result = await _outputVideoStream.WaitNextAsync();
      AssertResult(result);

      _ = TryGetValue(result.packet, out var outputVideo, (packet) =>
      {
        return packet.Get();
      });
      return outputVideo;
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (configType == ConfigType.OpenGLES)
      {
        var sinkNode = config.Node.Last((node) => node.Calculator == "GlScalerCalculator");
        _destinationBufferName = Tool.GetUnusedSidePacketName(config, "destination_buffer");

        sinkNode.InputSidePacket.Add($"DESTINATION:{_destinationBufferName}");
      }

      _outputVideoStream = new OutputStream<ImageFrame>(calculatorGraph, _OutputVideoStreamName, true);

      calculatorGraph.Initialize(config);
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

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("output_rotation", Packet.CreateInt((int)imageSource.rotation));
      sidePacket.Emplace("num_hands", Packet.CreateInt(maxNumHands));

      if (configType == ConfigType.OpenGLES)
      {
        sidePacket.Emplace(_destinationBufferName, _outputGpuBufferPacket);
      }

      return sidePacket;
    }
  }
}
