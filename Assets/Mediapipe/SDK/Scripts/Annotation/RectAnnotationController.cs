using UnityEngine;

namespace Mediapipe {
  public class RectAnnotationController : MonoBehaviour {
    private readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    public void Clear() {
      gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
    }

    /// <summary>
    ///   Renders a rectangle on a screen.
    ///   It is assumed that the screen vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="rect" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, NormalizedRect rect, bool isFlipped = false) {
      var localScale = screenTransform.localScale;
      var scale = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

      var centerX = isFlipped ? 0.5f - rect.XCenter : rect.XCenter - 0.5f;
      var centerY = 0.5f - rect.YCenter;
      var center = Vector3.Scale(new Vector3(centerX, centerY, 0), scale) + screenTransform.position;
      var rotation = isFlipped ? Mathf.Rad2Deg * rect.Rotation : -Mathf.Rad2Deg * rect.Rotation;
      var quaternion = Quaternion.Euler(0, 0, rotation);

      var topRel1 = quaternion * Vector3.Scale(new Vector3(-rect.Width / 2, rect.Height / 2, 0), scale);
      var topRel2 = quaternion * Vector3.Scale(new Vector3(rect.Width / 2, rect.Height / 2, 0), scale);

      var positions = new Vector3[] {
        center + topRel1,
        center + topRel2,
        center - topRel1,
        center - topRel2,
      };

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);
    }
  }
}
