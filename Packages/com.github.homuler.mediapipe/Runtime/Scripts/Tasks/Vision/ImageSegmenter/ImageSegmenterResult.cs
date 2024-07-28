// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Vision.ImageSegmenter
{
  /// <summary>
  ///   Output result of ImageSegmenter.
  /// </summary>
  public readonly struct ImageSegmenterResult
  {
    /// <summary>
    ///   multiple masks of float image where, for each mask,
    ///   each pixel represents the prediction confidence, usually in the [0, 1] range.
    /// </summary>
    public readonly List<Image> confidenceMasks;
    /// <summary>
    ///   a category mask of uint8 image where each pixel represents the class
    ///   which the pixel in the original image was predicted to belong to.
    /// </summary>
    public readonly Image categoryMask;

    internal ImageSegmenterResult(List<Image> confidenceMasks, Image categoryMask)
    {
      this.confidenceMasks = confidenceMasks;
      this.categoryMask = categoryMask;
    }

    public static ImageSegmenterResult Alloc(bool outputConfidenceMasks = false)
    {
      var confidenceMasks = outputConfidenceMasks ? new List<Image>() : null;
      return new ImageSegmenterResult(confidenceMasks, null);
    }

    public void CloneTo(ref ImageSegmenterResult destination)
    {
      var dstConfidenceMasks = destination.confidenceMasks;
      dstConfidenceMasks?.Clear();
      if (confidenceMasks != null)
      {
        dstConfidenceMasks ??= new List<Image>(confidenceMasks.Count);
        dstConfidenceMasks.Clear();
        dstConfidenceMasks.AddRange(confidenceMasks);
      }

      destination = new ImageSegmenterResult(dstConfidenceMasks, categoryMask);
    }
  }
}
