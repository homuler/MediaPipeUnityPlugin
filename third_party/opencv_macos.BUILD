# Copyright 2019 The MediaPipe Authors.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#      http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# CHANGES:
#  - support OpenCV 4

# Description:
#   OpenCV libraries for video/image processing on MacOS

load("@bazel_skylib//lib:paths.bzl", "paths")

licenses(["notice"])  # BSD license

exports_files(["LICENSE"])

# The path to OpenCV is a combination of the path set for "macos_opencv"
# in the WORKSPACE file and the prefix here.
PREFIX = "opt/opencv@4"

cc_library(
    name = "opencv",
    srcs = glob(
        [
            paths.join(PREFIX, "lib/libopencv_core.dylib"),
            paths.join(PREFIX, "lib/libopencv_calib3d.dylib"),
            paths.join(PREFIX, "lib/libopencv_features2d.dylib"),
            paths.join(PREFIX, "lib/libopencv_highgui.dylib"),
            paths.join(PREFIX, "lib/libopencv_imgcodecs.dylib"),
            paths.join(PREFIX, "lib/libopencv_imgproc.dylib"),
            paths.join(PREFIX, "lib/libopencv_video.dylib"),
            paths.join(PREFIX, "lib/libopencv_videoio.dylib"),
        ],
    ),
    hdrs = glob([paths.join(PREFIX, "include/opencv4/opencv2/**/*.h*")]),
    includes = [paths.join(PREFIX, "include/opencv4/")],
    linkstatic = 1,
    visibility = ["//visibility:public"],
)
