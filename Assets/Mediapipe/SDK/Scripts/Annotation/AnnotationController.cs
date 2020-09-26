using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe {
  public abstract class AnnotationController : MonoBehaviour {
    public abstract void Clear();

    /// <summary>
    ///   Each element of the returned vector is a scale factor to a normalized value in the corresponding coordinate.
    /// </summary>
    /// <param name="transform">Transform of the target screen</param>
    /// <remarks>
    ///   Currently, tt is assumed that the rotation of <paramref name="transform" /> is (90, 180, 0).
    /// </remarks>
    /// <example>
    ///   If the scale of <paramref name="transform"/> is (4, 1, 3),
    ///   it will return <c>new Vector3(40, 30, 1)</c>
    /// </example>
    protected Vector3 ScaleVector(Transform transform) {
      return new Vector3(10 * transform.localScale.x, 10 * transform.localScale.z, 1);
    }

    protected Vector3 GetPositionFromNormalizedPoint(Transform screenTransform, float x, float y, bool isFlipped) {
      var relX = (isFlipped ? -1 : 1) * (x - 0.5f);
      var relY = 0.5f - y;

      return Vector3.Scale(new Vector3(relX, relY, 0), ScaleVector(screenTransform)) + screenTransform.position;
    }

    protected Vector3 GetPosition(Transform screenTransform, NormalizedLandmark point, bool isFlipped) {
      return GetPositionFromNormalizedPoint(screenTransform, point.X, point.Y, isFlipped);
    }

    protected Vector3 GetPosition(Transform screenTransform, mplt.RelativeKeypoint point, bool isFlipped) {
      return GetPositionFromNormalizedPoint(screenTransform, point.X, point.Y, isFlipped);
    }

    /// <summary>
    ///   Returns a Vector3 array which represents a rectangle's vertices.
    ///   They are ordered clockwise from top-left point.
    /// </summary>
    protected Vector3[] GetPositionsFromNormalizedRect(
      Transform screenTransform, float centerX, float centerY, float width, float height, float rotation, bool isFlipped
    ) {
      var center = GetPositionFromNormalizedPoint(screenTransform, centerX, centerY, isFlipped);
      var quaternion = Quaternion.Euler(0, 0, (isFlipped ? 1 : -1) * Mathf.Rad2Deg * rotation);

      var scaleVec = ScaleVector(screenTransform);
      var topLeftRel = quaternion * Vector3.Scale(new Vector3(-width / 2, height / 2, 0), scaleVec);
      var topRightRel = quaternion * Vector3.Scale(new Vector3(width / 2, height / 2, 0), scaleVec);

      return new Vector3[] {
        center + topLeftRel,
        center + topRightRel,
        center - topLeftRel,
        center - topRightRel,
      };
    }

    protected Vector3[] GetPositions(Transform screenTransform, NormalizedRect rect, bool isFlipped) {
      return GetPositionsFromNormalizedRect(screenTransform, rect.XCenter, rect.YCenter, rect.Width, rect.Height, rect.Rotation, isFlipped);
    }

    protected Vector3[] GetPositions(Transform screenTransform, mplt.RelativeBoundingBox box, bool isFlipped) {
      // In a mediapipe coordinate system
      var centerX = box.Xmin + box.Width / 2;
      var centerY = box.Ymin + box.Height / 2;

      return GetPositionsFromNormalizedRect(screenTransform, centerX, centerY, box.Width, box.Height, 0, isFlipped);
    }

    protected float GetDistance(Transform screenTransform, NormalizedLandmark a, NormalizedLandmark b) {
      var aPos = GetPosition(screenTransform, a, false);
      var bPos = GetPosition(screenTransform, b, false);

      return Vector3.Distance(aPos, bPos);
    }
  }
}
