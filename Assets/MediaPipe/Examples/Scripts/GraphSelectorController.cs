using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GraphSelectorController : MonoBehaviour {
  [SerializeField] GameObject faceDetectionGraph = null;
  [SerializeField] GameObject faceMeshGraph = null;
  [SerializeField] GameObject irisTrackingGraph = null;
  [SerializeField] GameObject handTrackingGraph = null;
  [SerializeField] GameObject poseTrackingGraph = null;
  [SerializeField] GameObject holisticGraph = null;
  [SerializeField] GameObject hairSegmentationGraph = null;
  [SerializeField] GameObject objectDetectionGraph = null;
  [SerializeField] GameObject objectDetection3dGraph = null;
  [SerializeField] GameObject boxTrackingGraph = null;
  [SerializeField] GameObject instantMotionTrackingGraph = null;
  [SerializeField] GameObject officialDemoGraph = null;

  private GameObject sceneDirector;
  private Dictionary<string, GameObject> graphs;

  void Start() {
    sceneDirector = GameObject.Find("SceneDirector");

    var graphSelector = GetComponent<Dropdown>();
    graphSelector.onValueChanged.AddListener(delegate { OnValueChanged(graphSelector); });

    InitializeOptions();
  }

  void InitializeOptions() {
    graphs = new Dictionary<string, GameObject>();

    AddGraph("Face Detection", faceDetectionGraph);
    AddGraph("Face Mesh", faceMeshGraph);
    AddGraph("Iris Tracking", irisTrackingGraph);
    AddGraph("Hand Tracking", handTrackingGraph);
    AddGraph("Pose Tracking", poseTrackingGraph);
    AddGraph("Holistic", holisticGraph);
#if !UNITY_IOS
    AddGraph("Hair Segmentation", hairSegmentationGraph);
#endif
    AddGraph("Object Detection", objectDetectionGraph);
    AddGraph("Object Detection 3d", objectDetection3dGraph);
    AddGraph("Box Tracking", boxTrackingGraph);
    AddGraph("Instant Motion Tracking", instantMotionTrackingGraph);
    AddGraph("Official Demo", officialDemoGraph);

    var graphSelector = GetComponent<Dropdown>();
    graphSelector.ClearOptions();
    graphSelector.AddOptions(graphs.Select(pair => pair.Key).ToList());

    OnValueChanged(graphSelector);
  }

  void AddGraph(string label, GameObject graph) {
    if (graph != null) {
      graphs.Add(label, graph);
    }
  }

  void OnValueChanged(Dropdown dropdown) {
    var option = dropdown.options[dropdown.value];
    var graph = graphs[option.text];

    Debug.Log($"Graph Changed: {option.text}");
    sceneDirector.GetComponent<SceneDirector>().ChangeGraph(graph);
  }
}
