using System.Collections.Generic;
using UnityEngine;

using pbc = global::Google.Protobuf.Collections;
using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe {
  public class DetectionAnnotationController : AnnotationController {
    [SerializeField] protected GameObject relativeKeypointPrefab = null;

    private List<GameObject> Keypoints;
    private readonly Vector3[] emptyPositions = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };

    void Awake() {
      Keypoints = new List<GameObject>();
    }

    void OnDestroy() {
      foreach (var keypoint in Keypoints) {
        Destroy(keypoint);
      }
    }

    public override void Clear() {
      gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
      gameObject.GetComponent<TextMesh>().text = "";

      foreach (var keypoint in Keypoints) {
        keypoint.GetComponent<NodeAnnotationController>().Clear();
      }
    }

    /// <summary>
    ///   Renders a bounding box and its label on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="detection" />, y-axis is oriented from top to bottom.
    ///   Its location data is represented by relative bounding box.
    /// </remarks>
    public void Draw(Transform screenTransform, Detection detection, bool isFlipped = false) {
      if (detection.LocationData == null) {
        Clear();
        return;
      }

      DrawRectAndLabel(screenTransform, detection, isFlipped);
      DrawRelativeKeypoints(screenTransform, detection.LocationData.RelativeKeypoints, isFlipped);
    }

    private void DrawRectAndLabel(Transform screenTransform, Detection detection, bool isFlipped = false) {
      var positions = GetPositions(screenTransform, detection.LocationData.RelativeBoundingBox, isFlipped);

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);

      if (detection.Label.Count > 0) {
        // TODO: change font size
        gameObject.GetComponent<TextMesh>().text = $" {detection.Label[0]}, {detection.Score[0]:G3}";
        gameObject.transform.position = positions[0];
      }
    }

    private void DrawRelativeKeypoints(Transform screenTransform, pbc.RepeatedField<mplt.RelativeKeypoint> keypoints, bool isFlipped = false) {
      while (keypoints.Count > Keypoints.Count) {
        Keypoints.Add(Instantiate(relativeKeypointPrefab));
      }

      for (var i = 0; i < keypoints.Count; i++) {
        Keypoints[i].GetComponent<NodeAnnotationController>().Draw(screenTransform, keypoints[i], isFlipped);
      }

      for (var i = keypoints.Count; i < Keypoints.Count; i++) {
        Keypoints[i].GetComponent<NodeAnnotationController>().Clear();
      }
    }
  }
}
