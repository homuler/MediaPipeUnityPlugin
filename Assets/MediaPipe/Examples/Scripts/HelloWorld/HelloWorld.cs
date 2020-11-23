using Mediapipe;
using System.Collections;
using System.IO;
using UnityEngine;

public class HelloWorld : MonoBehaviour {
  /// <Summary>
  ///   A simple example to print out "Hello World!" from a MediaPipe graph.
  ///   Original C++ source code is <see cref="https://github.com/google/mediapipe/blob/master/mediapipe/examples/desktop/hello_world/hello_world.cc">HERE</see>
  /// </Summary>

  private SidePacket sidePacket;
  private HelloWorldGraph graph;
  private Coroutine graphRunner;

  void OnEnable() {
    var nameForGlog = Path.Combine(Application.dataPath, "MediaPipePlugin");
    var logDir = Path.Combine(Application.dataPath.Replace("/Assets", ""), "Logs", "MediaPipe");

    if (!Directory.Exists(logDir)) {
      Directory.CreateDirectory(logDir);
    }

    Glog.Initialize(nameForGlog, logDir);
  }

  void OnDestroy() {
    graph?.Stop();
  }

  void OnDisable() {
    Glog.Shutdown();
  }

  void Start () {
    graph = new HelloWorldGraph();
    graphRunner = StartCoroutine(RunGraph());
  }

  IEnumerator RunGraph() {
    graph.Initialize();

    graph.StartRun().AssertOk();

    var i = 10;

    while (i-- > 0) {
      yield return new WaitForEndOfFrame();

      var input = "Hello World!";

      graph.PushInput(input);
      graph.RenderOutput(null, input);
    }

    graph.Stop();
  }
}
