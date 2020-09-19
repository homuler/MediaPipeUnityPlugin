using Mediapipe;
using UnityEngine;

public class RectAnnotationController : MonoBehaviour {
  private readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

  public void Clear() {
    gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
  }

  /// <summary>
  ///   Renders a rectangle on a screen.
  ///   It is assumed that the rotation of the screen is (90, 180, 0).
  /// </summary>
  /// <remarks>
  ///   In <paramref name="rect" />, the coordinate of the left-top point is (0, 1).
  ///   If the rect is not found, its height and width will be zero.
  /// </remarks>
  public void Draw(WebCamScreenController screenController, NormalizedRect rect) {
    var transform = screenController.transform;
    var localScale = transform.localScale;
    var scale = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

    var center = Vector3.Scale(new Vector3(rect.XCenter - 0.5f, 0.5f - rect.YCenter, 0), scale) + transform.position;
    var rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * rect.Rotation); // counterclockwise
    var leftTopRel = rotation * Vector3.Scale(new Vector3(-rect.Width / 2, rect.Height / 2, 0), scale);
    var rightTopRel = rotation * Vector3.Scale(new Vector3(rect.Width / 2, rect.Height / 2, 0), scale);

    var positions = new Vector3[] {
      center + leftTopRel,
      center + rightTopRel,
      center - leftTopRel,
      center - rightTopRel,
    };

    gameObject.GetComponent<LineRenderer>().SetPositions(positions);
  }
}
