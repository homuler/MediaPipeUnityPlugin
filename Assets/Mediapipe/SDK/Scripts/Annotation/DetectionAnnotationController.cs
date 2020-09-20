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

      var center = screenTransform.position;
      var normalizedBottom = 0.5f - box.Ymin - box.Height;
      var normalizedLeft = isFlipped ? 0.5f - box.Xmin - box.Width : box.Xmin - 0.5f;
      var bottomLeftRel = Vector3.Scale(new Vector3(normalizedLeft, normalizedBottom, 0), scale);
      var topRightRel = bottomLeftRel + Vector3.Scale(new Vector3(box.Width, box.Height, 0), scale);
      var topLeftRel = new Vector3(bottomLeftRel.x, topRightRel.y, 0);
      var bottomRightRel = new Vector3(topRightRel.x, bottomLeftRel.y, 0);

      var positions = new Vector3[] {
        bottomLeftRel + center,
        topLeftRel + center,
        topRightRel + center,
        bottomRightRel + center,
      };

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);
      // TODO: change font size
      gameObject.GetComponent<TextMesh>().text = $" {detection.Label[0]}, {detection.Score[0]:G3}";
      gameObject.transform.position = topLeftRel + center;
    }
  }
}
