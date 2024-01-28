// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using AudioClassifierResult = Mediapipe.Tasks.Components.Containers.ClassificationResult;

namespace Mediapipe.Tasks.Audio.AudioClassifier
{
  /// <summary>
  ///   Options for the audio classifier task.
  /// </summary>
  public sealed class AudioClassifierOptions : Tasks.Core.ITaskOptions
  {
    /// <remarks>
    ///   Some field of <paramref name="classificationResult"/> can be reused to reduce GC.Alloc.
    ///   If you need to refer to the data later, copy the data.
    /// </remarks>
    /// <param name="classificationResult">
    ///   An `<see cref="AudioClassifierResult"/> object that contains a list of classifications.
    /// </param>
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(AudioClassifierResult classificationResult, long timestampMillisec);

    /// <summary>
    ///   Base options for the audio classifier task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the audio clips mode.
    ///   Audio classifier task has two running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The audio clips mode for running classification on independent audio clips.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The audio stream mode for running classification on the audio stream, such as from microphone.
    ///         In this mode,  the <see cref="resultCallback"/> below must be specified to receive the classification results asynchronously.
    ///        </description>
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
    ///   The user-defined result callback for processing audio stream data.
    ///   The result callback should only be specified when the running mode is set to the audio stream mode.
    /// </summary>
    public ResultCallback resultCallback { get; }

    public AudioClassifierOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.AUDIO_CLIPS,
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

    internal Proto.AudioClassifierGraphOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.AUDIO_CLIPS;

      var classifierOptions = new Components.Processors.Proto.ClassifierOptions();
      if (displayNamesLocale != null)
      {
        classifierOptions.DisplayNamesLocale = displayNamesLocale;
      }
      if (maxResults is int maxResultsValue)
      {
        classifierOptions.MaxResults = maxResultsValue;
      }
      if (scoreThreshold is float scoreThresholdValue)
      {
        classifierOptions.ScoreThreshold = scoreThresholdValue;
      }
      if (categoryAllowList != null)
      {
        classifierOptions.CategoryAllowlist.AddRange(categoryAllowList);
      }
      if (categoryDenyList != null)
      {
        classifierOptions.CategoryDenylist.AddRange(categoryDenyList);
      }

      return new Proto.AudioClassifierGraphOptions
      {
        BaseOptions = baseOptionsProto,
        ClassifierOptions = classifierOptions,
      };
    }

    CalculatorOptions Tasks.Core.ITaskOptions.ToCalculatorOptions()
    {
      var options = new CalculatorOptions();
      options.SetExtension(Proto.AudioClassifierGraphOptions.Extensions.Ext, ToProto());
      return options;
    }
  }
}
