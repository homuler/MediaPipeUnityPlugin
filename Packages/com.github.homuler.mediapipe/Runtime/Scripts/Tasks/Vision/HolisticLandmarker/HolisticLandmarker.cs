// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.HolisticLandmarker
{
  public sealed class HolisticLandmarker : Core.BaseVisionTaskApi
  {
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";

    private const string _POSE_LANDMARKS_STREAM_NAME = "pose_landmarks";
    private const string _POSE_LANDMARKS_TAG = "POSE_LANDMARKS";
    private const string _POSE_WORLD_LANDMARKS_STREAM_NAME = "pose_world_landmarks";
    private const string _POSE_WORLD_LANDMARKS_TAG = "POSE_WORLD_LANDMARKS";
    private const string _POSE_SEGMENTATION_MASK_STREAM_NAME = "pose_segmentation_mask";
    private const string _POSE_SEGMENTATION_MASK_TAG = "POSE_SEGMENTATION_MASK";
    private const string _FACE_LANDMARKS_STREAM_NAME = "face_landmarks";
    private const string _FACE_LANDMARKS_TAG = "FACE_LANDMARKS";
    private const string _FACE_BLENDSHAPES_STREAM_NAME = "extra_blendshapes";
    private const string _FACE_BLENDSHAPES_TAG = "FACE_BLENDSHAPES";
    private const string _LEFT_HAND_LANDMARKS_STREAM_NAME = "left_hand_landmarks";
    private const string _LEFT_HAND_LANDMARKS_TAG = "LEFT_HAND_LANDMARKS";
    private const string _LEFT_HAND_WORLD_LANDMARKS_STREAM_NAME = "left_hand_world_landmarks";
    private const string _LEFT_HAND_WORLD_LANDMARKS_TAG = "LEFT_HAND_WORLD_LANDMARKS";
    private const string _RIGHT_HAND_LANDMARKS_STREAM_NAME = "right_hand_landmarks";
    private const string _RIGHT_HAND_LANDMARKS_TAG = "RIGHT_HAND_LANDMARKS";
    private const string _RIGHT_HAND_WORLD_LANDMARKS_STREAM_NAME = "right_hand_world_landmarks";
    private const string _RIGHT_HAND_WORLD_LANDMARKS_TAG = "RIGHT_HAND_WORLD_LANDMARKS";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.holistic_landmarker.HolisticLandmarkerGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private HolisticLandmarker(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      GpuResources gpuResources,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, gpuResources, packetCallback)
    {
      _packetCallback = packetCallback;
    }

    /// <summary>
    ///   Creates an <see cref="HolisticLandmarker" /> object from a TensorFlow Lite model and the default <see cref="PoseLandmarkerOptions" />.
    ///
    ///   Note that the created <see cref="HolisticLandmarker" /> instance is in image mode,
    ///   for detecting pose landmarks on single image inputs.
    /// </summary>
    /// <param name="modelPath">Path to the model.</param>
    /// <param name="gpuResources">
    ///   <see cref="GpuResources"/> to set to the underlying <see cref="CalculatorGraph"/>.
    ///   To share the GL context with MediaPipe, <see cref="GlCalculatorHelper.InitializeForTest"/> must be called with it.
    /// </param>
    /// <returns>
    ///   <see cref="HolisticLandmarker" /> object that's created from the model and the default <see cref="HolisticLandmarkerOptions" />.
    /// </returns>
    public static HolisticLandmarker CreateFromModelPath(string modelPath, GpuResources gpuResources = null)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new HolisticLandmarkerOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options, gpuResources);
    }

    /// <summary>
    ///   Creates the <see cref="HolisticLandmarker" /> object from <paramref name="HolisticLandmarkerOptions" />.
    /// </summary>
    /// <param name="options">Options for the holistic landmarker task.</param>
    /// <param name="gpuResources">
    ///   <see cref="GpuResources"/> to set to the underlying <see cref="CalculatorGraph"/>.
    ///   To share the GL context with MediaPipe, <see cref="GlCalculatorHelper.InitializeForTest"/> must be called with it.
    /// </param>
    /// <returns>
    ///   <see cref="HolisticLandmarker" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static HolisticLandmarker CreateFromOptions(HolisticLandmarkerOptions options, GpuResources gpuResources = null)
    {
      var outputStreams = new List<string> {
        string.Join(":", _FACE_LANDMARKS_TAG, _FACE_LANDMARKS_STREAM_NAME),
        string.Join(":", _POSE_LANDMARKS_TAG, _POSE_LANDMARKS_STREAM_NAME),
        string.Join(":", _POSE_WORLD_LANDMARKS_TAG, _POSE_WORLD_LANDMARKS_STREAM_NAME),
        string.Join(":", _LEFT_HAND_LANDMARKS_TAG, _LEFT_HAND_LANDMARKS_STREAM_NAME),
        string.Join(":", _LEFT_HAND_WORLD_LANDMARKS_TAG, _LEFT_HAND_WORLD_LANDMARKS_STREAM_NAME),
        string.Join(":", _RIGHT_HAND_LANDMARKS_TAG, _RIGHT_HAND_LANDMARKS_STREAM_NAME),
        string.Join(":", _RIGHT_HAND_WORLD_LANDMARKS_TAG, _RIGHT_HAND_WORLD_LANDMARKS_STREAM_NAME),
        string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
      };
      if (options.outputSegmentationMask)
      {
        outputStreams.Add(string.Join(":", _POSE_SEGMENTATION_MASK_TAG, _POSE_SEGMENTATION_MASK_STREAM_NAME));
      }
      if (options.outputFaceBlendshapes)
      {
        outputStreams.Add(string.Join(":", _FACE_BLENDSHAPES_TAG, _FACE_BLENDSHAPES_STREAM_NAME));
      }

      var taskInfo = new Tasks.Core.TaskInfo<HolisticLandmarkerOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
        },
        outputStreams: outputStreams,
        taskOptions: options);

      return new HolisticLandmarker(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        gpuResources,
        BuildPacketsCallback(options));
    }

    /// <summary>
    ///   Performs holistic landmarks detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="HolisticLandmarker" /> is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <returns>
    ///   The holistic landmarks detection results.
    /// </returns>
    public HolisticLandmarkerResult Detect(Image image)
    {
      using var outputPackets = DetectInternal(image);

      var result = default(HolisticLandmarkerResult);
      _ = TryBuildHolisticLandmarkerResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs holistic landmarks detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="HolisticLandmarker" /> is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="result">
    ///   <see cref="HolisticLandmarkerResult"/> to which the result will be written.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if some faces are detected, <see langword="false"/> otherwise.
    /// </returns>
    public bool TryDetect(Image image, ref HolisticLandmarkerResult result)
    {
      using var outputPackets = DetectInternal(image);
      return TryBuildHolisticLandmarkerResult(outputPackets, ref result);
    }

    private PacketMap DetectInternal(Image image)
    {
      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImage(image));

      return ProcessImageData(packetMap);
    }

    /// <summary>
    ///   Performs holistic landmarks detection on the provided video frames.
    ///
    ///   Only use this method when the HolisticLandmarker is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <returns>
    ///   The holistic landmarks detection results.
    /// </returns>
    public HolisticLandmarkerResult DetectForVideo(Image image, long timestampMillisec)
    {
      using var outputPackets = DetectForVideoInternal(image, timestampMillisec);

      var result = default(HolisticLandmarkerResult);
      _ = TryBuildHolisticLandmarkerResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs holistic landmarks detection on the provided video frames.
    ///
    ///   Only use this method when the HolisticLandmarker is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <param name="result">
    ///   <see cref="HolisticLandmarkerResult"/> to which the result will be written.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if some poses are detected, <see langword="false"/> otherwise.
    /// </returns>
    public bool TryDetectForVideo(Image image, long timestampMillisec, ref HolisticLandmarkerResult result)
    {
      using var outputPackets = DetectForVideoInternal(image, timestampMillisec);
      return TryBuildHolisticLandmarkerResult(outputPackets, ref result);
    }

    private PacketMap DetectForVideoInternal(Image image, long timestampMillisec)
    {
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));

      return ProcessVideoData(packetMap);
    }

    /// <summary>
    ///   Sends live image data to perform holistic landmarks detection.
    ///
    ///   Only use this method when the HolisticLandmarker is created with the live stream
    ///   running mode. The input timestamps should be monotonically increasing for
    ///   adjacent calls of this method. This method will return immediately after the
    ///   input image is accepted. The results will be available via the
    ///   <see cref="HolisticLandmarkerOptions.ResultCallback" /> provided in the <see cref="HolisticLandmarkerOptions" />.
    ///   The <see cref="DetectAsync" /> method is designed to process live stream data such as camera
    ///   input. To lower the overall latency, pose landmarker may drop the input
    ///   images if needed. In other words, it's not guaranteed to have output per
    ///   input image.
    public void DetectAsync(Image image, long timestampMillisec)
    {
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));

      SendLiveStreamData(packetMap);
    }

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(HolisticLandmarkerOptions options)
    {
      var resultCallback = options.resultCallback;
      if (resultCallback == null)
      {
        return null;
      }

      var lockObj = new object();
      var holisticLandmarkerResult = new HolisticLandmarkerResult();

      return (PacketMap outputPackets) =>
      {
        using var outImagePacket = outputPackets.At<Image>(_IMAGE_OUT_STREAM_NAME);
        if (outImagePacket == null || outImagePacket.IsEmpty())
        {
          return;
        }

        using var image = outImagePacket.Get();
        var timestamp = outImagePacket.TimestampMicroseconds() / _MICRO_SECONDS_PER_MILLISECOND;

        lock (lockObj)
        {
          if (TryBuildHolisticLandmarkerResult(outputPackets, ref holisticLandmarkerResult))
          {
            resultCallback(in holisticLandmarkerResult, image, timestamp);
          }
          else
          {
            resultCallback(default, image, timestamp);
          }
        }
      };
    }

    private static bool TryBuildHolisticLandmarkerResult(PacketMap outputPackets, ref HolisticLandmarkerResult result)
    {
      using var poseLandmarksPacket = outputPackets.At<NormalizedLandmarks>(_POSE_LANDMARKS_STREAM_NAME);
      if (poseLandmarksPacket.IsEmpty())
      {
        return false;
      }
      var poseLandmarks = result.poseLandmarks;
      poseLandmarksPacket.Get(ref poseLandmarks);

      using var poseWorldLandmarksPacket = outputPackets.At<Landmarks>(_POSE_WORLD_LANDMARKS_STREAM_NAME);
      var poseWorldLandmarks = result.poseWorldLandmarks;
      poseWorldLandmarksPacket.Get(ref poseWorldLandmarks);

      using var faceLandmarksPacket = outputPackets.At<NormalizedLandmarks>(_FACE_LANDMARKS_STREAM_NAME);
      var faceLandmarks = result.faceLandmarks;
      if (faceLandmarksPacket.IsEmpty())
      {
        faceLandmarks.landmarks?.Clear();
      }
      else
      {
        faceLandmarksPacket.Get(ref faceLandmarks);
      }

      using var leftHandLandmarksPacket = outputPackets.At<NormalizedLandmarks>(_LEFT_HAND_LANDMARKS_STREAM_NAME);
      var leftHandLandmarks = result.leftHandLandmarks;
      if (leftHandLandmarksPacket.IsEmpty())
      {
        leftHandLandmarks.landmarks?.Clear();
      }
      else
      {
        leftHandLandmarksPacket.Get(ref leftHandLandmarks);
      }

      using var leftHandWorldLandmarksPacket = outputPackets.At<Landmarks>(_LEFT_HAND_WORLD_LANDMARKS_STREAM_NAME);
      var leftHandWorldLandmarks = result.leftHandWorldLandmarks;
      if (leftHandWorldLandmarksPacket.IsEmpty())
      {
        leftHandWorldLandmarks.landmarks?.Clear();
      }
      else
      {
        leftHandWorldLandmarksPacket.Get(ref leftHandWorldLandmarks);
      }

      using var rightHandLandmarksPacket = outputPackets.At<NormalizedLandmarks>(_RIGHT_HAND_LANDMARKS_STREAM_NAME);
      var rightHandLandmarks = result.rightHandLandmarks;
      if (rightHandLandmarksPacket.IsEmpty())
      {
        rightHandLandmarks.landmarks?.Clear();
      }
      else
      {
        rightHandLandmarksPacket.Get(ref rightHandLandmarks);
      }

      using var rightHandWorldLandmarksPacket = outputPackets.At<Landmarks>(_RIGHT_HAND_WORLD_LANDMARKS_STREAM_NAME);
      var rightHandWorldLandmarks = result.rightHandWorldLandmarks;
      if (rightHandWorldLandmarksPacket.IsEmpty())
      {
        rightHandWorldLandmarks.landmarks?.Clear();
      }
      else
      {
        rightHandWorldLandmarksPacket.Get(ref rightHandWorldLandmarks);
      }

      using var faceBlendshapesPacket = outputPackets.At<Classifications>(_FACE_BLENDSHAPES_STREAM_NAME);
      var faceBlendshapes = result.faceBlendshapes;
      faceBlendshapesPacket?.Get(ref faceBlendshapes);

      using var segmentationMaskPacket = outputPackets.At<Image>(_POSE_SEGMENTATION_MASK_STREAM_NAME);
      var segmentationMask = segmentationMaskPacket?.Get();

      result = new HolisticLandmarkerResult(
        faceLandmarks, poseLandmarks, poseWorldLandmarks,
        leftHandLandmarks, leftHandWorldLandmarks,
        rightHandLandmarks, rightHandWorldLandmarks,
        faceBlendshapes, segmentationMask);
      return true;
    }
  }
}
