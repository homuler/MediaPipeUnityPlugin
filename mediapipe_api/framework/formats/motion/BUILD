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
        ":optical_flow_field_data_cs",
    ],
    prefix = "Framework/Formats/Motion",
)

csharp_proto_src(
    name = "optical_flow_field_data_cs",
    proto_src = "mediapipe/framework/formats/motion/optical_flow_field_data.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats/motion:protos_src",
    ],
)
