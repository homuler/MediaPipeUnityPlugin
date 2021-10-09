// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

using mplt = Mediapipe.LocationData.Types;

namespace Mediapipe.Unity.CoordinateSystem
{
  /// <summary>
  ///   This class provides helper methods for converting from image coordinate values to local coordinate values in Unity, and vice versa.
  /// </summary>
  public static class ImageCoordinate
  {
    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">Column value in the image coordinate system</param>
    /// <param name="y">Row value in the image coordinate system</param>
    /// <param name="z">Depth value in local coordinate system</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, int x, int y, int z, Vector2 imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var rect = rectTransform.rect;
      var isInverted = IsInverted(imageRotation);
      var (rectX, rectY) = isInverted ? (y, x) : (x, y);
      var localX = ((IsXReversed(imageRotation, isMirrored) ? imageSize.x - rectX : rectX) * rect.width / imageSize.x) - (rect.width / 2);
      var localY = ((IsYReversed(imageRotation, isMirrored) ? imageSize.y - rectY : rectY) * rect.height / imageSize.y) - (rect.height / 2);
      return new Vector3(localX, localY, z);
    }

    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">Column value in the image coordinate system</param>
    /// <param name="y">Row value in the image coordinate system</param>
    /// <param name="z">Depth value in local coordinate system</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, int x, int y, int z, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPosition(rectTransform, x, y, z, new Vector2(rectTransform.rect.width, rectTransform.rect.height), imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">Column value in the image coordinate system</param>
    /// <param name="y">Row value in the image coordinate system</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPosition(RectTransform rectTransform, int x, int y, Vector2 imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPosition(rectTransform, x, y, 0, imageSize, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be outside <paramref name="rectTransform" />'s rect.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the image coordinate system</param>
    /// <param name="zScale">Ratio of Z value in image coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, float normalizedZ, float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var rect = rectTransform.rect;
      var isInverted = IsInverted(imageRotation);
      var (nx, ny) = isInverted ? (normalizedY, normalizedX) : (normalizedX, normalizedY);
      var x = IsXReversed(imageRotation, isMirrored) ? Mathf.LerpUnclamped(rect.xMax, rect.xMin, nx) : Mathf.LerpUnclamped(rect.xMin, rect.xMax, nx);
      var y = IsYReversed(imageRotation, isMirrored) ? Mathf.LerpUnclamped(rect.yMax, rect.yMin, ny) : Mathf.LerpUnclamped(rect.yMin, rect.yMax, ny);
      var z = zScale * normalizedZ;
      return new Vector3(x, y, z);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be outside <paramref name="rectTransform" />'s rect.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the image coordinate system</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, float normalizedZ, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      // Z usually uses roughly the same scale as X
      var zScale = IsInverted(imageRotation) ? rectTransform.rect.height : rectTransform.rect.width;
      return GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, normalizedZ, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be outside <paramref name="rectTransform" />'s rect.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, 0.0f, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left.
    /// </summary>
    /// <remarks>
    ///   Z values are always zero.
    /// </remarks>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="xMin">Leftmost X value</param>
    /// <param name="yMin">Topmost Y value</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, int xMin, int yMin, int width, int height, Vector2 imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var p = GetLocalPosition(rectTransform, xMin, yMin + height, imageSize, imageRotation, isMirrored);
      var q = GetLocalPosition(rectTransform, xMin + width, yMin, imageSize, imageRotation, isMirrored);
      return GetRectVertices(p, q);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedXMin">Normalized leftmost X value</param>
    /// <param name="normalizedYMin">Normalized topmost Y value</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVerticesNormalized(RectTransform rectTransform, float normalizedXMin, float normalizedYMin, float normalizedWidth, float normalizedHeight, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var p = GetLocalPositionNormalized(rectTransform, normalizedXMin, normalizedYMin + normalizedHeight, imageRotation, isMirrored);
      var q = GetLocalPositionNormalized(rectTransform, normalizedXMin + normalizedWidth, normalizedYMin, imageRotation, isMirrored);
      return GetRectVertices(p, q);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left before the rotation.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="xCenter">X value of the rectangle's center coordinate</param>
    /// <param name="yCenter">Y value of the rectangle's center coordinate</param>
    /// <param name="rotation">Clockwise rotation angle in radians</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRotatedRectVertices(RectTransform rectTransform, int xCenter, int yCenter, int width, int height, float rotation, Vector2 imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var (rectWidth, rectHeight) = IsInverted(imageRotation) ? (height, width) : (width, height);

