// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

/// <summary>
///   Classifier options for MediaPipe classification Tasks.
/// </summary>
public sealed class ClassifierOptions
{
  /// <summary>
  ///   The locale to use for display names specified through the TFLite Model Metadata, if any.
  ///   Defaults to English.
  /// </summary>
  public string displayNamesLocale { get; set; } = "en";

  /// <summary>
  ///   The maximum number of top-scored classification results to return.
  ///   If &lt; 0, all available results will be returned.
  /// </summary>
  public int maxResults { get; set; } = -1;

  /// <summary>
  ///   Score threshold to override the one provided in the model metadata (if any).
  ///   Results below this value are rejected.
  /// </summary>
  public float scoreThreshold { get; set; } = 0.0f;

  /// <summary>
  ///   The allowlist of category names.
  ///   If non-empty, detection results whose category name is not in this set will be filtered out.
  ///   Duplicate or unknown category names are ignored. Mutually exclusive with category_denylist.
  /// </summary>
  public List<string> categoryAllowlist { get; set; }

  /// <summary>
  ///   The denylist of category names.
  ///   If non-empty, detection results whose category name is in this set will be filtered out.
  ///   Duplicate or unknown category names are ignored. Mutually exclusive with category_allowlist.
  /// </summary>
  public List<string> categoryDenylist { get; set; }

  public ClassifierOptions()
  {
  }

  internal Mediapipe.Tasks.Components.Processors.Proto.ClassifierOptions ToProto()
  {
    var proto = new Mediapipe.Tasks.Components.Processors.Proto.ClassifierOptions()
    {
      DisplayNamesLocale = displayNamesLocale,
      MaxResults = maxResults,
      ScoreThreshold = scoreThreshold,
    };

    if (categoryAllowlist != null && categoryAllowlist.Count > 0)
    {
      proto.CategoryAllowlist.AddRange(categoryAllowlist);
    }

    if (categoryDenylist != null && categoryDenylist.Count > 0)
    {
      proto.CategoryDenylist.AddRange(categoryDenylist);
    }

    return proto;
  }
}
