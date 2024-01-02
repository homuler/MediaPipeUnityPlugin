// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Components.Containers
{
  /// <summary>
  ///   Defines a single classification result.
  ///
  ///   The label maps packed into the TFLite Model Metadata [1] are used to populate
  ///   the 'category_name' and 'display_name' fields.
  ///
  ///   [1]: https://www.tensorflow.org/lite/convert/metadata
  /// </summary>
  public readonly struct Category
  {
    /// <summary>
    ///   The index of the category in the classification model output.
    /// </summary>
    public readonly int index;
    /// <summary>
    ///   The score for this category, e.g. (but not necessarily) a probability in [0,1].
    /// </summary>
    public readonly float score;
    /// <summary>
    ///   The optional ID for the category, read from the label map packed in the
    ///   TFLite Model Metadata if present. Not necessarily human-readable.
    /// </summary>
    public readonly string categoryName;
    /// <summary>
    ///   The optional human-readable name for the category, read from the label map
    ///   packed in the TFLite Model Metadata if present.
    /// </summary>
    public readonly string displayName;

    internal Category(int index, float score, string categoryName, string displayName)
    {
      this.index = index;
      this.score = score;
      this.categoryName = categoryName;
      this.displayName = displayName;
    }

    internal Category(NativeCategory nativeCategory) : this(
      nativeCategory.index,
      nativeCategory.score,
      nativeCategory.categoryName,
      nativeCategory.displayName)
    {
    }

    public static Category CreateFrom(Classification proto)
    {
      var categoryName = proto.HasLabel ? proto.Label : null;
      var displayName = proto.HasDisplayName ? proto.DisplayName : null;
      return new Category(proto.Index, proto.Score, categoryName, displayName);
    }

    public override string ToString() => $"{{ \"index\": {index}, \"score\": {score}, \"categoryName\": \"{categoryName}\", \"displayName\": \"{displayName}\" }}";
  }
}
