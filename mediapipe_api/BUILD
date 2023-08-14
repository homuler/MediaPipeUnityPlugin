# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api:import_model.bzl", "pkg_asset")
load("@bazel_skylib//lib:selects.bzl", "selects")
load("@bazel_skylib//rules:common_settings.bzl", "bool_flag", "string_list_flag")
load("@rules_pkg//:pkg.bzl", "pkg_zip")
load("@build_bazel_apple_support//rules:universal_binary.bzl", "universal_binary")
load("@com_google_mediapipe//mediapipe/framework/tool:mediapipe_files.bzl", "mediapipe_files")

bool_flag(
    name = "macos_universal",
    build_setting_default = False,
)

config_setting(
    name = "build_universal_library",
    flag_values = {
        ":macos_universal": "True"
    },
)

string_list_flag(
    name = "solutions",
    build_setting_default = ["all"],
)

config_setting(
    name = "all_solutions_switch",
    flag_values = {
        ":solutions": "all",
    },
)

config_setting(
    name = "face_detection_switch",
    flag_values = {
        ":solutions": "face_detection",
    },
)

config_setting(
    name = "face_mesh_switch",
    flag_values = {
        ":solutions": "face_mesh",
    },
)

config_setting(
    name = "iris_switch",
    flag_values = {
        ":solutions": "iris",
    },
)

config_setting(
    name = "hands_switch",
    flag_values = {
        ":solutions": "hands",
    },
)

config_setting(
    name = "pose_switch",
    flag_values = {
        ":solutions": "pose",
    },
)

config_setting(
    name = "holistic_switch",
    flag_values = {
        ":solutions": "holistic",
    },
)

config_setting(
    name = "selfie_segmentation_switch",
    flag_values = {
        ":solutions": "selfie_segmentation",
    },
)

config_setting(
    name = "hair_segmentation_switch",
    flag_values = {
        ":solutions": "hair_segmentation",
    },
)

config_setting(
    name = "object_detection_switch",
    flag_values = {
        ":solutions": "object_detection",
    },
)

selects.config_setting_group(
    name = "enable_face_detection",
    match_any = [":all_solutions_switch", ":face_detection_switch"],
)

selects.config_setting_group(
    name = "enable_face_mesh",
    match_any = [":all_solutions_switch", ":face_mesh_switch"],
)

selects.config_setting_group(
    name = "enable_iris",
    match_any = [":all_solutions_switch", ":iris_switch"],
)

selects.config_setting_group(
    name = "enable_hands",
    match_any = [":all_solutions_switch", ":hands_switch"],
)

selects.config_setting_group(
    name = "enable_pose",
    match_any = [":all_solutions_switch", ":pose_switch"],
)

selects.config_setting_group(
    name = "enable_holistic",
    match_any = [":all_solutions_switch", ":holistic_switch"],
)

selects.config_setting_group(
    name = "enable_selfie_segmentation",
    match_any = [":all_solutions_switch", ":selfie_segmentation_switch"],
)

selects.config_setting_group(
    name = "enable_hair_segmentation",
    match_any = [":all_solutions_switch", ":hair_segmentation_switch"],
)

selects.config_setting_group(
    name = "enable_object_detection",
    match_any = [":all_solutions_switch", ":object_detection_switch"],
)

cc_library(
    name = "mediapipe_c",
    visibility = ["//visibility:public"],
    deps = [
        ":calculators",
        "//mediapipe_api/external:glog",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/external:stdlib",
        "//mediapipe_api/framework:calculator",
        "//mediapipe_api/framework:calculator_graph",
        "//mediapipe_api/framework:output_stream_poller",
        "//mediapipe_api/framework:timestamp",
        "//mediapipe_api/framework:validated_graph_config",
        "//mediapipe_api/framework/formats:classification",
        "//mediapipe_api/framework/formats:detection",
        "//mediapipe_api/framework/formats:image",
        "//mediapipe_api/framework/formats:image_frame",
        "//mediapipe_api/framework/formats:landmark",
        "//mediapipe_api/framework/formats:matrix_data",
        "//mediapipe_api/framework/formats:rect",
        "//mediapipe_api/framework/port:logging",
        "//mediapipe_api/tasks/cc/vision/face_geometry/proto:face_geometry",
        "//mediapipe_api/tasks/cc/core:task_runner",
        "//mediapipe_api/util:resource_util",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [],
        "//conditions:default": [
            "//mediapipe_api/gpu:gl_base",
            "//mediapipe_api/gpu:gl_context",
            "//mediapipe_api/gpu:gl_calculator_helper",
            "//mediapipe_api/gpu:gl_texture_buffer",
            "//mediapipe_api/gpu:gpu_buffer",
            "//mediapipe_api/gpu:gpu_buffer_format",
            "//mediapipe_api/gpu:gpu_shared_data_internal",
        ],
    }),
    alwayslink = True,
)

cc_binary(
    name = "libmediapipe_c.so",
    linkshared = True,
    linkstatic = True,
    visibility = ["//visibility:public"],
    deps = [":mediapipe_c"],
)

