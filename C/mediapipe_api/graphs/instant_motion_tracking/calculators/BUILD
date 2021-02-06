package(
    default_visibility = ["//visibility:public"],
)

load("//mediapipe_api:csharp_proto_src.bzl", "csharp_proto_src")

csharp_proto_src(
    name = "sticker_buffer_cs",
    proto_src = "mediapipe/graphs/instant_motion_tracking/calculators/sticker_buffer.proto",
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking/calculators:protos_src",
    ],
    import_prefix = "Graphs/InstantMotionTracking",
)
