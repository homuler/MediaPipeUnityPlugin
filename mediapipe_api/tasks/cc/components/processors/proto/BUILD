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
        ":classifier_options_cs",
    ],
    prefix = "Tasks/Components/Processors/Proto",
)

csharp_proto_src(
    name = "classifier_options_cs",
    proto_src = "mediapipe/tasks/cc/components/processors/proto/classifier_options.proto",
    deps = [
        "@mediapipe//mediapipe/tasks/cc/components/processors/proto:protos_src",
    ],
)
