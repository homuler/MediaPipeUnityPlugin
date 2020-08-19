package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "gl_base",
    srcs = ["gl_base.cc"],
    hdrs = ["gl_base.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:gl_base",
        "//mediapipe_api:common",
    ],
    alwayslink = True,
)

cc_library(
    name = "gl_context",
    srcs = ["gl_context.cc"],
    hdrs = ["gl_context.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:gl_context",
        "//mediapipe_api:common",
    ],
    alwayslink = True,
)

cc_library(
    name = "gpu_shared_data_internal",
    srcs = ["gpu_shared_data_internal.cc"],
    hdrs = ["gpu_shared_data_internal.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:gpu_shared_data_internal",
        "//mediapipe_api:common",
        "//mediapipe_api/framework/port:status",
        "//mediapipe_api/framework/port:statusor",
    ],
    alwayslink = True,
)

cc_library(
    name = "gl_calculator_helper",
    srcs = ["gl_calculator_helper.cc"],
    hdrs = ["gl_calculator_helper.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:gl_calculator_helper",
        "//mediapipe_api:common",
        "//mediapipe_api/framework:packet",
        "//mediapipe_api/framework/port:status",
        "//mediapipe_api/framework/port:statusor",
    ],
    alwayslink = True,
)

cc_library(
    name = "gpu_buffer_format",
    srcs = ["gpu_buffer_format.cc"],
    hdrs = ["gpu_buffer_format.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/gpu:gpu_buffer_format",
        "//mediapipe_api:common",
    ],
    alwayslink = True,
)
