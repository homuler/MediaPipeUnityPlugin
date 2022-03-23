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
        ":tone_models_cs",
        ":tone_estimation_cs",
        ":region_flow_computation_cs",
        ":motion_saliency_cs",
        ":motion_estimation_cs",
        ":motion_analysis_cs",
        ":region_flow_cs",
        ":motion_models_cs",
        ":camera_motion_cs",
        ":push_pull_filtering_cs",
        ":frame_selection_solution_evaluator_cs",
        ":frame_selection_cs",
        ":flow_packager_cs",
        ":tracking_cs",
        ":box_tracker_cs",
        ":tracked_detection_manager_config_cs",
        ":box_detector_cs",
    ],
    prefix = "Util",
)

csharp_proto_src(
    name = "tone_models_cs",
    proto_src = "mediapipe/util/tracking/tone_models.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "tone_estimation_cs",
    proto_src = "mediapipe/util/tracking/tone_estimation.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "region_flow_computation_cs",
    proto_src = "mediapipe/util/tracking/region_flow_computation.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "motion_saliency_cs",
    proto_src = "mediapipe/util/tracking/motion_saliency.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "motion_estimation_cs",
    proto_src = "mediapipe/util/tracking/motion_estimation.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "motion_analysis_cs",
    proto_src = "mediapipe/util/tracking/motion_analysis.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "region_flow_cs",
    proto_src = "mediapipe/util/tracking/region_flow.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "motion_models_cs",
    proto_src = "mediapipe/util/tracking/motion_models.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "camera_motion_cs",
    proto_src = "mediapipe/util/tracking/camera_motion.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "push_pull_filtering_cs",
    proto_src = "mediapipe/util/tracking/push_pull_filtering.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "frame_selection_solution_evaluator_cs",
    proto_src = "mediapipe/util/tracking/frame_selection_solution_evaluator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "frame_selection_cs",
    proto_src = "mediapipe/util/tracking/frame_selection.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "flow_packager_cs",
    proto_src = "mediapipe/util/tracking/flow_packager.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "tracking_cs",
    proto_src = "mediapipe/util/tracking/tracking.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "box_tracker_cs",
    proto_src = "mediapipe/util/tracking/box_tracker.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "tracked_detection_manager_config_cs",
    proto_src = "mediapipe/util/tracking/tracked_detection_manager_config.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)

csharp_proto_src(
    name = "box_detector_cs",
    proto_src = "mediapipe/util/tracking/box_detector.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/util/tracking:protos_src",
    ],
)
