# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

cc_library(
    name = "glog",
    srcs = ["glog.cc"],
    hdrs = ["glog.h"],
    visibility = ["//visibility:public"],
    deps = [
        "//mediapipe_api:common",
    ],
    alwayslink = True,
)

cc_library(
    name = "protobuf",
    srcs = ["protobuf.cc"],
    hdrs = ["protobuf.h"],
    visibility = ["//visibility:public"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework/port:parse_text_proto",
        "@com_google_protobuf//:protobuf",
    ],
    alwayslink = True,
)

cc_library(
    name = "stdlib",
    srcs = ["stdlib.cc"],
    hdrs = ["stdlib.h"],
    visibility = ["//visibility:public"],
    deps = [
        "//mediapipe_api:common",
    ],
    alwayslink = True,
)

cc_library(
    name = "wasm_patch",
    srcs = ["wasm_patch.cc"],
    hdrs = ["wasm_patch.h"],
    visibility = ["//visibility:public"],
    deps = [
        "//mediapipe_api:common",
    ],
    alwayslink = True,
)
