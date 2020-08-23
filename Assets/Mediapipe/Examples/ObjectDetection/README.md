# Object Detection on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
# CPU
@com_google_mediapipe//mediapipe/graphs/object_detection:desktop_tflite_calculators

# GPU
@com_google_mediapipe//mediapipe/graphs/object_detection:mobile_calculators
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
ssdlite_object_detection.tflite
ssdlite_object_detection_labelmap.txt
```
