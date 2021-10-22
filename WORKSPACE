workspace(name = "mediapipe_api")

# This file is almost the same as mediapipe's WORKSPACE file, but there exist some differences between the two.
#
# - com_google_mediapipe dependency
# - @//third_party -> @com_google_mediapipe//third_party
#    - exception: opencv_linux, opencv_windows, ffmpeg_linux, ffmpeg_macos
# - android_opencv sha256 is added
# - unity dependency
# - rules_foreign_cc's version
# - rules_pkg

load("@bazel_tools//tools/build_defs/repo:http.bzl", "http_archive")

http_archive(
    name = "bazel_skylib",
    sha256 = "1c531376ac7e5a180e0237938a2536de0c54d93f5c278634818e0efc952dd56c",
    type = "tar.gz",
    urls = [
        "https://github.com/bazelbuild/bazel-skylib/releases/download/1.0.3/bazel-skylib-1.0.3.tar.gz",
        "https://mirror.bazel.build/github.com/bazelbuild/bazel-skylib/releases/download/1.0.3/bazel-skylib-1.0.3.tar.gz",
    ],
)

load("@bazel_skylib//:workspace.bzl", "bazel_skylib_workspace")

bazel_skylib_workspace()

load("@bazel_skylib//lib:versions.bzl", "versions")

versions.check(minimum_bazel_version = "3.7.2")

http_archive(
    name = "rules_pkg",
    sha256 = "6b5969a7acd7b60c02f816773b06fcf32fbe8ba0c7919ccdc2df4f8fb923804a",
    url = "https://github.com/bazelbuild/rules_pkg/releases/download/0.3.0/rules_pkg-0.3.0.tar.gz",
)

load("@rules_pkg//:deps.bzl", "rules_pkg_dependencies")

rules_pkg_dependencies()

new_local_repository(
    name = "unity",
    build_file = "@//third_party:unity.BUILD",
    path = "/path/to/unity/2020.3.8f1",
)

# mediapipe
http_archive(
    name = "com_google_mediapipe",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@//third_party:mediapipe_opencv.diff",
        "@//third_party:mediapipe_workaround.diff",
        "@//third_party:mediapipe_visibility.diff",
        "@//third_party:mediapipe_model_path.diff",
        "@//third_party:mediapipe_extension.diff",
    ],
    sha256 = "d20c4c7a957c70be6018c704a5cea3c61314df22e4f88b452c19472f0227a48f",
    strip_prefix = "mediapipe-0.8.8",
    urls = ["https://github.com/google/mediapipe/archive/v0.8.8.tar.gz"],
)

# ABSL cpp library lts_2021_03_24, patch 2.
http_archive(
    name = "com_google_absl",
    patch_args = [
        "-p1",
    ],
    # Remove after https://github.com/abseil/abseil-cpp/issues/326 is solved.
    patches = [
        "@com_google_mediapipe//third_party:com_google_absl_f863b622fe13612433fdf43f76547d5edda0c93001.diff",
    ],
    sha256 = "59b862f50e710277f8ede96f083a5bb8d7c9595376146838b9580be90374ee1f",
    strip_prefix = "abseil-cpp-20210324.2",
    urls = [
        "https://github.com/abseil/abseil-cpp/archive/refs/tags/20210324.2.tar.gz",
    ],
)

http_archive(
    name = "rules_cc",
    strip_prefix = "rules_cc-main",
    urls = ["https://github.com/bazelbuild/rules_cc/archive/main.zip"],
)

http_archive(
    name = "rules_foreign_cc",
    sha256 = "30c970bfaeda3485100c62b13093da2be2c70ed99ec8d30f4fac6dd37cb25f34",
    strip_prefix = "rules_foreign_cc-0.6.0",
    url = "https://github.com/bazelbuild/rules_foreign_cc/archive/0.6.0.zip",
)

load("@rules_foreign_cc//foreign_cc:repositories.bzl", "rules_foreign_cc_dependencies")

rules_foreign_cc_dependencies()

