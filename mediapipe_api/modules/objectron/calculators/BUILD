# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(default_visibility = ["//visibility:public"])

cc_library(
    name = "annotation",
    srcs = ["annotation_data.cc"],
    hdrs = ["annotation_data.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:annotation_cc_proto",
    ],
    alwayslink = True,
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":object_cs",
        ":a_r_capture_metadata_cs",
        ":annotation_data_cs",
        ":camera_parameters_cs",
        ":frame_annotation_tracker_calculator_cs",
        ":belief_decoder_config_cs",
        ":tflite_tensors_to_objects_calculator_cs",
        ":tensors_to_objects_calculator_cs",
        ":lift_2d_frame_annotation_to_3d_calculator_cs",
        ":frame_annotation_to_rect_calculator_cs",
        ":filter_detection_calculator_cs",
    ],
    prefix = "Modules/Objectron/Calculators",
)


csharp_proto_src(
    name = "object_cs",
    proto_src = "mediapipe/modules/objectron/calculators/object.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "a_r_capture_metadata_cs",
    proto_src = "mediapipe/modules/objectron/calculators/a_r_capture_metadata.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "annotation_data_cs",
    proto_src = "mediapipe/modules/objectron/calculators/annotation_data.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "camera_parameters_cs",
    proto_src = "mediapipe/modules/objectron/calculators/camera_parameters.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "frame_annotation_tracker_calculator_cs",
    proto_src = "mediapipe/modules/objectron/calculators/frame_annotation_tracker_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "belief_decoder_config_cs",
    proto_src = "mediapipe/modules/objectron/calculators/belief_decoder_config.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "tflite_tensors_to_objects_calculator_cs",
    proto_src = "mediapipe/modules/objectron/calculators/tflite_tensors_to_objects_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "tensors_to_objects_calculator_cs",
    proto_src = "mediapipe/modules/objectron/calculators/tensors_to_objects_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "lift_2d_frame_annotation_to_3d_calculator_cs",
    proto_src = "mediapipe/modules/objectron/calculators/lift_2d_frame_annotation_to_3d_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "frame_annotation_to_rect_calculator_cs",
    proto_src = "mediapipe/modules/objectron/calculators/frame_annotation_to_rect_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "filter_detection_calculator_cs",
    proto_src = "mediapipe/modules/objectron/calculators/filter_detection_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)
