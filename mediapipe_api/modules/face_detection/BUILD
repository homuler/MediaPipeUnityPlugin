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
        ":face_detection_cs",
    ],
    prefix = "Modules/FaceDetection",
)

csharp_proto_src(
    name = "face_detection_cs",
    proto_src = "mediapipe/modules/face_detection/face_detection.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/face_detection:protos_src",
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)
