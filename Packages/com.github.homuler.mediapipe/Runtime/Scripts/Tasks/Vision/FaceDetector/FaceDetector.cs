// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

using FaceDetectionResult = Mediapipe.Tasks.Components.Containers.DetectionResult;

namespace Mediapipe.Tasks.Vision.FaceDetector
{
  public sealed class FaceDetector : Core.BaseVisionTaskApi
  {
    private const string _DETECTIONS_OUT_STREAM_NAME = "detections";
    private const string _DETECTIONS_TAG = "DETECTIONS";
    private const string _NORM_RECT_STREAM_NAME = "norm_rect_in";
    private const string _NORM_RECT_TAG = "NORM_RECT";
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.face_detector.FaceDetectorGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private FaceDetector(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, packetCallback)
    {
      _packetCallback = packetCallback;
    }

    /// <summary>
    ///   Creates an <see cref="FaceDetector" /> object from a TensorFlow Lite model and the default <see cref="FaceDetectorOptions" />.
    ///
    ///   Note that the created <see cref="FaceDetector" /> instance is in image mode,
    ///   for detecting faces on single image inputs.
    /// </summary>
    /// <param name="modelPath">Path to the model.</param>
    /// <returns>
    ///   <see cref="FaceDetector" /> object that's created from the model and the default <see cref="FaceDetectorOptions" />.
    /// </returns>
    public static FaceDetector CreateFromModelPath(string modelPath)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new FaceDetectorOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options);
    }

    /// <summary>
    ///   Creates the <see cref="FaceDetector" /> object from <paramref name="FaceDetectorOptions" />.
    /// </summary>
    /// <param name="options">Options for the face detector task.</param>
    /// <returns>
    ///   <see cref="FaceDetector" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static FaceDetector CreateFromOptions(FaceDetectorOptions options)
    {
      var taskInfo = new Tasks.Core.TaskInfo<FaceDetectorOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
          string.Join(":", _NORM_RECT_TAG, _NORM_RECT_STREAM_NAME),
        },
        outputStreams: new List<string> {
          string.Join(":", _DETECTIONS_TAG, _DETECTIONS_OUT_STREAM_NAME),
          string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
        },
        taskOptions: options);

      return new FaceDetector(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        BuildPacketsCallback(options.resultCallback));
    }

    /// <summary>
    ///   Performs face detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="FaceDetector" /> is created with the image running mode.
    /// </summary>
    /// <returns>
    ///   A face detection result object that contains a list of face detections,
    ///   each detection has a bounding box that is expressed in the unrotated input
    ///   frame of reference coordinates system, i.e. in `[0,image_width) x [0,
    ///   image_height)`, which are the dimensions of the underlying image data.
    /// </returns>
    public FaceDetectionResult Detect(Image image, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      var normalizedRect = ConvertToNormalizedRect(imageProcessingOptions, image, roiAllowed: false);

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(normalizedRect));
      var outputPackets = ProcessImageData(packetMap);

      var outDetectionsPacket = outputPackets.At<DetectionVectorPacket, List<Detection>>(_DETECTIONS_OUT_STREAM_NAME);
      if (outDetectionsPacket.IsEmpty())
      {
        return new FaceDetectionResult(new List<Components.Containers.Detection>());
      }
      return FaceDetectionResult.CreateFrom(outDetectionsPacket.Get());
    }

    /// <summary>
    ///   Performs face detection on the provided video frames.
    ///
    ///   Only use this method when the FaceDetector is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <returns>
    ///   A face detection result object that contains a list of face detections,
    ///   each detection has a bounding box that is expressed in the unrotated input
    ///   frame of reference coordinates system, i.e. in `[0,image_width) x [0,
    ///   image_height)`, which are the dimensions of the underlying image data.
    /// </returns>
    public FaceDetectionResult DetectForVideo(Image image, int timestampMs, Core.ImageProcessingOptions? imageProcessingOptions = null)
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

      var outDetectionsPacket = outputPackets.At<DetectionVectorPacket, List<Detection>>(_DETECTIONS_OUT_STREAM_NAME);
      if (outDetectionsPacket.IsEmpty())
      {
        return new FaceDetectionResult(new List<Components.Containers.Detection>());
      }
      return FaceDetectionResult.CreateFrom(outDetectionsPacket.Get());
    }

    /// <summary>
    ///   Sends live image data (an Image with a unique timestamp) to perform face detection.
    ///
    ///   Only use this method when the FaceDetector is created with the live stream
    ///   running mode. The input timestamps should be monotonically increasing for
    ///   adjacent calls of this method. This method will return immediately after the
    ///   input image is accepted. The results will be available via the
    ///   <see cref="FaceDetectorOptions.ResultCallback" /> provided in the <see cref="FaceDetectorOptions" />.
    ///   The <see cref="DetectAsync" /> method is designed to process live stream data such as camera
    ///   input. To lower the overall latency, face detector may drop the input
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

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(FaceDetectorOptions.ResultCallback resultCallback)
    {
      if (resultCallback == null)
      {
        return null;
      }

      return (PacketMap outputPackets) =>
      {
        var outImagePacket = outputPackets.At<ImagePacket, Image>(_IMAGE_OUT_STREAM_NAME);
        var outDetectionsPacket = outputPackets.At<DetectionVectorPacket, List<Detection>>(_DETECTIONS_OUT_STREAM_NAME);
        if (outImagePacket == null || outDetectionsPacket == null)
        {
          return;
        }

        if (outImagePacket.IsEmpty())
        {
          return;
        }
        var image = outImagePacket.Get();
        var timestamp = outImagePacket.Timestamp().Microseconds() / _MICRO_SECONDS_PER_MILLISECOND;

        if (outDetectionsPacket.IsEmpty())
        {
          resultCallback(
            new FaceDetectionResult(new List<Components.Containers.Detection>()),
            image,
            (int)timestamp);
          return;
        }

        var detectionProtoList = outDetectionsPacket.Get();
        resultCallback(FaceDetectionResult.CreateFrom(detectionProtoList), image, (int)timestamp);
      };
    }
  }
}
