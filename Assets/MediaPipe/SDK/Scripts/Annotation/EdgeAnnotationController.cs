using UnityEngine;

namespace Mediapipe {
  public class EdgeAnnotationController : AnnotationController {
    private static readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero };

    public override void Clear() {
      gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
    }

    /// <summary>
    ///   Renders a line on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="point" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, NormalizedLandmark a, NormalizedLandmark b, bool isFlipped = false) {
      var src = GetPositionFromNormalizedPoint(screenTransform, a.X, a.Y, isFlipped);
      var dst = GetPositionFromNormalizedPoint(screenTransform, b.X, b.Y, isFlipped);

      Draw(screenTransform, src, dst);
    }

    /// <summary>
    ///   Renders a line joining <paramref name="a" /> and <paramref name="b" /> on a screen.
    ///   It is assumed that the screen vertical to terrain and not inverted.
    /// </summary>
    public void Draw(Transform screenTransform, GameObject a, GameObject b) {
      Draw(screenTransform, a.transform.position, b.transform.position);
    }

    private void Draw(Transform screenTransform, Vector3 src, Vector3 dst) {
      var positions = new Vector3[] { src, dst };

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);
    }
  }
}
