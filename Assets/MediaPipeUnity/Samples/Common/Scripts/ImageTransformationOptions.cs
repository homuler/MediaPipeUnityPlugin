// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity.Sample
{
  public readonly struct ImageTransformationOptions
  {
    public readonly bool flipHorizontally;
    public readonly bool flipVertically;
    public readonly RotationAngle rotationAngle;

    private ImageTransformationOptions(bool flipHorizontally, bool flipVertically, RotationAngle rotationAngle)
    {
      this.flipHorizontally = flipHorizontally;
      this.flipVertically = flipVertically;
      this.rotationAngle = rotationAngle;
    }

    public static ImageTransformationOptions Build(bool shouldFlipHorizontally, bool isVerticallyFlipped, RotationAngle rotation)
    {
      var isInverted = CoordinateSystem.ImageCoordinate.IsInverted(rotation);
      var flipHorizontally = !isInverted && shouldFlipHorizontally;
      var flipVertically = !shouldFlipHorizontally ? !isVerticallyFlipped : isInverted ? isVerticallyFlipped : !isVerticallyFlipped;

      return new ImageTransformationOptions(flipHorizontally, flipVertically, rotation);
    }
  }

  public static class ImageSourceExtension
  {
    public static ImageTransformationOptions GetTransformationOptions(this ImageSource imageSource, bool expectedToBeMirrored = false)
    {
      var shouldFlipHorizontally = (imageSource.isFrontFacing || expectedToBeMirrored) ^ imageSource.isHorizontallyFlipped;
      var shouldFlipVertically = imageSource.isVerticallyFlipped;
      return ImageTransformationOptions.Build(shouldFlipHorizontally, shouldFlipVertically, imageSource.rotation);
    }
  } 
}
