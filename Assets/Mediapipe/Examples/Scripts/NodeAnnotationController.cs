using Mediapipe;
using UnityEngine;

public class NodeAnnotationController : MonoBehaviour {
  public void Clear() {
    gameObject.transform.localScale = Vector3.zero;
  }

  /// <summary>
  ///   Renders a sphere on a screen.
  ///   It is assumed that the screen vertical to terrain and not inverted.
  /// </summary>
  /// <param name="isFlipped">
  ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
  /// </param>
  /// <remarks>
  ///   In <paramref name="point" />, y-axis is oriented from top to bottom.
  /// </remarks>
  public void Draw(WebCamScreenController screenController, NormalizedLandmark point, bool isFlipped = false, float scale = 0.5f) {
    var transform = screenController.transform;
    var localScale = transform.localScale;
    var scaleVec = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

    var x = isFlipped ? 0.5f - point.X : point.X - 0.5f;
    var y = 0.5f - point.Y;

    gameObject.transform.position = Vector3.Scale(new Vector3(x, y, 0), scaleVec) + transform.position;
    gameObject.transform.localScale = scale * Vector3.one;
  }
}
