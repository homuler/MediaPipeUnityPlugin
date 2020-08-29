package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "image_frame",
    srcs = ["image_frame.cc"],
    hdrs = ["image_frame.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:image_frame",
        "//mediapipe_api:common",
        "//mediapipe_api/framework:packet",
        "//mediapipe_api/framework/port:status",
        "//mediapipe_api/framework/port:statusor",
    ],
    alwayslink = True,
)

cc_library(
    name = "landmark",
    srcs = ["landmark.cc"],
    hdrs = ["landmark.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:landmark_cc_proto",
        "//mediapipe_api:common",
        "//mediapipe_api/framework:packet",
    ],
    alwayslink = True,
)
