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
        ":anchor_cs",
    ],
    prefix = "Framework/Formats/ObjectDetection",
)

csharp_proto_src(
    name = "anchor_cs",
    proto_src = "mediapipe/framework/formats/object_detection/anchor.proto",
    deps = [
        "@mediapipe//mediapipe/framework/formats/object_detection:protos_src",
    ],
)
