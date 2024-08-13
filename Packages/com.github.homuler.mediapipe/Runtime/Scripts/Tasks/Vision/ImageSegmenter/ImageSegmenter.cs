// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Tasks.Vision.ImageSegmenter
{
  public sealed class ImageSegmenter : Core.BaseVisionTaskApi
  {
    private const string _CONFIDENCE_MASKS_STREAM_NAME = "confidence_masks";
    private const string _CONFIDENCE_MASKS_TAG = "CONFIDENCE_MASKS";
    private const string _CATEGORY_MASK_STREAM_NAME = "category_mask";
    private const string _CATEGORY_MASK_TAG = "CATEGORY_MASK";
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";
    private const string _NORM_RECT_STREAM_NAME = "norm_rect_in";
    private const string _NORM_RECT_TAG = "NORM_RECT";
    private const string _TENSORS_TO_SEGMENTATION_CALCULATOR_NAME = "mediapipe.tasks.TensorsToSegmentationCalculator";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.image_segmenter.ImageSegmenterGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private readonly NormalizedRect _normalizedRect = new NormalizedRect();

    private readonly Lazy<List<string>> _labels;

    public IReadOnlyList<string> labels => _labels.Value;

    private ImageSegmenter(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      GpuResources gpuResources,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, gpuResources, packetCallback)
    {
      _packetCallback = packetCallback;
      _labels = new Lazy<List<string>>(() => GetLabels());
    }

    /// <summary>
    ///   Creates an <see cref="ImageSegmenter" /> object from a TensorFlow Lite model and the default <see cref="ImageSegmenterOptions" />.
    ///
    ///   Note that the created <see cref="ImageSegmenter" /> instance is in image mode,
    ///   for performing image segmentation on single image inputs.
    /// </summary>
    /// <param name="modelPath">Path to the model.</param>
    /// <param name="gpuResources">
    ///   <see cref="GpuResources"/> to set to the underlying <see cref="CalculatorGraph"/>.
    ///   To share the GL context with MediaPipe, <see cref="GlCalculatorHelper.InitializeForTest"/> must be called with it.
    /// </param>
    /// <returns>
    ///   <see cref="ImageSegmenter" /> object that's created from the model and the default <see cref="ImageSegmenterOptions" />.
    /// </returns>
    public static ImageSegmenter CreateFromModelPath(string modelPath, GpuResources gpuResources = null)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new ImageSegmenterOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options, gpuResources);
    }

    /// <summary>
    ///   Creates the <see cref="ImageSegmenter" /> object from <paramref name="ImageSegmenterOptions" />.
    /// </summary>
    /// <param name="options">Options for the image segmenter task.</param>
    /// <param name="gpuResources">
    ///   <see cref="GpuResources"/> to set to the underlying <see cref="CalculatorGraph"/>.
    ///   To share the GL context with MediaPipe, <see cref="GlCalculatorHelper.InitializeForTest"/> must be called with it.
    /// </param>
    /// <returns>
    ///   <see cref="ImageSegmenter" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static ImageSegmenter CreateFromOptions(ImageSegmenterOptions options, GpuResources gpuResources = null)
    {
      var outputStreams = new List<string> {
        string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
      };

      if (options.outputConfidenceMasks)
      {
        outputStreams.Add(string.Join(":", _CONFIDENCE_MASKS_TAG, _CONFIDENCE_MASKS_STREAM_NAME));
      }
      if (options.outputCategoryMask)
      {
        outputStreams.Add(string.Join(":", _CATEGORY_MASK_TAG, _CATEGORY_MASK_STREAM_NAME));
      }

      var taskInfo = new Tasks.Core.TaskInfo<ImageSegmenterOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
          string.Join(":", _NORM_RECT_TAG, _NORM_RECT_STREAM_NAME),
        },
        outputStreams: outputStreams,
        taskOptions: options);

      return new ImageSegmenter(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        gpuResources,
        BuildPacketsCallback(options));
    }

    /// <summary>
    ///   Performs the actual segmentation task on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="ImageSegmenter" /> is created with the image running mode.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <returns>
    ///   If the output_type is CATEGORY_MASK, the returned vector of images is per-category segmented image mask.
    ///   If the output_type is CONFIDENCE_MASK, the returned vector of images contains only one confidence image mask.
    ///   A segmentation result object that contains a list of segmentation masks as images.
    /// </returns>
    public ImageSegmenterResult Segment(Image image, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      using var outputPackets = SegmentInternal(image, imageProcessingOptions);

      var result = default(ImageSegmenterResult);
      _ = TryBuildImageSegmenterResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs the actual segmentation task on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="ImageSegmenter" /> is created with the image running mode.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <param name="result">
    ///   <see cref="ImageSegmenterResult"/> to which the result will be written.
    ///
    ///   If the output_type is CATEGORY_MASK, the returned vector of images is per-category segmented image mask.
    ///   If the output_type is CONFIDENCE_MASK, the returned vector of images contains only one confidence image mask.
    ///   A segmentation result object that contains a list of segmentation masks as images.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if the segmentation is successful, <see langword="false"/> otherwise.
    /// </returns>
    public bool TrySegment(Image image, Core.ImageProcessingOptions? imageProcessingOptions, ref ImageSegmenterResult result)
    {
      using var outputPackets = SegmentInternal(image, imageProcessingOptions);
      return TryBuildImageSegmenterResult(outputPackets, ref result);
    }

    private PacketMap SegmentInternal(Image image, Core.ImageProcessingOptions? imageProcessingOptions)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImage(image));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProto(_normalizedRect));

      return ProcessImageData(packetMap);
    }

    /// <summary>
    ///   Performs segmentation on the provided video frames.
    ///
    ///   Only use this method when the ImageSegmenter is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <returns>
    ///   If the output_type is CATEGORY_MASK, the returned vector of images is per-category segmented image mask.
    ///   If the output_type is CONFIDENCE_MASK, the returned vector of images contains only one confidence image mask.
    ///   A segmentation result object that contains a list of segmentation masks as images.
    /// </returns>
    public ImageSegmenterResult SegmentForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      using var outputPackets = SegmentForVideoInternal(image, timestampMillisec, imageProcessingOptions);

      var result = default(ImageSegmenterResult);
      _ = TryBuildImageSegmenterResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs segmentation on the provided video frames.
    ///
    ///   Only use this method when the ImageSegmenter is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <param name="result">
    ///   <see cref="ImageSegmenterResult"/> to which the result will be written.
    ///
    ///   If the output_type is CATEGORY_MASK, the returned vector of images is per-category segmented image mask.
    ///   If the output_type is CONFIDENCE_MASK, the returned vector of images contains only one confidence image mask.
    ///   A segmentation result object that contains a list of segmentation masks as images.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if the segmentation is successful, <see langword="false"/> otherwise.
    /// </returns>
    public bool TrySegmentForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions, ref ImageSegmenterResult result)
    {
      using var outputPackets = SegmentForVideoInternal(image, timestampMillisec, imageProcessingOptions);
      return TryBuildImageSegmenterResult(outputPackets, ref result);
    }

    private PacketMap SegmentForVideoInternal(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProtoAt(_normalizedRect, timestampMicrosec));

      return ProcessVideoData(packetMap);
    }

    /// <summary>
    ///   Sends live image data (an Image with a unique timestamp) to perform image segmentation.
    ///
    ///   Only use this method when the ImageSegmenter is created with the live stream
    ///   running mode. The input timestamps should be monotonically increasing for
    ///   adjacent calls of this method. This method will return immediately after the
    ///   input image is accepted. The results will be available via the
    ///   <see cref="ImageSegmenterOptions.ResultCallback" /> provided in the <see cref="ImageSegmenterOptions" />.
    ///   The <see cref="SegmentAsync" /> method is designed to process live stream data such as camera
    ///   input. To lower the overall latency, image segmenter may drop the input
    ///   images if needed. In other words, it's not guaranteed to have output per
    ///   input image.
    public void SegmentAsync(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProtoAt(_normalizedRect, timestampMicrosec));

      SendLiveStreamData(packetMap);
    }

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(ImageSegmenterOptions options)
    {
      var resultCallback = options.resultCallback;
      if (resultCallback == null)
      {
        return null;
      }

      var segmentationResult = ImageSegmenterResult.Alloc(options.outputConfidenceMasks);

      return (PacketMap outputPackets) =>
      {
        using var outImagePacket = outputPackets.At<Image>(_IMAGE_OUT_STREAM_NAME);
        if (outImagePacket == null || outImagePacket.IsEmpty())
        {
          return;
        }

        using var image = outImagePacket.Get();
        var timestamp = outImagePacket.TimestampMicroseconds() / _MICRO_SECONDS_PER_MILLISECOND;

        if (TryBuildImageSegmenterResult(outputPackets, ref segmentationResult))
        {
          resultCallback(segmentationResult, image, timestamp);
        }
        else
        {
          resultCallback(default, image, timestamp);
        }
      };
    }

    private static bool TryBuildImageSegmenterResult(PacketMap outputPackets, ref ImageSegmenterResult result)
    {
      var found = false;
      List<Image> confidenceMasks = null;
      if (outputPackets.TryGet<List<Image>>(_CONFIDENCE_MASKS_STREAM_NAME, out var confidenceMasksPacket))
      {
        found = true;
        confidenceMasks = result.confidenceMasks ?? new List<Image>();
        confidenceMasksPacket.Get(confidenceMasks);
        confidenceMasksPacket.Dispose();
      }

      Image categoryMask = null;
      if (outputPackets.TryGet<Image>(_CATEGORY_MASK_STREAM_NAME, out var categoryMaskPacket))
      {
        found = true;
        categoryMask = categoryMaskPacket.Get();
        categoryMaskPacket.Dispose();
      }

      if (!found)
      {
        return false;
      }
      result = new ImageSegmenterResult(confidenceMasks, categoryMask);
      return true;
    }

    private List<string> GetLabels()
    {
      var graphConfig = GetGraphConfig();
      var labels = new List<string>();

      foreach (var node in graphConfig.Node)
      {
        if (node.Name.EndsWith(_TENSORS_TO_SEGMENTATION_CALCULATOR_NAME))
        {
          var options = node.Options.GetExtension(TensorsToSegmentationCalculatorOptions.Extensions.Ext);
          if (options?.LabelItems?.Count > 0)
          {
            foreach (var labelItem in options.LabelItems)
            {
              labels.Add(labelItem.Value.Name);
            }
            return labels;
          }
        }
      }

      return labels;
    }
  }
}
