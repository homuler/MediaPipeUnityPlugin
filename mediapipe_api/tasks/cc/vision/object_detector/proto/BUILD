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
        ":object_detector_options_cs",
    ],
    prefix = "Tasks/Vision/ObjectDetector/Proto",
)

csharp_proto_src(
    name = "object_detector_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/object_detector/proto/object_detector_options.proto",
    deps = [
        "@mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/object_detector/proto:protos_src",
        "@mediapipe//mediapipe/calculators/tensor:protos_src",
        "@mediapipe//mediapipe/framework:protos_src",
        "@mediapipe//mediapipe/gpu:protos_src",
    ],
)
