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
        ":flow_to_image_calculator_cs",
        ":opencv_video_encoder_calculator_cs",
        ":motion_analysis_calculator_cs",
        ":flow_packager_calculator_cs",
        ":box_tracker_calculator_cs",
        ":tracked_detection_manager_calculator_cs",
        ":box_detector_calculator_cs",
        ":video_pre_stream_calculator_cs",
    ],
    prefix = "Calculators/Video",
)

csharp_proto_src(
    name = "flow_to_image_calculator_cs",
    proto_src = "mediapipe/calculators/video/flow_to_image_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "opencv_video_encoder_calculator_cs",
    proto_src = "mediapipe/calculators/video/opencv_video_encoder_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "motion_analysis_calculator_cs",
    proto_src = "mediapipe/calculators/video/motion_analysis_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "flow_packager_calculator_cs",
    proto_src = "mediapipe/calculators/video/flow_packager_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "box_tracker_calculator_cs",
    proto_src = "mediapipe/calculators/video/box_tracker_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "tracked_detection_manager_calculator_cs",
    proto_src = "mediapipe/calculators/video/tracked_detection_manager_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "box_detector_calculator_cs",
    proto_src = "mediapipe/calculators/video/box_detector_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "video_pre_stream_calculator_cs",
    proto_src = "mediapipe/calculators/video/video_pre_stream_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/video:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
