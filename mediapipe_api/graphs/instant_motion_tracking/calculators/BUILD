load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

package(
    default_visibility = ["//visibility:public"],
)

cc_library(
    name = "transformations",
    srcs = ["transformations.cc"],
    hdrs = ["transformations.h"],
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking/calculators:tracked_anchor_manager_calculator",
        "//mediapipe_api:common",
        "//mediapipe_api/external:protobuf",
        "//mediapipe_api/framework:packet",
    ],
    alwayslink = True,
)

csharp_proto_src(
    name = "sticker_buffer_cs",
    proto_src = "mediapipe/graphs/instant_motion_tracking/calculators/sticker_buffer.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking/calculators:protos_src",
    ],
    import_prefix = "Graphs/InstantMotionTracking",
)
