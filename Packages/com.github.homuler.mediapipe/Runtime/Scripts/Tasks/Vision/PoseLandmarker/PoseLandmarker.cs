// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Vision.PoseLandmarker
{
  public sealed class PoseLandmarker : Core.BaseVisionTaskApi
  {
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";
    private const string _NORM_RECT_STREAM_NAME = "norm_rect_in";
    private const string _NORM_RECT_TAG = "NORM_RECT";
    private const string _SEGMENTATION_MASK_STREAM_NAME = "segmentation_mask";
    private const string _SEGMENTATION_MASK_TAG = "SEGMENTATION_MASK";
    private const string _NORM_LANDMARKS_STREAM_NAME = "norm_landmarks";
    private const string _NORM_LANDMARKS_TAG = "NORM_LANDMARKS";
    private const string _POSE_WORLD_LANDMARKS_STREAM_NAME = "world_landmarks";
    private const string _POSE_WORLD_LANDMARKS_TAG = "WORLD_LANDMARKS";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.pose_landmarker.PoseLandmarkerGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private PoseLandmarker(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, packetCallback)
    {
      _packetCallback = packetCallback;
    }

    /// <summary>
    ///   Creates an <see cref="PoseLandmarker" /> object from a TensorFlow Lite model and the default <see cref="PoseLandmarkerOptions" />.
    ///
    ///   Note that the created <see cref="PoseLandmarker" /> instance is in image mode,
    ///   for detecting pose landmarks on single image inputs.
    /// </summary>
    /// <param name="modelPath">Path to the model.</param>
    /// <returns>
    ///   <see cref="PoseLandmarker" /> object that's created from the model and the default <see cref="PoseLandmarkerOptions" />.
    /// </returns>
    public static PoseLandmarker CreateFromModelPath(string modelPath)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new PoseLandmarkerOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options);
    }

    /// <summary>
    ///   Creates the <see cref="PoseLandmarker" /> object from <paramref name="PoseLandmarkerOptions" />.
    /// </summary>
    /// <param name="options">Options for the pose landmarker task.</param>
    /// <returns>
    ///   <see cref="PoseLandmarker" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static PoseLandmarker CreateFromOptions(PoseLandmarkerOptions options)
    {
      var outputStreams = new List<string> {
        string.Join(":", _NORM_LANDMARKS_TAG, _NORM_LANDMARKS_STREAM_NAME),
        string.Join(":", _POSE_WORLD_LANDMARKS_TAG, _POSE_WORLD_LANDMARKS_STREAM_NAME),
        string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
      };
      if (options.outputSegmentationMasks)
      {
        outputStreams.Add(string.Join(":", _SEGMENTATION_MASK_TAG, _SEGMENTATION_MASK_STREAM_NAME));
      }
      var taskInfo = new Tasks.Core.TaskInfo<PoseLandmarkerOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
          string.Join(":", _NORM_RECT_TAG, _NORM_RECT_STREAM_NAME),
        },
        outputStreams: outputStreams,
        taskOptions: options);

      return new PoseLandmarker(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        BuildPacketsCallback(options.resultCallback));
    }

    /// <summary>
    ///   Performs pose landmarks detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="PoseLandmarker" /> is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <returns>
    ///   The pose landmarks detection results.
    /// </returns>
    public PoseLandmarkerResult Detect(Image image, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      var normalizedRect = ConvertToNormalizedRect(imageProcessingOptions, image, roiAllowed: false);

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(normalizedRect));
      var outputPackets = ProcessImageData(packetMap);

      return BuildPoseLandmarkerResult(outputPackets);
    }

    /// <summary>
    ///   Performs pose landmarks detection on the provided video frames.
    ///
    ///   Only use this method when the PoseLandmarker is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <returns>
    ///   The pose landmarks detection results.
    /// </returns>
    public PoseLandmarkerResult DetectForVideo(Image image, int timestampMs, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      var normalizedRect = ConvertToNormalizedRect(imageProcessingOptions, image, roiAllowed: false);

      PacketMap outputPackets = null;
      using (var timestamp = new Timestamp(timestampMs * _MICRO_SECONDS_PER_MILLISECOND))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image, timestamp));
        packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(normalizedRect).At(timestamp));
        outputPackets = ProcessVideoData(packetMap);
      }

      return BuildPoseLandmarkerResult(outputPackets);
    }

    /// <summary>
    ///   Sends live image data to perform pose landmarks detection.
    ///
    ///   Only use this method when the PoseLandmarker is created with the live stream
    ///   running mode. The input timestamps should be monotonically increasing for
    ///   adjacent calls of this method. This method will return immediately after the
    ///   input image is accepted. The results will be available via the
    ///   <see cref="PoseLandmarkerOptions.ResultCallback" /> provided in the <see cref="PoseLandmarkerOptions" />.
    ///   The <see cref="DetectAsync" /> method is designed to process live stream data such as camera
    ///   input. To lower the overall latency, pose landmarker may drop the input
    ///   images if needed. In other words, it's not guaranteed to have output per
    ///   input image.
    public void DetectAsync(Image image, int timestampMs, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      var normalizedRect = ConvertToNormalizedRect(imageProcessingOptions, image, roiAllowed: false);

      using (var timestamp = new Timestamp(timestampMs * _MICRO_SECONDS_PER_MILLISECOND))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image, timestamp));
        packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(normalizedRect).At(timestamp));

        SendLiveStreamData(packetMap);
      }
    }

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(PoseLandmarkerOptions.ResultCallback resultCallback)
    {
      if (resultCallback == null)
      {
        return null;
      }

      return (PacketMap outputPackets) =>
      {
        var outImagePacket = outputPackets.At<ImagePacket, Image>(_IMAGE_OUT_STREAM_NAME);
        if (outImagePacket == null || outImagePacket.IsEmpty())
        {
          return;
        }

        var image = outImagePacket.Get();
        var handLandmarkerResult = BuildPoseLandmarkerResult(outputPackets);
        var timestamp = outImagePacket.Timestamp().Microseconds() / _MICRO_SECONDS_PER_MILLISECOND;

        resultCallback(handLandmarkerResult, image, (int)timestamp);
      };
    }

    private static PoseLandmarkerResult BuildPoseLandmarkerResult(PacketMap outputPackets)
    {
      var poseLandmarksProtoPacket =
        outputPackets.At<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(_NORM_LANDMARKS_STREAM_NAME);
      if (poseLandmarksProtoPacket.IsEmpty())
      {
        return PoseLandmarkerResult.Empty();
      }

      var poseLandmarksProto = poseLandmarksProtoPacket.Get();
      var poseWorldLandmarksProto = outputPackets.At<LandmarkListVectorPacket, List<LandmarkList>>(_POSE_WORLD_LANDMARKS_STREAM_NAME).Get();
      var segmentationMasks = outputPackets.At<ImageVectorPacket, List<Image>>(_SEGMENTATION_MASK_STREAM_NAME)?.Get();

      return PoseLandmarkerResult.CreateFrom(poseLandmarksProto, poseWorldLandmarksProto, segmentationMasks);
    }
  }
}
