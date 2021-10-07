# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

cc_library(
    name = "resource_util",
    srcs = ["resource_util_custom.cc"],
    hdrs = ["resource_util_custom.h"],
    visibility = ["//visibility:public"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_absl//absl/strings",
        "@com_google_mediapipe//mediapipe/framework/port:ret_check",
        "@com_google_mediapipe//mediapipe/framework/port:status",
        "@com_google_mediapipe//mediapipe/util:resource_util",
    ],
    alwayslink = True,
)
