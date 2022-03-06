# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

filegroup(
    name = "proto_srcs",
    srcs = [
        ":tensors_to_detections_calculator_cs",
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
