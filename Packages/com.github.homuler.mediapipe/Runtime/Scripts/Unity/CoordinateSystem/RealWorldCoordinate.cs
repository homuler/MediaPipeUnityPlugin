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
    /// <param name="x">X in real world coordinates</param>
    /// <param name="y">Y in real world coordinates</param>
    /// <param name="z">Z in real world coordinates</param>
    /// <param name="scale">Ratio of real world coordinate values to local coordinate values</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 RealWorldToLocalPoint(float x, float y, float z, Vector3 scale,
                                                RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var (rx, ry) = IsInverted(imageRotation) ? (y, x) : (x, y);
      var realX = IsXReversed(imageRotation, isMirrored) ? -rx : rx;
      var realY = IsYReversed(imageRotation, isMirrored) ? -ry : ry;
      return Vector3.Scale(new Vector3(realX, realY, z), scale);
    }

    /// <summary>
    ///   Convert from real world coordinates to Unity local coordinates.
    ///   Assume that the origin is common to the two coordinate systems.
    /// </summary>
    /// <param name="x">X in real world coordinates</param>
    /// <param name="y">Y in real world coordinates</param>
    /// <param name="z">Z in real world coordinates</param>
    /// <param name="scaleX">Ratio of real world coordinate X to local coordinate X</param>
    /// <param name="scaleY">Ratio of real world coordinate Y to local coordinate Y</param>
    /// <param name="scaleZ">Ratio of real world coordinate Z to local coordinate Z</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 RealWorldToLocalPoint(float x, float y, float z, float scaleX = 1, float scaleY = 1, float scaleZ = 1,
                                                RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return RealWorldToLocalPoint(x, y, z, new Vector3(scaleX, scaleY, scaleZ), imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="landmark" /> in the local coordinate system.
    /// </summary>
    /// <param name="scale">Ratio of real world coordinate values to local coordinate values</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetPoint(this UnityEngine.Rect _, Landmark landmark, Vector3 scale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return RealWorldToLocalPoint(landmark.X, landmark.Y, landmark.Z, scale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   When the image is rotated back, returns whether the axis parallel to the X axis of the Unity coordinates is pointing in the same direction as the X axis of the Unity coordinates.
    ///   For example, if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation90" /> and <paramRef name="isMirrored" /> is <c>false</c>, this returns <c>true</c>
    ///   because the original Y axis will be exactly opposite the X axis in Unity coordinates if the image is rotated back.
    /// </summary>
    public static bool IsXReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation90 :
        rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation180;
    }

    /// <summary>
    ///   When the image is rotated back, returns whether the axis parallel to the X axis of the Unity coordinates is pointing in the same direction as the X axis of the Unity coordinates.
    ///   For example, if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation90" /> and <paramRef name="isMirrored" /> is <c>false</c>, this returns <c>true</c>
    ///   because the original X axis will be exactly opposite the Y axis in Unity coordinates if the image is rotated back.
    /// </summary>
    public static bool IsYReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation270 :
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation90;
    }

    /// <summary>
    ///   Returns <c>true</c> if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation90" /> or <see cref="RotationAngle.Rotation270" />.
    /// </summary>
    public static bool IsInverted(RotationAngle rotationAngle)
    {
      return rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation270;
    }
  }
}
