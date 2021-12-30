# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@bazel_skylib//rules:common_settings.bzl", "string_flag")
load("@rules_foreign_cc//foreign_cc:defs.bzl", "cmake")
load("@mediapipe_api//mediapipe_api:defs.bzl", "concat_dict_and_select")

package(default_visibility = ["//visibility:public"])

filegroup(
    name = "all",
    srcs = glob(["**"]),
)

string_flag(
    name = "switch",
    build_setting_default = "local",
    values = [
        "local",
        "cmake",
    ],
)

config_setting(
    name = "source_build",
    flag_values = {
        ":switch": "cmake",
    },
)

config_setting(
    name = "windows_dbg",
    constraint_values = ["@platforms//os:windows"],
    values = {"compilation_mode": "dbg"},
)

alias(
    name = "opencv",
    actual = select({
        ":source_build": ":opencv_from_source",
        "//conditions:default": ":opencv_binary",
    }),
)

alias(
    name = "opencv_binary",
    actual = select({
        "@com_google_mediapipe//mediapipe:android_x86": "@android_opencv//:libopencv_x86",
        "@com_google_mediapipe//mediapipe:android_x86_64": "@android_opencv//:libopencv_x86_64",
        "@com_google_mediapipe//mediapipe:android_armeabi": "@android_opencv//:libopencv_armeabi-v7a",
        "@com_google_mediapipe//mediapipe:android_arm": "@android_opencv//:libopencv_armeabi-v7a",
        "@com_google_mediapipe//mediapipe:android_arm64": "@android_opencv//:libopencv_arm64-v8a",
        "@com_google_mediapipe//mediapipe:ios": "@ios_opencv//:opencv",
        "@com_google_mediapipe//mediapipe:macos_arm64": "@macos_arm64_opencv//:opencv",
        "@com_google_mediapipe//mediapipe:macos_i386": "@macos_opencv//:opencv",
        "@com_google_mediapipe//mediapipe:macos_x86_64": "@macos_opencv//:opencv",
        "@com_google_mediapipe//mediapipe:windows": "@windows_opencv//:opencv",
        "@com_google_mediapipe//mediapipe:emscripten": "@wasm_opencv//:opencv",
        "//conditions:default": "@linux_opencv//:opencv",
    }),
)

OPENCV_MODULES = [
    "calib3d",
    "features2d",
    "highgui",
    "video",
    "videoio",
    "imgcodecs",
    "imgproc",
    "core",
]

OPENCV_3RDPARTY_LIBS = [
    "IlmImf",
    "libjpeg-turbo",
    "libpng",
    "libtiff",
    "zlib",
]

OPENCV_3RDPARTY_LIBS_M1 = OPENCV_3RDPARTY_LIBS + ["tegra_hal"]

cmake(
    name = "opencv_cmake",
    build_args = [
        "--verbose",
        "--parallel",
    ] + select({
        "@bazel_tools//src/conditions:darwin": ["`sysctl -n hw.ncpu`"],
        "//conditions:default": ["`nproc`"],
    }),
    # Values to be passed as -Dkey=value on the CMake command line;
    # here are serving to provide some CMake script configuration options
    cache_entries = concat_dict_and_select(
        {
            # The module list is always sorted alphabetically so that we do not
            # cause a rebuild when changing the link order.
            "BUILD_LIST": ",".join(sorted(OPENCV_MODULES)),
            "BUILD_opencv_apps": "OFF",
            "BUILD_opencv_python": "OFF",
            "BUILD_opencv_world": "ON",
            "BUILD_EXAMPLES": "OFF",
            "BUILD_PERF_TESTS": "OFF",
            "BUILD_TESTS": "OFF",
            "BUILD_JPEG": "ON",
            "BUILD_OPENEXR": "ON",
            "BUILD_PNG": "ON",
            "BUILD_TIFF": "ON",
            "BUILD_ZLIB": "ON",
            "WITH_1394": "OFF",
            "WITH_FFMPEG": "OFF",
            "WITH_GSTREAMER": "OFF",
            "WITH_GTK": "OFF",
            # Some symbols in ippicv and ippiw cannot be resolved, and they are excluded currently in the first place.
            # https://github.com/opencv/opencv/pull/16505
            "WITH_IPP": "OFF",
            "WITH_ITT": "OFF",
            "WITH_JASPER": "OFF",
            "WITH_V4L": "OFF",
            "WITH_WEBP": "OFF",
            "CV_ENABLE_INTRINSICS": "ON",
            "WITH_EIGEN": "ON",
            "ENABLE_CCACHE": "OFF",
            # flags for static build
            "BUILD_SHARED_LIBS": "OFF",
            "OPENCV_SKIP_PYTHON_LOADER": "ON",
            "OPENCV_SKIP_VISIBILITY_HIDDEN": "ON",
        },
        {
            "@bazel_tools//src/conditions:windows": {
                "CMAKE_CXX_FLAGS": "/std:c++14",
                # required to link to .dll statically
                "BUILD_WITH_STATIC_CRT": "OFF",
                "WITH_LAPACK": "ON",
            },
            "//conditions:default": {
                # https://github.com/opencv/opencv/issues/19846
                "WITH_LAPACK": "OFF",
                "WITH_PTHREADS": "ON",
                "WITH_PTHREADS_PF": "ON",
            },
        },
    ),
    generate_args = select({
        "@bazel_tools//src/conditions:windows": [
            "-G \"Visual Studio 16 2019\"",
            "-A x64",
        ],
        "//conditions:default": [],
    }),
    lib_source = "@opencv//:all",
    out_lib_dir = select({
        "@bazel_tools//src/conditions:windows": "x64/vc16/staticlib",
        "//conditions:default": "lib",
    }),
    out_static_libs = select({
        ":windows_dbg": ["opencv_world3416d.lib"],
        "@bazel_tools//src/conditions:windows": ["opencv_world3416.lib"],
        "//conditions:default": ["libopencv_world.a"],
    }),
)

