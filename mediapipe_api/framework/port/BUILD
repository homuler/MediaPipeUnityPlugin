# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

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
