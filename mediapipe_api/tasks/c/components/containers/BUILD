# Copyright (c) 2023 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "classification_result",
    srcs = ["classification_result.cc"],
    hdrs = ["classification_result.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:classification_cc_proto",
        "@com_google_mediapipe//mediapipe/tasks/c/components/containers:classification_result",
        "@com_google_mediapipe//mediapipe/tasks/c/components/containers:classification_result_converter",
        "@com_google_mediapipe//mediapipe/tasks/cc/components/containers:classification_result",
    ],
    alwayslink = True,
)

cc_library(
    name = "detection_result",
    srcs = ["detection_result.cc"],
    hdrs = ["detection_result.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:detection_cc_proto",
        "@com_google_mediapipe//mediapipe/tasks/c/components/containers:detection_result",
        "@com_google_mediapipe//mediapipe/tasks/c/components/containers:detection_result_converter",
        "@com_google_mediapipe//mediapipe/tasks/cc/components/containers:detection_result",
    ],
    alwayslink = True,
)

cc_library(
    name = "landmark",
    srcs = ["landmark.cc"],
    hdrs = ["landmark.h"],
    deps = [
        "//mediapipe_api:common",
        "@com_google_mediapipe//mediapipe/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:landmark_cc_proto",
        "@com_google_mediapipe//mediapipe/tasks/c/components/containers:landmark",
        "@com_google_mediapipe//mediapipe/tasks/c/components/containers:landmark_converter",
        "@com_google_mediapipe//mediapipe/tasks/cc/components/containers:landmark",
    ],
    alwayslink = True,
)
