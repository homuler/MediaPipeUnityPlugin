// Copyright 2019 The MediaPipe Authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// A simple example to print out "Hello World!" from a MediaPipe graph.

using Mediapipe;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
  /// <Summary>
  ///   A simple example to print out "Hello World!" from a MediaPipe graph.
  ///   Original C++ source code is <see cref="https://github.com/google/mediapipe/blob/master/mediapipe/examples/desktop/hello_world/hello_world.cc">HERE</see>
  /// </Summary>

  private HelloWorldGraph graph;

  void Start ()
  {
    graph = new HelloWorldGraph();

    if (!graph.IsOk())
    {
      Debug.Log("Failed to initialize the graph: " + graph.GetLastStatus());
      return;
    }

    graph.StartRun();

    if (!graph.IsOk())
    {
      Debug.Log(graph.GetLastStatus());
      return;
    }

    for (int i = 0; i < 10; i++)
    {
      var status = graph.AddStringToInputStream("Hello World!", i);

      if (!status.IsOk())
      {
        Debug.Log(status);
        return;
      }
    }

    graph.CloseInputStream();

    if (!graph.IsOk())
    {
      Debug.Log(graph.GetLastStatus());
      return;
    }

    int count = 0;

    while (graph.HasNextPacket())
    {
      Debug.Log($"#{++count} {graph.GetPacketValue()}");
    }

    graph.WaitUntilDone();

    Debug.Log(graph.GetLastStatus());
  }
}