# GoogleTest/GoogleMock framework. Used by most unit-tests.
# Last updated 2021-07-02.
http_archive(
    name = "com_google_googletest",
    sha256 = "de682ea824bfffba05b4e33b67431c247397d6175962534305136aa06f92e049",
    strip_prefix = "googletest-4ec4cd23f486bf70efcc5d2caa40f24368f752e3",
    urls = ["https://github.com/google/googletest/archive/4ec4cd23f486bf70efcc5d2caa40f24368f752e3.zip"],
)

# Google Benchmark library.
http_archive(
    name = "com_google_benchmark",
    build_file = "@com_google_mediapipe//third_party:benchmark.BUILD",
    strip_prefix = "benchmark-main",
    urls = ["https://github.com/google/benchmark/archive/main.zip"],
)

# gflags needed by glog
http_archive(
    name = "com_github_gflags_gflags",
    sha256 = "19713a36c9f32b33df59d1c79b4958434cb005b5b47dc5400a7a4b078111d9b5",
    strip_prefix = "gflags-2.2.2",
    url = "https://github.com/gflags/gflags/archive/v2.2.2.zip",
)

# 2020-08-21
http_archive(
    name = "com_github_glog_glog",
    sha256 = "58c9b3b6aaa4dd8b836c0fd8f65d0f941441fb95e27212c5eeb9979cfd3592ab",
    strip_prefix = "glog-0a2e5931bd5ff22fd3bf8999eb8ce776f159cda6",
    urls = [
        "https://github.com/google/glog/archive/0a2e5931bd5ff22fd3bf8999eb8ce776f159cda6.zip",
    ],
)

http_archive(
    name = "com_github_glog_glog_no_gflags",
    build_file = "@com_google_mediapipe//third_party:glog_no_gflags.BUILD",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@com_google_mediapipe//third_party:com_github_glog_glog_9779e5ea6ef59562b030248947f787d1256132ae.diff",
    ],
    sha256 = "58c9b3b6aaa4dd8b836c0fd8f65d0f941441fb95e27212c5eeb9979cfd3592ab",
    strip_prefix = "glog-0a2e5931bd5ff22fd3bf8999eb8ce776f159cda6",
    urls = [
        "https://github.com/google/glog/archive/0a2e5931bd5ff22fd3bf8999eb8ce776f159cda6.zip",
    ],
)

# easyexif
http_archive(
    name = "easyexif",
    build_file = "@com_google_mediapipe//third_party:easyexif.BUILD",
    strip_prefix = "easyexif-master",
    url = "https://github.com/mayanklahiri/easyexif/archive/master.zip",
)

# libyuv
http_archive(
    name = "libyuv",
    build_file = "@com_google_mediapipe//third_party:libyuv.BUILD",
    # Error: operand type mismatch for `vbroadcastss' caused by commit 8a13626e42f7fdcf3a6acbb0316760ee54cda7d8.
    urls = ["https://chromium.googlesource.com/libyuv/libyuv/+archive/2525698acba9bf9b701ba6b4d9584291a1f62257.tar.gz"],
)

# Note: protobuf-javalite is no longer released as a separate download, it's included in the main Java download.
# ...but the Java download is currently broken, so we use the "source" download.
http_archive(
    name = "com_google_protobuf_javalite",
    sha256 = "a79d19dcdf9139fa4b81206e318e33d245c4c9da1ffed21c87288ed4380426f9",
    strip_prefix = "protobuf-3.11.4",
    urls = ["https://github.com/protocolbuffers/protobuf/archive/v3.11.4.tar.gz"],
)

http_archive(
    name = "com_google_protobuf",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@com_google_mediapipe//third_party:com_google_protobuf_fixes.diff",
    ],
    sha256 = "a79d19dcdf9139fa4b81206e318e33d245c4c9da1ffed21c87288ed4380426f9",
    strip_prefix = "protobuf-3.11.4",
    urls = ["https://github.com/protocolbuffers/protobuf/archive/v3.11.4.tar.gz"],
)

http_archive(
    name = "com_google_audio_tools",
    strip_prefix = "multichannel-audio-tools-master",
    urls = ["https://github.com/google/multichannel-audio-tools/archive/master.zip"],
)

