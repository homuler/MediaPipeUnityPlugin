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
        ":effect_renderer_calculator_cs",
        ":env_generator_calculator_cs",
        ":geometry_pipeline_calculator_cs",
    ],
    prefix = "Modules/FaceGeometry",
)

csharp_proto_src(
    name = "effect_renderer_calculator_cs",
    proto_src = "mediapipe/modules/face_geometry/effect_renderer_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/face_geometry:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "env_generator_calculator_cs",
    proto_src = "mediapipe/modules/face_geometry/env_generator_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/face_geometry:protos_src",
        "@com_google_mediapipe//mediapipe/modules/face_geometry/protos:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "geometry_pipeline_calculator_cs",
    proto_src = "mediapipe/modules/face_geometry/geometry_pipeline_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/face_geometry:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

