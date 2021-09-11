using UnityEngine;

namespace Mediapipe.Unity.CoordinateSystem {
  /// <summary>
  ///   This class provides helper methods for converting real-world coordinate values to local coordinate values.
  ///   In real-world coordinate system, X axis is toward the right, Y axis is toward the bottom, and Z axis is toward the back.
  /// </summary>
  public static class RealWorldCoordinate {
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
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(RectTransform rectTransform, float x, float y, float z, Vector3 scale, bool isMirrored = false) {
      return Vector3.Scale(new Vector3(isMirrored ? - x : x, - y, z), scale);
    }

    /// <summary>
    ///   Get the coordinates represented by <paramref name="landmark" /> in local coordinate system.
    /// </summary>
    /// <param name="rectTransform">
    ///   <see cref="RectTransform" /> to be used for calculating local coordinates
    /// </param>
    /// <param name="scale">Ratio of real world coordinate values to local coordinate values</param>
    /// <param name="isMirrored">Set to true if the original coordinates is mirrored</param>
    public static Vector3 GetLocalPosition(this RectTransform rectTransform, Landmark landmark, Vector3 scale, bool isMirrored = false) {
      return GetLocalPosition(rectTransform, landmark.X, landmark.Y, landmark.Z, scale, isMirrored);
    }
  }
}
