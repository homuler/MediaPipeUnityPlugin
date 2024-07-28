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
        ":segmenter_options_cs",
        ":image_segmenter_graph_options_cs",
    ],
    prefix = "Tasks/Vision/ImageSegmenter/Proto",
)

csharp_proto_src(
    name = "segmenter_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/image_segmenter/proto/segmenter_options.proto",
    deps = [
        "@mediapipe//mediapipe/tasks/cc/vision/image_segmenter/proto:protos_src",
    ],
)


csharp_proto_src(
    name = "image_segmenter_graph_options_cs",
    proto_src = "mediapipe/tasks/cc/vision/image_segmenter/proto/image_segmenter_graph_options.proto",
    deps = [
        "@mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
        "@mediapipe//mediapipe/tasks/cc/vision/image_segmenter/proto:protos_src",
        "@mediapipe//mediapipe/calculators/tensor:protos_src",
        "@mediapipe//mediapipe/framework:protos_src",
        "@mediapipe//mediapipe/gpu:protos_src",
    ],
)
