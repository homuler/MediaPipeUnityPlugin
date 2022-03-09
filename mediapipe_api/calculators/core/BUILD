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
        ":concatenate_vector_calculator_cs",
        ":dequantize_byte_array_calculator_cs",
        ":packet_cloner_calculator_cs",
        ":packet_resampler_calculator_cs",
        ":packet_thinner_calculator_cs",
        ":split_vector_calculator_cs",
        ":quantize_float_vector_calculator_cs",
        ":sequence_shift_calculator_cs",
        ":gate_calculator_cs",
        ":constant_side_packet_calculator_cs",
        ":clip_vector_size_calculator_cs",
        ":flow_limiter_calculator_cs",
        ":graph_profile_calculator_cs",
    ],
    prefix = "Calculators/Core",
)

csharp_proto_src(
    name = "concatenate_vector_calculator_cs",
    proto_src = "mediapipe/calculators/core/concatenate_vector_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "dequantize_byte_array_calculator_cs",
    proto_src = "mediapipe/calculators/core/dequantize_byte_array_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_cloner_calculator_cs",
    proto_src = "mediapipe/calculators/core/packet_cloner_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_resampler_calculator_cs",
    proto_src = "mediapipe/calculators/core/packet_resampler_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_thinner_calculator_cs",
    proto_src = "mediapipe/calculators/core/packet_thinner_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "split_vector_calculator_cs",
    proto_src = "mediapipe/calculators/core/split_vector_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "quantize_float_vector_calculator_cs",
    proto_src = "mediapipe/calculators/core/quantize_float_vector_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "sequence_shift_calculator_cs",
    proto_src = "mediapipe/calculators/core/sequence_shift_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "gate_calculator_cs",
    proto_src = "mediapipe/calculators/core/gate_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "constant_side_packet_calculator_cs",
    proto_src = "mediapipe/calculators/core/constant_side_packet_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "clip_vector_size_calculator_cs",
    proto_src = "mediapipe/calculators/core/clip_vector_size_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "flow_limiter_calculator_cs",
    proto_src = "mediapipe/calculators/core/flow_limiter_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "graph_profile_calculator_cs",
    proto_src = "mediapipe/calculators/core/graph_profile_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