      Vector3 center = GetLocalPosition(rectTransform, xCenter, yCenter, imageSize, imageRotation, isMirrored);
      var isRotationReversed = isInverted ^ IsXReversed(imageRotation, isMirrored) ^ IsYReversed(imageRotation, isMirrored);
      var quaternion = Quaternion.Euler(0, 0, (isRotationReversed ? -1 : 1) * Mathf.Rad2Deg * rotation);

      var bottomLeftRel = quaternion * new Vector3(-rectWidth / 2, -rectHeight / 2, 0);
      var topLeftRel = quaternion * new Vector3(-rectWidth / 2, rectHeight / 2, 0);

      return new Vector3[] {
        center + bottomLeftRel,
        center + topLeftRel,
        center - bottomLeftRel,
        center - topLeftRel,
      };
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left before the rotation.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedXCenter">X value of the rectangle's center coordinate</param>
    /// <param name="normalizedYCenter">Y value of the rectangle's center coordinate</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="rotation">Clockwise rotation angle in radians</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRotatedRectVerticesNormalized(RectTransform rectTransform, float normalizedXCenter, float normalizedYCenter, float normalizedWidth, float normalizedHeight, float rotation, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var rect = rectTransform.rect;
      var isInverted = IsInverted(imageRotation);
      var width = rect.width * (isInverted ? normalizedHeight : normalizedWidth);
      var height = rect.height * (isInverted ? normalizedWidth : normalizedHeight);

      Vector3 center = GetLocalPositionNormalized(rectTransform, normalizedXCenter, normalizedYCenter, imageRotation, isMirrored);
      var isRotationReversed = isInverted ^ IsXReversed(imageRotation, isMirrored) ^ IsYReversed(imageRotation, isMirrored);
      var quaternion = Quaternion.Euler(0, 0, (isRotationReversed ? -1 : 1) * Mathf.Rad2Deg * rotation);

