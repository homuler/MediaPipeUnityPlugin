# Copyright (c) 2023 homuler
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
        ":tensors_to_segmentation_calculator_cs",
    ],
    prefix = "Tasks/Vision/ImageSegmenter/Proto",
)

csharp_proto_src(
    name = "tensors_to_segmentation_calculator_cs",
    proto_src = "mediapipe/tasks/cc/vision/image_segmenter/calculators/tensors_to_segmentation_calculator.proto",
    deps = [
        "@mediapipe//mediapipe/tasks/cc/vision/image_segmenter/calculators:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/image_segmenter/proto:protos_src",
        "@mediapipe//mediapipe/framework:protos_src",
        "@mediapipe//mediapipe/util:protos_src",
    ],
)
