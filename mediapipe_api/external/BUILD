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
        "@com_google_mediapipe//mediapipe/framework/port:parse_text_proto",
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
