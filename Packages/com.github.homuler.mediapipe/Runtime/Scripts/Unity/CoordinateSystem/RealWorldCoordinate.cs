// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.CoordinateSystem
{
  /// <summary>
  ///   This class provides helper methods for converting real-world coordinate values to local coordinate values.
  ///   In real-world coordinate system, X axis is toward the right, Y axis is toward the bottom, and Z axis is toward the back.
  /// </summary>
  public static class RealWorldCoordinate
  {
    /// <summary>
    ///   Convert from real world coordinates to Unity local coordinates.
    ///   Assume that the origin is common to the two coordinate systems.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">X in real world coordinates</param>
    /// <param name="y">Y in real world coordinates</param>
    /// <param name="z">Z in real world coordinates</param>
    /// <param name="scale">Ratio of real world coordinate values to local coordinate values</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(float x, float y, float z, Vector3 scale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var (rx, ry) = IsInverted(imageRotation) ? (y, x) : (x, y);
      var realX = IsXReversed(imageRotation, isMirrored) ? -rx : rx;
      var realY = IsYReversed(imageRotation, isMirrored) ? -ry : ry;
      return Vector3.Scale(new Vector3(realX, realY, z), scale);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="landmark" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="scale">Ratio of real world coordinate values to local coordinate values</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform _, Landmark landmark, Vector3 scale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPosition(landmark.X, landmark.Y, landmark.Z, scale, imageRotation, isMirrored);
    }

    public static bool IsXReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation90 :
        rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation180;
    }

    public static bool IsYReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation270 :
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation90;
    }

    public static bool IsInverted(RotationAngle rotationAngle)
    {
      return rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation270;
    }
  }
}
