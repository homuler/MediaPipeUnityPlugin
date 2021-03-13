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

using Mediapipe;
using UnityEngine;
using System;

/// <Summary>
///   A simple example to print out "Hello World!" from a MediaPipe graph.
///   This class is a translated version of
///   <see href="https://github.com/google/mediapipe/blob/v0.7.10/mediapipe/examples/desktop/hello_world/hello_world.cc">
///     hello_world.cc
///   </see>
///   in the official repository.
/// </Summary>
public class HelloWorldGraph : IDemoGraph<string> {
  private const string inputStream = "in";
  private const string outputStream = "out";
  private const string configText = @"
input_stream: ""in""
output_stream: ""out""
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
";

  private CalculatorGraph graph;
  private OutputStreamPoller<string> outputStreamPoller;
  private StringPacket outputPacket;

  public void Initialize() {
    graph = new CalculatorGraph(configText);
  }

  public void Initialize(GpuResources gpuResources, GlCalculatorHelper gpuHelper) {
    this.Initialize();
  }

  public Status StartRun() {
    outputStreamPoller = graph.AddOutputStreamPoller<string>(outputStream).Value();
    outputPacket = new StringPacket();

    return graph.StartRun();
  }

  public Status StartRun(Texture texture) {
    throw new NotSupportedException();
  }

  public Status PushInput(string text) {
    int timestamp = System.Environment.TickCount & System.Int32.MaxValue;
    var packet = new StringPacket(text, new Timestamp(timestamp));

    return graph.AddPacketToInputStream(inputStream, packet);
  }

  public void RenderOutput(WebCamScreenController screenController, string input) {
    if (outputStreamPoller.Next(outputPacket)) {
      Debug.Log($"{outputPacket.Get()}");
    }
  }

  public void Stop() {
    if (graph != null) {
      graph.CloseInputStream(inputStream).AssertOk();
      graph.WaitUntilDone().AssertOk();
    }
  }
}