      var bottomLeftRel = quaternion * new Vector3(-width / 2, -height / 2, 0);
      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);

      return new Vector3[] {
        center + bottomLeftRel,
        center + topLeftRel,
        center - bottomLeftRel,
        center - topLeftRel,
      };
    }

    private static Vector3[] GetRectVertices(Vector2 p, Vector2 q)
    {
      var leftX = Mathf.Min(p.x, q.x);
      var rightX = Mathf.Max(p.x, q.x);
      var bottomY = Mathf.Min(p.y, q.y);
      var topY = Mathf.Max(p.y, q.y);

      var bottomLeft = new Vector3(leftX, bottomY);
      var topLeft = new Vector3(leftX, topY);
      var topRight = new Vector3(rightX, topY);
      var bottomRight = new Vector3(rightX, bottomY);

      return new Vector3[] { bottomLeft, topLeft, topRight, bottomRight };
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="relativeKeypoint" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPosition(this RectTransform rectTransform, mplt.RelativeKeypoint relativeKeypoint, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPositionNormalized(rectTransform, relativeKeypoint.X, relativeKeypoint.Y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="normalizedLandmark" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, NormalizedLandmark normalizedLandmark, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPositionNormalized(rectTransform, normalizedLandmark.X, normalizedLandmark.Y, normalizedLandmark.Z, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="point2d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, NormalizedPoint2D point2d, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPositionNormalized(rectTransform, point2d.X, point2d.Y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="anchor3d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPosition(this RectTransform rectTransform, Anchor3d anchor3d, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetLocalPositionNormalized(rectTransform, anchor3d.x, anchor3d.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="anchor3d" /> in local coordinate system.
    ///   This method calculates the coordinates of the anchor so that it is projected to the correct position on the plane when viewed from the <paramref name="cameraPosition" />.
    /// </summary>
    /// <remarks>
    ///   Assume that the camera is oriented perpendicular to the plane.
    /// </remarks>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> that is attached to the target plane
    /// </param>
    /// <param name="cameraPosition">The position of the camera represented in local coordinates of the plane</param>
    /// <param name="defaultDepth">
    ///   Depth value when the anchor's Z is 1.0.
    ///   Depth here refers to the distance from the camera on the Z axis.
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, Anchor3d anchor3d, Vector3 cameraPosition, float defaultDepth, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      if (Mathf.Approximately(cameraPosition.z, 0.0f))
      {
        throw new System.ArgumentException("Z value of the camera position must not be zero");
      }

      var cameraDepth = Mathf.Abs(cameraPosition.z);
      var anchorPoint2d = rectTransform.GetLocalPosition(anchor3d, imageRotation, isMirrored);
      var anchorDepth = anchor3d.z * defaultDepth;

      // Maybe it should be defined as a CameraCoordinate method
      var x = ((anchorPoint2d.x - cameraPosition.x) * anchorDepth / cameraDepth) + cameraPosition.x;
      var y = ((anchorPoint2d.y - cameraPosition.y) * anchorDepth / cameraDepth) + cameraPosition.y;
      var z = cameraPosition.z > 0 ? cameraPosition.z - anchorDepth : cameraPosition.z + anchorDepth;
      return new Vector3(x, y, z);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="boundingBox" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, mplt.BoundingBox boundingBox, Vector2 imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetRectVertices(rectTransform, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, imageSize, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="boundingBox" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, mplt.RelativeBoundingBox boundingBox, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetRectVerticesNormalized(rectTransform, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="rect" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, Rect rect, Vector2 imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetRotatedRectVertices(rectTransform, rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation, imageSize, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="normalizedRect" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageRotation">Counterclockwise rotation angle of the input image</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, NormalizedRect normalizedRect, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return GetRotatedRectVerticesNormalized(rectTransform, normalizedRect.XCenter, normalizedRect.YCenter, normalizedRect.Width, normalizedRect.Height, normalizedRect.Rotation, imageRotation, isMirrored);
    }

    public static Vector2 GetNormalizedPosition(this RectTransform rectTransform, Vector2 localPosition, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var rect = rectTransform.rect;
      var normalizedX = IsXReversed(imageRotation, isMirrored) ? Mathf.InverseLerp(rect.width / 2, -rect.width / 2, localPosition.x) : Mathf.InverseLerp(-rect.width / 2, rect.width / 2, localPosition.x);
      var normalizedY = IsYReversed(imageRotation, isMirrored) ? Mathf.InverseLerp(rect.height / 2, -rect.height / 2, localPosition.y) : Mathf.InverseLerp(-rect.height / 2, rect.height / 2, localPosition.y);
      return IsInverted(imageRotation) ? new Vector2(normalizedY, normalizedX) : new Vector2(normalizedX, normalizedY);
    }

    /// <summary>
    ///   When the image is rotated back, returns whether the axis parallel to the X axis of the Unity coordinates is pointing in the same direction as the X axis of the Unity coordinates.
    ///   For example, if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation90" /> and <paramRef name="isMirrored" /> is <c>False</c>, this returns <c>True</c>
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
    ///   For example, if <paramref name="rotationAngle" /> is <see cref="RotationAngle.Rotation90" /> and <paramRef name="isMirrored" /> is <c>False</c>, this returns <c>True</c>
    ///   because the original X axis will be exactly opposite the Y axis in Unity coordinates if the image is rotated back.
    /// </summary>
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