cc_binary(
    name = "libmediapipe_c_.dylib",
    linkshared = True,
    linkstatic = True,
    visibility = ["//visibility:public"],
    deps = [":mediapipe_c"],
)

universal_binary(
    name = "libmediapipe_c_universal.dylib",
    binary = ":libmediapipe_c_.dylib",
)

genrule(
    name = "libmediapipe_c_dylib",
    srcs = select({
        ":build_universal_library": [":libmediapipe_c_universal.dylib"],
        "//conditions:default": [":libmediapipe_c_.dylib"],
    }),
    outs = ["libmediapipe_c.dylib"],
    cmd = select({
        ":build_universal_library": "cp $< $(@D)/libmediapipe_c.dylib",
        "//conditions:default": "cp $< $(@D)/libmediapipe_c.dylib",
    }),
)

cc_binary(
    name = "mediapipe_c.dll",
    linkshared = True,
    linkstatic = True,
    visibility = ["//visibility:public"],
    deps = [":mediapipe_c"],
)

cc_binary(
    name = "libmediapipe_c.bc",
    linkopts = [
        "-flto",
        "--oformat=object",
        "-s SIDE_MODULE=1",
    ],
    visibility = ["//visibility:public"],
    deps = [
        ":mediapipe_c",
        "//mediapipe_api/external:wasm_patch",
    ],
)

alias(
    name = "libmediapipe_c",
    actual = select({
        "@com_google_mediapipe//mediapipe:macos": ":libmediapipe_c_dylib",
        "@com_google_mediapipe//mediapipe:windows": ":mediapipe_c.dll",
        "@com_google_mediapipe//mediapipe:emscripten": ":libmediapipe_c.bc",
        "//conditions:default": ":libmediapipe_c.so",
    }),
)

cc_library(
    name = "common",
    srcs = ["common.cc"],
    hdrs = ["common.h"],
    visibility = ["//visibility:public"],
    deps = [
        "@com_google_mediapipe//mediapipe/framework/port:logging",
    ],
    alwayslink = True,
)

cc_library(
    name = "calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:pass_through_calculator",
        "@com_google_mediapipe//mediapipe/calculators/core:packet_presence_calculator",
        "@com_google_mediapipe//mediapipe/calculators/core:flow_limiter_calculator",
        "@com_google_mediapipe//mediapipe/calculators/image:image_transformation_calculator",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/gpu:gl_scaler_calculator",
            "@com_google_mediapipe//mediapipe/gpu:image_frame_to_gpu_buffer_calculator",
            "@com_google_mediapipe//mediapipe/gpu:gpu_buffer_to_image_frame_calculator",
        ],
    }) + select({
        ":enable_face_detection": [
            ":face_detection_calculators"
        ],
        "//conditions:default": [],
    }) + select({
        ":enable_face_mesh": [":face_mesh_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_iris": [":iris_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_hands": [":hands_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_pose": [":pose_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_holistic": [":holistic_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_selfie_segmentation": [":selfie_segmentation_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_hair_segmentation": [":hair_segmentation_calculators"],
        "//conditions:default": [],
    }) + select({
        ":enable_object_detection": [":object_detection_calculators"],
        "//conditions:default": [],
    }),
)

cc_library(
    name = "face_detection_calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/tool:switch_mux_calculator",
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_detector:face_detector",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_live_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:face_detection_full_range_desktop_live_deps",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_live_gpu_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:face_detection_full_range_mobile_gpu_deps",
        ],
    }),
)

cc_library(
    name = "face_mesh_calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/face_landmarker:face_landmarker",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_calculators",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_gpu_calculators",
        ],
    }),
)

cc_library(
    name = "iris_calculators",
    deps = select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "//mediapipe_api/modules/iris_landmark:iris_landmarks_from_face_landmarks_cpu",
            "@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_cpu_deps",
        ],
        "//conditions:default": [
            "//mediapipe_api/modules/iris_landmark:iris_landmarks_from_face_landmarks_gpu",
            "@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_gpu_deps",
        ],
    }),
)

cc_library(
    name = "hands_calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/tasks/cc/vision/hand_landmarker:hand_landmarker",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/hand_tracking:desktop_tflite_calculators",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/hand_tracking:mobile_calculators",
        ],
    }),
)

cc_library(
    name = "pose_calculators",
    deps = select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/pose_tracking:pose_tracking_cpu_deps",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/pose_tracking:pose_tracking_gpu_deps",
        ],
    }),
)

cc_library(
    name = "holistic_calculators",
    deps = select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/holistic_tracking:holistic_tracking_cpu_graph_deps",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/holistic_tracking:holistic_tracking_gpu_deps",
        ],
    }),
)

cc_library(
    name = "selfie_segmentation_calculators",
    deps = select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/selfie_segmentation:selfie_segmentation_cpu_deps",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/selfie_segmentation:selfie_segmentation_gpu_deps",
        ],
    }),
)

