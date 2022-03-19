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
#
licenses(["notice"])  # Apache License 2.0

package(default_visibility = ["//visibility:public"])

exports_files(["LICENSE"])

cc_library(
    name = "glog",
    visibility = ["//visibility:public"],
    deps = select({
        "@com_google_mediapipe//mediapipe:android_x86": [
            "@com_github_glog_glog_no_gflags//:glog",
        ],
        "@com_google_mediapipe//mediapipe:android_x86_64": [
            "@com_github_glog_glog_no_gflags//:glog",
        ],
        "@com_google_mediapipe//mediapipe:android_armeabi": [
            "@com_github_glog_glog_no_gflags//:glog",
        ],
        "@com_google_mediapipe//mediapipe:android_arm": [
            "@com_github_glog_glog_no_gflags//:glog",
        ],
        "@com_google_mediapipe//mediapipe:android_arm64": [
            "@com_github_glog_glog_no_gflags//:glog",
        ],
        "@com_google_mediapipe//mediapipe:ios": [
            "@com_github_glog_glog_no_gflags//:glog",
        ],
        "@com_google_mediapipe//mediapipe:macos": [
            "@com_github_glog_glog//:glog",
        ],
        "@com_google_mediapipe//mediapipe:windows": [
            "@com_github_glog_glog//:glog",
        ],
        "//conditions:default": [
            "@com_github_glog_glog//:glog",
        ],
    }),
)

cc_library(
    name = "libffmpeg",
    visibility = ["//visibility:public"],
    deps = select({
        "@com_google_mediapipe//mediapipe:android_x86": [],
        "@com_google_mediapipe//mediapipe:android_x86_64": [],
        "@com_google_mediapipe//mediapipe:android_armeabi": [],
        "@com_google_mediapipe//mediapipe:android_arm": [],
        "@com_google_mediapipe//mediapipe:android_arm64": [],
        "@com_google_mediapipe//mediapipe:ios": [],
        "@com_google_mediapipe//mediapipe:macos": [
            "@macos_ffmpeg//:libffmpeg",
        ],
        "//conditions:default": [
            "@linux_ffmpeg//:libffmpeg",
        ],
    }),
)
