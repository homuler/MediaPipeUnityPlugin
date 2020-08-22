# Face Mesh on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_calculators

# GPU
@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_gpu_calculators
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
face_detection_front.tflite
face_landmark.tflite
```
