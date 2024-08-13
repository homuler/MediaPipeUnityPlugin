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
using System.Threading.Tasks;

namespace Mediapipe.Unity.Sample.Holistic
{
  public readonly struct HolisticTrackingResult
  {
    public readonly Detection poseDetection;
    public readonly NormalizedLandmarkList poseLandmarks;
    public readonly NormalizedLandmarkList faceLandmarks;
    public readonly NormalizedLandmarkList leftHandLandmarks;
    public readonly NormalizedLandmarkList rightHandLandmarks;
    public readonly LandmarkList poseWorldLandmarks;
    public readonly ImageFrame segmentationMask;
    public readonly NormalizedRect poseRoi;

    public HolisticTrackingResult(Detection poseDetection, NormalizedLandmarkList poseLandmarks, NormalizedLandmarkList faceLandmarks, NormalizedLandmarkList leftHandLandmarks,
                                  NormalizedLandmarkList rightHandLandmarks, LandmarkList poseWorldLandmarks, ImageFrame segmentationMask, NormalizedRect poseRoi)
    {
      this.poseDetection = poseDetection;
      this.poseLandmarks = poseLandmarks;
      this.faceLandmarks = faceLandmarks;
      this.leftHandLandmarks = leftHandLandmarks;
      this.rightHandLandmarks = rightHandLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.segmentationMask = segmentationMask;
      this.poseRoi = poseRoi;
    }
  }

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

