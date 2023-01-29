# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":image_to_tensor_calculator_cs",
        ":inference_calculator_cs",
        ":landmarks_to_tensor_calculator_cs",
        ":tensor_converter_calculator_cs",
        ":tensors_to_classification_calculator_cs",
        ":tensors_to_detections_calculator_cs",
        ":tensors_to_floats_calculator_cs",
        ":tensors_to_landmarks_calculator_cs",
        ":tensors_to_segmentation_calculator_cs",
    ],
    prefix = "Calculators/Tensor",
)

csharp_proto_src(
    name = "inference_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/inference_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tensor_converter_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/tensor_converter_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tensors_to_detections_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/tensors_to_detections_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tensors_to_landmarks_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/tensors_to_landmarks_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "landmarks_to_tensor_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/landmarks_to_tensor_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tensors_to_floats_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/tensors_to_floats_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tensors_to_classification_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/tensors_to_classification_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "image_to_tensor_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/image_to_tensor_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)

csharp_proto_src(
    name = "tensors_to_segmentation_calculator_cs",
    proto_src = "mediapipe/calculators/tensor/tensors_to_segmentation_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)
