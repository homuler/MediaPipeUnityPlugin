// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.FaceDetection
{
  public class FaceDetectionGraph : GraphRunner
  {
    public enum ModelType
    {
      ShortRange = 0,
      FullRangeSparse = 1,
    }
    public ModelType modelType = ModelType.ShortRange;
#pragma warning disable IDE1006
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";
    private const string _FaceDetectionsStreamName = "face_detections";
    private OutputStream<DetectionVectorPacket, List<Detection>> _faceDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling().AssertOk();
      }
      else
      {
        _faceDetectionsStream.AddListener(FaceDetectionsCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      _faceDetectionsStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> faceDetections, bool allowBlock = true)
    {
      if (TryGetNext(_faceDetectionsStream, out faceDetections, allowBlock, GetCurrentTimestampMicrosec()))
      {
        OnFaceDetectionsOutput.Invoke(faceDetections);
        return true;
      }
      return false;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<FaceDetectionGraph>(graphPtr, packetPtr, (faceDetectionGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (faceDetectionGraph._faceDetectionsStream.TryGetPacketValue(packet, out var value, faceDetectionGraph.timeoutMicrosec))
          {
            faceDetectionGraph.OnFaceDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_detection_full_range_sparse.bytes"),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName));
      }
      else
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, true);
      }
      return calculatorGraph.Initialize(config);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("model_type", new IntPacket((int)modelType));

      Logger.LogInfo(TAG, $"Model Selection = {modelType}");

      return sidePacket;
    }
  }
}