    public event EventHandler<OutputStream<Detection>.OutputEventArgs> OnPoseDetectionOutput
    {
      add => _poseDetectionStream.AddListener(value, timeoutMicrosec);
      remove => _poseDetectionStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<NormalizedLandmarkList>.OutputEventArgs> OnPoseLandmarksOutput
    {
      add => _poseLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _poseLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<NormalizedLandmarkList>.OutputEventArgs> OnFaceLandmarksOutput
    {
      add => _faceLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _faceLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<NormalizedLandmarkList>.OutputEventArgs> OnLeftHandLandmarksOutput
    {
      add => _leftHandLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _leftHandLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<NormalizedLandmarkList>.OutputEventArgs> OnRightHandLandmarksOutput
    {
      add => _rightHandLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _rightHandLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<LandmarkList>.OutputEventArgs> OnPoseWorldLandmarksOutput
    {
      add => _poseWorldLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _poseWorldLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<ImageFrame>.OutputEventArgs> OnSegmentationMaskOutput
    {
      add => _segmentationMaskStream.AddListener(value, timeoutMicrosec);
      remove => _segmentationMaskStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<NormalizedRect>.OutputEventArgs> OnPoseRoiOutput
    {
      add => _poseRoiStream.AddListener(value, timeoutMicrosec);
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

    private OutputStream<Detection> _poseDetectionStream;
    private OutputStream<NormalizedLandmarkList> _poseLandmarksStream;
    private OutputStream<NormalizedLandmarkList> _faceLandmarksStream;
    private OutputStream<NormalizedLandmarkList> _leftHandLandmarksStream;
    private OutputStream<NormalizedLandmarkList> _rightHandLandmarksStream;
    private OutputStream<LandmarkList> _poseWorldLandmarksStream;
    private OutputStream<ImageFrame> _segmentationMaskStream;
    private OutputStream<NormalizedRect> _poseRoiStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _poseDetectionStream.StartPolling();
        _poseLandmarksStream.StartPolling();
        _faceLandmarksStream.StartPolling();
        _leftHandLandmarksStream.StartPolling();
        _rightHandLandmarksStream.StartPolling();
        _poseWorldLandmarksStream.StartPolling();
        _segmentationMaskStream.StartPolling();
        _poseRoiStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      _poseDetectionStream?.Dispose();
      _poseDetectionStream = null;
      _poseLandmarksStream?.Dispose();
      _poseLandmarksStream = null;
      _faceLandmarksStream?.Dispose();
      _faceLandmarksStream = null;
      _leftHandLandmarksStream?.Dispose();
      _leftHandLandmarksStream = null;
      _rightHandLandmarksStream?.Dispose();
      _rightHandLandmarksStream = null;
      _poseWorldLandmarksStream?.Dispose();
      _poseWorldLandmarksStream = null;
      _segmentationMaskStream?.Dispose();
      _segmentationMaskStream = null;
      _poseRoiStream?.Dispose();
      _poseRoiStream = null;
    }

    public void AddTextureFrameToInputStream(Experimental.TextureFrame textureFrame, GlContext glContext = null)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame, glContext);
    }

    public async Task<HolisticTrackingResult> WaitNextAsync()
    {
      var results = await WhenAll(
        _poseDetectionStream.WaitNextAsync(),
        _poseLandmarksStream.WaitNextAsync(),
        _faceLandmarksStream.WaitNextAsync(),
        _leftHandLandmarksStream.WaitNextAsync(),
        _rightHandLandmarksStream.WaitNextAsync(),
        _poseWorldLandmarksStream.WaitNextAsync(),
        _segmentationMaskStream.WaitNextAsync(),
        _poseRoiStream.WaitNextAsync()
      );
      AssertResult(results);

      _ = TryGetValue(results.Item1.packet, out var poseDetection, (packet) =>
      {
        return packet.Get(Detection.Parser);
      });
      _ = TryGetValue(results.Item2.packet, out var poseLandmarks, (packet) =>
      {
        return packet.Get(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results.Item3.packet, out var faceLandmarks, (packet) =>
      {
        return packet.Get(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results.Item4.packet, out var leftHandLandmarks, (packet) =>
      {
        return packet.Get(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results.Item5.packet, out var rightHandLandmarks, (packet) =>
      {
        return packet.Get(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results.Item6.packet, out var poseWorldLandmarks, (packet) =>
      {
        return packet.Get(LandmarkList.Parser);
      });
      _ = TryGetValue(results.Item7.packet, out var segmentationMask, (packet) =>
      {
        return packet.Get();
      });
      _ = TryGetValue(results.Item8.packet, out var poseRoi, (packet) =>
      {
        return packet.Get(NormalizedRect.Parser);
      });

      return new HolisticTrackingResult(poseDetection, poseLandmarks, faceLandmarks, leftHandLandmarks, rightHandLandmarks, poseWorldLandmarks, segmentationMask, poseRoi);
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

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      _poseDetectionStream = new OutputStream<Detection>(calculatorGraph, _PoseDetectionStreamName, true);
      _poseLandmarksStream = new OutputStream<NormalizedLandmarkList>(calculatorGraph, _PoseLandmarksStreamName, true);
      _faceLandmarksStream = new OutputStream<NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksStreamName, true);
      _leftHandLandmarksStream = new OutputStream<NormalizedLandmarkList>(calculatorGraph, _LeftHandLandmarksStreamName, true);
      _rightHandLandmarksStream = new OutputStream<NormalizedLandmarkList>(calculatorGraph, _RightHandLandmarksStreamName, true);
      _poseWorldLandmarksStream = new OutputStream<LandmarkList>(calculatorGraph, _PoseWorldLandmarksStreamName, true);
      _segmentationMaskStream = new OutputStream<ImageFrame>(calculatorGraph, _SegmentationMaskStreamName, true);
      _poseRoiStream = new OutputStream<NormalizedRect>(calculatorGraph, _PoseRoiStreamName, true);

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        validatedGraphConfig.Initialize(config);

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
            Debug.Log($"Min Detection Confidence = {minDetectionConfidence}");
          }
        }

        foreach (var calculator in thresholdingCalculators)
        {
          if (calculator.Options.HasExtension(ThresholdingCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(ThresholdingCalculatorOptions.Extensions.Ext);
            options.Threshold = minTrackingConfidence;
            Debug.Log($"Min Tracking Confidence = {minTrackingConfidence}");
          }
        }
        calculatorGraph.Initialize(cannonicalizedConfig);
      }
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();

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

      sidePacket.Emplace("output_rotation", Packet.CreateInt((int)outputRotation));
      sidePacket.Emplace("output_horizontally_flipped", Packet.CreateBool(outputHorizontallyFlipped));
      sidePacket.Emplace("output_vertically_flipped", Packet.CreateBool(outputVerticallyFlipped));

      Debug.Log($"outtput_rotation = {outputRotation}, output_horizontally_flipped = {outputHorizontallyFlipped}, output_vertically_flipped = {outputVerticallyFlipped}");

      sidePacket.Emplace("refine_face_landmarks", Packet.CreateBool(refineFaceLandmarks));
      sidePacket.Emplace("model_complexity", Packet.CreateInt((int)modelComplexity));
      sidePacket.Emplace("smooth_landmarks", Packet.CreateBool(smoothLandmarks));
      sidePacket.Emplace("enable_segmentation", Packet.CreateBool(enableSegmentation));
      sidePacket.Emplace("smooth_segmentation", Packet.CreateBool(smoothSegmentation));

      Debug.Log($"Refine Face Landmarks = {refineFaceLandmarks}");
      Debug.Log($"Model Complexity = {modelComplexity}");
      Debug.Log($"Smooth Landmarks = {smoothLandmarks}");
      Debug.Log($"Enable Segmentation = {enableSegmentation}");
      Debug.Log($"Smooth Segmentation = {smoothSegmentation}");

      return sidePacket;
    }
  }
}
