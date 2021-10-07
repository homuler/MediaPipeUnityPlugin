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

# Description:
#   OpenCV libraries for video/image processing on Linux

licenses(["notice"])  # BSD license

exports_files(["LICENSE"])

# The following build rule assumes that OpenCV is installed by
# 'apt-get install libopencv-core-dev libopencv-highgui-dev \'
# '                libopencv-calib3d-dev libopencv-features2d-dev \'
# '                libopencv-imgproc-dev libopencv-video-dev'
# on Debian buster/Ubuntu 18.04.
# If you install OpenCV separately, please modify the build rule accordingly.
cc_library(
    name = "opencv",
    srcs = glob(
        [
            "lib/libopencv_core.so",
            "lib/libopencv_calib3d.so",
            "lib/libopencv_features2d.so",
            "lib/libopencv_highgui.so",
            "lib/libopencv_imgcodecs.so",
            "lib/libopencv_imgproc.so",
            "lib/libopencv_video.so",
            "lib/libopencv_videoio.so",
        ],
    ),
    hdrs = glob([
        # For OpenCV 3.x
        "include/opencv2/**/*.h*",
        # For OpenCV 4.x
        # "include/opencv4/opencv2/**/*.h*",
    ]),
    includes = [
        # For OpenCV 3.x
        "include/",
        # For OpenCV 4.x
        # "include/opencv4/",
    ],
    linkstatic = 1,
    visibility = ["//visibility:public"],
)

filegroup(
    name = "opencv_libs",
    srcs = [
        "lib/libopencv_calib3d.so",
        "lib/libopencv_core.so",
        "lib/libopencv_features2d.so",
        "lib/libopencv_highgui.so",
        "lib/libopencv_imgcodecs.so",
        "lib/libopencv_imgproc.so",
        "lib/libopencv_video.so",
        "lib/libopencv_videoio.so",
    ],
    visibility = ["//visibility:public"],
)
