# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load(
    "@mediapipe//mediapipe/framework/tool:mediapipe_graph.bzl",
    "mediapipe_simple_subgraph",
)

package(default_visibility = ["//visibility:public"])

mediapipe_simple_subgraph(
    name = "iris_landmarks_from_face_landmarks_gpu",
    graph = "iris_landmarks_from_face_landmarks_gpu.pbtxt",
    register_as = "IrisLandmarksFromFaceLandmarksGpu",
    deps = [
        "@mediapipe//mediapipe/calculators/core:concatenate_proto_list_calculator",
        "@mediapipe//mediapipe/calculators/core:split_proto_list_calculator",
        "@mediapipe//mediapipe/graphs/iris_tracking/calculators:update_face_landmarks_calculator",
        "@mediapipe//mediapipe/modules/iris_landmark:iris_landmark_left_and_right_gpu",
    ],
)

mediapipe_simple_subgraph(
    name = "iris_landmarks_from_face_landmarks_cpu",
    graph = "iris_landmarks_from_face_landmarks_cpu.pbtxt",
    register_as = "IrisLandmarksFromFaceLandmarksCpu",
    deps = [
        "@mediapipe//mediapipe/calculators/core:concatenate_proto_list_calculator",
        "@mediapipe//mediapipe/calculators/core:split_proto_list_calculator",
        "@mediapipe//mediapipe/graphs/iris_tracking/calculators:update_face_landmarks_calculator",
        "@mediapipe//mediapipe/modules/iris_landmark:iris_landmark_left_and_right_cpu",
    ],
)
