# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

# Description:
#   OpenCV libraries for WASM (LLVM IR bitcode)

licenses(["notice"])  # BSD license

exports_files(["LICENSE"])

# OpenCV needs to be built manually.
# cf. https://docs.opencv.org/3.4/d4/da1/tutorial_js_setup.html
#
# Note that the output must be LLVM IR bitcode, not WASM.
#    emcmake python ./platforms/js/build_js.py build_js --build_flags="-emit-llvm -s USE_WEBGL2=1 -flto --oformat=object -s SIDE_MODULE=1"
cc_library(
    name = "opencv",
    srcs = [
        "lib/libopencv_video.a",
        "lib/libopencv_imgproc.a",
        "lib/libopencv_features2d.a",
        "lib/libopencv_calib3d.a",
        "lib/libopencv_core.a",
    ],
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
