# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":thresholding_calculator_cs",
    ],
    prefix = "Calculators/Util",
)

csharp_proto_src(
    name = "thresholding_calculator_cs",
    proto_src = "mediapipe/calculators/util/thresholding_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)
