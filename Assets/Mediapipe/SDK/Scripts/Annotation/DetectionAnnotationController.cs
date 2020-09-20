using UnityEngine;

namespace Mediapipe {
  public class DetectionAnnotationController : MonoBehaviour {
    private readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    public void Clear() {
      gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
      gameObject.GetComponent<TextMesh>().text = "";
    }

    /// <summary>
    ///   Renders a bounding box and its label on a screen.
    ///   It is assumed that the screen vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="detection" />, y-axis is oriented from top to bottom.
    ///   Its location data is represented by relative bounding box.
    /// </remarks>
    public void Draw(Transform screenTransform, Detection detection, bool isFlipped = false) {
      var localScale = screenTransform.localScale;
      var scale = new Vector3(10 * localScale.x, 10 * localScale.z, 1);
      var box = detection.LocationData.RelativeBoundingBox;

      var minX = isFlipped ? 0.5f - box.Xmin : box.Xmin - 0.5f;
      var minY = 0.5f - box.Ymin;
      var min = Vector3.Scale(new Vector3(minX, minY, 0), scale) + screenTransform.position;
      var max = min + Vector3.Scale(new Vector3(box.Width / 2, box.Height / 2, 0), scale);

      var positions = new Vector3[] {
        min,
        new Vector3(min.x, max.y, 0),
        max,
        new Vector3(max.x, min.y, 0),
      };

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);
      gameObject.GetComponent<TextMesh>().text = $"{detection.Label}, {detection.Score}";
      gameObject.transform.position = min;
    }
  }
}
