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
        ":face_blendshapes_graph_options_cs",
        ":face_landmarker_graph_options_cs",
        ":face_landmarks_detector_graph_options_cs",
    ],
    prefix = "Tasks/Vision/FaceLandmarker/Proto",
)

csharp_proto_src(
    name = "face_blendshapes_graph_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_landmarker/proto/face_blendshapes_graph_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_landmarker/proto:protos_src",
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "face_landmarker_graph_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_landmarker/proto/face_landmarker_graph_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_detector/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_geometry/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_landmarker/proto:protos_src",
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "face_landmarks_detector_graph_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/face_landmarker/proto/face_landmarks_detector_graph_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_landmarker/proto:protos_src",
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
