using Mediapipe;
using UnityEngine;

public class HelloWorld : MonoBehaviour {
  private HelloWorldGraph graph;

  void Start () {
    graph = new HelloWorldGraph();

    if (!graph.IsOk()) {
      Debug.Log("Failed to initialize the graph: " + graph.GetLastStatus());
      return;
    }

    graph.StartRun();

    if (!graph.IsOk()) {
      Debug.Log(graph.GetLastStatus());
      return;
    }

    for (int i = 0; i < 10; i++) {
      var status = graph.AddStringToInputStream("Hello World!", i);

      if (!status.IsOk()) {
        Debug.Log(status);
        return;
      }
    }

    graph.CloseInputStream();

    if (!graph.IsOk()) {
      Debug.Log(graph.GetLastStatus());
      return;
    }

    int count = 0;

    while (graph.HasNextPacket()) {
      Debug.Log($"#{++count} {graph.GetPacketValue()}");
    }

    graph.WaitUntilDone();

    Debug.Log(graph.GetLastStatus());
  }
}
