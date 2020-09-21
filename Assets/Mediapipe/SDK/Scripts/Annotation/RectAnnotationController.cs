using UnityEngine;

namespace Mediapipe {
  public class RectAnnotationController : AnnotationController {
    private readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    public override void Clear() {
      gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
    }

    /// <summary>
    ///   Renders a rectangle on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="rect" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, NormalizedRect rect, bool isFlipped = false) {
      var positions = GetPositions(screenTransform, rect, isFlipped);

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);
    }
  }
}
