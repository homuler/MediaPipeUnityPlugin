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
    name = "gl_base",
    hdrs = ["gl_base.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/gpu:gl_base",
    ],
    alwayslink = True,
)

cc_library(
    name = "gl_calculator_helper",
    srcs = ["gl_calculator_helper.cc"],
    hdrs = ["gl_calculator_helper.h"],
    copts = [
        "-Wno-return-type-c-linkage",
    ],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:status",
        "@com_google_mediapipe//mediapipe/gpu:gl_calculator_helper",
    ],
    alwayslink = True,
)

cc_library(
    name = "gl_context",
    srcs = ["gl_context.cc"],
    hdrs = ["gl_context.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/gpu:gl_context",
    ],
    alwayslink = True,
)

cc_library(
    name = "gl_texture_buffer",
    srcs = ["gl_texture_buffer.cc"],
    hdrs = ["gl_texture_buffer.h"],
    copts = [
        "-Wno-return-type-c-linkage",
    ],
    deps = [
        ":gl_context",
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/gpu:gl_texture_buffer",
    ],
    alwayslink = True,
)

cc_library(
    name = "gpu_buffer",
    srcs = ["gpu_buffer.cc"],
    hdrs = ["gpu_buffer.h"],
    copts = [
        "-Wno-return-type-c-linkage",
    ],
    deps = [
        ":gl_texture_buffer",
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:status",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/gpu:gpu_buffer",
    ],
    alwayslink = True,
)

cc_library(
    name = "gpu_buffer_format",
    srcs = ["gpu_buffer_format.cc"],
    hdrs = ["gpu_buffer_format.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/gpu:gpu_buffer_format",
    ],
    alwayslink = True,
)

cc_library(
    name = "gpu_shared_data_internal",
    srcs = ["gpu_shared_data_internal.cc"],
    hdrs = ["gpu_shared_data_internal.h"],
    deps = [
        ":gl_context",
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:status",
        "@com_google_mediapipe//mediapipe/gpu:gpu_shared_data_internal",
    ],
    alwayslink = True,
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":gpu_origin_cs",
        ":scale_mode_cs",
        ":gl_scaler_calculator_cs",
        ":gl_surface_sink_calculator_cs",
        ":copy_calculator_cs",
        ":gl_context_options_cs",
    ],
    prefix = "Gpu",
)

csharp_proto_src(
    name = "gpu_origin_cs",
    proto_src = "mediapipe/gpu/gpu_origin.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)

csharp_proto_src(
    name = "scale_mode_cs",
    proto_src = "mediapipe/gpu/scale_mode.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)

csharp_proto_src(
    name = "gl_scaler_calculator_cs",
    proto_src = "mediapipe/gpu/gl_scaler_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "gl_surface_sink_calculator_cs",
    proto_src = "mediapipe/gpu/gl_surface_sink_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "copy_calculator_cs",
    proto_src = "mediapipe/gpu/copy_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "gl_context_options_cs",
    proto_src = "mediapipe/gpu/gl_context_options.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
