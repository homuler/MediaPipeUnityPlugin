// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.Core
{
  /// <summary>
  ///   Options for image processing.
  ///
  ///   If both region-or-interest and rotation are specified, the crop around the
  ///   region-of-interest is extracted first, then the specified rotation is applied
  ///   to the crop.
  /// </summary>
  public readonly struct ImageProcessingOptions
  {
    public readonly Components.Containers.RectF? regionOfInterest;
    public readonly int rotationDegrees;

    public ImageProcessingOptions(Components.Containers.RectF? regionOfInterest = null, int rotationDegrees = 0)
    {
      this.regionOfInterest = regionOfInterest;
      this.rotationDegrees = rotationDegrees;
    }
  }
}
