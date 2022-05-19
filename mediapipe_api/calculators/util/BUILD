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
        ":annotation_overlay_calculator_cs",
        ":detection_label_id_to_text_calculator_cs",
        ":filter_detections_calculator_cs",
        ":timed_box_list_id_to_label_calculator_cs",
        ":latency_cs",
        ":non_max_suppression_calculator_cs",
        ":packet_frequency_cs",
        ":packet_frequency_calculator_cs",
        ":packet_latency_calculator_cs",
        ":association_calculator_cs",
        ":rect_to_render_data_calculator_cs",
        ":rect_to_render_scale_calculator_cs",
        ":detections_to_render_data_calculator_cs",
        ":landmarks_to_render_data_calculator_cs",
        ":timed_box_list_to_render_data_calculator_cs",
        ":labels_to_render_data_calculator_cs",
        ":thresholding_calculator_cs",
        ":detections_to_rects_calculator_cs",
        ":landmark_projection_calculator_cs",
        ":landmarks_to_floats_calculator_cs",
        ":rect_transformation_calculator_cs",
        ":landmarks_to_detection_calculator_cs",
        ":landmarks_smoothing_calculator_cs",
        ":visibility_smoothing_calculator_cs",
        ":visibility_copy_calculator_cs",
        ":top_k_scores_calculator_cs",
        ":local_file_contents_calculator_cs",
        ":logic_calculator_cs",
        ":refine_landmarks_from_heatmap_calculator_cs",
        # https://github.com/protocolbuffers/protobuf/issues/9605
        # ":landmarks_refinement_calculator_cs",
    ],
    prefix = "Calculators/Util",
)

csharp_proto_src(
    name = "annotation_overlay_calculator_cs",
    proto_src = "mediapipe/calculators/util/annotation_overlay_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "detection_label_id_to_text_calculator_cs",
    proto_src = "mediapipe/calculators/util/detection_label_id_to_text_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "filter_detections_calculator_cs",
    proto_src = "mediapipe/calculators/util/filter_detections_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "timed_box_list_id_to_label_calculator_cs",
    proto_src = "mediapipe/calculators/util/timed_box_list_id_to_label_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "latency_cs",
    proto_src = "mediapipe/calculators/util/latency.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
    ],
)

csharp_proto_src(
    name = "non_max_suppression_calculator_cs",
    proto_src = "mediapipe/calculators/util/non_max_suppression_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_frequency_cs",
    proto_src = "mediapipe/calculators/util/packet_frequency.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_frequency_calculator_cs",
    proto_src = "mediapipe/calculators/util/packet_frequency_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "packet_latency_calculator_cs",
    proto_src = "mediapipe/calculators/util/packet_latency_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "collection_has_min_size_calculator_cs",
    proto_src = "mediapipe/calculators/util/collection_has_min_size_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "association_calculator_cs",
    proto_src = "mediapipe/calculators/util/association_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "rect_to_render_data_calculator_cs",
    proto_src = "mediapipe/calculators/util/rect_to_render_data_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "rect_to_render_scale_calculator_cs",
    proto_src = "mediapipe/calculators/util/rect_to_render_scale_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "detections_to_render_data_calculator_cs",
    proto_src = "mediapipe/calculators/util/detections_to_render_data_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "landmarks_to_render_data_calculator_cs",
    proto_src = "mediapipe/calculators/util/landmarks_to_render_data_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "timed_box_list_to_render_data_calculator_cs",
    proto_src = "mediapipe/calculators/util/timed_box_list_to_render_data_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "labels_to_render_data_calculator_cs",
    proto_src = "mediapipe/calculators/util/labels_to_render_data_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
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

csharp_proto_src(
    name = "detections_to_rects_calculator_cs",
    proto_src = "mediapipe/calculators/util/detections_to_rects_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "landmark_projection_calculator_cs",
    proto_src = "mediapipe/calculators/util/landmark_projection_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "landmarks_to_floats_calculator_cs",
    proto_src = "mediapipe/calculators/util/landmarks_to_floats_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "rect_transformation_calculator_cs",
    proto_src = "mediapipe/calculators/util/rect_transformation_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "landmarks_to_detection_calculator_cs",
    proto_src = "mediapipe/calculators/util/landmarks_to_detection_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "landmarks_smoothing_calculator_cs",
    proto_src = "mediapipe/calculators/util/landmarks_smoothing_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "visibility_smoothing_calculator_cs",
    proto_src = "mediapipe/calculators/util/visibility_smoothing_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "visibility_copy_calculator_cs",
    proto_src = "mediapipe/calculators/util/visibility_copy_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "top_k_scores_calculator_cs",
    proto_src = "mediapipe/calculators/util/top_k_scores_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "local_file_contents_calculator_cs",
    proto_src = "mediapipe/calculators/util/local_file_contents_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "logic_calculator_cs",
    proto_src = "mediapipe/calculators/util/logic_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "refine_landmarks_from_heatmap_calculator_cs",
    proto_src = "mediapipe/calculators/util/refine_landmarks_from_heatmap_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "landmarks_refinement_calculator_cs",
    proto_src = "mediapipe/calculators/util/landmarks_refinement_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/util:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
