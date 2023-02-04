# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(default_visibility = ["//visibility:public"])

pkg_files(
  name = "proto_srcs",
  srcs = [
    ":opencv_image_encoder_calculator_cs",
    ":scale_image_calculator_cs",
    ":set_alpha_calculator_cs",
    ":image_cropping_calculator_cs",
    ":bilateral_filter_calculator_cs",
    ":recolor_calculator_cs",
    ":segmentation_smoothing_calculator_cs",
    ":rotation_mode_cs",
    ":image_transformation_calculator_cs",
    ":image_clone_calculator_cs",
    ":mask_overlay_calculator_cs",
    ":opencv_encoded_image_to_image_frame_calculator_cs",
    ":feature_detector_calculator_cs",
    ":warp_affine_calculator_cs",
  ],
  prefix = "Calculators/Image",
)

csharp_proto_src(
    name = "opencv_image_encoder_calculator_cs",
    proto_src = "mediapipe/calculators/image/opencv_image_encoder_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "scale_image_calculator_cs",
    proto_src = "mediapipe/calculators/image/scale_image_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "set_alpha_calculator_cs",
    proto_src = "mediapipe/calculators/image/set_alpha_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "image_cropping_calculator_cs",
    proto_src = "mediapipe/calculators/image/image_cropping_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "bilateral_filter_calculator_cs",
    proto_src = "mediapipe/calculators/image/bilateral_filter_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "recolor_calculator_cs",
    proto_src = "mediapipe/calculators/image/recolor_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/util:protos_src",
    ],
)

csharp_proto_src(
    name = "segmentation_smoothing_calculator_cs",
    proto_src = "mediapipe/calculators/image/segmentation_smoothing_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "rotation_mode_cs",
    proto_src = "mediapipe/calculators/image/rotation_mode.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
    ],
)

csharp_proto_src(
    name = "image_transformation_calculator_cs",
    proto_src = "mediapipe/calculators/image/image_transformation_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)

csharp_proto_src(
    name = "image_clone_calculator_cs",
    proto_src = "mediapipe/calculators/image/image_clone_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "mask_overlay_calculator_cs",
    proto_src = "mediapipe/calculators/image/mask_overlay_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "opencv_encoded_image_to_image_frame_calculator_cs",
    proto_src = "mediapipe/calculators/image/opencv_encoded_image_to_image_frame_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "feature_detector_calculator_cs",
    proto_src = "mediapipe/calculators/image/feature_detector_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
    ],
)

csharp_proto_src(
    name = "warp_affine_calculator_cs",
    proto_src = "mediapipe/calculators/image/warp_affine_calculator.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/calculators/image:protos_src",
        "@com_google_mediapipe//mediapipe/framework:protos_src",
        "@com_google_mediapipe//mediapipe/gpu:protos_src",
    ],
)
