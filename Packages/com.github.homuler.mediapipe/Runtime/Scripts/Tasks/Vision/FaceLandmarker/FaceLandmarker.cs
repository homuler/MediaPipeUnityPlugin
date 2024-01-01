// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Vision.FaceLandmarker
{
  public sealed class FaceLandmarker : Core.BaseVisionTaskApi
  {
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";
    private const string _NORM_RECT_STREAM_NAME = "norm_rect_in";
    private const string _NORM_RECT_TAG = "NORM_RECT";
    private const string _NORM_LANDMARKS_STREAM_NAME = "norm_landmarks";
    private const string _NORM_LANDMARKS_TAG = "NORM_LANDMARKS";
    private const string _BLENDSHAPES_STREAM_NAME = "blendshapes";
    private const string _BLENDSHAPES_TAG = "BLENDSHAPES";
    private const string _FACE_GEOMETRY_STREAM_NAME = "face_geometry";
    private const string _FACE_GEOMETRY_TAG = "FACE_GEOMETRY";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.face_landmarker.FaceLandmarkerGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private readonly NormalizedRect _normalizedRect = new NormalizedRect();

    private FaceLandmarker(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, packetCallback)
    {
      _packetCallback = packetCallback;
    }

    /// <summary>
    ///   Creates an <see cref="FaceLandmarker" /> object from a TensorFlow Lite model and the default <see cref="FaceLandmarkerOptions" />.
    ///
    ///   Note that the created <see cref="FaceLandmarker" /> instance is in image mode,
    ///   for detecting face landmarks on single image inputs.
    /// </summary>
    /// <param name="modelPath">Path to the model.</param>
    /// <returns>
    ///   <see cref="FaceLandmarker" /> object that's created from the model and the default <see cref="FaceLandmarkerOptions" />.
    /// </returns>
    public static FaceLandmarker CreateFromModelPath(string modelPath)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new FaceLandmarkerOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options);
    }

    /// <summary>
    ///   Creates the <see cref="FaceLandmarker" /> object from <paramref name="FaceLandmarkerOptions" />.
    /// </summary>
    /// <param name="options">Options for the face landmarker task.</param>
    /// <returns>
    ///   <see cref="FaceLandmarker" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static FaceLandmarker CreateFromOptions(FaceLandmarkerOptions options)
    {
      var outputStreams = new List<string> {
        string.Join(":", _NORM_LANDMARKS_TAG, _NORM_LANDMARKS_STREAM_NAME),
        string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
      };
      if (options.outputFaceBlendshapes)
      {
        outputStreams.Add(string.Join(":", _BLENDSHAPES_TAG, _BLENDSHAPES_STREAM_NAME));
      }
      if (options.outputFaceTransformationMatrixes)
      {
        outputStreams.Add(string.Join(":", _FACE_GEOMETRY_TAG, _FACE_GEOMETRY_STREAM_NAME));
      }
      var taskInfo = new Tasks.Core.TaskInfo<FaceLandmarkerOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
          string.Join(":", _NORM_RECT_TAG, _NORM_RECT_STREAM_NAME),
        },
        outputStreams: outputStreams,
        taskOptions: options);

      return new FaceLandmarker(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        BuildPacketsCallback(options.resultCallback));
    }

    /// <summary>
    ///   Performs face landmarks detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="FaceLandmarker" /> is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <returns>
    ///   The face landmarks detection results.
    /// </returns>
    public FaceLandmarkerResult Detect(Image image, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(_normalizedRect));
      var outputPackets = ProcessImageData(packetMap);

      return BuildFaceLandmarkerResult(outputPackets);
    }

    /// <summary>
    ///   Performs face landmarks detection on the provided video frames.
    ///
    ///   Only use this method when the FaceLandmarker is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <returns>
    ///   The face landmarks detection results.
    /// </returns>
    public FaceLandmarkerResult DetectForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);

      PacketMap outputPackets = null;
      using (var timestamp = new Timestamp(timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image, timestamp));
        packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(_normalizedRect).At(timestamp));
        outputPackets = ProcessVideoData(packetMap);
      }

      return BuildFaceLandmarkerResult(outputPackets);
    }

    /// <summary>
    ///   Sends live image data to perform face landmarks detection.
    ///
    ///   Only use this method when the FaceLandmarker is created with the live stream
    ///   running mode. The input timestamps should be monotonically increasing for
    ///   adjacent calls of this method. This method will return immediately after the
    ///   input image is accepted. The results will be available via the
    ///   <see cref="FaceLandmarkerOptions.ResultCallback" /> provided in the <see cref="FaceLandmarkerOptions" />.
    ///   The <see cref="DetectAsync" /> method is designed to process live stream data such as camera
    ///   input. To lower the overall latency, face landmarker may drop the input
    ///   images if needed. In other words, it's not guaranteed to have output per
    ///   input image.
    public void DetectAsync(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);

      using (var timestamp = new Timestamp(timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace(_IMAGE_IN_STREAM_NAME, new ImagePacket(image, timestamp));
        packetMap.Emplace(_NORM_RECT_STREAM_NAME, new NormalizedRectPacket(_normalizedRect).At(timestamp));

        SendLiveStreamData(packetMap);
      }
    }

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(FaceLandmarkerOptions.ResultCallback resultCallback)
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
        var faceLandmarkerResult = BuildFaceLandmarkerResult(outputPackets);
        var timestamp = outImagePacket.Timestamp().Microseconds() / _MICRO_SECONDS_PER_MILLISECOND;

        resultCallback(faceLandmarkerResult, image, timestamp);
      };
    }

    private static FaceLandmarkerResult BuildFaceLandmarkerResult(PacketMap outputPackets)
    {
      var faceLandmarksProtoListPacket =
        outputPackets.At<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(_NORM_LANDMARKS_STREAM_NAME);
      if (faceLandmarksProtoListPacket.IsEmpty())
      {
        return FaceLandmarkerResult.Empty();
      }

      var faceLandmarksProtoList = faceLandmarksProtoListPacket.Get();

      var faceBlendshapesProtoList =
        outputPackets.At<ClassificationListVectorPacket, List<ClassificationList>>(_BLENDSHAPES_STREAM_NAME)?.Get();

      var faceTransformationMatrixesProtoList =
        outputPackets.At<FaceGeometry.FaceGeometryVectorPacket, List<FaceGeometry.Proto.FaceGeometry>>(_FACE_GEOMETRY_STREAM_NAME)?.Get();

      return FaceLandmarkerResult.CreateFrom(faceLandmarksProtoList, faceBlendshapesProtoList, faceTransformationMatrixesProtoList);
    }
  }
}
