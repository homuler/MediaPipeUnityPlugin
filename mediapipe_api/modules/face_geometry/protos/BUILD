# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(default_visibility = ["//visibility:public"])

cc_library(
    name = "face_geometry",
    srcs = ["face_geometry.cc"],
    hdrs = ["face_geometry.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/modules/face_geometry/protos:face_geometry_cc_proto",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "face_geometry_cs",
    proto_src = "mediapipe/modules/face_geometry/protos/face_geometry.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
        "@com_google_mediapipe//mediapipe/modules/face_geometry/protos:protos_src",
    ],
)

csharp_proto_src(
    name = "mesh_3d_cs",
    proto_src = "mediapipe/modules/face_geometry/protos/mesh_3d.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/face_geometry/protos:protos_src",
    ],
)