cc_library(
    name = "hair_segmentation_calculators",
    deps = select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/hair_segmentation:desktop_calculators",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/hair_segmentation:mobile_calculators",
            "@com_google_mediapipe//mediapipe/calculators/image:color_convert_calculator",
        ],
    }),
)

cc_library(
    name = "object_detection_calculators",
    deps = select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/object_detection:desktop_tflite_calculators",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/object_detection:mobile_calculators",
        ],
    }),
)

pkg_zip(
    name = "mediapipe_desktop",
    srcs = [
        ":libmediapipe_c",
    ],
)

mediapipe_files(srcs = [
    "blaze_face_short_range.tflite",
    "face_landmarker_v2.task",
    "face_landmarker_v2_with_blendshapes.task",
    "hand_landmarker.task",
])

pkg_asset(
    name = "mediapipe_assets",
    srcs = select({
        ":enable_face_detection": [":face_detection_assets"],
        "//conditions:default": [],
    }) + select({
        ":enable_face_mesh": ["face_mesh_assets"],
        "//conditions:default": [],
    }) + select({
        ":enable_iris": ["iris_assets"],
        "//conditions:default": [],
    }) + select({
        ":enable_hands": [":hands_assets"],
        "//conditions:default": [],
    }) + select({
        ":enable_pose": [":pose_assets"],
        "//conditions:default": [],
    }) + select({
        ":enable_holistic": [":holistic_assets"],
        "//conditions:default": [],
    }) + select({
        ":enable_selfie_segmentation": [
            "@com_google_mediapipe//mediapipe/modules/selfie_segmentation:selfie_segmentation.tflite",
        ],
        "//conditions:default": [],
    }) + select({
        ":enable_hair_segmentation": [
            "@com_google_mediapipe//mediapipe/models:hair_segmentation.tflite",
        ],
        "//conditions:default": [],
    }) + select({
        ":enable_object_detection": [":object_detection_assets"],
        "//conditions:default": [],
    }),
)

filegroup(
    name = "face_detection_assets",
    srcs = [
        ":blaze_face_short_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_full_range_sparse.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_full_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_short_range.tflite",
    ],
)

filegroup(
    name = "face_mesh_assets",
    srcs = [
        ":face_landmarker_v2.task",
        ":face_landmarker_v2_with_blendshapes.task",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_short_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark_with_attention.tflite",
    ],
)

filegroup(
    name = "iris_assets",
    srcs = [
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_short_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/iris_landmark:iris_landmark.tflite",
    ],
)

filegroup(
    name = "hands_assets",
    srcs = [
        ":hand_landmarker.task",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:hand_landmark_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:hand_landmark_lite.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:handedness.txt",
        "@com_google_mediapipe//mediapipe/modules/palm_detection:palm_detection_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/palm_detection:palm_detection_lite.tflite",
    ],
)

filegroup(
    name = "pose_assets",
    srcs = [
        "@com_google_mediapipe//mediapipe/modules/pose_detection:pose_detection.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_heavy.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_lite.tflite",
    ],
)

filegroup(
    name = "holistic_assets",
    srcs = [
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_short_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark_with_attention.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:hand_landmark_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:handedness.txt",
        "@com_google_mediapipe//mediapipe/modules/holistic_landmark:hand_recrop.tflite",
        "@com_google_mediapipe//mediapipe/modules/iris_landmark:iris_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/palm_detection:palm_detection_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_detection:pose_detection.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_heavy.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_lite.tflite",
    ],
)

filegroup(
    name = "object_detection_assets",
    srcs = [
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection.tflite",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection_labelmap.txt",
    ],
)

pkg_zip(
    name = "mediapipe_proto_srcs",
    srcs = [
        "//mediapipe_api/calculators/audio:proto_srcs",
        "//mediapipe_api/calculators/core:proto_srcs",
        "//mediapipe_api/calculators/image:proto_srcs",
        "//mediapipe_api/calculators/tensor:proto_srcs",
        "//mediapipe_api/calculators/tflite:proto_srcs",
        "//mediapipe_api/calculators/util:proto_srcs",
        "//mediapipe_api/calculators/video:proto_srcs",
        "//mediapipe_api/framework:proto_srcs",
        "//mediapipe_api/framework/formats:proto_srcs",
        "//mediapipe_api/framework/formats/annotation:proto_srcs",
        "//mediapipe_api/framework/formats/motion:proto_srcs",
        "//mediapipe_api/framework/formats/object_detection:proto_srcs",
        "//mediapipe_api/gpu:proto_srcs",
        "//mediapipe_api/modules/face_detection:proto_srcs",
        "//mediapipe_api/modules/holistic_landmark/calculators:proto_srcs",
        "//mediapipe_api/tasks/cc/core/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/components/containers/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/face_detector/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/face_landmarker/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/face_geometry/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/face_geometry/calculators:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/hand_detector/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/hand_landmarker/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/pose_detector/proto:proto_srcs",
        "//mediapipe_api/tasks/cc/vision/pose_landmarker/proto:proto_srcs",
        "//mediapipe_api/util:proto_srcs",
        "//mediapipe_api/util/tracking:proto_srcs",
    ],
)
