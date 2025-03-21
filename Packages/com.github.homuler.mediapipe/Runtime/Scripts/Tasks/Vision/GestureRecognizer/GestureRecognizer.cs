// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.GestureRecognizer
{
  /// <summary>
  ///   Class that performs gesture recognition on images.
  /// </summary>
  public sealed class GestureRecognizer : Core.BaseVisionTaskApi
  {
    private const string _HAND_GESTURE_STREAM_NAME = "hand_gestures";
    private const string _HAND_GESTURE_TAG = "HAND_GESTURES";
    private const string _HANDEDNESS_STREAM_NAME = "handedness";
    private const string _HANDEDNESS_TAG = "HANDEDNESS";
    private const string _HAND_LANDMARKS_STREAM_NAME = "landmarks";
    private const string _HAND_LANDMARKS_TAG = "LANDMARKS";
    private const string _HAND_WORLD_LANDMARKS_STREAM_NAME = "world_landmarks";
    private const string _HAND_WORLD_LANDMARKS_TAG = "WORLD_LANDMARKS";
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";
    private const string _NORM_RECT_STREAM_NAME = "norm_rect_in";
    private const string _NORM_RECT_TAG = "NORM_RECT";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.gesture_recognizer.GestureRecognizerGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private readonly NormalizedRect _normalizedRect = new NormalizedRect();

    private GestureRecognizer(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      GpuResources gpuResources,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, gpuResources, packetCallback)
    {
      _packetCallback = packetCallback;
    }

    /// <summary>
    ///   Creates an <see cref="GestureRecognizer" /> object from a TensorFlow Lite model and the default <see cref="GestureRecognizerOptions" />.
    ///
    ///   Note that the created <see cref="GestureRecognizer" /> instance is in image mode, for
    /// <param name="modelPath">Path to the model.</param>
    /// <returns>
    ///   <see cref="GestureRecognizer" /> object that's created from the model and the default <see cref="GestureRecognizerOptions" />.
    /// </returns>
    public static GestureRecognizer CreateFromModelPath(string modelPath, GpuResources gpuResources = null)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new GestureRecognizerOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options, gpuResources);
    }

    /// <summary>
    ///   Creates the <see cref="GestureRecognizer" /> object from <see cref="GestureRecognizerOptions" />.
    /// </summary>
    /// <param name="options">Options for the gesture recognizer task.</param>
    /// <returns>
    ///   <see cref="GestureRecognizer" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static GestureRecognizer CreateFromOptions(GestureRecognizerOptions options, GpuResources gpuResources = null)
    {
      var taskInfo = new Tasks.Core.TaskInfo<GestureRecognizerOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
          string.Join(":", _NORM_RECT_TAG, _NORM_RECT_STREAM_NAME),
        },
        outputStreams: new List<string> {
          string.Join(":", _HAND_GESTURE_TAG, _HAND_GESTURE_STREAM_NAME),
          string.Join(":", _HANDEDNESS_TAG, _HANDEDNESS_STREAM_NAME),
          string.Join(":", _HAND_LANDMARKS_TAG, _HAND_LANDMARKS_STREAM_NAME),
          string.Join(":", _HAND_WORLD_LANDMARKS_TAG, _HAND_WORLD_LANDMARKS_STREAM_NAME),
          string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
        },
        taskOptions: options);

      return new GestureRecognizer(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        gpuResources,
        BuildPacketsCallback(options));
    }

    /// <summary>
    ///   Performs hand gesture recognition on the given image.
    ///
    ///   Only use this method when the GestureRecognizer is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <returns>The hand gesture recognition results.</returns>
    public GestureRecognizerResult Recognize(Image image, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      using var outputPackets = RecognizeInternal(image, imageProcessingOptions);

      var result = default(GestureRecognizerResult);
      _ = TryBuildGestureRecognizerResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs hand gesture recognition on the given image.
    ///
    ///   Only use this method when the GestureRecognizer is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <param name="result">
    ///   <see cref="GestureRecognizerResult"/> to which the result will be written.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if some gestures are detected, <see langword="false"/> otherwise.
    /// </returns>
    public bool TryRecognize(Image image, Core.ImageProcessingOptions? imageProcessingOptions, ref GestureRecognizerResult result)
    {
      using var outputPackets = RecognizeInternal(image, imageProcessingOptions);
      return TryBuildGestureRecognizerResult(outputPackets, ref result);
    }

    private PacketMap RecognizeInternal(Image image, Core.ImageProcessingOptions? imageProcessingOptions)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImage(image));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProto(_normalizedRect));

      return ProcessImageData(packetMap);
    }

    /// <summary>
    ///   Performs hand gesture recognition on the provided video frame.
    ///
    ///   Only use this method when the GestureRecognizer is created with the video running mode.
    ///
    ///   Only use this method when the GestureRecognizer is created with the video running mode.
    ///   It's required to provide the video frame's timestamp (in milliseconds) along with the video frame.
    ///   The input timestamps should be monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="timestampMs">The timestamp of the input video frame in milliseconds.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <returns>The hand gesture recognition results.</returns>
    public GestureRecognizerResult RecognizeForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      using var outputPackets = RecognizeForVideoInternal(image, timestampMillisec, imageProcessingOptions);

      var result = default(GestureRecognizerResult);
      _ = TryBuildGestureRecognizerResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs hand gesture recognition on the provided video frame.
    ///
    ///   Only use this method when the GestureRecognizer is created with the video running mode.
    ///
    ///   Only use this method when the GestureRecognizer is created with the video running mode.
    ///   It's required to provide the video frame's timestamp (in milliseconds) along with the video frame.
    ///   The input timestamps should be monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="timestampMillisec">The timestamp of the input video frame in milliseconds.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <param name="result">
    ///   <see cref="GestureRecognizerResult"/> to which the result will be written.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if some gestures are detected, <see langword="false"/> otherwise.
    /// </returns>
    public bool TryRecognizeForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions, ref GestureRecognizerResult result)
    {
      using var outputPackets = RecognizeForVideoInternal(image, timestampMillisec, imageProcessingOptions);
      return TryBuildGestureRecognizerResult(outputPackets, ref result);
    }

    private PacketMap RecognizeForVideoInternal(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProtoAt(_normalizedRect, timestampMicrosec));

      return ProcessVideoData(packetMap);
    }

    /// <summary>
    ///   Sends live image data to perform gesture recognition.
    ///
    ///   The results will be available via the "result_callback" provided in the
    ///   GestureRecognizerOptions. Only use this method when the GestureRecognizer
    ///   is created with the live stream running mode.
    ///
    ///   Only use this method when the GestureRecognizer is created with the live
    ///   stream running mode. The input timestamps should be monotonically increasing
    ///   for adjacent calls of this method. This method will return immediately after
    ///   the input image is accepted. The results will be available via the
    ///   <see cref="GestureRecognizerOptions.resultCallback" /> provided in the
    ///   <see cref="GestureRecognizerOptions" />. The <see cref="RecognizeAsync" />
    ///   method is designed to process live stream data such as camera input. To
    ///   lower the overall latency, gesture recognizer may drop the input images if
    ///   needed. In other words, it's not guaranteed to have output per input
    ///   image.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="timestampMillisec">
    ///   The timestamp of the input video frame in milliseconds.
    /// </param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <exception cref="InvalidOperationException">
    ///   Thrown when the <see cref="GestureRecognizer" /> is not in live stream mode.
    /// </exception>
    public void RecognizeAsync(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProtoAt(_normalizedRect, timestampMicrosec));

      SendLiveStreamData(packetMap);
    }

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(GestureRecognizerOptions options)
    {
      var resultCallback = options.resultCallback;
      if (resultCallback == null)
      {
        return null;
      }

      var lockObj = new object();
      var result = GestureRecognizerResult.Alloc(Math.Max(options.numHands, 0));

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
          if (TryBuildGestureRecognizerResult(outputPackets, ref result))
          {
            resultCallback(result, image, timestamp);
          }
          else
          {
            resultCallback(default, image, timestamp);
          }
        }
      };
    }

    private static bool TryBuildGestureRecognizerResult(PacketMap outputPackets, ref GestureRecognizerResult result)
    {
      using var gesturesPacket = outputPackets.At<List<Classifications>>(_HAND_GESTURE_STREAM_NAME);
      if (gesturesPacket.IsEmpty())
      {
        return false;
      }

      var gestures = result.gestures ?? new List<Classifications>();
      gesturesPacket.Get(gestures);

      using var handednessPacket = outputPackets.At<List<Classifications>>(_HANDEDNESS_STREAM_NAME);
      var handedness = result.handedness ?? new List<Classifications>();
      handednessPacket.Get(handedness);

      using var handLandmarksPacket = outputPackets.At<List<NormalizedLandmarks>>(_HAND_LANDMARKS_STREAM_NAME);
      var handLandmarks = result.handLandmarks ?? new List<NormalizedLandmarks>();
      handLandmarksPacket.Get(handLandmarks);

      using var handWorldLandmarksPacket = outputPackets.At<List<Landmarks>>(_HAND_WORLD_LANDMARKS_STREAM_NAME);
      var handWorldLandmarks = result.handWorldLandmarks ?? new List<Landmarks>();
      handWorldLandmarksPacket.Get(handWorldLandmarks);

      result = new GestureRecognizerResult(gestures, handedness, handLandmarks, handWorldLandmarks);
      return true;
    }
  }
}
