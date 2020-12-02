load("@rules_pkg//:pkg.bzl", "pkg_zip")
load("//mediapipe_api:import_model.bzl", "pkg_model")

cc_library(
    name = "mediapipe_c",
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
        "//mediapipe_api/util:resource_manager",
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
    visibility = ["//visibility:public"],
    alwayslink = True,
)

cc_binary(
    name = "libmediapipe_c",
    deps = [":mediapipe_c"],
    linkshared = True,
    linkstatic = True,
    visibility = ["//visibility:public"],
)

# It is recommended that you include only neccessary calculators.
cc_library(
    name = "calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:pass_through_calculator",
        "@com_google_mediapipe//mediapipe/calculators/core:packet_presence_calculator",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_live_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hand_tracking:desktop_tflite_calculators",
            "@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_cpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/pose_tracking:upper_body_pose_tracking_cpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/object_detection:desktop_tflite_calculators",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_live_gpu_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_gpu_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hair_segmentation:mobile_calculators",
            "@com_google_mediapipe//mediapipe/graphs/hand_tracking:mobile_calculators",
            "@com_google_mediapipe//mediapipe/graphs/iris_tracking:iris_tracking_gpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/pose_tracking:upper_body_pose_tracking_gpu_deps",
            "@com_google_mediapipe//mediapipe/graphs/object_detection:mobile_calculators",
        ],
    }),
)

cc_library(
    name = "common",
    srcs = ["common.cc"],
    hdrs = ["common.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/framework/port:logging",
    ],
    visibility = ["//visibility:public"],
    alwayslink = True,
)

pkg_model(
    name = "mediapipe_models",
    srcs = [
        "@com_google_mediapipe//mediapipe/models:face_detection_front.tflite",
        "@com_google_mediapipe//mediapipe/models:face_detection_front_labelmap.txt",
        "@com_google_mediapipe//mediapipe/models:face_landmark.tflite",
        "@com_google_mediapipe//mediapipe/models:hair_segmentation.tflite",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection.tflite",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection_labelmap.txt",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:hand_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/hand_landmark:handedness.txt",
        "@com_google_mediapipe//mediapipe/modules/iris_landmark:iris_landmark.tflite",
        "@com_google_mediapipe//mediapipe/modules/palm_detection:palm_detection.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_detection:pose_detection.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_upper_body.tflite",
    ],
)

pkg_zip(
    name = "mediapipe_proto_srcs",
    srcs = [
        "//mediapipe_api/framework/formats:classification_cs",
        "//mediapipe_api/framework/formats:detection_cs",
        "//mediapipe_api/framework/formats:landmark_cs",
        "//mediapipe_api/framework/formats:location_data_cs",
        "//mediapipe_api/framework/formats:rasterization_cs",
        "//mediapipe_api/framework/formats:rect_cs",
    ],
    # TODO: keep directory structure
)
