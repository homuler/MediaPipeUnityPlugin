# Mediapipe Unity Plugin

This is a sample Unity (2019.3.12f1) Plugin to use Mediapipe (only tested on Linux).

## Prerequisites
### Build
Please build the native mediapipe plugins...

```sh
git clone https://github.com/homuler/mediapipe.git
cd mediapipe
git checkout feature/c-api-for-unity

# Running on CPU
bazel build -c opt --define MEDIAPIPE_DISABLE_GPU=1 //mediapipe/apis:mediapipe_c
```

and place the generated shared object (e.g. `libmediapipe_c.so`) in Assets/Mediapipe/SDK/Plugins/.

#### ATTENTION!
You should edit BUILD file before building so as to only include necessary calculators.
For more information, please see the README of each scenes and the [BUILD file](https://github.com/homuler/mediapipe/blob/feature/c-api-for-unity/mediapipe/apis/BUILD).

### Models
It is also necessary to place dependent models so that Unity can read them.
For example, if your graph depends on `face_detection_front.tflite`, then you can place the model file under `Assets/Mediapipe/SDK/Models/` and set the path to the `model_path` value in your config file.

## Example Scenes
- Hello World!
- Face Detection (on CPU/GPU)

### TODO
- [ ] Face Mesh (on CPU/GPU)
- [ ] Iris (on CPU/GPU)
- [ ] Hands (on CPU/GPU)
- [ ] Hair Segmentation (on CPU/GPU)
- [ ] Object Detection (on CPU/GPU)
- [ ] Box Tracking (on CPU/GPU)
- [ ] AutoFlip (on CPU/GPU)

## LICENSE
MIT
