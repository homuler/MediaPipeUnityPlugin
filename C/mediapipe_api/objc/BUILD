load("@build_bazel_rules_apple//apple:ios.bzl", "ios_framework")

objc_library(
    name = "mediapipe_c_ios",
    deps = [
        "@ios_opencv//:OpencvFramework",
        "//mediapipe_api:mediapipe_c",
    ],
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
)

ios_framework(
    name = "MediaPipeUnity",
    bundle_id = "YOUR_BUNDLE_ID_HERE.MediaPipeUnity",
    families = [
        "iphone",
        "ipad",
    ],
    infoplists = ["Info.plist"],
    minimum_os_version = "11.0",
    deps = [
        ":mediapipe_c_ios",
    ],
    visibility = ["//visibility:public"],
)
