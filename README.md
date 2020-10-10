# MediaPipe Unity Plugin
This is a sample Unity (2019.4.10f1) Plugin to use MediaPipe.

## Platforms
- [x] Linux Desktop (tested on ArchLinux)
- [ ] Android (under construction)
- [ ] OS X
- [ ] iOS

## Prerequisites
### MediaPipe
Please be sure to install required packages and check if you can run the official demos on your machine.

### OpenCV
By default, it is assumed that you use OpenCV 3 and it is installed under `/usr` (e.g. `/usr/lib/libopencv_core.so`).
If your version or path is different, please edit [C/third_party/opencv_linux.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/third_party/opencv_linux.BUILD) and [C/WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/WORKSPACE).

### Protocol Buffer
The protocol buffer compiler is required.
It is also necessary to install .NET Core SDK(3.x) and .NET Core runtime 2.1 to build `Google.Protobuf.dll`.

### Build
```sh
git clone https://github.com/homuler/MediaPipeUnityPlugin.git
cd MediaPipeUnityPlugin

# build libraries
make

# Or if you prefer to run graphs on CPU
#    make MODE=cpu

make install
```

You may want to edit BUILD file before building so as to only include necessary calculators to reduce the library size.
For more information, please see the [BUILD file](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/mediapipe_api/BUILD).

### Models
The models used in example scenes are copied under `Assets/MediaPipe/SDK/Models` by running `make install`.

If you'd like to use other models, you should place them so that Unity can read.
For example, if your graph depends on `face_detection_front.tflite`, then you can place the model file under `Assets/MediaPipe/SDK/Models/` and set the path to the `model_path` value in your config file.

If neccessary, you can also change the model paths for subgraphs (e.g. FaceDetectionFrontCpu) by updating [mediapipe_model_path.diff](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/C/third_party/mediapipe_model_path.diff).

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
[OpenCV's path](https://github.com/homuler/MediaPipeUnityPlugin#opencv) may not be configured properly.\

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
- [ ] Android
- [ ] iOS
- [ ] Windows

## LICENSE
MIT
