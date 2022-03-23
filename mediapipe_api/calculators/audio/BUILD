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
        ":mfcc_mel_calculators_cs",
        ":rational_factor_resample_calculator_cs",
        ":spectrogram_calculator_cs",
        ":stabilized_log_calculator_cs",
        ":time_series_framer_calculator_cs",
    ],
    prefix = "Calculators/Audio",
)

csharp_proto_src(
    name = "mfcc_mel_calculators_cs",
    proto_src = "mediapipe/calculators/audio/mfcc_mel_calculators.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/audio:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "rational_factor_resample_calculator_cs",
    proto_src = "mediapipe/calculators/audio/rational_factor_resample_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/audio:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "spectrogram_calculator_cs",
    proto_src = "mediapipe/calculators/audio/spectrogram_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/audio:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "stabilized_log_calculator_cs",
    proto_src = "mediapipe/calculators/audio/stabilized_log_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/audio:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "time_series_framer_calculator_cs",
    proto_src = "mediapipe/calculators/audio/time_series_framer_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/audio:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
