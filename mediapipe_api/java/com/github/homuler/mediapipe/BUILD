# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api/java/com/google/mediapipe:mediapipe_aar.bzl", "mediapipe_aar")

# A sample aar to extend UnityPlayerActivity
mediapipe_aar(
    name = "mediapipe_android",
    srcs = [
        # "MediaPipeUnityPlayerActivity.java",
    ],
    assets = [
        # If preferable, model files can be included to aar
    ],
    jni_deps = [
        "//mediapipe_api:mediapipe_c",
    ],
    deps = [
        # "@unity//:activity",
        "@opencv",
    ],
)
