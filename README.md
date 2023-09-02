# MediaPipe Unity Plugin

This is a Unity (2022.3.6f1) [Native Plugin](https://docs.unity3d.com/Manual/NativePlugins.html) to use [MediaPipe](https://github.com/google/mediapipe) (0.10.1).

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
        var graph = new CalculatorGraph(_ConfigText);
        var poller = graph.AddOutputStreamPoller<string>("out").Value();
        graph.StartRun().AssertOk();

        for (var i = 0; i < 10; i++)
        {
            graph.AddPacketToInputStream("in", new StringPacket("Hello World!", new Timestamp(i))).AssertOk();
        }

        graph.CloseInputStream("in").AssertOk();
        var packet = new StringPacket();

        while (poller.Next(packet))
        {
            Debug.Log(packet.Get());
        }
        graph.WaitUntilDone().AssertOk();
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
[^2]: Running MediaPipe on Windows is [experimental](https://google.github.io/mediapipe/getting_started/install.html#installing-on-windows).

## :plate_with_cutlery: Try the sample app

### Example Solutions

Here is a list of [solutions](https://google.github.io/mediapipe/solutions/solutions.html) that you can try in the sample app.

> :bell: The graphs you can run are not limited to the ones in this list.

|                         |      Android       |        iOS         |    Linux (GPU)     |    Linux (CPU)     |    macOS (CPU)     |   Windows (CPU)    | WebGL |
| :---------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | ----- |
|     Face Detection      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|        Face Mesh        | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          Iris           | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          Hands          | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          Pose           | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|        Holistic         | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|   Selfie Segmentation   | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|    Hair Segmentation    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|    Object Detection     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
### UnityEditor

Select any scenes under `Mediapipe/Samples/Scenes` and play.

### Desktop, Android, iOS

Select proper Inference Mode and Asset Loader Type from the Inspector Window.

#### Preferable Inference Mode

If you've built native libraries for CPU (i.e. `--desktop cpu`), select `CPU` for inference mode.\
When building for Android/iOS, make sure that you select `GPU` for inference mode, because `CPU` inference mode is not supported currently.

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
