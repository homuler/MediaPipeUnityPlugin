# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@build_bazel_rules_apple//apple:ios.bzl", "ios_framework")

objc_library(
    name = "mediapipe_c_ios",
    sdk_frameworks = [
        "Accelerate",
        "AVFoundation",
        "CoreVideo",
        "CoreGraphics",
        "CoreMedia",
        "GLKit",
        "OpenGLES",
        "QuartzCore",
    ],
    deps = [
        "//mediapipe_api:mediapipe_c",
        "@ios_opencv//:OpencvFramework",
    ],
)

ios_framework(
    name = "MediaPipeUnity",
    bundle_id = "PLEASE_SET_YOUR_BUNDLE_ID_HERE.MediaPipeUnity",
    families = [
        "iphone",
        "ipad",
    ],
    infoplists = ["Info.plist"],
    minimum_os_version = "11.0",
    visibility = ["//visibility:public"],
    deps = [
        ":mediapipe_c_ios",
    ],
)
