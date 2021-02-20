package(
    default_visibility = ["//visibility:public"],
)

load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

cc_library(
    name = "model_matrix",
    srcs = ["model_matrix.cc"],
    hdrs = ["model_matrix.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/object_detection_3d/calculators:model_matrix_cc_proto",
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "model_matrix_cs",
    proto_src = "mediapipe/graphs/object_detection_3d/calculators/model_matrix.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/object_detection_3d/calculators:protos_src",
    ],
    import_prefix = "Graphs/ObjectDetection3d",
)
