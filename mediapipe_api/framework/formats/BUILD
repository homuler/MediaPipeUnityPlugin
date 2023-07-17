# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@rules_pkg//pkg:mappings.bzl", "pkg_files")
load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "classification",
    srcs = ["classification.cc"],
    hdrs = ["classification.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:classification_cc_proto",
    ],
    alwayslink = True,
)

cc_library(
    name = "detection",
    srcs = ["detection.cc"],
    hdrs = ["detection.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:detection_cc_proto",
    ],
    alwayslink = True,
)

cc_library(
    name = "image",
    srcs = ["image.cc"],
    hdrs = ["image.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/framework:packet",
        "@com_google_absl//absl/status",
        "@com_google_mediapipe//mediapipe/framework/formats:image",
    ],
    alwayslink = True,
)

cc_library(
    name = "image_frame",
    srcs = ["image_frame.cc"],
    hdrs = ["image_frame.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:status",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:image_frame",
    ],
    alwayslink = True,
)

cc_library(
    name = "landmark",
    srcs = ["landmark.cc"],
    hdrs = ["landmark.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:landmark_cc_proto",
    ],
    alwayslink = True,
)

cc_library(
    name = "matrix_data",
    srcs = ["matrix_data.cc"],
    hdrs = ["matrix_data.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external/absl:status",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:matrix",
        "@com_google_mediapipe//mediapipe/framework/formats:matrix_data_cc_proto",
    ],
    alwayslink = True,
)

cc_library(
    name = "rect",
    srcs = ["rect.cc"],
    hdrs = ["rect.h"],
    deps = [
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
        "@com_google_mediapipe//mediapipe/framework/formats:rect_cc_proto",
    ],
    alwayslink = True,
)

pkg_files(
    name = "proto_srcs",
    srcs = [
        ":affine_transform_data_cs",
        ":classification_cs",
        ":detection_cs",
        ":image_file_properties_cs",
        ":image_format_cs",
        ":landmark_cs",
        ":location_data_cs",
        ":matrix_data_cs",
        ":rect_cs",
        ":time_series_header_cs",
    ],
    prefix = "Framework/Formats",
)


csharp_proto_src(
    name = "affine_transform_data_cs",
    proto_src = "mediapipe/framework/formats/affine_transform_data.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "classification_cs",
    proto_src = "mediapipe/framework/formats/classification.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "detection_cs",
    proto_src = "mediapipe/framework/formats/detection.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats/annotation:protos_src",
    ],
)

csharp_proto_src(
    name = "image_file_properties_cs",
    proto_src = "mediapipe/framework/formats/image_file_properties.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "image_format_cs",
    proto_src = "mediapipe/framework/formats/image_format.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "landmark_cs",
    proto_src = "mediapipe/framework/formats/landmark.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "location_data_cs",
    proto_src = "mediapipe/framework/formats/location_data.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
        "@com_google_mediapipe//mediapipe/framework/formats/annotation:protos_src",
    ],
)

csharp_proto_src(
    name = "matrix_data_cs",
    proto_src = "mediapipe/framework/formats/matrix_data.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "rect_cs",
    proto_src = "mediapipe/framework/formats/rect.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)

csharp_proto_src(
    name = "time_series_header_cs",
    proto_src = "mediapipe/framework/formats/time_series_header.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/framework/formats:protos_src",
    ],
)
