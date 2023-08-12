// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Components.Containers
{
  /// <summary>
  ///   Defines classification results for a given classifier head.
  /// </summary>
  public readonly struct Classifications
  {
    /// <summary>
    ///   The array of predicted categories, usually sorted by descending scores,
    ///   e.g. from high to low probability.
    /// </summary>
    public readonly IReadOnlyList<Category> categories;
    /// <summary>
    ///   The index of the classifier head (i.e. output tensor) these categories
    ///   refer to. This is useful for multi-head models.
    /// </summary>
    public readonly int headIndex;
    /// <summary>
    ///   The optional name of the classifier head, as provided in the TFLite Model
    ///   Metadata [1] if present. This is useful for multi-head models.
    ///
    ///   [1]: https://www.tensorflow.org/lite/convert/metadata
    /// </summary>
    public readonly string headName;

    internal Classifications(IReadOnlyList<Category> categories, int headIndex, string headName)
    {
      this.categories = categories;
      this.headIndex = headIndex;
      this.headName = headName;
    }

    public static Classifications CreateFrom(Proto.Classifications proto)
    {
      var categories = new List<Category>(proto.ClassificationList.Classification.Count);
      foreach (var classification in proto.ClassificationList.Classification)
      {
        categories.Add(Category.CreateFrom(classification));
      }
      return new Classifications(categories, proto.HeadIndex, proto.HasHeadName ? proto.HeadName : null);
    }

    public static Classifications CreateFrom(ClassificationList proto, int headIndex = 0, string headName = null)
    {
      var categories = new List<Category>(proto.Classification.Count);
      foreach (var classification in proto.Classification)
      {
        categories.Add(Category.CreateFrom(classification));
      }
      return new Classifications(categories, headIndex, headName);
    }

    public override string ToString()
      => $"{{ \"categories\": {Util.Format(categories)}, \"headIndex\": {headIndex}, \"headName\": {Util.Format(headName)} }}";
  }

  /// <summary>
  ///   Defines classification results of a model.
  /// </summary>
  public readonly struct ClassificationResult
  {
    /// <summary>
    ///   The classification results for each head of the model.
    /// </summary>
    public readonly IReadOnlyList<Classifications> classifications;

    /// <summary>
    ///   The optional timestamp (in milliseconds) of the start of the chunk of data
    ///   corresponding to these results.
    ///
    ///   This is only used for classification on time series (e.g. audio
    ///   classification). In these use cases, the amount of data to process might
    ///   exceed the maximum size that the model can process: to solve this, the
    ///   input data is split into multiple chunks starting at different timestamps.
    /// </summary>
    public readonly long? timestampMs;

    internal ClassificationResult(IReadOnlyList<Classifications> classifications, long? timestampMs)
    {
      this.classifications = classifications;
      this.timestampMs = timestampMs;
    }

    public static ClassificationResult CreateFrom(Proto.ClassificationResult proto)
    {
      var classifications = new List<Classifications>(proto.Classifications.Count);
      foreach (var classification in proto.Classifications)
      {
        classifications.Add(Classifications.CreateFrom(classification));
      }
#pragma warning disable IDE0004 // for Unity 2020.3.x
      return new ClassificationResult(classifications, proto.HasTimestampMs ? (long?)proto.TimestampMs : null);
#pragma warning restore IDE0004 // for Unity 2020.3.x
    }

    public override string ToString() => $"{{ \"classifications\": {Util.Format(classifications)}, \"timestampMs\": {Util.Format(timestampMs)} }}";
  }
}
