load(
    "@com_google_mediapipe//mediapipe/framework/tool:mediapipe_graph.bzl",
    "mediapipe_simple_subgraph",
)

package(default_visibility = ["//visibility:public"])

mediapipe_simple_subgraph(
    name = "region_tracking_cpu",
    graph = "region_tracking_cpu.pbtxt",
    register_as = "RegionTrackingSubgraphCpu",
    deps = [
        "@com_google_mediapipe//mediapipe/graphs/instant_motion_tracking/calculators:tracked_anchor_manager_calculator",
        "@com_google_mediapipe//mediapipe/graphs/tracking/subgraphs:box_tracking_cpu",
    ],
)
