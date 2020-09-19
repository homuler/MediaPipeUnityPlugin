using Mediapipe;
using UnityEngine;

public class NodeAnnotationController : MonoBehaviour {
  public void Clear() {
    gameObject.transform.localScale = Vector3.zero;
  }

  /// <summary>
  ///   Renders a line on a screen.
  ///   It is assumed that the rotation of the screen is (90, 180, 0).
  /// </summary>
  /// <remarks>
  ///   In <paramref name="point" />, the coordinate of the left-top point is (0, 0).
  ///   Its z value will be ignored.
  /// </remarks>
  public void Draw(WebCamScreenController screenController, NormalizedLandmark point, float scale = 0.5f) {
    var transform = screenController.transform;
    var localScale = transform.localScale;
    var scaleVec = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

    gameObject.transform.position = Vector3.Scale(new Vector3(point.X - 0.5f, 0.5f - point.Y, 0), scaleVec) + transform.position;
    gameObject.transform.localScale = scale * Vector3.one;
  }
}
