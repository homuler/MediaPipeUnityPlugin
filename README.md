# MediaPipe Unity Plugin

This is a Unity (>= 2021.3) [Native Plugin](https://docs.unity3d.com/Manual/NativePlugins.html) to use [MediaPipe](https://github.com/google/mediapipe) (0.10.14).

The goal of this project is to port the MediaPipe API (C++) _one by one_ to C# so that it can be called from Unity.\
This approach may sacrifice performance when you need to call multiple APIs in a loop, but it gives you the flexibility to use MediaPipe instead.

With this plugin, you can

- Write MediaPipe code in C#.
- Run MediaPipe's official solution on Unity.
- Run your custom `Calculator` and `CalculatorGraph` on Unity.
  - :warning: Depending on the type of input/output, you may need to write C++ code.

## :smile_cat: Hello World!

Here is a Hello World! example.\
Compare it with [the official code](https://github.com/google/mediapipe/blob/cf101e62a9d49a51be76836b2b8e5ba5c06b5da0/mediapipe/examples/desktop/hello_world/hello_world.cc)!

```cs
using Mediapipe;
using UnityEngine;

public sealed class HelloWorld : MonoBehaviour
{
    private const string _ConfigText = @"
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

    private void Start()
    {
        using var graph = new CalculatorGraph(_ConfigText);
        using var poller = graph.AddOutputStreamPoller<string>("out");
        graph.StartRun();

        for (var i = 0; i < 10; i++)
        {
            graph.AddPacketToInputStream("in", Packet.CreateStringAt("Hello World!", i));
        }

        graph.CloseInputStream("in");
        var packet = new Packet<string>();

        while (poller.Next(packet))
        {
            Debug.Log(packet.Get());
        }
        graph.WaitUntilDone();
    }
}
```

For more detailed usage, see [the API Overview](https://github.com/homuler/MediaPipeUnityPlugin/wiki/API-Overview) page or the tutorial on [the Getting Started page](https://github.com/homuler/MediaPipeUnityPlugin/wiki/Getting-Started).

## :hammer_and_wrench: Installation

This repository **does not contain required libraries** (e.g. `libmediapipe_c.so`, `Google.Protobuf.dll`, etc).\
You can download them from [the release page](https://github.com/homuler/MediaPipeUnityPlugin/releases) instead.

|                 file                  |                                                      contents                                                      |
| :-----------------------------------: | :----------------------------------------------------------------------------------------------------------------: |
|    `MediaPipeUnityPlugin-all.zip`     | All the source code with required libraries. If you need to run sample scenes on your mobile devices, prefer this. |
| `com.github.homuler.mediapipe-*.tgz`  |                      [A tarball package](https://docs.unity3d.com/Manual/upm-ui-tarball.html)                      |
| `MediaPipeUnityPlugin.*.unitypackage` |                                               A `.unitypackage` file                                               |

If you want to customize the package or minify the package size, you need to build them by yourself.\
For a step-by-step guide, please refer to the [Installation Guide](https://github.com/homuler/MediaPipeUnityPlugin/wiki/Installation-Guide) on Wiki.\
You can also make use of [the Package Workflow](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/.github/workflows/package.yml) on Github Actions after forking this repository.

> :warning: libraries that can be built differ depending on your environment.

### Supported Platforms

> :warning: GPU mode is not supported on macOS and Windows.

|                            |       Editor       |   Linux (x86_64)   |   macOS (x86_64)   |   macOS (ARM64)    |  Windows (x86_64)  |      Android       |        iOS         | WebGL |
| :------------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :---: |
|     Linux (AMD64) [^1]     | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: |                    |       |
|         Intel Mac          | :heavy_check_mark: |                    | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |       |
|           M1 Mac           | :heavy_check_mark: |                    |                    | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: |       |
| Windows 10/11 (AMD64) [^2] | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: | :heavy_check_mark: |                    |       |

[^1]: Tested on Arch Linux.
[^2]: Running MediaPipe on Windows is [experimental](https://ai.google.dev/edge/mediapipe/framework/getting_started/install#installing_on_windows).

### Supported Solutions
This plugin implements the following [MediaPipe Tasks](https://ai.google.dev/edge/mediapipe/solutions/tasks) C# APIs.

cf. [The official available solutions](https://ai.google.dev/edge/mediapipe/solutions/guide#available_solutions)

|         Solution         |      Android       |        iOS         |       Linux        |       macOS        |      Windows       |
| :----------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: |
|    LLM Inference API     |                    |                    |                    |                    |
|     Object detection     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
|   Image classification   |                    |                    |                    |                    |                    |
|    Image segmentation    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| Interactive segmentation |                    |                    |                    |                    |                    |
| Hand landmark detection  | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
|   Gesture recognition    |                    |                    |                    |                    |                    |
|     Image embedding      |                    |                    |                    |                    |                    |
|      Face detection      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
| Face landmark detection  | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
|     Face stylization     |                    |                    |                    |                    |                    |
| Pose landmark detection  | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
|     Image generation     |                    |                    |                    |                    |                    |
|   Text classification    |                    |                    |                    |                    |                    |
|      Text embedding      |                    |                    |                    |                    |                    |
|    Language detector     |                    |                    |                    |                    |                    |
|   Audio classification   | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |

### Legacy Solutions
You can also use [MediaPipe Framework](https://ai.google.dev/edge/mediapipe/framework), which allows you to run [Legacy Solutions](https://ai.google.dev/edge/mediapipe/solutions/guide#legacy). However, please note that support for these solutions has ended.

## :plate_with_cutlery: Try the sample app

### Example Solutions
Some solutions (including Legacy solutions) can be tested using the sample app.
Please check [`Assets/MediaPipeUnity/Samples/Scenes`](https://github.com/homuler/MediaPipeUnityPlugin/tree/master/Assets/MediaPipeUnity/Samples/Scenes) to see which solutions have samples.

### UnityEditor

Select any scenes under `Assets/MediaPipeUnity/Samples/Scenes` and play.

### Desktop, Android, iOS

Select proper Inference Mode and Asset Loader Type from the Inspector Window.

#### Preferable Inference Mode

If the native libraries are built for CPU (i.e. `--desktop cpu`), select `CPU` for inference mode.\
For the libraries distributed on the release page, only the CPU is available for use on Windows and macOS.

![preferable-inference-mode](https://github.com/homuler/MediaPipeUnityPlugin/assets/4690128/129d18be-8184-43f7-8ac8-56db4df9f9a7)

#### Asset Loader Type

The default Asset Loader Type is set to `Local`, which only works on UnityEditor.\
To run it on your devices, switch it to `StreamingAssets` and copy the required resources under [`StreamingAssets`](https://docs.unity3d.com/2022.3/Documentation/Manual/StreamingAssets.html) (if you're using `MediaPipeUnityPlugin-all.zip`, the `StreamingAssets` directory already contains them).

![asset-loader-type](https://github.com/homuler/MediaPipeUnityPlugin/assets/4690128/f7059140-4da9-4201-a232-83ff07cd63df)

See [the tutorial](https://github.com/homuler/MediaPipeUnityPlugin/wiki/Getting-Started#load-model-files) for more details.

## :book: Wiki

https://github.com/homuler/MediaPipeUnityPlugin/wiki

## :scroll: LICENSE

[MIT](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/LICENSE)

Note that some files are distributed under other licenses.

- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/e6c19885c6d3c6f410c730952aeed2852790d306/LICENSE))
- emscripten ([MIT](https://github.com/emscripten-core/emscripten/blob/7c873832e933e86855f5ef5f7c6438f0e457c94e/LICENSE))
  - `third_party/mediapipe_emscripten_patch.diff` contains code copied from emscripten
- FontAwesome ([LICENSE](https://github.com/FortAwesome/Font-Awesome/blob/7cbd7f9951be31f9d06b6ac97739a700320b9130/LICENSE.txt))
  - Sample scenes use Font Awesome fonts

See also [Third Party Notices.md](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/Third%20Party%20Notices.md).
