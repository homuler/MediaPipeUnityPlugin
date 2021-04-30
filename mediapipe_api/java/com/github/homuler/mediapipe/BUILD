load("//mediapipe_api/java/com/google/mediapipe:mediapipe_aar.bzl", "mediapipe_aar")

# A sample aar to extend UnityPlayerActivity
mediapipe_aar(
    name = "mediapipe_android",
    srcs = [
      # "MediaPipeUnityPlayerActivity.java",
    ],
    deps = [
      # "@unity//:activity",
      "@opencv//:opencv",
    ],
    jni_deps = [
      "//mediapipe_api:mediapipe_c",
    ],
    assets = [
      # If preferable, model files can be included to aar
    ],
)
