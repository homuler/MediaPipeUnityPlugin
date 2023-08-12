# Copyright (c) 2023 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(default_visibility = ["//visibility:public"])

cc_library(
    name = "face_geometry",
    srcs = ["face_geometry.cc"],
    hdrs = ["face_geometry.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/framework:packet",
        "//mediapipe_api/external:protobuf",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/proto:face_geometry_cc_proto",
    ],
    alwayslink = True,
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":face_geometry_cs",
        ":face_geometry_graph_options_cs",
        ":mesh_3d_cs",
    ],
    prefix = "Tasks/Vision/FaceGeometry/Proto",
)

csharp_proto_src(
    name = "face_geometry_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_geometry/proto/face_geometry.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/proto:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "face_geometry_graph_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_geometry/proto/face_geometry_graph_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "mesh_3d_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_geometry/proto/mesh_3d.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/proto:protos_src",
    ],
)
