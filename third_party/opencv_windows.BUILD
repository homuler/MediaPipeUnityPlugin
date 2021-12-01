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
#   OpenCV libraries for video/image processing on Windows

licenses(["notice"])  # BSD license

exports_files(["LICENSE"])

OPENCV_VERSION = "3416"  # 3.4.16

# The following build rule assumes that the executable "opencv-3.4.16-vc14_vc15.exe"
# is downloaded and the files are extracted to local.
# If you install OpenCV separately, please modify the build rule accordingly.
cc_library(
    name = "opencv",
    srcs = [
        "x64/vc15/lib/opencv_world" + OPENCV_VERSION + ".lib",
        "x64/vc15/bin/opencv_world" + OPENCV_VERSION + ".dll",
    ],
    hdrs = glob(["include/opencv2/**/*.h*"]),
    includes = ["include/"],
    linkstatic = 1,
    visibility = ["//visibility:public"],
)

filegroup(
    name = "opencv_libs",
    srcs = [
        "x64/vc15/bin/opencv_world" + OPENCV_VERSION + ".dll",
    ],
    visibility = ["//visibility:public"],
)
