android_library(
    name = "mediapipe_android",
    manifest = "AndroidManifest.xml",
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
    ],
    assets_dir = "",
    visibility = ["//visibility:public"],
    deps = [
        "@com_google_mediapipe//mediapipe/java/com/google/mediapipe/framework:android_framework",
        "@com_google_mediapipe//mediapipe/java/com/google/mediapipe/glutil",
        "//third_party:opencv",
    ],
)

android_binary(
    name = "helloworld",
    manifest = "AndroidManifest.xml",
    multidex = "native",
    deps = [
        ":mediapipe_android",
        "//mediapipe_api:mediapipe_test",
    ],
)
