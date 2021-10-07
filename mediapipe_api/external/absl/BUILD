# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "status",
    srcs = ["status.cc"],
    hdrs = ["status.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_absl//absl/status",
    ],
    alwayslink = True,
)

cc_library(
    name = "statusor",
    hdrs = ["statusor.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_absl//absl/status:statusor",
    ],
    alwayslink = True,
)