cc_library(
    name = "opencv_from_source",
    srcs = select({
        ":windows_dbg": ["opencv_world3416d.lib"],
        "@bazel_tools//src/conditions:windows": ["opencv_world3416.lib"],
        "//conditions:default": ["libopencv_world.a"],
    }) + select({
        ":windows_dbg": ["%sd.lib" % lib for lib in OPENCV_3RDPARTY_LIBS],
        "@bazel_tools//src/conditions:windows": ["%s.lib" % lib for lib in OPENCV_3RDPARTY_LIBS],
        "@bazel_tools//src/conditions:darwin_arm64": ["3rdparty/m1/lib%s.a" % lib for lib in OPENCV_3RDPARTY_LIBS_M1],
        "//conditions:default": ["3rdparty/default/lib%s.a" % lib for lib in OPENCV_3RDPARTY_LIBS],
    }),
    hdrs = glob(["include/opencv2/**/*.h*"]),
    data = select({
        ":windows_dbg": [
            ":opencv_static_libs_win_dbg",
        ],
        "@bazel_tools//src/conditions:windows": [
            ":opencv_static_libs_win",
        ],
        "@bazel_tools//src/conditions:darwin_arm64": [
            ":opencv_static_libs",
            ":opencv_3rdparty_libs_m1",
        ],
        "//conditions:default": [
            ":opencv_static_libs",
            ":opencv_3rdparty_libs",
        ],
    }),
    includes = ["include/"],
    linkopts = select({
        "@bazel_tools//src/conditions:windows": [],
        "//conditions:default": [
            "-ldl",
            "-lm",
            "-lpthread",
        ],
    }) + select({
        "@bazel_tools//src/conditions:windows": [],
        "@bazel_tools//src/conditions:darwin": [],
        "//conditions:default": ["-lrt"],
    }),
    deps = [":opencv_cmake"],
)

filegroup(
    name = "opencv_gen_dir",
    srcs = [":opencv_cmake"],
    output_group = "gen_dir",
)

genrule(
    name = "opencv_static_libs",
    srcs = [":opencv_gen_dir"],
    outs = ["libopencv_world.a"],
    cmd = "cp $</lib/libopencv_world.a $(@D)",
)

genrule(
    name = "opencv_3rdparty_libs",
    srcs = [":opencv_gen_dir"],
    outs = ["3rdparty/default/lib%s.a" % lib for lib in OPENCV_3RDPARTY_LIBS],
    cmd = "mkdir -p $(@D)/3rdparty/default && cp $</share/OpenCV/3rdparty/lib/*.a $(@D)/3rdparty/default",
)

genrule(
    name = "opencv_3rdparty_libs_m1",
    srcs = [":opencv_gen_dir"],
    outs = ["3rdparty/m1/lib%s.a" % lib for lib in OPENCV_3RDPARTY_LIBS_M1],
    cmd = "mkdir -p $(@D)/3rdparty/m1 && cp $</share/OpenCV/3rdparty/lib/*.a $(@D)/3rdparty/m1",
)

genrule(
    name = "opencv_static_libs_win",
    srcs = [":opencv_gen_dir"],
    outs = ["opencv_world3416.lib"] + ["%s.lib" % lib for lib in OPENCV_3RDPARTY_LIBS],
    cmd = "ls -alh $</x64/vc16/staticlib && cp -f $</x64/vc16/staticlib/*.lib $(@D)",
)

genrule(
    name = "opencv_static_libs_win_dbg",
    srcs = [":opencv_gen_dir"],
    outs = ["opencv_world3416d.lib"] + ["%sd.lib" % lib for lib in OPENCV_3RDPARTY_LIBS],
    cmd = "cp -f $</x64/vc16/staticlib/*.lib $(@D)",
)
