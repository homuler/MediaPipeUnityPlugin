package(
    default_visibility = ["//visibility:public"],
)

genrule(
    name = "asset3d",
    srcs = ["@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/instantmotiontracking/assets:robot/robot.obj.uuu.zip"],
    outs = ["robot/robot.obj.uuu"],
    cmd = "unzip -p $< > $@",
)

filegroup(
    name = "assets",
    srcs = [
        ":asset3d",
        "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/instantmotiontracking/assets:robot/robot_texture.jpg",
    ],
)
