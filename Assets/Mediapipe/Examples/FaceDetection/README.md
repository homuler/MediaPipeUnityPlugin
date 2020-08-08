# Face Detection on Desktop

## Dependent Calculators
Please include those calculators when building a shared object file.

```txt
//mediapipe/calculators/core:flow_limiter_calculator
//mediapipe/calculators/image:image_transformation_calculator
//mediapipe/calculators/tflite:ssd_anchors_calculator
//mediapipe/calculators/tflite:tflite_converter_calculator
//mediapipe/calculators/tflite:tflite_inference_calculator
//mediapipe/calculators/tflite:tflite_tensors_to_detections_calculator
//mediapipe/calculators/util:annotation_overlay_calculator
//mediapipe/calculators/util:detection_label_id_to_text_calculator
//mediapipe/calculators/util:detection_letterbox_removal_calculator
//mediapipe/calculators/util:detections_to_render_data_calculator
//mediapipe/calculators/util:non_max_suppression_calculator
```

## Dependent Models
Please place these files under `Assets/Mediapipe/SDK/Models`.

```txt
face_detection_front.tflite
face_detection_front_labelmap.txt
```

## Build
```sh
### Running on CPU
bazel build -c opt --define MEDIAPIPE_DISABLE_GPU=1 //mediapipe/apis:mediapipe_c
```
