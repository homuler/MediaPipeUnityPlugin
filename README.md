# MediaPipe Unity Plugin
This is a Unity (2019.4.10f1) Plugin to use MediaPipe.

## Platforms
- [x] Linux Desktop (tested on ArchLinux)
- [x] Android (ARM v7)
- [x] OS X
- [ ] iOS

## Prerequisites
### MediaPipe
Please be sure to install required packages and check if you can run the official demos on your machine.

### OpenCV
By default, it is assumed that OpenCV 3 is installed under `/usr` (e.g. `/usr/lib/libopencv_core.so`).
If your version or path is different, please edit [C/third_party/opencv_linux.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/third_party/opencv_linux.BUILD) and [C/WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/WORKSPACE).

### .NET Core
This project uses protocol buffers to communicate with MediaPipe, and it is necessary to install .NET Core SDK(3.x) and .NET Core runtime 2.1 to build `Google.Protobuf.dll`.

## Build
### Libraries
1. Clone the repository
    ```sh
    git clone https://github.com/homuler/MediaPipeUnityPlugin.git
    cd MediaPipeUnityPlugin
    ```

2. Build native libraries
    ```sh
    # For Desktop GPU
    make gpu

    # For Desktop CPU
    make cpu

    # For Android
    make android_arm
    ```

3. Copy the libraries and model files under `Assets`
    ```sh
    # Copy libraries to `Assets/MediaPipe/SDK/Plugins` and
    # model files to `Assets/MediaPipe/SDK/Models`
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
In UnityEditor, you can run example scenes after running `make gpu/cpu` and `make install`.

### Desktop
To include model files in the package, it is neccessary to build an AssetBundle before building the app.
You can build it by clicking **Assets > Build AssetBundles** from the menu.\
The AssetBundle file will be created under `Assets/StreamingAssets`.

### Android
See [Desktop](#Desktop) to build AssetBundles.\
Example scenes for mobile device is not implemented yet, but `DesktopGPU` scene can be run on Android devices.

If you prefer, model files can be included in `mediapipe_android.aar` instead, and in that case, skip the AssetBundle build step.

## Example Scenes
- Hello World!
- Face Detection (on CPU/GPU)
- Face Mesh (on CPU/GPU)
- Iris Tracking (on CPU/GPU)
- Hand Tracking (on CPU/GPU)
- Pose Tracking (on CPU/GPU)
- Hair Segmentation (on GPU)
- Object Detection (on CPU/GPU)

### Troubleshooting
#### DllNotFoundException: mediapipe_c
[OpenCV's path](https://github.com/homuler/MediaPipeUnityPlugin#opencv) may not be configured properly.

If you're sure the path is correct, please check on **Load on startup** in the plugin inspector, click **Apply** button, and restart Unity Editor.
Some helpful logs will be output in the console.

#### InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000
If you encounter an error like below and you use OpenGL Core as the Unity's graphics APIs, please try Vulkan.

```txt
InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000_mediapipe/mediapipe/gpu/gl_context_egl.cc:261)
```

### TODO
- [ ] Prepare API Documents
- [ ] Box Tracking (on CPU/GPU)
- [ ] iOS
- [ ] Windows

## LICENSE
MIT

Note that some files are distributed under other licenses.
- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/master/LICENSE))
