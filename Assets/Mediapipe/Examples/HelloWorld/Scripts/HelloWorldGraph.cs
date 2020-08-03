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

public class HelloWorldGraph : CalculatorGraph {
  /// <Summary>
  ///   A simple example to print out "Hello World!" from a MediaPipe graph.
  ///   Original C++ source code is <see cref="https://github.com/google/mediapipe/blob/master/mediapipe/examples/desktop/hello_world/hello_world.cc">HERE</see>
  /// </Summary>

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

  public readonly OutputStreamPoller<string> outputStreamPoller;

  public HelloWorldGraph() : base(configText) {
    var statusOrPoller = AddOutputStreamPoller();

    if (!statusOrPoller.IsOk()) {
      Debug.Log("Failed to add output stream: out");

      throw new System.SystemException(statusOrPoller.status.ToString());
    }

    outputStreamPoller = statusOrPoller.GetValue();
  }

  public Status StartRun() {
    return base.StartRun(new Mediapipe.SidePacket());
  }

  public Status AddStringToInputStream(string text, int timestamp) {
    var packet = Mediapipe.StringPacket.BuildAt(text, timestamp);

    return base.AddPacketToInputStream(inputStream, packet.GetPtr());
  }

  public Status CloseInputStream() {
    return base.CloseInputStream(inputStream);
  }

  private StatusOrPoller<string> AddOutputStreamPoller() {
    return new StatusOrPoller<string>(AddOutputStreamPoller(outputStream));
  }
}
