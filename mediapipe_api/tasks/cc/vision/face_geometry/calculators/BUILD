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
        ":face_geometry_pipeline_calculator_options_cs",
    ],
    prefix = "Tasks/Vision/FaceGeometry",
)

csharp_proto_src(
    name = "face_geometry_pipeline_calculator_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_geometry/calculators/geometry_pipeline_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
