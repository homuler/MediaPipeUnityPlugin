# Mediapipe Unity Plugin

This is a sample Unity (2019.3.12f1) Plugin to use Mediapipe (only tested on Linux).

## Prerequisites
### Build
Please build the native mediapipe plugins...

```sh
git clone https://github.com/homuler/MediapipeUnityPlugin.git
cd MediapipeUnityPlugin/C

# Running on CPU
bazel build -c opt --define MEDIAPIPE_DISABLE_GPU=1 //mediapipe_api:mediapipe_c

# Running on GPU
bazel build -c opt --copt -DMESA_EGL_NO_X11_HEADERS --copt -DEGL_NO_X11 //mediapipe_api:mediapipe_c
```

and place the generated shared object (e.g. `libmediapipe_c.so`) in Assets/Mediapipe/SDK/Plugins/.

#### ATTENTION!
You should edit BUILD file before building so as to only include necessary calculators.
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
- [ ] AutoFlip (on CPU/GPU)
- [ ] Render landmarks on Unity

## LICENSE
MIT
