// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Unity.MediaPipeVideo
{
  public class MediaPipeVideoGraph : GraphRunner
  {
    public int maxNumHands = 2;

    public event EventHandler<OutputEventArgs<ImageFrame>> OnOutput
    {
      add => _outputVideoStream.AddListener(value);
      remove => _outputVideoStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private GpuBufferPacket _outputGpuBufferPacket;
    private string _destinationBufferName;
    private TextureFrame _destinationTexture;

    private const string _OutputVideoStreamName = "output_video";
    private OutputStream<ImageFramePacket, ImageFrame> _outputVideoStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (configType != ConfigType.OpenGLES)
      {
        _outputVideoStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _outputVideoStream?.Close();
      _outputVideoStream = null;
      base.Stop();
    }


    public override IEnumerator Initialize(RunningMode runningMode)
    {
      if (runningMode == RunningMode.Async)
      {
        throw new ArgumentException("Asynchronous mode is not supported");
      }
      return base.Initialize(runningMode);
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

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out ImageFrame outputVideo, bool allowBlock = true)
    {
      return TryGetNext(_outputVideoStream, out outputVideo, allowBlock, GetCurrentTimestampMicrosec());
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (configType == ConfigType.OpenGLES)
      {
        var sinkNode = config.Node.Last((node) => node.Calculator == "GlScalerCalculator");
        _destinationBufferName = Tool.GetUnusedSidePacketName(config, "destination_buffer");

        sinkNode.InputSidePacket.Add($"DESTINATION:{_destinationBufferName}");
      }

      if (runningMode == RunningMode.NonBlockingSync)
      {
        _outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(
            calculatorGraph, _OutputVideoStreamName, config.AddPacketPresenceCalculator(_OutputVideoStreamName), timeoutMicrosec);
      }
      else
      {
        _outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _OutputVideoStreamName, true, timeoutMicrosec);
      }

      return calculatorGraph.Initialize(config);
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
