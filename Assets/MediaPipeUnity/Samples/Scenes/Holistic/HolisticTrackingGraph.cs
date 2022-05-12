// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

using Google.Protobuf;

namespace Mediapipe.Unity.Holistic
{
  public class HolisticTrackingGraph : GraphRunner
  {
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
      Heavy = 2,
    }

    public bool refineFaceLandmarks = false;
    public ModelComplexity modelComplexity = ModelComplexity.Lite;
    public bool smoothLandmarks = true;
    public bool enableSegmentation = true;
    public bool smoothSegmentation = true;

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

    public event EventHandler<OutputEventArgs<Detection>> OnPoseDetectionOutput
    {
      add => _poseDetectionStream.AddListener(value);
      remove => _poseDetectionStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<NormalizedLandmarkList>> OnPoseLandmarksOutput
    {
      add => _poseLandmarksStream.AddListener(value);
      remove => _poseLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<NormalizedLandmarkList>> OnFaceLandmarksOutput
    {
      add => _faceLandmarksStream.AddListener(value);
      remove => _faceLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<NormalizedLandmarkList>> OnLeftHandLandmarksOutput
    {
      add => _leftHandLandmarksStream.AddListener(value);
      remove => _leftHandLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<NormalizedLandmarkList>> OnRightHandLandmarksOutput
    {
      add => _rightHandLandmarksStream.AddListener(value);
      remove => _rightHandLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<LandmarkList>> OnPoseWorldLandmarksOutput
    {
      add => _poseWorldLandmarksStream.AddListener(value);
      remove => _poseWorldLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<ImageFrame>> OnSegmentationMaskOutput
    {
      add => _segmentationMaskStream.AddListener(value);
      remove => _segmentationMaskStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<NormalizedRect>> OnPoseRoiOutput
    {
      add => _poseRoiStream.AddListener(value);
      remove => _poseRoiStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _PoseDetectionStreamName = "pose_detection";
    private const string _PoseLandmarksStreamName = "pose_landmarks";
    private const string _FaceLandmarksStreamName = "face_landmarks";
    private const string _LeftHandLandmarksStreamName = "left_hand_landmarks";
    private const string _RightHandLandmarksStreamName = "right_hand_landmarks";
    private const string _PoseWorldLandmarksStreamName = "pose_world_landmarks";
    private const string _SegmentationMaskStreamName = "segmentation_mask";
    private const string _PoseRoiStreamName = "pose_roi";

    private OutputStream<DetectionPacket, Detection> _poseDetectionStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _poseLandmarksStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _faceLandmarksStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _leftHandLandmarksStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _rightHandLandmarksStream;
    private OutputStream<LandmarkListPacket, LandmarkList> _poseWorldLandmarksStream;
    private OutputStream<ImageFramePacket, ImageFrame> _segmentationMaskStream;
    private OutputStream<NormalizedRectPacket, NormalizedRect> _poseRoiStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _poseDetectionStream.StartPolling().AssertOk();
        _poseLandmarksStream.StartPolling().AssertOk();
        _faceLandmarksStream.StartPolling().AssertOk();
        _leftHandLandmarksStream.StartPolling().AssertOk();
        _rightHandLandmarksStream.StartPolling().AssertOk();
        _poseWorldLandmarksStream.StartPolling().AssertOk();
        _segmentationMaskStream.StartPolling().AssertOk();
        _poseRoiStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _poseDetectionStream?.Close();
      _poseDetectionStream = null;
      _poseLandmarksStream?.Close();
      _poseLandmarksStream = null;
      _faceLandmarksStream?.Close();
      _faceLandmarksStream = null;
      _leftHandLandmarksStream?.Close();
      _leftHandLandmarksStream = null;
      _rightHandLandmarksStream?.Close();
      _rightHandLandmarksStream = null;
      _poseWorldLandmarksStream?.Close();
      _poseWorldLandmarksStream = null;
      _segmentationMaskStream?.Close();
      _segmentationMaskStream = null;
      _poseRoiStream?.Close();
      _poseRoiStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out Detection poseDetection, out NormalizedLandmarkList poseLandmarks, out NormalizedLandmarkList faceLandmarks, out NormalizedLandmarkList leftHandLandmarks,
                           out NormalizedLandmarkList rightHandLandmarks, out LandmarkList poseWorldLandmarks, out ImageFrame segmentationMask, out NormalizedRect poseRoi, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_poseDetectionStream, out poseDetection, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_poseLandmarksStream, out poseLandmarks, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_faceLandmarksStream, out faceLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_leftHandLandmarksStream, out leftHandLandmarks, allowBlock, currentTimestampMicrosec);
      var r5 = TryGetNext(_rightHandLandmarksStream, out rightHandLandmarks, allowBlock, currentTimestampMicrosec);
      var r6 = TryGetNext(_poseWorldLandmarksStream, out poseWorldLandmarks, allowBlock, currentTimestampMicrosec);
      var r7 = TryGetNext(_segmentationMaskStream, out segmentationMask, allowBlock, currentTimestampMicrosec);
      var r8 = TryGetNext(_poseRoiStream, out poseRoi, allowBlock, currentTimestampMicrosec);

      return r1 || r2 || r3 || r4 || r5 || r6 || r7 || r8;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset(refineFaceLandmarks ? "face_landmark_with_attention.bytes" : "face_landmark.bytes"),
        WaitForAsset("iris_landmark.bytes"),
        WaitForAsset("hand_landmark_full.bytes"),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
        WaitForAsset("palm_detection_full.bytes"),
        WaitForAsset("pose_detection.bytes"),
        WaitForPoseLandmarkModel(),
      };
    }

    private WaitForResult WaitForPoseLandmarkModel()
    {
      switch (modelComplexity)
      {
        case ModelComplexity.Lite: return WaitForAsset("pose_landmark_lite.bytes");
        case ModelComplexity.Full: return WaitForAsset("pose_landmark_full.bytes");
        case ModelComplexity.Heavy: return WaitForAsset("pose_landmark_heavy.bytes");
        default: throw new InternalException($"Invalid model complexity: {modelComplexity}");
      }
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(
            calculatorGraph, _PoseDetectionStreamName, config.AddPacketPresenceCalculator(_PoseDetectionStreamName), timeoutMicrosec);
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(
            calculatorGraph, _PoseLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseLandmarksStreamName), timeoutMicrosec);
        _faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(
            calculatorGraph, _FaceLandmarksStreamName, config.AddPacketPresenceCalculator(_FaceLandmarksStreamName), timeoutMicrosec);
        _leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(
            calculatorGraph, _LeftHandLandmarksStreamName, config.AddPacketPresenceCalculator(_LeftHandLandmarksStreamName), timeoutMicrosec);
        _rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(
            calculatorGraph, _RightHandLandmarksStreamName, config.AddPacketPresenceCalculator(_RightHandLandmarksStreamName), timeoutMicrosec);
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(
            calculatorGraph, _PoseWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseWorldLandmarksStreamName), timeoutMicrosec);
        _segmentationMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(
            calculatorGraph, _SegmentationMaskStreamName, config.AddPacketPresenceCalculator(_SegmentationMaskStreamName), timeoutMicrosec);
        _poseRoiStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(
            calculatorGraph, _PoseRoiStreamName, config.AddPacketPresenceCalculator(_PoseRoiStreamName), timeoutMicrosec);
      }
      else
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName, true, timeoutMicrosec);
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, true, timeoutMicrosec);
        _faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksStreamName, true, timeoutMicrosec);
        _leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _LeftHandLandmarksStreamName, true, timeoutMicrosec);
        _rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _RightHandLandmarksStreamName, true, timeoutMicrosec);
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, true, timeoutMicrosec);
        _segmentationMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _SegmentationMaskStreamName, true, timeoutMicrosec);
        _poseRoiStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _PoseRoiStreamName, true, timeoutMicrosec);
      }

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        var status = validatedGraphConfig.Initialize(config);

        if (!status.Ok()) { return status; }

        var extensionRegistry = new ExtensionRegistry() { TensorsToDetectionsCalculatorOptions.Extensions.Ext, ThresholdingCalculatorOptions.Extensions.Ext };
        var cannonicalizedConfig = validatedGraphConfig.Config(extensionRegistry);

        var poseDetectionCalculatorPattern = new Regex("__posedetection[a-z]+__TensorsToDetectionsCalculator$");
        var tensorsToDetectionsCalculators = cannonicalizedConfig.Node.Where((node) => poseDetectionCalculatorPattern.Match(node.Name).Success).ToList();

        var poseTrackingCalculatorPattern = new Regex("tensorstoposelandmarksandsegmentation__ThresholdingCalculator$");
        var thresholdingCalculators = cannonicalizedConfig.Node.Where((node) => poseTrackingCalculatorPattern.Match(node.Name).Success).ToList();

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

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);

      // TODO: refactoring
      // The orientation of the output image must match that of the input image.
      var isInverted = CoordinateSystem.ImageCoordinate.IsInverted(imageSource.rotation);
      var outputRotation = imageSource.rotation;
      var outputHorizontallyFlipped = !isInverted && imageSource.isHorizontallyFlipped;
      var outputVerticallyFlipped = (!runningMode.IsSynchronous() && imageSource.isVerticallyFlipped) ^ (isInverted && imageSource.isHorizontallyFlipped);

      if ((outputHorizontallyFlipped && outputVerticallyFlipped) || outputRotation == RotationAngle.Rotation180)
      {
        outputRotation = outputRotation.Add(RotationAngle.Rotation180);
        outputHorizontallyFlipped = !outputHorizontallyFlipped;
        outputVerticallyFlipped = !outputVerticallyFlipped;
      }

      sidePacket.Emplace("output_rotation", new IntPacket((int)outputRotation));
      sidePacket.Emplace("output_horizontally_flipped", new BoolPacket(outputHorizontallyFlipped));
      sidePacket.Emplace("output_vertically_flipped", new BoolPacket(outputVerticallyFlipped));

      Logger.LogDebug($"output_rotation = {outputRotation}, output_horizontally_flipped = {outputHorizontallyFlipped}, output_vertically_flipped = {outputVerticallyFlipped}");

      sidePacket.Emplace("refine_face_landmarks", new BoolPacket(refineFaceLandmarks));
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));
      sidePacket.Emplace("enable_segmentation", new BoolPacket(enableSegmentation));
      sidePacket.Emplace("smooth_segmentation", new BoolPacket(smoothSegmentation));

      Logger.LogInfo(TAG, $"Refine Face Landmarks = {refineFaceLandmarks}");
      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Enable Segmentation = {enableSegmentation}");
      Logger.LogInfo(TAG, $"Smooth Segmentation = {smoothSegmentation}");

      return sidePacket;
    }
  }
}
