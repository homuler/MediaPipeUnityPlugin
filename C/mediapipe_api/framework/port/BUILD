package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "logging",
    srcs = ["logging.cc"],
    hdrs = ["logging.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework/port:logging",
    ],
    alwayslink = True,
)


cc_library(
    name = "status",
    srcs = ["status.cc"],
    hdrs = ["status.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework/port:status",
    ],
    alwayslink = True,
)

cc_library(
    name = "statusor",
    hdrs = ["statusor.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework/port:statusor",
    ],
    alwayslink = True,
)
