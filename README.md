# MediaPipe Unity Plugin
This is a Unity (2019.4.18f1) Plugin to use MediaPipe.

## Platforms
- [x] Linux Desktop (tested on ArchLinux)
- [x] Android
- [x] iOS
- [x] macOS (CPU only)
- [x] Windows 10 (CPU only, experimental)

## Prerequisites
### MediaPipe
Please be sure to install required packages and check if you can run the official demos on your machine.

### OpenCV
By default, it is assumed that OpenCV 3 is installed under `/usr` (e.g. `/usr/lib/libopencv_core.so`).
If your version or path is different, please edit [C/third_party/opencv_linux.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/third_party/opencv_linux.BUILD) and [C/WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/WORKSPACE).

### .NET Core
This project uses protocol buffers to communicate with MediaPipe, and it is necessary to install .NET Core SDK(3.x) and .NET Core runtime 2.1 to build `Google.Protobuf.dll`.

## Build
1. Clone the repository
    ```sh
    git clone https://github.com/homuler/MediaPipeUnityPlugin.git
    cd MediaPipeUnityPlugin
    ```

2. Build native libraries
    ### Desktop GPU (Linux only)
    ```sh
    make
    make install
    ```

    ### Desktop CPU (Linux, macOS)
    ```sh
    make cpu
    make install
    ```

    ### Windows 10
    ```sh
    # If `python.exe` is installed at 'C:\path\to\python.exe'
    make cpu PYTHON_BIN_PATH="C://path//to//python.exe"
    make install
    ```

    ### Android
    ```sh
    # ARM64
    make android_arm64
    make install

    # ARMv7
    make android_arm
    make install
    ```

    ### iOS
    ```sh
    make ios_arm64
    make install
    ```

Note that you cannot build libraries for multiple platforms at the same time,
because the built result will be overwritten.\
If you'd like to build `libmediapipe_c.so` and `mediapipe_android.aar`, please `make` them individually.
```sh
make gpu
make install

make android_arm
make install
```

## Run example scenes
### UnityEditor
On UnityEditor, you can run example scenes after running `make gpu/cpu` and `make install`.
Note that you need to run those commands even if you have run `make android_arm64` or `make ios_arm64`.

### Desktop
If you'd like to run graphs on CPU, uncheck `Use GPU` from the inspector window.
![scene-director-use-gpu](https://user-images.githubusercontent.com/4690128/107133987-4f51b180-6931-11eb-8a75-4993a5c70cc1.png)
To include model files in the package, it is neccessary to build an AssetBundle before building the app.
You can build it by clicking **Assets > Build AssetBundles** from the menu.\
The AssetBundle file will be created under `Assets/StreamingAssets`.

### Android
See [Desktop](#Desktop) to build AssetBundles.\
If you prefer, model files can be included in `mediapipe_android.aar` instead, and in that case, skip the AssetBundle build step.

### iOS
See [Desktop](#Desktop) to build AssetBundles.\

## Example Graphs
[]()                    | Android | iOS | Linux (GPU) | Linux (CPU) | macOS | Windows
:---------------------: | :-----: | :-: | :---------: | :---------: | :---: | :------:
Face Detection          | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Face Mesh               | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Iris                    | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Hands                   | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Pose                    | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Holistic (with iris)    | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Hair Segmentationon     | ✅       |     | ✅           |             |       |
Object Detectionn       | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Box Tracking            | ✅       | ✅   | ✅           | ✅           | ✅     | 🔺
Instant Motion Tracking |         |     |             |             |       |
Objectron               |         |     |             |             |       |
KNIFT                   |         |     |             |             |       |

## Troubleshooting
### DllNotFoundException: mediapipe_c
[OpenCV's path](https://github.com/homuler/MediaPipeUnityPlugin#opencv) may not be configured properly.

If you're sure the path is correct, please check on **Load on startup** in the plugin inspector, click **Apply** button, and restart Unity Editor.
Some helpful logs will be output in the console.

### InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000
If you encounter an error like below and you use OpenGL Core as the Unity's graphics APIs, please try Vulkan.

```txt
InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000_mediapipe/mediapipe/gpu/gl_context_egl.cc:261)
```

### Debug MediaPipe
Set an environment variable `GLOG_v` before loading native libraries (e.g. `libmediapipe_c.so`).

```cs
void OnEnable() {
    // see https://github.com/google/glog#setting-flags
    System.Environment.SetEnvironmentVariable("GLOG_v", "2");
}
```

MediaPipe logs will be output to log files (e.g. `Editor.log`).

## TODO
- [ ] Prepare API Documents
- [ ] Implement cross-platform APIs to send images to MediaPipe
- [ ] use CVPixelBuffer on iOS
- [ ] Box Tracking (on CPU/GPU)
- [ ] Instant Motion Tracking
- [ ] Objectron
- [ ] KNIFT

## LICENSE
MIT

Note that some files are distributed under other licenses.
- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/master/LICENSE))
