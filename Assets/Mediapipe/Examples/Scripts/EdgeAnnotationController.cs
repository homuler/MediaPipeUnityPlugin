using Mediapipe;
using UnityEngine;

public class EdgeAnnotationController : MonoBehaviour {
  private static readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero };

  public void Clear() {
    gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
  }

  /// <summary>
  ///   Renders a line on a screen.
  ///   It is assumed that the screen vertical to terrain and not inverted.
  /// </summary>
  /// <param name="isFlipped">
  ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
  /// </param>
  /// <remarks>
  ///   In <paramref name="point" />, y-axis is oriented from top to bottom.
  /// </remarks>
  public void Draw(WebCamScreenController screenController, NormalizedLandmark a, NormalizedLandmark b, bool isFlipped = false) {
    var transform = screenController.transform;
    var localScale = transform.localScale;
    var scale = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

    var srcX = isFlipped ? 0.5f - a.X : a.X - 0.5f;
    var srcY = 0.5f - a.Y;
    var dstX = isFlipped ? 0.5f - b.X : b.X - 0.5f;
    var dstY = 0.5f - b.Y;
    var src = Vector3.Scale(new Vector3(srcX, srcY, 0), scale) + transform.position;
    var dst = Vector3.Scale(new Vector3(dstX, dstY, 0), scale) + transform.position;

    Draw(screenController, src, dst);
  }

  /// <summary>
  ///   Renders a line joining <paramref name="a" /> and <paramref name="b" /> on a screen.
  ///   It is assumed that the screen vertical to terrain and not inverted.
  /// </summary>
  public void Draw(WebCamScreenController screenController, GameObject a, GameObject b) {
    Draw(screenController, a.transform.position, b.transform.position);
  }

  private void Draw(WebCamScreenController screenController, Vector3 src, Vector3 dst) {
    var positions = new Vector3[] { src, dst };

    gameObject.GetComponent<LineRenderer>().SetPositions(positions);
  }
}
