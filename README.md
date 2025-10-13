# MediaPipe Unity Plugin

This is a Unity (>= 2022.3) [Native Plugin](https://docs.unity3d.com/Manual/NativePlugins.html) to use [MediaPipe](https://github.com/google/mediapipe) (0.10.22).

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

For more detailed usage, see [the API Overview](https://github.com/homuler/MediaPipeUnityPlugin/wiki/API-Overview) page or [the tutorials](./docs).

## :hammer_and_wrench: Installation

Please first download the pre-built package from the [releases page](https://github.com/homuler/MediaPipeUnityPlugin/releases).

| file                                  | contents                                                                 |
| ------------------------------------- | ------------------------------------------------------------------------ |
| MediaPipeUnityPlugin-all.zip          | All the source code with required libraries.                             |
| MediaPipeUnityPlugin-all-stripped.zip | Same as `MediaPipeUnityPlugin-all.zip` but the symbols are stripped.     |
| com.github.homuler.mediapipe-\*.tgz   | A [tarball package](https://docs.unity3d.com/Manual/upm-ui-tarball.html) |
| MediaPipeUnityPlugin.\*.unitypackage  | A .unitypackage file                                                     |

If you need to run sample scenes on your mobile devices, prefer `MediaPipeUnityPlugin-all.zip` or `MediaPipeUnityPlugin-all-stripped.zip`.\
To run sample scenes on your mobile devices, you need to place required models properly, but most required setup is already done in `MediaPipeUnityPlugin-all.zip`.

## Build the plugin by yourself

> :warning: In most cases, you don't need to build the plugin by yourself. Only if the pre-built package doesn't work for you, please build the plugin by yourself.

This repository **doesn't include required libraries or models**, so if you clone this repository, you need to build the plugin by yourself.\
See [the build guide](./docs/Build.md) for more details.

## Build a package by yourself

If you want, you can also build the plugin by yourself using `MediaPipeUnityPlugin-all(-stripped).zip`.

### Build a unity package

1. Open this project
1. Click `Tools > Export Unitypackage`
   ![export-unity-package](https://user-images.githubusercontent.com/4690128/163669270-2d5365eb-eac1-46b1-aed5-83c28a377090.png)

- `MediaPipeUnity.[version].unitypackage` file will be created at the project root.

### Build a local tarball file

1. Install `npm` command
1. Build a tarball file

```sh
cd Packages/com.github.homuler.mediapipe
npm pack
# com.github.homuler.mediapipe-[version].tgz will be created

mv com.github.homuler.mediapipe-[version].tgz your/favorite/path
```

## Supported Platforms

> :warning: GPU mode is not supported on macOS and Windows.

|                            |       Editor       |   Linux (x86_64)   |   macOS (x86_64)   |   macOS (ARM64)    |  Windows (x86_64)  |      Android       |        iOS         | WebGL |
| :------------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :---: |
|     Linux (AMD64) [^1]     | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: |                    |       |
|         Intel Mac          | :heavy_check_mark: |                    | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |       |
|           M1 Mac           | :heavy_check_mark: |                    |                    | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: |       |
| Windows 10/11 (AMD64) [^2] | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: | :heavy_check_mark: |                    |       |

[^1]: Tested on Arch Linux.
[^2]: Running MediaPipe on Windows is [experimental](https://ai.google.dev/edge/mediapipe/framework/getting_started/install#installing_on_windows).

## Supported Solutions

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
|   Gesture recognition    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |
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

# :book: Usage

Once you've downloaded the pre-built package, please import the plugin into your project.

- [.unitypackage](https://docs.unity3d.com/Manual/upm-ui-import.html)
- [.tar.gz](https://docs.unity3d.com/Manual/upm-ui-tarball.html)

## For Android

:skull_and_crossbones: If you need to build your app for Android, **please ensure you include `libstdc++_shared.so` in your APK**[^3], otherwise `DllNotFoundException` will be thrown at runtime.

[^3]: `mediapipe_android.aar` contains `libopencv_java4.so` and it depends on `libstdc++_shared.so`. However, some project or plugins may already include `libstdc++_shared.so`, so we don't include `libstdc++_shared.so` in `mediapipe_android.aar`.

The easiest way to include `libstdc++_shared.so` in your APK is to place it in the `Assets/Plugins/Android` directory of your project.

You can also include `libstdc++_shared.so` at build time by adding the following code to your [`mainTemplate.gradle` file](https://docs.unity3d.com/Manual/gradle-templates.html), and the sample project is using this method.

<details>

<summary>NDK >= 23</summary>

```gradle
// Include libc++_shared.so
task copyLibcppShared {
    doLast {
        def ndkDir = android.ndkDirectory
        def abiFilters = android.defaultConfig.ndk.abiFilters
        def destDir = file("$projectDir/src/main/jniLibs")

        // Mapping from ABI to architecture triple (for NDK 23+)
        def abiToTriple = [
            'arm64-v8a': 'aarch64-linux-android',
            'armeabi-v7a': 'arm-linux-androideabi',
            'x86': 'i686-linux-android',
            'x86_64': 'x86_64-linux-android',
            'riscv64': 'riscv64-linux-android'
        ]

        // Find the prebuilt directory (usually there's only one)
        def prebuiltDir = null
        def prebuiltBase = file("$ndkDir/toolchains/llvm/prebuilt")
        if (prebuiltBase.exists()) {
            def prebuiltDirs = prebuiltBase.listFiles()?.findAll { it.isDirectory() }
            if (prebuiltDirs && prebuiltDirs.size() > 0) {
                prebuiltDir = prebuiltDirs[0]
            }
        }

        abiFilters.each { abi ->
            if (prebuiltDir != null) {
                def triple = abiToTriple[abi]
                if (triple != null) {
                    def libcppPath = file("$prebuiltDir/sysroot/usr/lib/$triple/libc++_shared.so")
                    if (libcppPath.exists()) {
                        def destAbiDir = file("$destDir/$abi")
                        copy {
                            from libcppPath
                            into destAbiDir
                        }
                    }
                }
            }
        }
    }
}

task cleanCopyLibcppShared {
    doLast {
        def destDir = file("$projectDir/src/main/jniLibs")
        def abiFilters = android.defaultConfig.ndk.abiFilters

        abiFilters.each { abi ->
            def libcppFile = file("$destDir/$abi/libc++_shared.so")
            if (libcppFile.exists()) {
                libcppFile.delete()
            }
        }
    }
}
clean.dependsOn 'cleanCopyLibcppShared'

tasks.whenTaskAdded { task ->
    if (task.name == "mergeDebugJniLibFolders" || task.name == "mergeReleaseJniLibFolders") {
        task.dependsOn("copyLibcppShared")
    }
}
```

</details>

<details>

<summary>NDK < 23</summary>

```gradle
// Include libc++_shared.so
task copyLibcppShared(type: Copy) {
    def ndkDir = android.ndkDirectory
    from("$ndkDir/sources/cxx-stl/llvm-libc++/libs") { include '**/libc++_shared.so' }
    into("$projectDir/src/main/jniLibs")
}
clean.dependsOn 'cleanCopyLibcppShared'

tasks.whenTaskAdded { task ->
    if (task.name == "mergeDebugJniLibFolders" || task.name == "mergeReleaseJniLibFolders") {
        task.dependsOn("copyLibcppShared")
    }
}
```

</details>

## :plate_with_cutlery: Try the sample app

Before using the plugin in your project, it's strongly recommended that you check if sample scenes work.

![test-face-mesh](https://user-images.githubusercontent.com/4690128/163668702-26357605-c1f2-4678-8fce-3adc258a9aa1.png)

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

## :warning: Technical Limitations

### UnityEditor / Your application may crash!

Since this plugin uses native libraries under the hood, if there is a bug in those libraries, the UnityEditor or the application may crash at runtime.

Additionally, in some cases, MediaPipe may crash the entire program by sending a `SIGABRT` signal instead of throwing an exception.

This may not be a problem in production since it usually happens when there's a fatal bug in the application code, and such bugs are probably fixed before release.\
However, in a development environment, it is very annoying since the UnityEditor crashes.

On Linux and macOS, this plugin avoids UnityEditor crashing by handling `SIGABRT`, so if UnityEditor crashes, please let us know!\
On Windows, there seem to be no ways to handle `SIGABRT` properly, so if you cannot tolerate this, use a different OS.

### Graphics API

If you want to run inference using a GPU, you cannot use OpenGL Core API.
Otherwise, you will encounter an error like the following:

```txt
InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000_mediapipe/mediapipe/gpu/gl_context_egl.cc:261)
```

In practice, this error only occurs on PC standalone builds, and in such cases, please switch the Graphics API to Vulkan.

## :scroll: LICENSE

[MIT](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/LICENSE)

Note that some files are distributed under other licenses.

- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/e6c19885c6d3c6f410c730952aeed2852790d306/LICENSE))
- emscripten ([MIT](https://github.com/emscripten-core/emscripten/blob/7c873832e933e86855f5ef5f7c6438f0e457c94e/LICENSE))
  - `third_party/mediapipe_emscripten_patch.diff` contains code copied from emscripten
- FontAwesome ([LICENSE](https://github.com/FortAwesome/Font-Awesome/blob/7cbd7f9951be31f9d06b6ac97739a700320b9130/LICENSE.txt))
  - Sample scenes use Font Awesome fonts

See also [Third Party Notices.md](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/Third%20Party%20Notices.md).
