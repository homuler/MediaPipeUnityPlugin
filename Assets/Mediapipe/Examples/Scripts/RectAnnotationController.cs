using Mediapipe;
using UnityEngine;

public class RectAnnotationController : MonoBehaviour {
  private GameObject webCamScreen;

  void Awake() {
    webCamScreen = GameObject.Find("WebCamScreen");
  }

  public void Clear() {
    Destroy(gameObject);
  }

  /// <summary>
  ///   Renders a rectangle on WebCamScreen.
  ///   It is assumed that the rotation of WebCamScreen is (90, 180, 0).
  /// </summary>
  /// <remarks>In <paramref name="rect" />, the coordinate of the left-top point is (0, 0)</remarks>
  public void Draw(NormalizedRect rect) {
    var transform = webCamScreen.transform;
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
