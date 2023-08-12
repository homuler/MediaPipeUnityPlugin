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
        ":classifications_cs",
    ],
    prefix = "Tasks/Components/Containers/Proto",
)

csharp_proto_src(
    name = "classifications_cs",
    proto_src = "mediapipe/tasks/cc/components/containers/proto/classifications.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/components/containers/proto:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)
