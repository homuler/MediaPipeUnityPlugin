// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.CoordinateSystem
{
  /// <summary>
  ///   This class provides helper methods for converting camera coordinate values to local coordinate values in Unity.
  ///   See <see cref="https://google.github.io/mediapipe/solutions/objectron.html#coordinate-systems" /> for more details.
  /// </summary>
  /// <remarks>
  ///   Assume that the origin is common to the two coordinate systems.
  /// </remarks>
  public static class CameraCoordinate
  {
    /// <summary>
    ///   Convert from camera coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">X in camera coordinates</param>
    /// <param name="y">Y in camera coordinates</param>
    /// <param name="z">Z in camera coordinates</param>
    /// <param name="focalLength">Normalized focal lengths in image coordinates</param>
    /// <param name="principalPoint">Normalized principal point in image coordinates</param>
    /// <param name="zScale">Ratio of Z values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, float x, float y, float z, Vector2 focalLength, Vector2 principalPoint, float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var pixelX = ((-focalLength.x * x / z) + principalPoint.x) / 2;
      var pixelY = ((focalLength.y * y / z) + principalPoint.y) / 2;
      // Reverse the sign of Z because camera coordinate system is right-handed
      var rect = rectTransform.rect;
      return RealWorldCoordinate.GetLocalPosition(pixelX, pixelY, -z, new Vector3(rect.width, rect.height, zScale), imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="point3d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="focalLength">Focal lengths in image coordinates</param>
    /// <param name="principalPoint">Principal point in image coordinates</param>
    /// <param name="zScale">Ratio of values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, Point3D point3d, Vector2 focalLength, Vector2 principalPoint, float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPosition(rectTransform, point3d.X, point3d.Y, point3d.Z, focalLength, principalPoint, zScale, imageRotation, isMirrored);
    }

    public static bool IsXReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation270 :
        rotationAngle == RotationAngle.Rotation180 || rotationAngle == RotationAngle.Rotation270;
    }

    public static bool IsYReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation180 || rotationAngle == RotationAngle.Rotation270 :
        rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation180;
    }

    public static bool IsInverted(RotationAngle rotationAngle)
    {
      return rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation270;
    }
  }
}
