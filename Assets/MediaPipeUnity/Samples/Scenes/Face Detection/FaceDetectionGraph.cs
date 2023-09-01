// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity.Sample.FaceDetection
{
  public class FaceDetectionGraph : GraphRunner
  {
    public enum ModelType
    {
      ShortRange = 0,
      FullRangeSparse = 1,
    }
    public ModelType modelType = ModelType.ShortRange;

    private float _minDetectionConfidence = 0.5f;
    public float minDetectionConfidence
    {
      get => _minDetectionConfidence;
      set => _minDetectionConfidence = Mathf.Clamp01(value);
    }

    public event EventHandler<OutputEventArgs<List<Detection>>> OnFaceDetectionsOutput
    {
      add => _faceDetectionsStream.AddListener(value);
      remove => _faceDetectionsStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _FaceDetectionsStreamName = "face_detections";
    private OutputStream<DetectionVectorPacket, List<Detection>> _faceDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _faceDetectionsStream?.Close();
      _faceDetectionsStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> faceDetections, bool allowBlock = true)
    {
      return TryGetNext(_faceDetectionsStream, out faceDetections, allowBlock, GetCurrentTimestampMicrosec());
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_detection_full_range_sparse.bytes"),
      };
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(
            calculatorGraph, _FaceDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName), timeoutMicrosec);
      }
      else
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, true, timeoutMicrosec);
      }

      var faceDetectionCalculators = config.Node.Where((node) => node.Calculator.StartsWith("FaceDetection")).ToList();
      foreach (var calculator in faceDetectionCalculators)
      {
        var calculatorOptions = new CalculatorOptions();
        calculatorOptions.SetExtension(FaceDetectionOptions.Extensions.Ext, new FaceDetectionOptions { MinScoreThresh = minDetectionConfidence });
        calculator.Options = calculatorOptions;
        Logger.LogInfo(TAG, $"Min Detection Confidence ({calculator.Calculator}) = {minDetectionConfidence}");
      }

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        validatedGraphConfig.Initialize(config);
        calculatorGraph.Initialize(config);
      }
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("model_type", new IntPacket((int)modelType));

      Logger.LogInfo(TAG, $"Model Selection = {modelType}");

      return sidePacket;
    }
  }
}
