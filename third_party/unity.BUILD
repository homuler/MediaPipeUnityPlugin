# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

licenses(["notice"])  # BSD license

exports_files(["LICENSE"])

java_import(
    name = "classes.jar",
    jars = [
        "Editor/Data/PlaybackEngines/AndroidPlayer/Variations/mono/Release/Classes/classes.jar",
    ],
    neverlink = True,
    visibility = ["//visibility:public"],
)

android_library(
    name = "activity",
    srcs = glob(
        ["Editor/Data/PlaybackEngines/AndroidPlayer/Source/**/*.java"],
    ),
    visibility = ["//visibility:public"],
    deps = [
        ":classes.jar",
    ],
)
