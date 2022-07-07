# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

load("@bazel_skylib//rules:common_settings.bzl", "string_flag")
load("@bazel_skylib//lib:selects.bzl", "selects")
load("@rules_foreign_cc//foreign_cc:defs.bzl", "cmake")
load("@bazel_rules_dict//:dict.bzl", "define_string_dict", "merge_dict")

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
        "cmake_static",
        "cmake_dynamic",
    ],
)

config_setting(
    name = "cmake_static",
    flag_values = {
        ":switch": "cmake_static",
    },
)

config_setting(
    name = "cmake_dynamic",
    flag_values = {
        ":switch": "cmake_dynamic",
    },
)

config_setting(
    name = "local_build",
    flag_values = {
        ":switch": "local",
    },
)

selects.config_setting_group(
    name = "source_build",
    match_any = [":cmake_static", ":cmake_dynamic"],
)

config_setting(
    name = "dbg_build",
    values = {"compilation_mode": "dbg"},
)

selects.config_setting_group(
    name = "dbg_cmake_dynamic_win",
    match_all = ["@bazel_tools//src/conditions:windows", ":cmake_dynamic", ":dbg_build"],
)

selects.config_setting_group(
    name = "dbg_cmake_static_win",
    match_all = ["@bazel_tools//src/conditions:windows", ":cmake_static", ":dbg_build"],
)

selects.config_setting_group(
    name = "local_build_win",
    match_all = ["@bazel_tools//src/conditions:windows", ":local_build"],
)

