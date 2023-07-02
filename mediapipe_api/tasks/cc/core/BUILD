# Copyright (c) 2023 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "task_runner",
    srcs = ["task_runner.cc"],
    hdrs = ["task_runner.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "@com_google_mediapipe//mediapipe/tasks/cc/core:mediapipe_builtin_op_resolver",
        "@com_google_mediapipe//mediapipe/tasks/cc/core:task_runner",
        "@com_google_absl//absl/status:statusor",
    ],
    alwayslink = True,
)