# 2020-07-09
http_archive(
    name = "pybind11_bazel",
    sha256 = "75922da3a1bdb417d820398eb03d4e9bd067c4905a4246d35a44c01d62154d91",
    strip_prefix = "pybind11_bazel-203508e14aab7309892a1c5f7dd05debda22d9a5",
    urls = ["https://github.com/pybind/pybind11_bazel/archive/203508e14aab7309892a1c5f7dd05debda22d9a5.zip"],
)

http_archive(
    name = "pybind11",
    build_file = "@pybind11_bazel//:pybind11.BUILD",
    sha256 = "616d1c42e4cf14fa27b2a4ff759d7d7b33006fdc5ad8fd603bb2c22622f27020",
    strip_prefix = "pybind11-2.7.1",
    urls = [
        "https://storage.googleapis.com/mirror.tensorflow.org/github.com/pybind/pybind11/archive/v2.7.1.tar.gz",
        "https://github.com/pybind/pybind11/archive/v2.7.1.tar.gz",
    ],
)

http_archive(
    name = "ceres_solver",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@com_google_mediapipe//third_party:ceres_solver_compatibility_fixes.diff",
    ],
    sha256 = "db12d37b4cebb26353ae5b7746c7985e00877baa8e7b12dc4d3a1512252fff3b",
    strip_prefix = "ceres-solver-2.0.0",
    url = "https://github.com/ceres-solver/ceres-solver/archive/2.0.0.zip",
)

http_archive(
    name = "opencv",
    build_file = "@//third_party:opencv.BUILD",
    sha256 = "1ed6f5b02a7baf14daca04817566e7c98ec668cec381e0edf534fa49f10f58a2",
    strip_prefix = "opencv-3.4.10",
    urls = ["https://github.com/opencv/opencv/archive/3.4.10.tar.gz"],
)

new_local_repository(
    name = "linux_opencv",
    build_file = "@//third_party:opencv_linux.BUILD",
    path = "/usr",
)

new_local_repository(
    name = "linux_ffmpeg",
    build_file = "@//third_party:ffmpeg_linux.BUILD",
    path = "/usr",
)

new_local_repository(
    name = "macos_opencv",
    build_file = "@com_google_mediapipe//third_party:opencv_macos.BUILD",
    path = "/usr/local/opt/opencv@3",
)

new_local_repository(
    name = "macos_ffmpeg",
    build_file = "@//third_party:ffmpeg_macos.BUILD",
    path = "/usr/local/opt/ffmpeg",
)

new_local_repository(
    name = "windows_opencv",
    build_file = "@//third_party:opencv_windows.BUILD",
    path = "C:\\opencv\\build",
)

http_archive(
    name = "android_opencv",
    build_file = "@com_google_mediapipe//third_party:opencv_android.BUILD",
    sha256 = "cdb0e190c3734edd4052a3535d9e4310af912a9f70a421b1621711942a1028d5",
    strip_prefix = "OpenCV-android-sdk",
    type = "zip",
    url = "https://github.com/opencv/opencv/releases/download/3.4.3/opencv-3.4.3-android-sdk.zip",
)

# After OpenCV 3.2.0, the pre-compiled opencv2.framework has google protobuf symbols, which will
# trigger duplicate symbol errors in the linking stage of building a mediapipe ios app.
# To get a higher version of OpenCV for iOS, opencv2.framework needs to be built from source with
# '-DBUILD_PROTOBUF=OFF -DBUILD_opencv_dnn=OFF'.
http_archive(
    name = "ios_opencv",
    build_file = "@com_google_mediapipe//third_party:opencv_ios.BUILD",
    sha256 = "7dd536d06f59e6e1156b546bd581523d8df92ce83440002885ec5abc06558de2",
    type = "zip",
    url = "https://github.com/opencv/opencv/releases/download/3.2.0/opencv-3.2.0-ios-framework.zip",
)

