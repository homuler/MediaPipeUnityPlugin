# Hand Tracking on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
@com_google_mediapipe//mediapipe/graphs/hand_tracking:desktop_tflite_calculators

# GPU
@com_google_mediapipe//mediapipe/graphs/hand_tracking:mobile_calculators
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
hand_landmark.tflite
handness.txt
palm_detection.tflite
palm_detection_labelmap.txt
```
