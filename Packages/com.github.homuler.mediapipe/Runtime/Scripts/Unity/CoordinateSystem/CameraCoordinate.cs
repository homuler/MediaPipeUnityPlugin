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
    public static Vector3 CameraToRealWorld(float x, float y, float z, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var (rx, ry) = isInverted ? (y, x) : (x, y);
      return new Vector3(IsXReversed(imageRotation, isMirrored) ? -rx : rx, IsYReversed(imageRotation, isMirrored) ? -ry : ry, -z);
    }

    /// <summary>
    ///   Convert from camera coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="x">X in camera coordinates</param>
    /// <param name="y">Y in camera coordinates</param>
    /// <param name="z">Z in camera coordinates</param>
    /// <param name="xMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="xMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="yMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="yMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="focalLengthX">Normalized focal length X in NDC space</param>
    /// <param name="focalLengthY">Normalized focal length Y in NDC space</param>
    /// <param name="principalX">Normalized principal point X in NDC space</param>
    /// <param name="principalY">Normalized principal point Y in NDC space</param>
    /// <param name="zScale">Ratio of Z values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 CameraToLocalPoint(float x, float y, float z, float xMin, float xMax, float yMin, float yMax,
                                             float focalLengthX, float focalLengthY, float principalX, float principalY,
                                             float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var (ndcX, ndcY) = ((-focalLengthX * x / z) + principalX, (focalLengthY * y / z) + principalY);
      var (width, height) = (xMax - xMin, yMax - yMin);
      var (nx, ny) = ((1 + ndcX) / 2.0f, (1 - ndcY) / 2.0f);
      var (rectX, rectY) = IsInverted(imageRotation) ? (ny * width, nx * height) : (nx * width, ny * height);
      var localX = (IsXReversed(imageRotation, isMirrored) ? width - rectX : rectX) + xMin;
      var localY = (IsYReversed(imageRotation, isMirrored) ? height - rectY : rectY) + yMin;
      // Reverse the sign of Z because camera coordinate system is right-handed
      var localZ = -z * zScale;

      return new Vector3(localX, localY, localZ);
    }

    /// <summary>
    ///   Convert from camera coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="x">X in camera coordinates</param>
    /// <param name="y">Y in camera coordinates</param>
    /// <param name="z">Z in camera coordinates</param>
    /// <param name="xMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="xMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="yMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="yMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="focalLength">Normalized focal lengths in NDC space</param>
    /// <param name="principalPoint">Normalized principal point in NDC space</param>
    /// <param name="zScale">Ratio of Z values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 CameraToLocalPoint(float x, float y, float z, float xMin, float xMax, float yMin, float yMax, Vector2 focalLength, Vector2 principalPoint,
                                             float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToLocalPoint(x, y, z, xMin, xMax, yMin, yMax, focalLength.x, focalLength.y, principalPoint.x, principalPoint.y, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from camera coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="x">X in camera coordinates</param>
    /// <param name="y">Y in camera coordinates</param>
    /// <param name="z">Z in camera coordinates</param>
    /// <param name="focalLengthX">Normalized focal length X in NDC space</param>
    /// <param name="focalLengthY">Normalized focal length Y in NDC space</param>
    /// <param name="principalX">Normalized principal point X in NDC space</param>
    /// <param name="principalY">Normalized principal point Y in NDC space</param>
    /// <param name="zScale">Ratio of Z values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 CameraToPoint(UnityEngine.Rect rectangle, float x, float y, float z,
                                        float focalLengthX, float focalLengthY, float principalX, float principalY,
                                        float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToLocalPoint(x, y, z, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, focalLengthX, focalLengthY, principalX, principalY,
                                zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from camera coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="x">X in camera coordinates</param>
    /// <param name="y">Y in camera coordinates</param>
    /// <param name="z">Z in camera coordinates</param>
    /// <param name="focalLength">Normalized focal lengths in NDC space</param>
    /// <param name="principalPoint">Normalized principal point in NDC space</param>
    /// <param name="zScale">Ratio of Z values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 CameraToPoint(UnityEngine.Rect rectangle, float x, float y, float z,
                                        Vector2 focalLength, Vector2 principalPoint,
                                        float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToLocalPoint(x, y, z, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, focalLength.x, focalLength.y, principalPoint.x, principalPoint.y,
                                zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from camera coordinates to local coordinates in Unity.
    ///   It is assumed that the principal point is (0, 0) in the camera coordinate system and the focal length is (1, 1).
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="x">X in camera coordinates</param>
    /// <param name="y">Y in camera coordinates</param>
    /// <param name="z">Z in camera coordinates</param>
    /// <param name="zScale">Ratio of Z values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 CameraToPoint(UnityEngine.Rect rectangle, float x, float y, float z,
                                        float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToLocalPoint(x, y, z, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, Vector2.one, Vector2.zero, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="point3d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="focalLengthX">Normalized focal length X in NDC space</param>
    /// <param name="focalLengthY">Normalized focal length Y in NDC space</param>
    /// <param name="principalX">Normalized principal point X in NDC space</param>
    /// <param name="principalY">Normalized principal point Y in NDC space</param>
    /// <param name="zScale">Ratio of values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetPoint(this UnityEngine.Rect rectangle, Point3D point3d, float focalLengthX, float focalLengthY, float principalX, float principalY,
                                   float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToPoint(rectangle, point3d.X, point3d.Y, point3d.Z, focalLengthX, focalLengthY, principalX, principalY, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="point3d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="focalLength">Normalized focal lengths in NDC space</param>
    /// <param name="principalPoint">Normalized principal point in NDC space</param>
    /// <param name="zScale">Ratio of values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetPoint(this UnityEngine.Rect rectangle, Point3D point3d, Vector2 focalLength, Vector2 principalPoint,
                                   float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToPoint(rectangle, point3d.X, point3d.Y, point3d.Z, focalLength, principalPoint, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="point3d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="zScale">Ratio of values in camera coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetPoint(this UnityEngine.Rect rectangle, Point3D point3d,
                                   float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return CameraToPoint(rectangle, point3d.X, point3d.Y, point3d.Z, zScale, imageRotation, isMirrored);
    }

    public static Quaternion GetApproximateQuaternion(ObjectAnnotation objectAnnotation, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var isXReversed = IsXReversed(imageRotation, isMirrored);
      var isYReversed = IsYReversed(imageRotation, isMirrored);
      var forward = GetZDir(objectAnnotation, isXReversed, isYReversed, isInverted);
      var upward = GetYDir(objectAnnotation, isXReversed, isYReversed, isInverted);

      return Quaternion.LookRotation(forward, upward);
    }

    public static (Vector3, Vector3, Vector3) GetDirections(ObjectAnnotation objectAnnotation, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var isXReversed = IsXReversed(imageRotation, isMirrored);
      var isYReversed = IsYReversed(imageRotation, isMirrored);
      var scale = objectAnnotation.Scale;
      var xDir = scale[0] * GetXDir(objectAnnotation, isXReversed, isYReversed, isInverted);
      var yDir = scale[1] * GetYDir(objectAnnotation, isXReversed, isYReversed, isInverted);
      var zDir = scale[2] * GetZDir(objectAnnotation, isXReversed, isYReversed, isInverted);
      return (xDir, yDir, zDir);
    }

    private static Vector3 GetXDir(ObjectAnnotation objectAnnotation, bool isXReversed, bool isYReversed, bool isInverted)
    {
      var points = objectAnnotation.Keypoints;
      var v1 = GetDirection(points[1].Point3D, points[5].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v2 = GetDirection(points[2].Point3D, points[6].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v3 = GetDirection(points[3].Point3D, points[7].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v4 = GetDirection(points[4].Point3D, points[8].Point3D, isXReversed, isYReversed, isInverted).normalized;
      return (v1 + v2 + v3 + v4) / 4;
    }

    private static Vector3 GetYDir(ObjectAnnotation objectAnnotation, bool isXReversed, bool isYReversed, bool isInverted)
    {
      var points = objectAnnotation.Keypoints;
      var v1 = GetDirection(points[1].Point3D, points[3].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v2 = GetDirection(points[2].Point3D, points[4].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v3 = GetDirection(points[5].Point3D, points[7].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v4 = GetDirection(points[6].Point3D, points[8].Point3D, isXReversed, isYReversed, isInverted).normalized;
      return (v1 + v2 + v3 + v4) / 4;
    }

    private static Vector3 GetZDir(ObjectAnnotation objectAnnotation, bool isXReversed, bool isYReversed, bool isInverted)
    {
      var points = objectAnnotation.Keypoints;
      var v1 = GetDirection(points[2].Point3D, points[1].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v2 = GetDirection(points[4].Point3D, points[3].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v3 = GetDirection(points[6].Point3D, points[5].Point3D, isXReversed, isYReversed, isInverted).normalized;
      var v4 = GetDirection(points[8].Point3D, points[7].Point3D, isXReversed, isYReversed, isInverted).normalized;
      return (v1 + v2 + v3 + v4) / 4;
    }

    private static Vector3 GetDirection(Point3D from, Point3D to, bool isXReversed, bool isYReversed, bool isInverted)
    {
      var xDiff = to.X - from.X;
      var yDiff = to.Y - from.Y;
      var (xDir, yDir) = isInverted ? (yDiff, xDiff) : (xDiff, yDiff);
      // convert from right-handed to left-handed
      return new Vector3(isXReversed ? -xDir : xDir, isYReversed ? -yDir : yDir, from.Z - to.Z);
    }

    /// <summary>
    ///   When the image is rotated back, returns whether the axis parallel to the X axis of the Unity coordinates is pointing in the same direction as the X axis of the Unity coordinates.
    ///   For example, if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation180" /> and <paramRef name="isMirrored" /> is <c>false</c>, this returns <c>true</c>
    ///   because the original Y axis will be exactly opposite the X axis in Unity coordinates if the image is rotated back.
    /// </summary>
    public static bool IsXReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation0 || rotationAngle == RotationAngle.Rotation270 :
        rotationAngle == RotationAngle.Rotation180 || rotationAngle == RotationAngle.Rotation270;
    }

    /// <summary>
    ///   When the image is rotated back, returns whether the axis parallel to the X axis of the Unity coordinates is pointing in the same direction as the X axis of the Unity coordinates.
    ///   For example, if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation180" /> and <paramRef name="isMirrored" /> is <c>false</c>, this returns <c>true</c>
    ///   because the original X axis will be exactly opposite the Y axis in Unity coordinates if the image is rotated back.
    /// </summary>
    public static bool IsYReversed(RotationAngle rotationAngle, bool isMirrored = false)
    {
      return isMirrored ?
        rotationAngle == RotationAngle.Rotation180 || rotationAngle == RotationAngle.Rotation270 :
        rotationAngle == RotationAngle.Rotation90 || rotationAngle == RotationAngle.Rotation180;
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
