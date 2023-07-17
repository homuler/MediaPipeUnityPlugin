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

cc_library(
    name = "validated_graph_config",
    srcs = ["validated_graph_config.cc"],
    hdrs = ["validated_graph_config.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/external/absl:statusor",
        "@com_google_mediapipe//mediapipe/framework:validated_graph_config",
    ],
    alwayslink = True,
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":calculator_cs",
        ":calculator_options_cs",
        ":calculator_profile_cs",
        ":mediapipe_options_cs",
        ":packet_factory_cs",
        ":packet_generator_cs",
        ":status_handler_cs",
        ":stream_handler_cs",
        ":thread_pool_executor_cs",
    ],
    prefix = "Framework",
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
    name = "calculator_profile_cs",
    proto_src = "mediapipe/framework/calculator_profile.proto",
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

csharp_proto_src(
    name = "thread_pool_executor_cs",
    proto_src = "mediapipe/framework/thread_pool_executor.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
