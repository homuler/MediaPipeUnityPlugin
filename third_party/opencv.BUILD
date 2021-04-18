load("@bazel_skylib//rules:common_settings.bzl", "string_flag", "string_list_flag")
load("@rules_foreign_cc//foreign_cc:defs.bzl", "cmake")
load("@mediapipe_api//mediapipe_api:defs.bzl", "concat_dict_and_select")

package(default_visibility = ["//visibility:public"])

filegroup(
  name = "all",
  srcs = glob(["**"]),
)

string_flag(
    name = "switch",
    values = ["local", "cmake"],
    build_setting_default = "local",
)

string_list_flag(
    name = "deps",
    # Currently, the only meaningful value is "ffmpeg"
    build_setting_default = [],
)

config_setting(
    name = "source_build",
    flag_values = {
        ":switch": "cmake",
    },
)

config_setting(
    name = "with_ffmpeg",
    flag_values = {
        ":deps": "ffmpeg",
    },
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
        "@com_google_mediapipe//mediapipe:macos": "@macos_opencv//:opencv",
        "@com_google_mediapipe//mediapipe:windows": "@windows_opencv//:opencv",
        "//conditions:default": "@linux_opencv//:opencv",
    }),
)

# Note: this determines the order in which the libraries are passed to the
# linker, so if library A depends on library B, library B must come _after_.
# Hence core is at the bottom.
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
    "quirc",
    "zlib",
]

cmake(
    name = "opencv_cmake",
    # Values to be passed as -Dkey=value on the CMake command line;
    # here are serving to provide some CMake script configuration options
    cache_entries = concat_dict_and_select({
        "CMAKE_BUILD_TYPE": "Release",
        # The module list is always sorted alphabetically so that we do not
        # cause a rebuild when changing the link order.
        "BUILD_LIST": ",".join(sorted(OPENCV_MODULES)),
        "BUILD_opencv_apps": "OFF",
        "BUILD_opencv_python": "OFF",
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
        # https://github.com/opencv/opencv/issues/19846
        "WITH_LAPACK": "OFF",
        "WITH_PTHREADS": "ON",
        "WITH_PTHREADS_PF": "ON",
        "ENABLE_CCACHE": "OFF",
        # flags for static build
        "BUILD_SHARED_LIBS": "OFF",
        "OPENCV_SKIP_PYTHON_LOADER": "ON",
        "OPENCV_SKIP_VISIBILITY_HIDDEN": "ON",
    }, {
        ":with_ffmpeg": { "WITH_FFMPEG": "ON" },
        "//conditions:default": { "WITH_FFMPEG": "OFF" },
    }),
    lib_source = "@opencv//:all",
    build_args = [
        "--parallel",
    ] + select({
        "@bazel_tools//src/conditions:darwin": ["`sysctl -n hw.ncpu`"],
        "//conditions:default" : ["`nproc`"],
    }),
    out_static_libs = ["libopencv_%s.a" % (module) for module in OPENCV_MODULES],
)

cc_library(
    name = "opencv_from_source",
    srcs = [
        "libopencv_%s.a" % (module) for module in OPENCV_MODULES
    ] + [
        "lib%s.a" % (lib) for lib in OPENCV_3RDPARTY_LIBS
    ],
    hdrs = glob(["include/opencv2/**/*.h*"]),
    includes = ["include/"],
    deps = [
        ":opencv_cmake",
    ] + select({
        ":with_ffmpeg": ["@//third_party:libffmpeg"],
        "//conditions:default": [],
    }),
    data = [
        ":opencv_static_libs",
        ":opencv_3rdparty_libs",
    ],
    linkopts = [
        "-ldl",
        "-lm",
        "-lpthread",
    ] + select({
        "@bazel_tools//src/conditions:darwin": [],
        "//conditions:default": ["-lrt"],
    }),
)

filegroup(
  name = "opencv_gen_dir",
  srcs = [":opencv_cmake"],
  output_group = "gen_dir",
)

genrule(
    name = "opencv_static_libs",
    srcs = [":opencv_gen_dir"],
    outs = ["libopencv_%s.a" % (lib) for lib in OPENCV_MODULES],
    cmd = "cp $</lib/*.a $(@D)",
)

genrule(
    name = "opencv_3rdparty_libs",
    srcs = [":opencv_gen_dir"],
    outs = ["lib%s.a" % (lib) for lib in OPENCV_3RDPARTY_LIBS],
    cmd = "cp $</share/OpenCV/3rdparty/lib/*.a $(@D)",
)
