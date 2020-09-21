using UnityEngine;

namespace Mediapipe {
  public class NodeAnnotationController : AnnotationController {
    public override void Clear() {
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
    public void Draw(Transform screenTransform, NormalizedLandmark point, bool isFlipped = false, float scale = 0.5f) {
      gameObject.transform.position = GetPosition(screenTransform, point, isFlipped);
      gameObject.transform.localScale = scale * Vector3.one;
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
      gameObject.transform.position = GetPosition(screenTransform, point, isFlipped);
      gameObject.transform.localScale = scale * Vector3.one;
    }
  }
}
