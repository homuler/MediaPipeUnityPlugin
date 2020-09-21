using UnityEngine;

namespace Mediapipe {
  public class RelativeKeypointAnnotationController : MonoBehaviour {
    public void Clear() {
      gameObject.transform.localScale = Vector3.zero;
    }

    /// <summary>
    ///   Renders a sphere on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="point" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, LocationData.Types.RelativeKeypoint point, bool isFlipped = false, float scale = 0.3f) {
      var localScale = screenTransform.localScale;
      var scaleVec = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

      var x = isFlipped ? 0.5f - point.X : point.X - 0.5f;
      var y = 0.5f - point.Y;

      gameObject.transform.position = Vector3.Scale(new Vector3(x, y, 0), scaleVec) + screenTransform.position;
      gameObject.transform.localScale = scale * Vector3.one;
    }
  }
}
