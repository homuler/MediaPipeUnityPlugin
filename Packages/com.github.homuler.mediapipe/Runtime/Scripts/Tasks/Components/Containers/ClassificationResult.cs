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
    public readonly List<Category> categories;
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

    internal Classifications(List<Category> categories, int headIndex, string headName)
    {
      this.categories = categories;
      this.headIndex = headIndex;
      this.headName = headName;
    }

    public static Classifications CreateFrom(Proto.Classifications proto)
    {
      var classifications = default(Classifications);

      Copy(proto, ref classifications);
      return classifications;
    }

    public static Classifications CreateFrom(ClassificationList proto, int headIndex = 0, string headName = null)
    {
      var classifications = default(Classifications);

      Copy(proto, headIndex, headName, ref classifications);
      return classifications;
    }

    public static void Copy(Proto.Classifications source, ref Classifications destination)
    {
      var categories = destination.categories ?? new List<Category>(source.ClassificationList.Classification.Count);
      categories.Clear();
      for (var i = 0; i < source.ClassificationList.Classification.Count; i++)
      {
        categories.Add(Category.CreateFrom(source.ClassificationList.Classification[i]));
      }
      destination = new Classifications(categories, source.HeadIndex, source.HasHeadName ? source.HeadName : null);
    }

    public static void Copy(ClassificationList source, int headIndex, string headName, ref Classifications destination)
    {
      var categories = destination.categories ?? new List<Category>(source.Classification.Count);
      categories.Clear();
      for (var i = 0; i < source.Classification.Count; i++)
      {
        categories.Add(Category.CreateFrom(source.Classification[i]));
      }

      destination = new Classifications(categories, headIndex, headName);
    }

    public static void Copy(ClassificationList source, ref Classifications destination) => Copy(source, 0, null, ref destination);

    internal static void Copy(NativeClassifications source, ref Classifications destination)
    {
      var categories = destination.categories ?? new List<Category>((int)source.categoriesCount);
      categories.Clear();
      foreach (var nativeCategory in source.categories)
      {
        categories.Add(new Category(nativeCategory));
      }
      destination = new Classifications(categories, source.headIndex, source.headName);
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
    public readonly List<Classifications> classifications;

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

    internal ClassificationResult(List<Classifications> classifications, long? timestampMs)
    {
      this.classifications = classifications;
      this.timestampMs = timestampMs;
    }

    public static ClassificationResult Alloc(int capacity) => new ClassificationResult(new List<Classifications>(capacity), null);

    public static ClassificationResult CreateFrom(Proto.ClassificationResult proto)
    {
      var classificationResult = default(ClassificationResult);
      Copy(proto, ref classificationResult);

      return classificationResult;
    }

    public static void Copy(Proto.ClassificationResult source, ref ClassificationResult destination)
    {
      var classifications = destination.classifications ?? new List<Classifications>(source.Classifications.Count);
      for (var i = 0; i < source.Classifications.Count; i++)
      {
        classifications.Add(Classifications.CreateFrom(source.Classifications[i]));
      }
#pragma warning disable IDE0004 // for Unity 2020.3.x
      destination = new ClassificationResult(classifications, source.HasTimestampMs ? (long?)source.TimestampMs : null);
#pragma warning restore IDE0004 // for Unity 2020.3.x
    }

    internal static void Copy(NativeClassificationResult source, ref ClassificationResult destination)
    {
      var classificationsList = destination.classifications ?? new List<Classifications>((int)source.classificationsCount);
      classificationsList.ResizeTo((int)source.classificationsCount);

      var i = 0;
      foreach (var nativeClassifications in source.classifications)
      {
        var classifications = classificationsList[i];
        Classifications.Copy(nativeClassifications, ref classifications);
        classificationsList[i++] = classifications;
      }

      destination = new ClassificationResult(classificationsList, source.hasTimestampMs ? (long?)source.timestampMs : null);
    }

    public override string ToString() => $"{{ \"classifications\": {Util.Format(classifications)}, \"timestampMs\": {Util.Format(timestampMs)} }}";
  }
}
