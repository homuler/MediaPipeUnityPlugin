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
        ":holistic_landmarker_graph_options_cs",
    ],
    prefix = "Tasks/Vision/HolisticLandmarker/Proto",
)

csharp_proto_src(
    name = "holistic_landmarker_graph_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/holistic_landmarker/proto/holistic_landmarker_graph_options.proto",
    deps = [
        "@mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/holistic_landmarker/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/face_detector/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/face_landmarker/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/hand_landmarker/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/pose_detector/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/pose_landmarker/proto:protos_src",
        "@mediapipe//mediapipe/calculators/tensor:protos_src",
        "@mediapipe//mediapipe/framework:protos_src",
        "@mediapipe//mediapipe/gpu:protos_src",
    ],
)
