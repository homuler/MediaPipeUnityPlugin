# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "transformations",
    srcs = ["transformations.cc"],
    hdrs = ["transformations.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking/calculators:tracked_anchor_manager_calculator",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "sticker_buffer_cs",
    import_prefix = "Graphs/InstantMotionTracking",
    proto_src = "mediapipe/graphs/instant_motion_tracking/calculators/sticker_buffer.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking/calculators:protos_src",
    ],
)