http_archive(
    name = "stblib",
    build_file = "@com_google_mediapipe//third_party:stblib.BUILD",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@com_google_mediapipe//third_party:stb_image_impl.diff",
    ],
    sha256 = "13a99ad430e930907f5611325ec384168a958bf7610e63e60e2fd8e7b7379610",
    strip_prefix = "stb-b42009b3b9d4ca35bc703f5310eedc74f584be58",
    urls = ["https://github.com/nothings/stb/archive/b42009b3b9d4ca35bc703f5310eedc74f584be58.tar.gz"],
)

# You may run setup_android.sh to install Android SDK and NDK.
android_ndk_repository(
    name = "androidndk",
    api_level = 21,
)

android_sdk_repository(
    name = "androidsdk",
)

# iOS basic build deps.

http_archive(
    name = "build_bazel_rules_apple",
    patch_args = [
        "-p1",
    ],
    patches = [
        # Bypass checking ios unit test runner when building MP ios applications.
        "@com_google_mediapipe//third_party:build_bazel_rules_apple_bypass_test_runner_check.diff",
    ],
    sha256 = "7a7afdd4869bb201c9352eed2daf37294d42b093579b70423490c1b4d4f6ce42",
    url = "https://github.com/bazelbuild/rules_apple/releases/download/0.19.0/rules_apple.0.19.0.tar.gz",
)

load(
    "@build_bazel_rules_apple//apple:repositories.bzl",
    "apple_rules_dependencies",
)

apple_rules_dependencies()

load(
    "@build_bazel_rules_swift//swift:repositories.bzl",
    "swift_rules_dependencies",
)

swift_rules_dependencies()

http_archive(
    name = "build_bazel_apple_support",
    sha256 = "122ebf7fe7d1c8e938af6aeaee0efe788a3a2449ece5a8d6a428cb18d6f88033",
    urls = [
        "https://storage.googleapis.com/mirror.tensorflow.org/github.com/bazelbuild/apple_support/releases/download/0.7.1/apple_support.0.7.1.tar.gz",
        "https://github.com/bazelbuild/apple_support/releases/download/0.7.1/apple_support.0.7.1.tar.gz",
    ],
)

load(
    "@build_bazel_apple_support//lib:repositories.bzl",
    "apple_support_dependencies",
)

apple_support_dependencies()

# More iOS deps.

http_archive(
    name = "google_toolbox_for_mac",
    build_file = "@com_google_mediapipe//third_party:google_toolbox_for_mac.BUILD",
    sha256 = "e3ac053813c989a88703556df4dc4466e424e30d32108433ed6beaec76ba4fdc",
    strip_prefix = "google-toolbox-for-mac-2.2.1",
    url = "https://github.com/google/google-toolbox-for-mac/archive/v2.2.1.zip",
)

# Maven dependencies.

RULES_JVM_EXTERNAL_TAG = "4.0"

RULES_JVM_EXTERNAL_SHA = "31701ad93dbfe544d597dbe62c9a1fdd76d81d8a9150c2bf1ecf928ecdf97169"

http_archive(
    name = "rules_jvm_external",
    sha256 = RULES_JVM_EXTERNAL_SHA,
    strip_prefix = "rules_jvm_external-%s" % RULES_JVM_EXTERNAL_TAG,
    url = "https://github.com/bazelbuild/rules_jvm_external/archive/%s.zip" % RULES_JVM_EXTERNAL_TAG,
)

load("@rules_jvm_external//:defs.bzl", "maven_install")

