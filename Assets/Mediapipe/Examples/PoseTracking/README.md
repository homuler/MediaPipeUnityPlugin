# Pose Tracking on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
@com_google_mediapipe//mediapipe/graphs/pose_tracking:upper_body_pose_tracking_cpu_deps

# GPU
@com_google_mediapipe//mediapipe/graphs/pose_tracking:upper_body_pose_tracking_gpu_deps
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
pose_detection.tflite
```
