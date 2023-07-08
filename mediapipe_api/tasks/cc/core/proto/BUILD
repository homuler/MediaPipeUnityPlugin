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
        ":base_options_cs",
        ":external_file_cs",
        ":acceleration_cs",
    ],
    prefix = "Tasks/Core/Proto",
)

csharp_proto_src(
    name = "base_options_cs",
    proto_src = "mediapipe/tasks/cc/core/proto/base_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
    ],
)

csharp_proto_src(
    name = "external_file_cs",
    proto_src = "mediapipe/tasks/cc/core/proto/external_file.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
    ],
)

csharp_proto_src(
    name = "acceleration_cs",
    proto_src = "mediapipe/tasks/cc/core/proto/acceleration.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/tensor:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/tasks/cc/core/proto:protos_src",
    ],
)
