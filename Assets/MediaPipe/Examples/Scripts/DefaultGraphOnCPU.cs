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

/// <summary>
///   This class is a translated version of
///   <see href="https://github.com/google/mediapipe/blob/v0.7.10/mediapipe/examples/desktop/demo_run_graph_main.cc">
///     demo_run_graph_main.cc
///   </see>
///   in the official repository.
/// </summary>
public class DefaultGraphOnCPU : DemoGraph {
  private const string outputStream = "output_video";

  private OutputStreamPoller<ImageFrame> outputStreamPoller;
  private ImageFramePacket outputPacket;

  public override Status StartRun() {
    outputStreamPoller = graph.AddOutputStreamPoller<ImageFrame>(outputStream).ConsumeValueOrDie();
    outputPacket = new ImageFramePacket();

    return graph.StartRun();
  }

  public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame) {
    var texture = screenController.GetScreen();

    if (!outputStreamPoller.Next(outputPacket)) {
      Debug.LogWarning("Failed to fetch an output packet, rendering the input image");
      texture.SetPixels32(textureFrame.GetPixels32());
    } else {
      texture.SetPixels32(outputPacket.Get().GetColor32s());
    }

    texture.Apply();
  }
}
