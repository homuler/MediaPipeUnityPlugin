// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Google.Protobuf;

namespace Mediapipe.Unity.FaceMesh
{
  public class FaceMeshGraph : GraphRunner
  {
    public int maxNumFaces = 1;
    public bool refineLandmarks = true;

    private float _minDetectionConfidence = 0.5f;
    public float minDetectionConfidence
    {
      get => _minDetectionConfidence;
      set => _minDetectionConfidence = Mathf.Clamp01(value);
    }

    private float _minTrackingConfidence = 0.5f;
    public float minTrackingConfidence
    {
      get => _minTrackingConfidence;
      set => _minTrackingConfidence = Mathf.Clamp01(value);
    }

    public event EventHandler<OutputEventArgs<List<Detection>>> OnFaceDetectionsOutput
    {
      add => _faceDetectionsStream.AddListener(value);
      remove => _faceDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedLandmarkList>>> OnMultiFaceLandmarksOutput
    {
      add => _multiFaceLandmarksStream.AddListener(value);
      remove => _multiFaceLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedRect>>> OnFaceRectsFromLandmarksOutput
    {
      add => _faceRectsFromLandmarksStream.AddListener(value);
      remove => _faceRectsFromLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedRect>>> OnFaceRectsFromDetectionsOutput
    {
      add => _faceRectsFromDetectionsStream.AddListener(value);
      remove => _faceRectsFromDetectionsStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private const string _FaceDetectionsStreamName = "face_detections";
    private const string _MultiFaceLandmarksStreamName = "multi_face_landmarks";
    private const string _FaceRectsFromLandmarksStreamName = "face_rects_from_landmarks";
    private const string _FaceRectsFromDetectionsStreamName = "face_rects_from_detections";

    private OutputStream<DetectionVectorPacket, List<Detection>> _faceDetectionsStream;
    private OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> _multiFaceLandmarksStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _faceRectsFromLandmarksStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _faceRectsFromDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling().AssertOk();
        _multiFaceLandmarksStream.StartPolling().AssertOk();
        _faceRectsFromLandmarksStream.StartPolling().AssertOk();
        _faceRectsFromDetectionsStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _faceDetectionsStream?.Close();
      _faceDetectionsStream = null;
      _multiFaceLandmarksStream?.Close();
      _multiFaceLandmarksStream = null;
      _faceRectsFromLandmarksStream?.Close();
      _faceRectsFromLandmarksStream = null;
      _faceRectsFromDetectionsStream?.Close();
      _faceRectsFromDetectionsStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> faceDetections, out List<NormalizedLandmarkList> multiFaceLandmarks,
                           out List<NormalizedRect> faceRectsFromLandmarks, out List<NormalizedRect> faceRectsFromDetections, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_faceDetectionsStream, out faceDetections, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_multiFaceLandmarksStream, out multiFaceLandmarks, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_faceRectsFromLandmarksStream, out faceRectsFromLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_faceRectsFromDetectionsStream, out faceRectsFromDetections, allowBlock, currentTimestampMicrosec);

      return r1 || r2 || r3 || r4;
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(
            calculatorGraph, _FaceDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName), timeoutMicrosec);
        _multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(
            calculatorGraph, _MultiFaceLandmarksStreamName, config.AddPacketPresenceCalculator(_MultiFaceLandmarksStreamName), timeoutMicrosec);
        _faceRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(
            calculatorGraph, _FaceRectsFromLandmarksStreamName, config.AddPacketPresenceCalculator(_FaceRectsFromLandmarksStreamName), timeoutMicrosec);
        _faceRectsFromDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(
            calculatorGraph, _FaceRectsFromDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName), timeoutMicrosec);
      }
      else
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, true, timeoutMicrosec);
        _multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiFaceLandmarksStreamName, true, timeoutMicrosec);
        _faceRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromLandmarksStreamName, true, timeoutMicrosec);
        _faceRectsFromDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromDetectionsStreamName, true, timeoutMicrosec);
      }

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        var status = validatedGraphConfig.Initialize(config);

        if (!status.Ok()) { return status; }

        var extensionRegistry = new ExtensionRegistry() { TensorsToDetectionsCalculatorOptions.Extensions.Ext, ThresholdingCalculatorOptions.Extensions.Ext };
        var cannonicalizedConfig = validatedGraphConfig.Config(extensionRegistry);
        var tensorsToDetectionsCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "TensorsToDetectionsCalculator").ToList();
        var thresholdingCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "ThresholdingCalculator").ToList();

        foreach (var calculator in tensorsToDetectionsCalculators)
        {
          if (calculator.Options.HasExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext);
            options.MinScoreThresh = minDetectionConfidence;
            Logger.LogInfo(TAG, $"Min Detection Confidence = {minDetectionConfidence}");
          }
        }

        foreach (var calculator in thresholdingCalculators)
        {
          if (calculator.Options.HasExtension(ThresholdingCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(ThresholdingCalculatorOptions.Extensions.Ext);
            options.Threshold = minTrackingConfidence;
            Logger.LogInfo(TAG, $"Min Tracking Confidence = {minTrackingConfidence}");
          }
        }
        return calculatorGraph.Initialize(cannonicalizedConfig);
      }
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset(refineLandmarks ? "face_landmark_with_attention.bytes" : "face_landmark.bytes"),
      };
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("num_faces", new IntPacket(maxNumFaces));
      sidePacket.Emplace("with_attention", new BoolPacket(refineLandmarks));

      Logger.LogInfo(TAG, $"Max Num Faces = {maxNumFaces}");
      Logger.LogInfo(TAG, $"Refine Landmarks = {refineLandmarks}");

      return sidePacket;
    }
  }
}
