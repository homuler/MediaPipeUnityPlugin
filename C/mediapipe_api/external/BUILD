cc_library(
    name = "glog",
    srcs = ["glog.cc"],
    hdrs = ["glog.h"],
    deps = [
        "//mediapipe_api:common",
    ],
    visibility = ["//visibility:public"],
    alwayslink = True,
)

cc_library(
    name = "protobuf",
    srcs = ["protobuf.cc"],
    hdrs = ["protobuf.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_protobuf//:protobuf",
    ],
    visibility = ["//visibility:public"],
    alwayslink = True,
)

cc_library(
    name = "stdlib",
    srcs = ["stdlib.cc"],
    hdrs = ["stdlib.h"],
    deps = [
        "//mediapipe_api:common",
    ],
    visibility = ["//visibility:public"],
    alwayslink = True,
)
