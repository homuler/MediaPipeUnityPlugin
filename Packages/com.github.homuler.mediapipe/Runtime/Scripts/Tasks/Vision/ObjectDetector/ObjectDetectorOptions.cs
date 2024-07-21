// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Vision.ObjectDetector
{
  /// <summary>
  ///   Options for the object detector task.
  /// </summary>
  public sealed class ObjectDetectorOptions : Tasks.Core.ITaskOptions
  {
    /// <remarks>
    ///   Some field of <paramref name="detectionResult"/> can be reused to reduce GC.Alloc.
    ///   If you need to refer to the data later, copy the data.
    /// </remarks>
    /// <param name="detectionResult">
    ///   A detection result object that contains a list of detections,
    ///   each detection has a bounding box that is expressed in the unrotated
    ///   input frame of reference coordinates system,
    ///   i.e. in `[0,image_width) x [0,image_height)`, which are the dimensions
    ///   of the underlying image data.
    /// </param>
    /// <param name="image">
    ///   The input image that the object detector runs on.
    /// </param>
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(Components.Containers.DetectionResult detectionResult, Image image, long timestampMillisec);

    /// <summary>
    ///   Base options for the object detector task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the image mode.
    ///   Object detector task has three running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The image mode for detecting objects on single image inputs.</description>
    ///     </item>
    ///     <item>
    ///       <description>The video mode for detecting objects on the decoded frames of a video.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The live stream mode or detecting objects on the live stream of input data, such as from camera.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    public Core.RunningMode runningMode { get; }

    /// <summary>
    ///   The locale to use for display names specified through the TFLite Model Metadata.
    /// </summary>
    public string displayNamesLocale { get; }
    /// <summary>
    ///   The maximum number of top-scored classification results to return.
    /// </summary>
    public int? maxResults { get; }
    /// <summary>
    ///   Overrides the ones provided in the model metadata. Results below this value are rejected.
    /// </summary>
    public float? scoreThreshold { get; }
    /// <summary>
    ///   Allowlist of category names.
    ///   If non-empty, classification results whose category name is not in this set will be filtered out.
    ///   Duplicate or unknown category names are ignored. Mutually exclusive with <see cref="categoryDenylist"/>.
    /// </summary>
    public List<string> categoryAllowList { get; }
    /// <summary>
    ///   Denylist of category names.
    ///   If non-empty, classification results whose category name is in this set will be filtered out.
    ///   Duplicate or unknown category names are ignored. Mutually exclusive with <see cref="categoryAllowList"/>.
    /// </summary>
    public List<string> categoryDenyList { get; }
    /// <summary>
    ///   The user-defined result callback for processing live stream data.
    ///   The result callback should only be specified when the running mode is set to the live stream mode.
    /// </summary>
    public ResultCallback resultCallback { get; }

    public ObjectDetectorOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      string displayNamesLocale = null,
      int? maxResults = null,
      float? scoreThreshold = null,
      List<string> categoryAllowList = null,
      List<string> categoryDenyList = null,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.displayNamesLocale = displayNamesLocale;
      this.maxResults = maxResults;
      this.scoreThreshold = scoreThreshold;
      this.categoryAllowList = categoryAllowList;
      this.categoryDenyList = categoryDenyList;
      this.resultCallback = resultCallback;
    }

    internal Proto.ObjectDetectorOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.IMAGE;

      var options = new Proto.ObjectDetectorOptions
      {
        BaseOptions = baseOptionsProto,
      };

      if (displayNamesLocale != null)
      {
        options.DisplayNamesLocale = displayNamesLocale;
      }
      if (maxResults is int maxResultsValue)
      {
        options.MaxResults = maxResultsValue;
      }
      if (scoreThreshold is float scoreThresholdValue)
      {
        options.ScoreThreshold = scoreThresholdValue;
      }
      if (categoryAllowList != null)
      {
        options.CategoryAllowlist.AddRange(categoryAllowList);
      }
      if (categoryDenyList != null)
      {
        options.CategoryDenylist.AddRange(categoryDenyList);
      }

      return options;
    }

    CalculatorOptions Tasks.Core.ITaskOptions.ToCalculatorOptions()
    {
      var options = new CalculatorOptions();
      options.SetExtension(Proto.ObjectDetectorOptions.Extensions.Ext, ToProto());
      return options;
    }
  }
}
