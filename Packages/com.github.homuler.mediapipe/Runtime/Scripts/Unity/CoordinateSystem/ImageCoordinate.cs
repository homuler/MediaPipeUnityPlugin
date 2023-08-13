// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

using mplt = Mediapipe.LocationData.Types;
using mptcc = Mediapipe.Tasks.Components.Containers;

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
    /// <param name="x">Column value in the image coordinate system</param>
    /// <param name="y">Row value in the image coordinate system</param>
    /// <param name="z">Depth value in the local coordinate system</param>
    /// <param name="screenWidth">
    ///   The target screen width. The returned value will be local to this screen.
    /// </param>
    /// <param name="xMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="xMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="yMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="yMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the image coordinates is mirrored</param>
    public static Vector3 ImageToLocalPoint(int x, int y, int z, float xMin, float xMax, float yMin, float yMax,
                                            int imageWidth, int imageHeight, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var (rectX, rectY) = isInverted ? (y, x) : (x, y);
      var (width, height) = (xMax - xMin, yMax - yMin);
      var localX = ((IsXReversed(imageRotation, isMirrored) ? imageWidth - rectX : rectX) * width / imageWidth) + xMin;
      var localY = ((IsYReversed(imageRotation, isMirrored) ? imageHeight - rectY : rectY) * height / imageHeight) + yMin;
      return new Vector3(localX, localY, z);
    }

    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="x">Column value in the image coordinate system</param>
    /// <param name="y">Row value in the image coordinate system</param>
    /// <param name="z">Depth value in the local coordinate system</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the image coordinates is mirrored</param>
    public static Vector3 ImageToPoint(UnityEngine.Rect rectangle, int x, int y, int z,
                                       int imageWidth, int imageHeight, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToLocalPoint(x, y, z, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, imageWidth, imageHeight, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="position">
    ///   The position in the image coordinate system.
    ///   If <c>position.z</c> is not zero, it's assumed to be the depth value in the local coordinate system.
    /// </param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the image coordinates is mirrored</param>
    public static Vector3 ImageToPoint(UnityEngine.Rect rectangle, Vector3Int position,
                                       Vector2Int imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToPoint(rectangle, position.x, position.y, position.z, imageSize.x, imageSize.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="x">Column value in the image coordinate system</param>
    /// <param name="y">Row value in the image coordinate system</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the image coordinates is mirrored</param>
    public static Vector3 ImageToPoint(UnityEngine.Rect rectangle, int x, int y,
                                       int imageWidth, int imageHeight, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToLocalPoint(x, y, 0, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, imageWidth, imageHeight, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert from image coordinates to local coordinates in Unity.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="position">The position in the image coordinate system</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the image coordinates is mirrored</param>
    public static Vector3 ImageToPoint(UnityEngine.Rect rectangle, Vector2Int position, Vector2Int imageSize,
                                       RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToPoint(rectangle, position.x, position.y, imageSize.x, imageSize.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the image coordinate system</param>
    /// <param name="xMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="xMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="yMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="yMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="zScale">Ratio of Z value in image coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToLocalPoint(float normalizedX, float normalizedY, float normalizedZ, float xMin, float xMax, float yMin, float yMax,
                                                      float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var (nx, ny) = isInverted ? (normalizedY, normalizedX) : (normalizedX, normalizedY);
      var x = IsXReversed(imageRotation, isMirrored) ? Mathf.LerpUnclamped(xMax, xMin, nx) : Mathf.LerpUnclamped(xMin, xMax, nx);
      var y = IsYReversed(imageRotation, isMirrored) ? Mathf.LerpUnclamped(yMax, yMin, ny) : Mathf.LerpUnclamped(yMin, yMax, ny);
      var z = zScale * normalizedZ;
      return new Vector3(x, y, z);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the image coordinate system</param>
    /// <param name="zScale">Ratio of Z value in image coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToPoint(UnityEngine.Rect rectangle, float normalizedX, float normalizedY, float normalizedZ,
                                                 float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToLocalPoint(normalizedX, normalizedY, normalizedZ, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="position">The position in the image coordinate system</param>
    /// <param name="zScale">Ratio of Z value in image coordinates to local coordinates in Unity</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToPoint(UnityEngine.Rect rectangle, Vector3 position,
                                                 float zScale, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, position.x, position.y, position.z, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the image coordinate system</param>
    /// <param name="xMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="xMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="yMin">The minimum X coordinate of the target rectangle</param>
    /// <param name="yMax">The maximum X coordinate of the target rectangle</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToLocalPoint(float normalizedX, float normalizedY, float normalizedZ, float xMin, float xMax, float yMin, float yMax,
                                                      RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      // Z usually uses roughly the same scale as X
      var zScale = IsInverted(imageRotation) ? (yMax - yMin) : (xMax - xMin);
      return ImageNormalizedToLocalPoint(normalizedX, normalizedY, normalizedZ, xMin, xMax, yMin, yMax, zScale, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the image coordinate system</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToPoint(UnityEngine.Rect rectangle, float normalizedX, float normalizedY, float normalizedZ,
                                                 RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToLocalPoint(normalizedX, normalizedY, normalizedZ, rectangle.xMin, rectangle.xMax, rectangle.yMin, rectangle.yMax, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="position">The position in the image coordinate system</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToPoint(UnityEngine.Rect rectangle, Vector3 position, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, position.x, position.y, position.z, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="normalizedX">Normalized x value in the image coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the image coordinate system</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToPoint(UnityEngine.Rect rectangle, float normalizedX, float normalizedY,
                                                 RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, normalizedX, normalizedY, 0, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the image coordinate system to local coordinate values in Unity.
    ///   If the normalized values are out of [0, 1], the return value will be off the target screen.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="position">The position in the image coordinate system</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 ImageNormalizedToPoint(UnityEngine.Rect rectangle, Vector2 position, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, position.x, position.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Returns a <see cref="Vector3" /> array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left.
    /// </summary>
    /// <remarks>
    ///   Z values are always zero.
    /// </remarks>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="xMin">Leftmost X value</param>
    /// <param name="yMin">Topmost Y value</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] ImageToRectVertices(UnityEngine.Rect rectangle, int xMin, int yMin, int width, int height,
                                                int imageWidth, int imageHeight, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var p = ImageToPoint(rectangle, xMin, yMin + height, imageWidth, imageHeight, imageRotation, isMirrored);
      var q = ImageToPoint(rectangle, xMin + width, yMin, imageWidth, imageHeight, imageRotation, isMirrored);
      return GetRectVertices(p, q);
    }

    /// <summary>
    ///   Returns a <see cref="Vector3" /> array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left.
    /// </summary>
    /// <remarks>
    ///   Z values are always zero.
    /// </remarks>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="xMin">Leftmost X value</param>
    /// <param name="yMin">Topmost Y value</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] ImageToRectVertices(UnityEngine.Rect rectangle, int xMin, int yMin, int width, int height,
                                                Vector2Int imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, xMin, yMin, width, height, imageSize.x, imageSize.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Returns a <see cref="Vector3" /> array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left.
    /// </summary>
    /// <remarks>
    ///   Z values are always zero.
    /// </remarks>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="normalizedXMin">Normalized leftmost X value</param>
    /// <param name="normalizedYMin">Normalized topmost Y value</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] ImageNormalizedToRectVertices(UnityEngine.Rect rectangle, float normalizedXMin, float normalizedYMin, float normalizedWidth, float normalizedHeight,
                                                          RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var p = ImageNormalizedToPoint(rectangle, normalizedXMin, normalizedYMin + normalizedHeight, imageRotation, isMirrored);
      var q = ImageNormalizedToPoint(rectangle, normalizedXMin + normalizedWidth, normalizedYMin, imageRotation, isMirrored);
      return GetRectVertices(p, q);
    }

    /// <summary>
    ///   Returns a <see cref="Vector3" /> array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left before the rotation.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="xCenter">X value of the rectangle's center coordinate</param>
    /// <param name="yCenter">Y value of the rectangle's center coordinate</param>
    /// <param name="rotation">Clockwise rotation angle in radians</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] ImageToRectVertices(UnityEngine.Rect rectangle, int xCenter, int yCenter, int width, int height, float rotation,
                                                int imageWidth, int imageHeight, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var (rectWidth, rectHeight) = IsInverted(imageRotation) ? (height, width) : (width, height);

      var center = ImageToPoint(rectangle, xCenter, yCenter, imageWidth, imageHeight, imageRotation, isMirrored);
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
    ///   Returns a <see cref="Vector3" /> array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left before the rotation.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="xCenter">X value of the rectangle's center coordinate</param>
    /// <param name="yCenter">Y value of the rectangle's center coordinate</param>
    /// <param name="rotation">Clockwise rotation angle in radians</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] ImageToRectVertices(UnityEngine.Rect rectangle, int xCenter, int yCenter, int width, int height, float rotation,
                                                Vector2Int imageSize, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, xCenter, yCenter, width, height, rotation, imageSize.x, imageSize.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Returns a <see cref="Vector3" /> array which represents a rectangle's vertices.
    ///   They are in clockwise order, starting from the coordinate that was in the bottom-left before the rotation.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="normalizedXCenter">X value of the rectangle's center coordinate</param>
    /// <param name="normalizedYCenter">Y value of the rectangle's center coordinate</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="rotation">Clockwise rotation angle in radians</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] ImageNormalizedToRectVertices(UnityEngine.Rect rectangle, float normalizedXCenter, float normalizedYCenter, float normalizedWidth, float normalizedHeight,
                                                          float rotation, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var isInverted = IsInverted(imageRotation);
      var width = rectangle.width * (isInverted ? normalizedHeight : normalizedWidth);
      var height = rectangle.height * (isInverted ? normalizedWidth : normalizedHeight);

      var center = ImageNormalizedToPoint(rectangle, normalizedXCenter, normalizedYCenter, imageRotation, isMirrored);
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
    ///   Get the coordinates represented by <paramref name="relativeKeypoint" /> in the local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetPoint(this UnityEngine.Rect rectangle, mplt.RelativeKeypoint relativeKeypoint,
                                   RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, relativeKeypoint.X, relativeKeypoint.Y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="normalizedKeypoint" /> in the local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetPoint(this UnityEngine.Rect rectangle, mptcc.NormalizedKeypoint normalizedKeypoint,
                                   RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, normalizedKeypoint.x, normalizedKeypoint.y, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="normalizedLandmark" /> in the local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetPoint(this UnityEngine.Rect rectangle, NormalizedLandmark normalizedLandmark,
                                   RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, normalizedLandmark.X, normalizedLandmark.Y, normalizedLandmark.Z, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="normalizedLandmark" /> in the local coordinate system.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetPoint(this UnityEngine.Rect rectangle, in mptcc.NormalizedLandmark normalizedLandmark,
                                   RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToPoint(rectangle, normalizedLandmark.x, normalizedLandmark.y, normalizedLandmark.z, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="boundingBox" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, mplt.BoundingBox boundingBox, int imageWidth, int imageHeight,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, imageWidth, imageHeight, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="boundingBox" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, mplt.BoundingBox boundingBox, Vector2Int imageSize,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, imageSize, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="boundingBox" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, mplt.RelativeBoundingBox boundingBox,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToRectVertices(rectangle, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="rect" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, mptcc.Rect rect, int imageWidth, int imageHeight,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, imageWidth, imageHeight, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="rect" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, mptcc.Rect rect, Vector2Int imageSize,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top, imageSize, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="rect" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageWidth">Image width in pixels</param>
    /// <param name="imageHeight">Image width in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, Rect rect, int imageWidth, int imageHeight,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation, imageWidth, imageHeight, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="rect" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, Rect rect, Vector2Int imageSize,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageToRectVertices(rectangle, rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation, imageSize, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="normalizedRect" />'s vertex coordinates in the local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this UnityEngine.Rect rectangle, NormalizedRect normalizedRect,
                                            RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      return ImageNormalizedToRectVertices(rectangle, normalizedRect.XCenter, normalizedRect.YCenter, normalizedRect.Width, normalizedRect.Height, normalizedRect.Rotation, imageRotation, isMirrored);
    }

    /// <summary>
    ///   Get the image normalized point corresponding to <paramref name="localPosition" />.
    /// </summary>
    /// <param name="rectangle">Rectangle to get a point inside</param>
    /// <param name="imageRotation">
    ///   Counterclockwise rotation angle of the input image in the image coordinate system.
    ///   In the local coordinate system, this value will often represent a clockwise rotation angle.
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 PointToImageNormalized(this UnityEngine.Rect rectangle, Vector2 localPosition, RotationAngle imageRotation = RotationAngle.Rotation0, bool isMirrored = false)
    {
      var normalizedX = IsXReversed(imageRotation, isMirrored) ?
          Mathf.InverseLerp(rectangle.width / 2, -rectangle.width / 2, localPosition.x) :
          Mathf.InverseLerp(-rectangle.width / 2, rectangle.width / 2, localPosition.x);
      var normalizedY = IsYReversed(imageRotation, isMirrored) ?
          Mathf.InverseLerp(rectangle.height / 2, -rectangle.height / 2, localPosition.y) :
          Mathf.InverseLerp(-rectangle.height / 2, rectangle.height / 2, localPosition.y);
      return IsInverted(imageRotation) ? new Vector2(normalizedY, normalizedX) : new Vector2(normalizedX, normalizedY);
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
