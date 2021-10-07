# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "model_matrix",
    srcs = ["model_matrix.cc"],
    hdrs = ["model_matrix.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/graphs/object_detection_3d/calculators:model_matrix_cc_proto",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "model_matrix_cs",
    import_prefix = "Graphs/ObjectDetection3d",
    proto_src = "mediapipe/graphs/object_detection_3d/calculators/model_matrix.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/object_detection_3d/calculators:protos_src",
    ],
)
