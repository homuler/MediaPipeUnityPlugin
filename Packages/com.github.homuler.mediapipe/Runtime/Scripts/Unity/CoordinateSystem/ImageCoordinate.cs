using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity.CoordinateSystem {
  /// <summary>
  ///   This class provides helper methods for converting from image coordinate values to local coordinate values in Unity, and vice versa.
  /// </summary>
  public static class ImageCoordinate {
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, int x, int y, int z, Vector2 imageSize, bool isMirrored = false) {
      var rect = rectTransform.rect;
      var localX = (isMirrored ? imageSize.x - x : x) * rect.width / imageSize.x;
      var localY = (imageSize.y - y) * rect.height / imageSize.y;
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, int x, int y, int z, bool isMirrored = false) {
      return GetLocalPosition(rectTransform, x, y, z, new Vector2(rectTransform.rect.width, rectTransform.rect.height), isMirrored);
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPosition(RectTransform rectTransform, int x, int y, Vector2 imageSize, bool isMirrored = false) {
      return GetLocalPosition(rectTransform, x, y, 0, imageSize, isMirrored);
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, float normalizedZ, float zScale, bool isMirrored = false) {
      var rect = rectTransform.rect;

      var x = isMirrored ? Mathf.LerpUnclamped(rect.xMax, rect.xMin, normalizedX) : Mathf.LerpUnclamped(rect.xMin, rect.xMax, normalizedX);
      var y = Mathf.LerpUnclamped(rect.yMax, rect.yMin, normalizedY);
      // z usually uses roughly the same scale as x
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, float normalizedZ, bool isMirrored = false) {
      // Z usually uses roughly the same scale as X
      return GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, normalizedZ, rectTransform.rect.width, isMirrored);
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, 0.0f, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices, being ordered clockwise from bottom-left point.
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, int xMin, int yMin, int width, int height, Vector2 imageSize, bool isMirrored = false) {
      var bottomLeft = GetLocalPosition(rectTransform, xMin, yMin + height, imageSize, isMirrored);
      var topRight = GetLocalPosition(rectTransform, xMin + width, yMin, imageSize, isMirrored);

      return GetRectVertices(bottomLeft, topRight, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices, being ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedXMin">Normalized leftmost X value</param>
    /// <param name="normalizedYMin">Normalized topmost Y value</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVerticesNormalized(RectTransform rectTransform, float normalizedXMin, float normalizedYMin, float normalizedWidth, float normalizedHeight, bool isMirrored = false) {
      var bottomLeft = GetLocalPositionNormalized(rectTransform, normalizedXMin, normalizedYMin + normalizedHeight, isMirrored);
      var topRight = GetLocalPositionNormalized(rectTransform, normalizedXMin + normalizedWidth, normalizedYMin, isMirrored);

      return GetRectVertices(bottomLeft, topRight, isMirrored);
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRotatedRectVertices(RectTransform rectTransform, int xCenter, int yCenter, int width, int height, float rotation, Vector2 imageSize, bool isMirrored = false) {
      Vector3 center = GetLocalPosition(rectTransform, xCenter, yCenter, imageSize, isMirrored);
      var quaternion = Quaternion.Euler(0, 0, (isMirrored ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var bottomLeftRel = quaternion * new Vector3(-width / 2, -height / 2, 0);
      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);

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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRotatedRectVerticesNormalized(RectTransform rectTransform, float normalizedXCenter, float normalizedYCenter, float normalizedWidth, float normalizedHeight, float rotation, bool isMirrored = false) {
      Vector3 center = GetLocalPositionNormalized(rectTransform, normalizedXCenter, normalizedYCenter, isMirrored);
      var quaternion = Quaternion.Euler(0, 0, (isMirrored ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var rect = rectTransform.rect;
      var width = rect.width * normalizedWidth;
      var height = rect.height * normalizedHeight;
      var bottomLeftRel = quaternion * new Vector3(-width / 2, -height / 2, 0);
      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);

      return new Vector3[] {
        center + bottomLeftRel,
        center + topLeftRel,
        center - bottomLeftRel,
        center - topLeftRel,
      };
    }

    static Vector3[] GetRectVertices(Vector2 bottomLeft, Vector2 topRight, bool isMirrored = false) {
      var topLeft = new Vector3(bottomLeft.x, topRight.y);
      var bottomRight = new Vector3(topRight.x, bottomLeft.y);

      if (!isMirrored) {
        return new Vector3[] { bottomLeft, topLeft, topRight, bottomRight };
      }
      return new Vector3[] { bottomRight, topRight, topLeft, bottomLeft };
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="relativeKeypoint" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPosition(this RectTransform rectTransform, mplt.RelativeKeypoint relativeKeypoint, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, relativeKeypoint.X, relativeKeypoint.Y, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="normalizedLandmark" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, NormalizedLandmark normalizedLandmark, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, normalizedLandmark.X, normalizedLandmark.Y, normalizedLandmark.Z, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="point2d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, NormalizedPoint2D point2d, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, point2d.X, point2d.Y, isMirrored);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="anchor3d" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector2 GetLocalPosition(this RectTransform rectTransform, Anchor3d anchor3d, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, anchor3d.X, anchor3d.Y, isMirrored);
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, Anchor3d anchor3d, Vector3 cameraPosition, float defaultDepth, bool isMirrored = false) {
      if (Mathf.Approximately(cameraPosition.z, 0.0f)) {
        throw new System.ArgumentException("Z value of the camera position must not be zero");
      }

      var cameraDepth = Mathf.Abs(cameraPosition.z);
      var anchorPoint2d = rectTransform.GetLocalPosition(anchor3d, isMirrored);
      var anchorDepth = anchor3d.Z * defaultDepth;

      // Maybe it should be defined as a CameraCoordinate method
      var x = (anchorPoint2d.x - cameraPosition.x) * anchorDepth / cameraDepth + cameraPosition.x;
      var y = (anchorPoint2d.y - cameraPosition.y) * anchorDepth / cameraDepth + cameraPosition.y;
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, mplt.BoundingBox boundingBox, Vector2 imageSize, bool isMirrored = false) {
      return GetRectVertices(rectTransform, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, imageSize, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="boundingBox" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, mplt.RelativeBoundingBox boundingBox, bool isMirrored = false) {
      return GetRectVerticesNormalized(rectTransform, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="rect" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="imageSize">Image size in pixels</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, Rect rect, Vector2 imageSize, bool isMirrored = false) {
      return GetRotatedRectVertices(rectTransform, rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation, imageSize, isMirrored);
    }

    /// <summary>
    ///   Get a Vector3 array which represents <paramref name="normalizedRect" />'s vertex coordinates in local coordinate system.
    ///   They are ordered clockwise from bottom-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(this RectTransform rectTransform, NormalizedRect normalizedRect, bool isMirrored = false) {
      return GetRotatedRectVerticesNormalized(rectTransform, normalizedRect.XCenter, normalizedRect.YCenter, normalizedRect.Width, normalizedRect.Height, normalizedRect.Rotation, isMirrored);
    }

    public static Vector2 GetNormalizedPosition(this RectTransform rectTransform, Vector2 localPosition, bool isMirrored = false) {
      var rect = rectTransform.rect;
      var normalizedX = isMirrored ? Mathf.InverseLerp(rect.width / 2, -rect.width / 2, localPosition.x) : Mathf.InverseLerp(-rect.width / 2, rect.width / 2, localPosition.x);
      var normalizedY = Mathf.InverseLerp(rect.height / 2, -rect.height / 2, localPosition.y);
      return new Vector2(normalizedX, normalizedY);
    }
  }
}
