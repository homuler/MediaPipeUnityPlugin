using Mediapipe;
using UnityEngine;

public class EdgeAnnotationController : MonoBehaviour {
  private static readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero };

  public void Clear() {
    gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
  }

  /// <summary>
  ///   Renders a line on a screen.
  ///   It is assumed that the rotation of the screen is (90, 180, 0).
  /// </summary>
  /// <remarks>
  ///   In <paramref name="a" /> and <paramref name="b" />, the coordinate of the left-top point is (0, 0).
  ///   Their z values will be ignored.
  /// </remarks>
  public void Draw(WebCamScreenController screenController, NormalizedLandmark a, NormalizedLandmark b) {
    var transform = screenController.transform;
    var localScale = transform.localScale;
    var scale = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

    var src = Vector3.Scale(new Vector3(a.X - 0.5f, 0.5f - a.Y, 0), scale) + transform.position;
    var dst = Vector3.Scale(new Vector3(b.X - 0.5f, 0.5f - b.Y, 0), scale) + transform.position;

    Draw(screenController, src, dst);
  }

  public void Draw(WebCamScreenController screenController, GameObject a, GameObject b) {
    Draw(screenController, a.transform.position, b.transform.position);
  }

  private void Draw(WebCamScreenController screenController, Vector3 src, Vector3 dst) {
    var positions = new Vector3[] { src, dst };

    gameObject.GetComponent<LineRenderer>().SetPositions(positions);
  }
}
