# Face Detection on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_tflite_calculators

# GPU
@com_google_mediapipe//mediapipe/graphs/face_detection:mobile_calculators
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
face_detection_front.tflite
face_detection_front_labelmap.txt
```
