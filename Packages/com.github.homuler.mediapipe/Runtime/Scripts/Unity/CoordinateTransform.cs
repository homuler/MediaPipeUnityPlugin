using System;
using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity {
  public static class CoordinateTransform {
    /// <summary>
    ///   Convert values in the MediaPipe coordinate system to local coordinate values in Unity.
    ///   X and Y represent the positions in pixels and (0, 0) is located at top left corner of the <paramref name="rectTransform" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">X value in the MediaPipe's coordinate system</param>
    /// <param name="y">Y value in the MediaPipe's coordinate system</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, int x, int y, bool isMirrored = false) {
      var rect = rectTransform.rect;
      return new Vector3(isMirrored ? rect.width - x : x, rect.height - y, 0);
    }

    /// <summary>
    ///   Convert values in the MediaPipe coordinate system to local coordinate values in Unity.
    ///   (0, 0, 0) is located in the middle of the <paramref name="rectTransform" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">X value in the MediaPipe's coordinate system</param>
    /// <param name="y">Y value in the MediaPipe's coordinate system</param>
    /// <param name="z">Z value in the MediaPipe's coordinate system</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, float x, float y, float z, Vector3 scale, bool isMirrored = false) {
      return Vector3.Scale(new Vector3(isMirrored ? - x : x, - y, z), scale); // + rectTransform.localPosition;
    }

    /// <summary>
    ///   Convert values in the MediaPipe coordinate system to local coordinate values in Unity.
    ///   (0, 0) is located in the middle of the <paramref name="rectTransform" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="x">X value in the MediaPipe's coordinate system</param>
    /// <param name="y">Y value in the MediaPipe's coordinate system</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, float x, float y, Vector3 scale, bool isMirrored = false) {
      return GetLocalPosition(rectTransform, x, y, 0, scale, isMirrored);
    }

    /// <summary>
    ///   Convert normalized values in the MediaPipe coordinate system to local coordinate values in Unity.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedX">Normalized x value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedZ">Normalized z value in the MediaPipe's coordinate system</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, float normalizedZ, bool isMirrored = false) {
      var rect = rectTransform.rect;

      var x = isMirrored ? Mathf.Lerp(rect.xMax, rect.xMin, normalizedX) : Mathf.Lerp(rect.xMin, rect.xMax, normalizedX);
      var y = Mathf.Lerp(rect.yMax, rect.yMin, normalizedY);
      // z usually uses roughly the same scale as x
      var z = rect.width * normalizedZ;

      return new Vector3(x, y, z) + rectTransform.localPosition;
    }

    /// <summary>
    ///   Convert normalized values in the MediaPipe coordinate system to local coordinate values in Unity.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedX">Normalized x value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedY">Normalized y value in the MediaPipe's coordinate system</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPositionNormalized(RectTransform rectTransform, float normalizedX, float normalizedY, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, 0.0f, isMirrored);
    }

    /// <summary>
    ///   Calculate the local coordinates of <paramref name="relativeKeypoint" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, mplt.RelativeKeypoint relativeKeypoint, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, relativeKeypoint.X, relativeKeypoint.Y, isMirrored);
    }

    /// <summary>
    ///   Calculate the local coordinates of <paramref name="landmark" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, Landmark landmark, Vector3 scale, bool isMirrored = false) {
      return GetLocalPosition(rectTransform, landmark.X, landmark.Y, landmark.Z, scale, isMirrored);
    }

    /// <summary>
    ///   Calculate the local coordinates of <paramref name="landmark" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, NormalizedLandmark normalizedLandmark, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, normalizedLandmark.X, normalizedLandmark.Y, normalizedLandmark.Z, isMirrored);
    }

    /// <summary>
    ///   Calculate the local coordinates of <paramref name="point2d" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, NormalizedPoint2D point2d, bool isMirrored = false) {
      return GetLocalPositionNormalized(rectTransform, point2d.X, point2d.Y, isMirrored);
    }

    /// <summary>
    ///   Calculate the local coordinates of <paramref name="point3d" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="dimension">Input image's dimension. <see cref="Vector3.z" /> will be used to scale z value.</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, Point3D point3d, Vector2 focalLength, Vector2 principalPoint, Vector3 dimension, bool isMirrored = false) {
      var x = -focalLength.x * point3d.X / point3d.Z + principalPoint.x;
      var y = focalLength.y * point3d.Y / point3d.Z + principalPoint.y;
      // camera coordinate system is right-handed
      return GetLocalPosition(rectTransform, x, y, -point3d.Z, dimension, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    ///   X and Y represent the positions in pixels and (0, 0) is located at top left corner of the <paramref name="rectTransform" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="xMin">Left x value in the MediaPipe's coordinate system</param>
    /// <param name="yMin">Top y value in the MediaPipe's coordinate system</param>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, int xMin, int yMin, int width, int height, bool isMirrored = false) {
      var topLeft = GetLocalPosition(rectTransform, xMin, yMin, isMirrored);
      var bottomRight = GetLocalPosition(rectTransform, xMin + width, yMin + height, isMirrored);

      return GetRectVertices(topLeft, bottomRight);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedXMin">Normalized left x value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedYMin">Normalized top y value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVerticesNormalized(RectTransform rectTransform, float normalizedXMin, float normalizedYMin, float normalizedWidth, float normalizedHeight, bool isMirrored = false) {
      var topLeft = GetLocalPositionNormalized(rectTransform, normalizedXMin, normalizedYMin, isMirrored);
      var bottomRight = GetLocalPositionNormalized(rectTransform, normalizedXMin + normalizedWidth, normalizedYMin + normalizedHeight, isMirrored);

      return GetRectVertices(topLeft, bottomRight);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    ///   X and Y represent the positions in pixels and (0, 0) is located at top left corner of the <paramref name="rectTransform" />.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="xCenter">Center x value in the MediaPipe's coordinate system</param>
    /// <param name="yCenter">center y value in the MediaPipe's coordinate system</param>
    /// <param name="width">width</param>
    /// <param name="height">height</param>
    /// <param name="rotation">Rotation angle in radians (clockwise)</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, int xCenter, int yCenter, int width, int height, float rotation, bool isMirrored = false) {
      var center = GetLocalPosition(rectTransform, xCenter, yCenter, isMirrored);
      var quaternion = Quaternion.Euler(0, 0, (isMirrored ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);
      var topRightRel = quaternion * new Vector3(width / 2, height / 2, 0);

      return new Vector3[] {
        center + topLeftRel,
        center + topRightRel,
        center - topLeftRel,
        center - topRightRel,
      };
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedXCenter">Normalized center x value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedYCenter">Normalized center y value in the MediaPipe's coordinate system</param>
    /// <param name="normalizedWidth">Normalized width</param>
    /// <param name="normalizedHeight">Normalized height</param>
    /// <param name="rotation">Rotation angle in radians (clockwise)</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVerticesNormalized(RectTransform rectTransform, float normalizedXCenter, float normalizedYCenter, float normalizedWidth, float normalizedHeight, float rotation, bool isMirrored = false) {
      var center = GetLocalPositionNormalized(rectTransform, normalizedXCenter, normalizedYCenter, isMirrored);
      var quaternion = Quaternion.Euler(0, 0, (isMirrored ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var rect = rectTransform.rect;
      var width = rect.width * normalizedWidth;
      var height = rect.height * normalizedHeight;
      var topLeftRel = quaternion * new Vector3(-width / 2, height / 2, 0);
      var topRightRel = quaternion * new Vector3(width / 2, height / 2, 0);

      return new Vector3[] {
        center + topLeftRel,
        center + topRightRel,
        center - topLeftRel,
        center - topRightRel,
      };
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="boundingBox">
    ///   <see cref="Mediapipe.LocationData.Types.BoundingBox" /> in the MediaPipe's coordinate system
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, mplt.BoundingBox boundingBox, bool isMirrored = false) {
      return GetRectVertices(rectTransform, boundingBox.Xmin, boundingBox.Ymin, boundingBox.Width, boundingBox.Height, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="relativeBoundingBox">
    ///   <see cref="Mediapipe.LocationData.Types.RelativeBoundingBox" /> in the MediaPipe's coordinate system
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, mplt.RelativeBoundingBox relativeBoundingBox, bool isMirrored = false) {
      return GetRectVerticesNormalized(rectTransform, relativeBoundingBox.Xmin, relativeBoundingBox.Ymin, relativeBoundingBox.Width, relativeBoundingBox.Height, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="locationData">
    ///   <see cref="LocationData" /> in the MediaPipe's coordinate system
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, LocationData locationData, bool isMirrored = false) {
      switch (locationData.Format) {
        case mplt.Format.BoundingBox: {
          return GetRectVertices(rectTransform, locationData.BoundingBox, isMirrored);
        }
        case mplt.Format.RelativeBoundingBox: {
          return GetRectVertices(rectTransform, locationData.RelativeBoundingBox, isMirrored);
        }
        default: {
          throw new ArgumentException($"The format of locationData isn't BoundingBox but {locationData.Format}");
        }
      }
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="rect">
    ///   <see cref="Rect" /> in the MediaPipe's coordinate system
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, Rect rect, bool isMirrored = false) {
      return GetRectVertices(rectTransform, rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation, isMirrored);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="normalizedRect">
    ///   <see cref="NormalizedRect" /> in the MediaPipe's coordinate system
    /// </param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3[] GetRectVertices(RectTransform rectTransform, NormalizedRect normalizedRect, bool isMirrored = false) {
      return GetRectVerticesNormalized(rectTransform, normalizedRect.XCenter, normalizedRect.YCenter, normalizedRect.Width, normalizedRect.Height, normalizedRect.Rotation, isMirrored);
    }

    static Vector3[] GetRectVertices(Vector3 topLeft, Vector3 bottomRight) {
      return new Vector3[] {
        topLeft,
        new Vector3(bottomRight.x, topLeft.y, 0.0f),
        bottomRight,
        new Vector3(topLeft.x, bottomRight.y, 0.0f),
      };
    }
  }
}
