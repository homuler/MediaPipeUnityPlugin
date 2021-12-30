# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//:pkg.bzl", "pkg_zip")
load("//mediapipe_api:import_model.bzl", "pkg_asset")

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
        "//mediapipe_api/framework/formats:classification",
        "//mediapipe_api/framework/formats:detection",
        "//mediapipe_api/framework/formats:image_frame",
        "//mediapipe_api/framework/formats:landmark",
        "//mediapipe_api/framework/formats:rect",
        "//mediapipe_api/framework/port:logging",
        "//mediapipe_api/graphs/object_detection_3d/calculators:model_matrix",
        "//mediapipe_api/graphs/instant_motion_tracking/calculators:transformations",
        "//mediapipe_api/modules/face_geometry/protos:face_geometry",
        "//mediapipe_api/modules/objectron/calculators:annotation",
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
    name = "libmediapipe_c.dylib",
    linkshared = True,
    linkstatic = True,
    visibility = ["//visibility:public"],
    deps = [":mediapipe_c"],
)

cc_binary(
    name = "libmediapipe_c.dll",
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
        "@com_google_mediapipe//mediapipe:macos": ":libmediapipe_c.dylib",
        "@com_google_mediapipe//mediapipe:windows": ":libmediapipe_c.dll",
        "@com_google_mediapipe//mediapipe:emscripten": ":libmediapipe_c.bc",
        "//conditions:default": ":libmediapipe_c.so",
    }),
)

# It is recommended that you include only neccessary calculators.
cc_library(
    name = "calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:pass_through_calculator",
        "@com_google_mediapipe//mediapipe/calculators/core:packet_presence_calculator",
        "@com_google_mediapipe//mediapipe/calculators/core:flow_limiter_calculator",
        "@com_google_mediapipe//mediapipe/calculators/image:image_transformation_calculator",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "//mediapipe_api/graphs/instant_motion_tracking/subgraphs:region_tracking_cpu",
            "//mediapipe_api/modules/iris_landmark:iris_landmarks_from_face_landmarks_cpu",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_live_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:face_detection_full_range_desktop_live_deps",
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hair_segmentation:desktop_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hand_tracking:desktop_tflite_calculators",
            "@com_google_mediapipe//mediapipe/graphs/holistic_tracking:holistic_tracking_cpu_graph_deps",
            "@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_cpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/object_detection:desktop_tflite_calculators",
            "@com_google_mediapipe//mediapipe/graphs/object_detection_3d:desktop_cpu_calculators",
            "@com_google_mediapipe//mediapipe/graphs/pose_tracking:pose_tracking_cpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/tracking:desktop_calculators",
        ],
        "//conditions:default": [
            "//mediapipe_api/modules/iris_landmark:iris_landmarks_from_face_landmarks_gpu",
            # ATTENTION!
            #   If you're building for Android with debug symobls (--compilation_mode=dbg), exclude `instant_motion_tracking_deps`
            #   or exclude the other deps defined under `mediapipe/graphs`.
            #   Otherwise the app will crash (MediaPipe may have some bugs).
            "@com_google_mediapipe//mediapipe/gpu:gl_scaler_calculator",
            "@com_google_mediapipe//mediapipe/gpu:image_frame_to_gpu_buffer_calculator",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_live_gpu_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:face_detection_full_range_mobile_gpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_gpu_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hair_segmentation:mobile_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hand_tracking:mobile_calculators",
            "@com_google_mediapipe//mediapipe/graphs/holistic_tracking:holistic_tracking_gpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking:instant_motion_tracking_deps",
            "@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_gpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/object_detection:mobile_calculators",
            "@com_google_mediapipe//mediapipe/graphs/object_detection_3d:mobile_calculators",
            "@com_google_mediapipe//mediapipe/graphs/pose_tracking:pose_tracking_gpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/tracking:mobile_calculators",
        ],
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

pkg_zip(
    name = "mediapipe_desktop",
    srcs = [":libmediapipe_c"],
)

pkg_asset(
    name = "mediapipe_assets",
    srcs = [
        "@com_google_mediapipe//mediapipe/models:hair_segmentation.tflite",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection.tflite",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection_labelmap.txt",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_full_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_full_range_sparse.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_detection:face_detection_short_range.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/face_landmark:face_landmark_with_attention.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:hand_landmark_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:hand_landmark_lite.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:handedness.txt",
        "@com_google_mediapipe//mediapipe/modules/holistic_landmark:hand_recrop.tflite",
        "@com_google_mediapipe//mediapipe/modules/iris_landmark:iris_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_3d_camera.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_3d_chair.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_3d_chair_1stage.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_3d_cup.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_3d_sneakers.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_3d_sneakers_1stage.tflite",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_oidv4_labelmap.txt",
        "@com_google_mediapipe//mediapipe/modules/objectron:object_detection_ssd_mobilenetv2_oidv4_fp16.tflite",
        "@com_google_mediapipe//mediapipe/modules/palm_detection:palm_detection_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/palm_detection:palm_detection_lite.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_detection:pose_detection.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_full.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_heavy.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_lite.tflite",
    ],
)

pkg_zip(
    name = "mediapipe_proto_srcs",
    srcs = [
        "//mediapipe_api/framework:calculator_cs",
        "//mediapipe_api/framework:calculator_options_cs",
        "//mediapipe_api/framework:mediapipe_options_cs",
        "//mediapipe_api/framework:packet_factory_cs",
        "//mediapipe_api/framework:packet_generator_cs",
        "//mediapipe_api/framework:status_handler_cs",
        "//mediapipe_api/framework:stream_handler_cs",
        "//mediapipe_api/framework/formats:classification_cs",
        "//mediapipe_api/framework/formats:detection_cs",
        "//mediapipe_api/framework/formats:landmark_cs",
        "//mediapipe_api/framework/formats:location_data_cs",
        "//mediapipe_api/framework/formats:matrix_data_cs",
        "//mediapipe_api/framework/formats:rasterization_cs",
        "//mediapipe_api/framework/formats:rect_cs",
        "//mediapipe_api/graphs/instant_motion_tracking/calculators:sticker_buffer_cs",
        "//mediapipe_api/graphs/object_detection_3d/calculators:model_matrix_cs",
        "//mediapipe_api/modules/face_geometry/protos:face_geometry_cs",
        "//mediapipe_api/modules/face_geometry/protos:mesh_3d_cs",
        "//mediapipe_api/modules/objectron/calculators:a_r_capture_metadata_cs",
        "//mediapipe_api/modules/objectron/calculators:annotation_data_cs",
        "//mediapipe_api/modules/objectron/calculators:object_cs",
    ],
    # TODO: keep directory structure
)

pkg_zip(
    name = "opencv_libs",
    srcs = select({
        "@opencv//:source_build": ["@opencv//:opencv_libs"],
        "@com_google_mediapipe//mediapipe:windows": ["@windows_opencv//:opencv_libs"],
        "@com_google_mediapipe//mediapipe:macos": [],
        "@com_google_mediapipe//mediapipe:android": [],
        "@com_google_mediapipe//mediapipe:ios": [],
        "//conditions:default": ["@linux_opencv//:opencv_libs"],
    }),
)
