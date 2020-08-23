# Iris Tracking on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_cpu_deps

# GPU
@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_gpu_deps
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
face_detection_front.tflite
face_landmark.tflite
iris_landmark.tflite
```

## Notes
The extimation of depth from iris depends on the camera's focal length.
Please adjust `WebCamScreenController#focalLengthPx` before running this example on GPU.
