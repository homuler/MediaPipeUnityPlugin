# Hand Tracking on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
## single hand
@com_google_mediapipe//mediapipe/graphs/hand_tracking:desktop_tflite_calculators

## multi hand
@com_google_mediapipe//mediapipe/graphs/hand_tracking:multi_hand_desktop_tflite_calculators

# GPU
## single hand
@com_google_mediapipe//mediapipe/graphs/hand_tracking:mobile_calculators

## multi hand
@com_google_mediapipe//mediapipe/graphs/hand_tracking:multi_hand_mobile_calculators
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
hand_landmark.tflite
handness.txt
palm_detection.tflite
palm_detection_labelmap.txt
```
