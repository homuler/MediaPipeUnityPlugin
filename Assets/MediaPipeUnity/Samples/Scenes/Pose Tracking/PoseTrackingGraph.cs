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

namespace Mediapipe.Unity.PoseTracking
{
  public class PoseTrackingGraph : GraphRunner
  {
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
      Heavy = 2,
    }

    public ModelComplexity modelComplexity = ModelComplexity.Full;
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

    public event EventHandler<OutputEventArgs<NormalizedRect>> OnRoiFromLandmarksOutput
    {
      add => _roiFromLandmarksStream.AddListener(value);
      remove => _roiFromLandmarksStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _PoseDetectionStreamName = "pose_detection";
    private const string _PoseLandmarksStreamName = "pose_landmarks";
    private const string _PoseWorldLandmarksStreamName = "pose_world_landmarks";
    private const string _SegmentationMaskStreamName = "segmentation_mask";
    private const string _RoiFromLandmarksStreamName = "roi_from_landmarks";

    private OutputStream<DetectionPacket, Detection> _poseDetectionStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _poseLandmarksStream;
    private OutputStream<LandmarkListPacket, LandmarkList> _poseWorldLandmarksStream;
    private OutputStream<ImageFramePacket, ImageFrame> _segmentationMaskStream;
    private OutputStream<NormalizedRectPacket, NormalizedRect> _roiFromLandmarksStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _poseDetectionStream.StartPolling().AssertOk();
        _poseLandmarksStream.StartPolling().AssertOk();
        _poseWorldLandmarksStream.StartPolling().AssertOk();
        _segmentationMaskStream.StartPolling().AssertOk();
        _roiFromLandmarksStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _poseDetectionStream?.Close();
      _poseDetectionStream = null;
      _poseLandmarksStream?.Close();
      _poseLandmarksStream = null;
      _poseWorldLandmarksStream?.Close();
      _poseWorldLandmarksStream = null;
      _segmentationMaskStream?.Close();
      _segmentationMaskStream = null;
      _roiFromLandmarksStream?.Close();
      _roiFromLandmarksStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out Detection poseDetection, out NormalizedLandmarkList poseLandmarks, out LandmarkList poseWorldLandmarks, out ImageFrame segmentationMask, out NormalizedRect roiFromLandmarks, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_poseDetectionStream, out poseDetection, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_poseLandmarksStream, out poseLandmarks, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_poseWorldLandmarksStream, out poseWorldLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_segmentationMaskStream, out segmentationMask, allowBlock, currentTimestampMicrosec);
      var r5 = TryGetNext(_roiFromLandmarksStream, out roiFromLandmarks, allowBlock, currentTimestampMicrosec);

      return r1 || r2 || r3 || r4 || r5;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("pose_detection.bytes"),
        WaitForPoseLandmarkModel(),
      };
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(
            calculatorGraph, _PoseDetectionStreamName, config.AddPacketPresenceCalculator(_PoseDetectionStreamName), timeoutMicrosec);
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(
            calculatorGraph, _PoseLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseLandmarksStreamName), timeoutMicrosec);
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(
            calculatorGraph, _PoseWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_PoseWorldLandmarksStreamName), timeoutMicrosec);
        _segmentationMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(
            calculatorGraph, _SegmentationMaskStreamName, config.AddPacketPresenceCalculator(_SegmentationMaskStreamName), timeoutMicrosec);
        _roiFromLandmarksStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(
            calculatorGraph, _RoiFromLandmarksStreamName, config.AddPacketPresenceCalculator(_RoiFromLandmarksStreamName), timeoutMicrosec);
      }
      else
      {
        _poseDetectionStream = new OutputStream<DetectionPacket, Detection>(calculatorGraph, _PoseDetectionStreamName, true, timeoutMicrosec);
        _poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, true, timeoutMicrosec);
        _poseWorldLandmarksStream = new OutputStream<LandmarkListPacket, LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, true, timeoutMicrosec);
        _segmentationMaskStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _SegmentationMaskStreamName, true, timeoutMicrosec);
        _roiFromLandmarksStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _RoiFromLandmarksStreamName, true, timeoutMicrosec);
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

      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", new BoolPacket(smoothLandmarks));
      sidePacket.Emplace("enable_segmentation", new BoolPacket(enableSegmentation));
      sidePacket.Emplace("smooth_segmentation", new BoolPacket(smoothSegmentation));

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Enable Segmentation = {enableSegmentation}");
      Logger.LogInfo(TAG, $"Smooth Segmentation = {smoothSegmentation}");

      return sidePacket;
    }
  }
}
