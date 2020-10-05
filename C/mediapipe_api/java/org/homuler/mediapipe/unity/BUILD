load("//mediapipe_api/java/com/google/mediapipe:mediapipe_aar.bzl", "mediapipe_aar")

mediapipe_aar(
    name = "mediapipe_android",
    srcs = [
      "MediaPipeUnityPlayerActivity.java",
    ],
    deps = [
      "@unity//:activity",
      "//third_party:opencv",
    ],
    jni_deps = [
      "//mediapipe_api:mediapipe_c",
    ],
    assets = [
        "@com_google_mediapipe//mediapipe/models:face_detection_front.tflite",
        "@com_google_mediapipe//mediapipe/models:face_detection_front_labelmap.txt",
        "@com_google_mediapipe//mediapipe/models:face_landmark.tflite",
        "@com_google_mediapipe//mediapipe/models:hair_segmentation.tflite",
        "@com_google_mediapipe//mediapipe/models:hand_landmark.tflite",
        "@com_google_mediapipe//mediapipe/models:handedness.txt",
        "@com_google_mediapipe//mediapipe/models:iris_landmark.tflite",
        "@com_google_mediapipe//mediapipe/models:palm_detection.tflite",
        "@com_google_mediapipe//mediapipe/models:palm_detection_labelmap.txt",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection.tflite",
        "@com_google_mediapipe//mediapipe/models:ssdlite_object_detection_labelmap.txt",
        "@com_google_mediapipe//mediapipe/modules/pose_detection:pose_detection.tflite",
        "@com_google_mediapipe//mediapipe/modules/pose_landmark:pose_landmark_upper_body.tflite",
    ]
)
