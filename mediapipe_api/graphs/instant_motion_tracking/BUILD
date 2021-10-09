# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

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
