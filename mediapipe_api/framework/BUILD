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
    name = "calculator",
    srcs = ["calculator.cc"],
    hdrs = ["calculator.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "@com_google_mediapipe//mediapipe/framework:calculator_framework",
    ],
    alwayslink = True,
)

cc_library(
    name = "calculator_graph",
    srcs = ["calculator_graph.cc"],
    hdrs = ["calculator_graph.h"],
    deps = [
        ":packet",
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:status",
        "//mediapipe_api/external/absl:statusor",
        "@com_google_mediapipe//mediapipe/framework:calculator_framework",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/gpu:gl_calculator_helper",
            "@com_google_mediapipe//mediapipe/gpu:gpu_shared_data_internal",
        ],
    }),
    alwayslink = True,
)

cc_library(
    name = "output_stream_poller",
    srcs = ["output_stream_poller.cc"],
    hdrs = ["output_stream_poller.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:statusor",
        "@com_google_mediapipe//mediapipe/framework:calculator_framework",
    ],
    alwayslink = True,
)

cc_library(
    name = "packet",
    srcs = ["packet.cc"],
    hdrs = ["packet.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "@com_google_mediapipe//mediapipe/framework:packet",
    ],
    alwayslink = True,
)

cc_library(
    name = "timestamp",
    srcs = ["timestamp.cc"],
    hdrs = ["timestamp.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework:timestamp",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "calculator_cs",
    proto_src = "mediapipe/framework/calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "calculator_options_cs",
    proto_src = "mediapipe/framework/calculator_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "mediapipe_options_cs",
    proto_src = "mediapipe/framework/mediapipe_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_factory_cs",
    proto_src = "mediapipe/framework/packet_factory.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_generator_cs",
    proto_src = "mediapipe/framework/packet_generator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "status_handler_cs",
    proto_src = "mediapipe/framework/status_handler.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "stream_handler_cs",
    proto_src = "mediapipe/framework/stream_handler.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
