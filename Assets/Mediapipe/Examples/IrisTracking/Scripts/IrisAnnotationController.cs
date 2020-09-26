using Mediapipe;
using System.Collections.Generic;
using UnityEngine;

public class IrisAnnotationController : AnnotationController {
  [SerializeField] GameObject nodePrefab = null;
  [SerializeField] GameObject circlePrefab = null;

  private const int NodeSize = 10;
  private List<GameObject> nodes;
  private GameObject leftIris;
  private GameObject rightIris;

  void Awake() {
    nodes = new List<GameObject>(NodeSize);
    for (var i = 0; i < NodeSize; i++) {
      nodes.Add(Instantiate(nodePrefab));
    }

    leftIris = Instantiate(circlePrefab);
    rightIris = Instantiate(circlePrefab);
  }

  public override void Clear() {
    foreach (var landmark in nodes) {
      landmark.GetComponent<NodeAnnotationController>().Clear();
    }

    leftIris.GetComponent<CircleAnnotationController>().Clear();
    rightIris.GetComponent<CircleAnnotationController>().Clear();
  }

  /// <summary>
  ///   Renders iris landmarks on a screen.
  ///   It is assumed that the screen is vertical to terrain and not inverted.
  /// </summary>
  /// <param name="isFlipped">
  ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
  /// </param>
  /// <remarks>
  ///   In <paramref name="landmarks" />, y-axis is oriented from top to bottom.
  /// </remarks>
  public void Draw(Transform screenTransform, IList<NormalizedLandmark> landmarks, bool isFlipped = false) {
    for (var i = 0; i < NodeSize; i++) {
      var landmark = landmarks[i];
      var node = nodes[i];

      node.GetComponent<NodeAnnotationController>().Draw(screenTransform, landmark, isFlipped, 0.3f);
    }

    DrawIrisCircle(screenTransform, landmarks, isFlipped, true);
    DrawIrisCircle(screenTransform, landmarks, isFlipped, false);
  }

  private void DrawIrisCircle(Transform screenTransform, IList<NormalizedLandmark> landmarks, bool isFlipped, bool isLeft = true) {
    int offset = isLeft ? 0 : 5;
    var r = GetIrisRadius(screenTransform, landmarks[offset + 1], landmarks[offset + 2], landmarks[offset + 3], landmarks[offset + 4]);

    var circle = isLeft ? leftIris : rightIris;
    var center = landmarks[offset];
    circle.GetComponent<CircleAnnotationController>().Draw(screenTransform, center, r, isFlipped);
  }

  private float GetIrisRadius(Transform screenTransform, NormalizedLandmark l1, NormalizedLandmark l2, NormalizedLandmark l3, NormalizedLandmark l4) {
    var r1 = GetDistance(screenTransform, l1, l3);
    var r2 = GetDistance(screenTransform, l2, l4);

    return (r1 + r2) / 4;
  }
}
