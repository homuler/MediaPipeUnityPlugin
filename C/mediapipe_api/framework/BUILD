package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "calculator_graph",
    srcs = ["calculator_graph.cc"],
    hdrs = ["calculator_graph.h"],
    deps = [
        ":packet",
        "@com_google_mediapipe//mediapipe/framework:calculator_framework",
        "@com_google_mediapipe//mediapipe/framework/port:parse_text_proto",
        "//mediapipe_api:common",
        "//mediapipe_api/framework/port:status",
        "//mediapipe_api/framework/port:statusor",
    ] + select({
        "@com_google_mediapipe//mediapipe/gpu:disable_gpu": [],
        "//conditions:default": [
            "//mediapipe_api/gpu:gl_calculator_helper",
            "//mediapipe_api/gpu:gpu_shared_data_internal",
        ],
    }),
    alwayslink = True,
)

cc_library(
    name = "packet",
    srcs = ["packet.cc"],
    hdrs = ["packet.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/framework:packet",
        "//mediapipe_api:common",
        "//mediapipe_api:protobuf",
    ],
    alwayslink = True,
)
