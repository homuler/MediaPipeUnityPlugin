# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(default_visibility = ["//visibility:public"])

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":ssd_anchors_calculator_cs",
        ":tflite_custom_op_resolver_calculator_cs",
        ":tflite_inference_calculator_cs",
        ":tflite_converter_calculator_cs",
        ":tflite_tensors_to_segmentation_calculator_cs",
        ":tflite_tensors_to_detections_calculator_cs",
        ":tflite_tensors_to_classification_calculator_cs",
        ":tflite_tensors_to_landmarks_calculator_cs",
    ],
    prefix = "Calculators/Tflite",
)

csharp_proto_src(
    name = "ssd_anchors_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/ssd_anchors_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats/object_detection:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_custom_op_resolver_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_custom_op_resolver_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_inference_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_inference_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_converter_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_converter_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_tensors_to_segmentation_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_tensors_to_segmentation_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_tensors_to_detections_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_tensors_to_detections_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_tensors_to_classification_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_tensors_to_classification_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_tensors_to_landmarks_calculator_cs",
    proto_src = "mediapipe/calculators/tflite/tflite_tensors_to_landmarks_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tflite:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