# Important: there can only be one maven_install rule. Add new maven deps here.
maven_install(
    artifacts = [
        "androidx.concurrent:concurrent-futures:1.0.0-alpha03",
        "androidx.lifecycle:lifecycle-common:2.3.1",
        "androidx.activity:activity:1.2.2",
        "androidx.exifinterface:exifinterface:1.3.3",
        "androidx.fragment:fragment:1.3.4",
        "androidx.annotation:annotation:aar:1.1.0",
        "androidx.appcompat:appcompat:aar:1.1.0-rc01",
        "androidx.camera:camera-core:1.0.0-beta10",
        "androidx.camera:camera-camera2:1.0.0-beta10",
        "androidx.camera:camera-lifecycle:1.0.0-beta10",
        "androidx.constraintlayout:constraintlayout:aar:1.1.3",
        "androidx.core:core:aar:1.1.0-rc03",
        "androidx.legacy:legacy-support-v4:aar:1.0.0",
        "androidx.recyclerview:recyclerview:aar:1.1.0-beta02",
        "androidx.test.espresso:espresso-core:3.1.1",
        "com.github.bumptech.glide:glide:4.11.0",
        "com.google.android.material:material:aar:1.0.0-rc01",
        "com.google.auto.value:auto-value:1.8.1",
        "com.google.auto.value:auto-value-annotations:1.8.1",
        "com.google.code.findbugs:jsr305:latest.release",
        "com.google.flogger:flogger-system-backend:0.6",
        "com.google.flogger:flogger:0.6",
        "com.google.guava:guava:27.0.1-android",
        "com.google.guava:listenablefuture:1.0",
        "junit:junit:4.12",
        "org.hamcrest:hamcrest-library:1.3",
    ],
    fetch_sources = True,
    repositories = [
        "https://maven.google.com",
        "https://dl.google.com/dl/android/maven2",
        "https://repo1.maven.org/maven2",
        "https://jcenter.bintray.com",
    ],
    version_conflict_policy = "pinned",
)

# Needed by TensorFlow
http_archive(
    name = "io_bazel_rules_closure",
    sha256 = "e0a111000aeed2051f29fcc7a3f83be3ad8c6c93c186e64beb1ad313f0c7f9f9",
    strip_prefix = "rules_closure-cf1e44edb908e9616030cc83d085989b8e6cd6df",
    urls = [
        "http://mirror.tensorflow.org/github.com/bazelbuild/rules_closure/archive/cf1e44edb908e9616030cc83d085989b8e6cd6df.tar.gz",
        "https://github.com/bazelbuild/rules_closure/archive/cf1e44edb908e9616030cc83d085989b8e6cd6df.tar.gz",  # 2019-04-04
    ],
)

# Tensorflow repo should always go after the other external dependencies.
# 2021-07-29
_TENSORFLOW_GIT_COMMIT = "52a2905cbc21034766c08041933053178c5d10e3"

_TENSORFLOW_SHA256 = "06d4691bcdb700f3275fa0971a1585221c2b9f3dffe867963be565a6643d7f56"

http_archive(
    name = "org_tensorflow",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@com_google_mediapipe//third_party:org_tensorflow_compatibility_fixes.diff",
        "@com_google_mediapipe//third_party:org_tensorflow_objc_cxx17.diff",
        # Diff is generated with a script, don't update it manually.
        "@com_google_mediapipe//third_party:org_tensorflow_custom_ops.diff",
        "@//third_party:tensorflow_python_path.diff",
    ],
    sha256 = _TENSORFLOW_SHA256,
    strip_prefix = "tensorflow-%s" % _TENSORFLOW_GIT_COMMIT,
    urls = [
        "https://github.com/tensorflow/tensorflow/archive/%s.tar.gz" % _TENSORFLOW_GIT_COMMIT,
    ],
)

load("@org_tensorflow//tensorflow:workspace3.bzl", "tf_workspace3")

tf_workspace3()

load("@org_tensorflow//tensorflow:workspace2.bzl", "tf_workspace2")

tf_workspace2()

# Edge TPU
http_archive(
    name = "libedgetpu",
    sha256 = "14d5527a943a25bc648c28a9961f954f70ba4d79c0a9ca5ae226e1831d72fe80",
    strip_prefix = "libedgetpu-3164995622300286ef2bb14d7fdc2792dae045b7",
    urls = [
        "https://github.com/google-coral/libedgetpu/archive/3164995622300286ef2bb14d7fdc2792dae045b7.tar.gz",
    ],
)

load("@libedgetpu//:workspace.bzl", "libedgetpu_dependencies")

libedgetpu_dependencies()

load("@coral_crosstool//:configure.bzl", "cc_crosstool")

cc_crosstool(name = "crosstool")
