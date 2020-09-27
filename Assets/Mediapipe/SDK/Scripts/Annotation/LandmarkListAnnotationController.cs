using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Edge = System.Tuple<int ,int>;

namespace Mediapipe {
  public abstract class LandmarkListAnnotationController : AnnotationController {
    [SerializeField] protected GameObject nodePrefab = null;
    [SerializeField] protected GameObject edgePrefab = null;
    [SerializeField] protected float NodeScale = 0.5f;

    private List<GameObject> Nodes;
    private List<GameObject> Edges;

    void Awake() {
      Nodes = new List<GameObject>(NodeSize);
      for (var i = 0; i < NodeSize; i++) {
        Nodes.Add(Instantiate(nodePrefab));
      }

      Edges = new List<GameObject>(EdgeSize);
      for (var i = 0; i < EdgeSize; i++) {
        Edges.Add(Instantiate(edgePrefab));
      }
    }

    void OnDestroy() {
      foreach (var landmark in Nodes) {
        Destroy(landmark);
      }

      foreach (var line in Edges) {
        Destroy(line);
      }
    }

    public override void Clear() {
      foreach (var landmark in Nodes) {
        landmark.GetComponent<NodeAnnotationController>().Clear();
      }

      foreach (var line in Edges) {
        line.GetComponent<EdgeAnnotationController>().Clear();
      }
    }

    /// <summary>
    ///   Renders landmarks on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="landmarkList" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, NormalizedLandmarkList landmarkList, bool isFlipped = false) {
      if (isEmpty(landmarkList)) {
        Clear();
        return;
      }

      for (var i = 0; i < NodeSize; i++) {
        var landmark = landmarkList.Landmark[i];
        var node = Nodes[i];

        node.GetComponent<NodeAnnotationController>().Draw(screenTransform, landmark, isFlipped, NodeScale);
      }

      for (var i = 0; i < EdgeSize; i++) {
        var connection = Connections[i];
        var edge = Edges[i];

        var a = Nodes[connection.Item1];
        var b = Nodes[connection.Item2];

        edge.GetComponent<EdgeAnnotationController>().Draw(screenTransform, a, b);
      }
    }

    protected abstract IList<Edge> Connections { get; }

    protected abstract int NodeSize { get; }

    protected int EdgeSize {
      get { return Connections.Count; }
    }

    private bool isEmpty(NormalizedLandmarkList landmarkList) {
      return landmarkList.Landmark.All(landmark => {
        return landmark.X == 0 && landmark.Y == 0 && landmark.Z == 0;
      });
    }
  }
}
