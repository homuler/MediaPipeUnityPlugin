# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("//mediapipe_api:import_model.bzl", "copy_file")

package(
    default_visibility = ["//visibility:public"],
)

copy_file(
    name = "camera_texture",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/camera:texture.jpg",
    out = "camera_texture.jpg",
)

copy_file(
    name = "chair_texture",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/chair:texture.jpg",
    out = "chair_texture.jpg",
)

copy_file(
    name = "cup_texture",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/cup:texture.jpg",
    out = "cup_texture.jpg",
)

copy_file(
    name = "sneaker_texture",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/sneaker:texture.jpg",
    out = "sneaker_texture.jpg",
)

copy_file(
    name = "camera_mesh",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/camera:model.obj.uuu",
    out = "camera.obj.uuu",
)

copy_file(
    name = "chair_mesh",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/chair:model.obj.uuu",
    out = "chair.obj.uuu",
)

copy_file(
    name = "cup_mesh",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/cup:model.obj.uuu",
    out = "cup.obj.uuu",
)

copy_file(
    name = "sneaker_mesh",
    src = "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets/sneaker:model.obj.uuu",
    out = "sneaker.obj.uuu",
)

filegroup(
    name = "assets",
    srcs = [
        ":camera_mesh",
        ":camera_texture",
        ":chair_mesh",
        ":chair_texture",
        ":cup_mesh",
        ":cup_texture",
        ":sneaker_mesh",
        ":sneaker_texture",
        "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets:box.obj.uuu",
        "@com_google_mediapipe//mediapipe/examples/android/src/java/com/google/mediapipe/apps/objectdetection3d/assets:classic_colors.png",
    ],
)
