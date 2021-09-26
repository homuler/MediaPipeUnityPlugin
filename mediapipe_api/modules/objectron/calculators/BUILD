load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(default_visibility = ["//visibility:public"])

cc_library(
    name = "annotation",
    srcs = ["annotation_data.cc"],
    hdrs = ["annotation_data.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:annotation_cc_proto",
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "annotation_data_cs",
    proto_src = "mediapipe/modules/objectron/calculators/annotation_data.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "object_cs",
    proto_src = "mediapipe/modules/objectron/calculators/object.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)

csharp_proto_src(
    name = "a_r_capture_metadata_cs",
    proto_src = "mediapipe/modules/objectron/calculators/a_r_capture_metadata.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/modules/objectron/calculators:protos_src",
    ],
)
