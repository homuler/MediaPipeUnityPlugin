cc_library(
    name = "resource_util",
    srcs = ["resource_util_custom.cc"],
    hdrs = ["resource_util_custom.h"],
    deps = [
      "@com_google_absl//absl/strings",
      "@com_google_mediapipe//mediapipe/framework/port:ret_check",
      "@com_google_mediapipe//mediapipe/framework/port:status",
      "@com_google_mediapipe//mediapipe/util:resource_util",
      "//mediapipe_api:common",
    ],
    visibility = ["//visibility:public"],
    alwayslink = True,
)