alias(
    name = "opencv",
    actual = select({
        ":source_build": ":opencv_cmake",
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

filegroup(
    name = "opencv_world_dll",
    srcs = select({
        ":source_build": [":opencv_world_dll_from_source"],
        ":local_build_win": ["@windows_opencv//:opencv_world_dll"],
        "//conditions:default": [],
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

# ENABLE_NEON=ON
OPENCV_3RDPARTY_LIBS_M1 = OPENCV_3RDPARTY_LIBS + ["tegra_hal"]

define_string_dict(
    name = "common_cache_entries",
    value = {
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
)

define_string_dict(
    name = "build_shared_libs",
    value = select({
        ":cmake_dynamic": { "BUILD_SHARED_LIBS": "ON" },
        ":cmake_static": { "BUILD_SHARED_LIBS": "OFF" },
    }),
)

define_string_dict(
    name = "windows_cache_entries",
    value = select({
        ":cmake_dynamic": {
            "CMAKE_CXX_FLAGS": "/std:c++14",
            "WITH_LAPACK": "ON",
        },
        ":cmake_static": {
            "CMAKE_CXX_FLAGS": "/std:c++14",
            # required to link to .dll statically
            "BUILD_WITH_STATIC_CRT": "OFF",
            "WITH_LAPACK": "ON",
        },
    }),
)

define_string_dict(
    name = "*nix_build_entries",
    value = {
        # https://github.com/opencv/opencv/issues/19846
        "WITH_LAPACK": "OFF",
        "WITH_PTHREADS": "ON",
        "WITH_PTHREADS_PF": "ON",
    },
)

define_string_dict(
    name = "darwin_arch_entries",
    value = select({
        "@cpuinfo//:macos_arm64": {
            "CMAKE_SYSTEM_NAME": "Darwin",
            "CMAKE_SYSTEM_PROCESSOR": "arm64",
            "CMAKE_SYSTEM_ARCHITECTURES": "arm64",
            "CMAKE_OSX_ARCHITECTURES": "arm64",
        },
        "//conditions:default": {
            "CMAKE_SYSTEM_NAME": "Darwin",
            "CMAKE_SYSTEM_PROCESSOR": "x86_64",
            "CMAKE_SYSTEM_ARCHITECTURES": "x86_64",
            "CMAKE_OSX_ARCHITECTURES": "x86_64",
        },
    })
)

merge_dict(
    name = "*nix_cache_entries",
    deps = [
        ":*nix_build_entries",
    ] + select({
        "@bazel_tools//src/conditions:darwin": [":darwin_arch_entries"],
        "//conditions:default": [],
    }),
)

merge_dict(
    name = "cache_entries",
    deps = [
        ":common_cache_entries",
        ":build_shared_libs",
    ] + select({
        "@bazel_tools//src/conditions:windows": [":windows_cache_entries"],
        "//conditions:default": ["*nix_cache_entries"],
    }),
)

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
    cache_entries_target = ":cache_entries",
    generate_args = select({
        "@bazel_tools//src/conditions:windows": [
            "-G \"Visual Studio 16 2019\"",
            "-A x64",
        ],
        "//conditions:default": [],
    }),
    lib_source = "@opencv//:all",
    out_lib_dir = select({
        "@bazel_tools//src/conditions:windows": "x64/vc16",
        "//conditions:default": ".", # need to include lib/ and share/OpenCV/3rdparty/lib when building static libs
    }),
    out_static_libs = select({
        ":cmake_dynamic": [],
        ":dbg_cmake_static_win": ["staticlib/opencv_world3416d.lib"],
        "@bazel_tools//src/conditions:windows": ["staticlib/opencv_world3416.lib"],
        "//conditions:default": ["lib/libopencv_world.a"],
    }) + select({
        ":cmake_dynamic": [],
        ":dbg_cmake_static_win": ["staticlib/%sd.lib" % lib for lib in OPENCV_3RDPARTY_LIBS],
        "@bazel_tools//src/conditions:windows": ["staticlib/%s.lib" % lib for lib in OPENCV_3RDPARTY_LIBS],
        "@cpuinfo//:macos_arm64": ["share/OpenCV/3rdparty/lib/lib%s.a" % lib for lib in OPENCV_3RDPARTY_LIBS_M1],
        "//conditions:default": ["share/OpenCV/3rdparty/lib/lib%s.a" % lib for lib in OPENCV_3RDPARTY_LIBS],
    }) + select({
        ":cmake_static": [],
        ":dbg_cmake_dynamic_win": ["lib/opencv_world3416d.lib"],
        "@bazel_tools//src/conditions:windows": ["lib/opencv_world3416.lib"],
        "//conditions:default": [],
    }),
    out_shared_libs =  select({
        ":cmake_static": [],
        ":dbg_cmake_dynamic_win": ["bin/opencv_world3416d.dll"],
        "@bazel_tools//src/conditions:windows": ["bin/opencv_world3416.dll"],
        "@bazel_tools//src/conditions:darwin": ["lib/libopencv_world.3.4.dylib"],
        "//conditions:default": ["lib/libopencv_world.so"],
    }),
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
)

filegroup(
    name = "opencv_world_dll_from_source",
    srcs = select({
        ":cmake_static": [],
        "@bazel_tools//src/conditions:windows": [":opencv_world_windows"],
        "@bazel_tools//src/conditions:darwin": [":opencv_world_darwin"],
        "//conditions:default": [":opencv_world_linux"],
    }),
)

filegroup(
    name = "opencv_gen_dir",
    srcs = [":opencv_cmake"],
    output_group = "gen_dir",
)

genrule(
    name = "opencv_world_linux",
    srcs = [":opencv_gen_dir"],
    outs = ["libopencv_world.so"],
    cmd = "cp $</lib/libopencv_world.so $(@D)",
)

genrule(
    name = "opencv_world_darwin",
    srcs = [":opencv_gen_dir"],
    outs = ["libopencv_world.3.4.dylib"],
    cmd = "cp $</lib/libopencv_world.3.4.dylib $(@D)",
)

filegroup(
    name = "opencv_world_windows",
    srcs = select({
        ":dbg_build": [":opencv_world3416d_dll"],
        "//conditions:default": [":opencv_world3416_dll"],
    }),
)

genrule(
    name = "opencv_world3416_dll",
    srcs = [":opencv_gen_dir"],
    outs = ["opencv_world3416.dll"],
    cmd = "cp -f $</x64/vc16/bin/opencv_world3416.dll $(@D)",
)

genrule(
    name = "opencv_world3416d_dll",
    srcs = [":opencv_gen_dir"],
    outs = ["opencv_world3416d.dll"],
    cmd = "cp -f $</x64/vc16/bin/opencv_world3416d.dll $(@D)",
)
