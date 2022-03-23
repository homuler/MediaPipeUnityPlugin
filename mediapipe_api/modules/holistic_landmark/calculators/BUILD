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
        ":roi_tracking_calculator_cs",
    ],
    prefix = "Modules/HolisticLandmark/Calculators",
)

csharp_proto_src(
    name = "roi_tracking_calculator_cs",
    proto_src = "mediapipe/modules/holistic_landmark/calculators/roi_tracking_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/holistic_landmark/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
