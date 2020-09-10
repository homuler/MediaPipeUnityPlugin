# Mediapipe Unity Plugin
This is a sample Unity (2019.3.12f1) Plugin to use Mediapipe (only tested on Linux).

## Prerequisites
### MediaPipe
Please be sure to install required packages and check if you can run the official demos on your machine.

### OpenCV
By default, it is assumed that you use OpenCV 3 and it is installed under `/usr`.
If your version or path is different, please edit `C/third_party/opencv_linux.BUILD` or `C/WORKSPACE`.

### Build
Please build native plugins(mediapiie, protobuf) and place them under `Assets/Mediapipe/SDK/Plugins`.
You can do that by running `make`.

```sh
git clone https://github.com/homuler/MediapipeUnityPlugin.git
cd MediapipeUnityPlugin

# build libraries to run models on GPU
make

# or to run models on CPU
make MODE=cpu
```

#### ATTENTION!
You may want to edit BUILD file before building so as to only include necessary calculators to reduce the library size.
For more information, please see the README of each scenes and the [BUILD file](https://github.com/homuler/MediapipeUnityPlugin/blob/master/C/mediapipe_api/BUILD).

### Models
It is also necessary to place dependent models so that Unity can read them.
For example, if your graph depends on `face_detection_front.tflite`, then you can place the model file under `Assets/Mediapipe/SDK/Models/` and set the path to the `model_path` value in your config file.

If neccessary, you can also change the model paths for subgraphs (e.g. FaceDetectionFrontCpu) by updating [mediapipe_model_path.diff](https://github.com/homuler/MediapipeUnityPlugin/blob/master/C/third_party/mediapipe_model_path.diff).

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
If you encounter an error like below and you use OpenGL Core as the Unity's graphics APIs, please try Vulkan.

```txt
InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000_mediapipe/mediapipe/gpu/gl_context_egl.cc:261)
```

### TODO
- [ ] Box Tracking (on CPU/GPU)
- [ ] Render annotations on Unity
- [ ] Android
- [ ] iOS
- [ ] Windows

## LICENSE
MIT
