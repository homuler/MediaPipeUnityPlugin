cc_binary(
    name = "mediapipe_c",
    deps = [
        ":calculators",
        ":protobuf",
        "//mediapipe_api/framework:calculator_graph",
        "//mediapipe_api/framework/formats:image_frame",
        "//mediapipe_api/framework/port:logging",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [],
        "//conditions:default": [
            "//mediapipe_api/gpu:gl_base",
            "//mediapipe_api/gpu:gl_context",
            "//mediapipe_api/gpu:gl_calculator_helper",
            "//mediapipe_api/gpu:gpu_buffer_format",
            "//mediapipe_api/gpu:gpu_shared_data_internal",
        ],
    }),
    visibility = None,
    linkshared = True,
)


# It is recommended that you include only neccessary calculators.
cc_library(
    name = "calculators",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/core:pass_through_calculator",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [
            "@com_google_mediapipe//mediapipe/graphs/face_mesh:desktop_live_calculators",
            "@com_google_mediapipe//mediapipe/graphs/face_detection:desktop_tflite_calculators",
        ],
        "//conditions:default": [
            "@com_google_mediapipe//mediapipe/graphs/face_detection:mobile_calculators",
        ],
    }),
)

cc_library(
    name = "common",
    hdrs = ["common.h"],
    deps = [],
    visibility = ["//visibility:public"],
)

cc_library(
    name = "protobuf",
    srcs = ["protobuf.cc"],
    hdrs = ["protobuf.h"],
    deps = [
        "@com_google_protobuf//:protobuf",
        ":common",
    ],
    alwayslink = True,
)
