using Mediapipe;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Edge = System.Tuple<int ,int>;

public class HandLandmarkAnnotationController : MonoBehaviour {
  private static readonly IList<Edge> connections = new List<Edge> {
    new Edge(0, 1),
    new Edge(1, 2),
    new Edge(2, 3),
    new Edge(3, 4),
    new Edge(0, 5),
    new Edge(5, 9),
    new Edge(9, 13),
    new Edge(13, 17),
    new Edge(0, 17),
    new Edge(5, 6),
    new Edge(6, 7),
    new Edge(7, 8),
    new Edge(9, 10),
    new Edge(10, 11),
    new Edge(11, 12),
    new Edge(13, 14),
    new Edge(14, 15),
    new Edge(15, 16),
    new Edge(17, 18),
    new Edge(18, 19),
    new Edge(19, 20),
  };

  private static readonly int NodeSize = 21;
  private static readonly int EdgeSize = connections.Count;

  [SerializeField] GameObject nodePrefab = null;
  [SerializeField] GameObject edgePrefab = null;

  private List<GameObject> nodes = new List<GameObject>(NodeSize);
  private List<GameObject> edges = new List<GameObject>(EdgeSize);

  void Awake() {
    for (var i = 0; i < NodeSize; i++) {
      nodes.Add(Instantiate(nodePrefab));
    }

    for (var i = 0; i < EdgeSize; i++) {
      edges.Add(Instantiate(edgePrefab));
    }
  }

  public void Clear() {
    foreach (var landmark in nodes) {
      landmark.GetComponent<NodeAnnotationController>().Clear();
    }

    foreach (var line in edges) {
      line.GetComponent<EdgeAnnotationController>().Clear();
    }
  }

  public void Draw(WebCamScreenController screenController, NormalizedLandmarkList landmarkList) {
    if (isEmpty(landmarkList)) {
      Clear();
      return;
    }

    var transform = screenController.transform;
    var localScale = transform.localScale;
    var scale = new Vector3(10 * localScale.x, 10 * localScale.z, 1);

    for (var i = 0; i < NodeSize; i++) {
      var landmark = landmarkList.Landmark[i];
      var node = nodes[i];

      node.GetComponent<NodeAnnotationController>().Draw(screenController, landmark);
    }

    for (var i = 0; i < EdgeSize; i++) {
      var connection = connections[i];
      var edge = edges[i];

      var a = nodes[connection.Item1];
      var b = nodes[connection.Item2];

      edge.GetComponent<EdgeAnnotationController>().Draw(screenController, a, b);
    }
  }

  private bool isEmpty(NormalizedLandmarkList landmarkList) {
    return landmarkList.Landmark.All(landmark => {
      return landmark.X == 0 && landmark.Y == 0 && landmark.Z == 0;
    });
  }
}
