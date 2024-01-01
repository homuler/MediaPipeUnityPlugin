workspace(name = "mediapipe_api")

load("@bazel_tools//tools/build_defs/repo:http.bzl", "http_archive")

http_archive(
    name = "bazel_skylib",
    sha256 = "74d544d96f4a5bb630d465ca8bbcfe231e3594e5aae57e1edbf17a6eb3ca2506",
    urls = [
        "https://storage.googleapis.com/mirror.tensorflow.org/github.com/bazelbuild/bazel-skylib/releases/download/1.3.0/bazel-skylib-1.3.0.tar.gz",
        "https://github.com/bazelbuild/bazel-skylib/releases/download/1.3.0/bazel-skylib-1.3.0.tar.gz",
    ],
)

load("@bazel_skylib//:workspace.bzl", "bazel_skylib_workspace")

bazel_skylib_workspace()

load("@bazel_skylib//lib:versions.bzl", "versions")

versions.check(minimum_bazel_version = "6.1.1")

http_archive(
    name = "rules_pkg",
    sha256 = "62eeb544ff1ef41d786e329e1536c1d541bb9bcad27ae984d57f18f314018e66",
    url = "https://github.com/bazelbuild/rules_pkg/releases/download/0.6.0/rules_pkg-0.6.0.tar.gz",
)

load("@rules_pkg//:deps.bzl", "rules_pkg_dependencies")

rules_pkg_dependencies()

# mediapipe
http_archive(
    name = "com_google_mediapipe",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@//third_party:mediapipe_workaround.diff",
        "@//third_party:mediapipe_opencv.diff",
        "@//third_party:mediapipe_visibility.diff",
        "@//third_party:mediapipe_model_path.diff",
        "@//third_party:mediapipe_extension.diff",
    ],
    sha256 = "c4719fce7a00e097daf3ad972963422ebe190297f5f2288d3ab05e4f856c74c4",
    strip_prefix = "mediapipe-0.10.9",
    urls = ["https://github.com/google/mediapipe/archive/v0.10.9.tar.gz"],
)

# ABSL cpp library lts_2023_01_25.
http_archive(
    name = "com_google_absl",
    urls = [
        "https://github.com/abseil/abseil-cpp/archive/refs/tags/20230125.0.tar.gz",
    ],
    patches = [
        "@com_google_mediapipe//third_party:com_google_absl_windows_patch.diff",
    ],
    patch_args = [
        "-p1",
    ],
    strip_prefix = "abseil-cpp-20230125.0",
    sha256 = "3ea49a7d97421b88a8c48a0de16c16048e17725c7ec0f1d3ea2683a2a75adc21",
)

http_archive(
    name = "rules_cc",
    strip_prefix = "rules_cc-2f8c04c04462ab83c545ab14c0da68c3b4c96191",
    # The commit can be updated if the build passes. Last updated 6/23/22.
    urls = ["https://github.com/bazelbuild/rules_cc/archive/2f8c04c04462ab83c545ab14c0da68c3b4c96191.zip"],
)

# cf. https://github.com/bazelbuild/rules_foreign_cc/issues/1051
http_archive(
    name = "rules_foreign_cc",
    sha256 = "6041f1374ff32ba711564374ad8e007aef77f71561a7ce784123b9b4b88614fc",
    strip_prefix = "rules_foreign_cc-0.8.0",
    url = "https://github.com/bazelbuild/rules_foreign_cc/archive/0.8.0.tar.gz",
)

load("@rules_foreign_cc//foreign_cc:repositories.bzl", "rules_foreign_cc_dependencies")

rules_foreign_cc_dependencies()

http_archive(
    name = "com_google_protobuf",
    sha256 = "87407cd28e7a9c95d9f61a098a53cf031109d451a7763e7dd1253abf8b4df422",
    strip_prefix = "protobuf-3.19.1",
    urls = ["https://github.com/protocolbuffers/protobuf/archive/v3.19.1.tar.gz"],
    patches = [
        "@com_google_mediapipe//third_party:com_google_protobuf_fixes.diff"
    ],
    patch_args = [
        "-p1",
    ],
)

load("//third_party:android_configure.bzl", "android_configure")

android_configure(name = "local_config_android")

load("@local_config_android//:android_configure.bzl", "android_workspace")

android_workspace()

# Load Zlib before initializing TensorFlow and the iOS build rules to guarantee
# that the target @zlib//:mini_zlib is available
http_archive(
    name = "zlib",
    build_file = "@com_google_mediapipe//third_party:zlib.BUILD",
    sha256 = "b3a24de97a8fdbc835b9833169501030b8977031bcb54b3b3ac13740f846ab30",
    strip_prefix = "zlib-1.2.13",
    url = "http://zlib.net/fossils/zlib-1.2.13.tar.gz",
    patches = [
        "@com_google_mediapipe//third_party:zlib.diff",
    ],
    patch_args = [
        "-p1",
    ],
)

# iOS basic build deps.
http_archive(
    name = "build_bazel_apple_support",
    patch_args = [
        "-p1",
    ],
    patches = [
        "@//third_party:build_bazel_apple_support_transitions.diff"
    ],
    sha256 = "9f7bb62c3ae889e0eae8c18458fd8764e2e537687d9a1d85885d6af980e4fc31",
    urls = [
        "https://github.com/bazelbuild/apple_support/releases/download/1.6.0/apple_support.1.6.0.tar.gz",
    ],
)

# iOS basic build deps.
http_archive(
    name = "build_bazel_rules_apple",
    sha256 = "3e2c7ae0ddd181c4053b6491dad1d01ae29011bc322ca87eea45957c76d3a0c3",
    url = "https://github.com/bazelbuild/rules_apple/releases/download/2.1.0/rules_apple.2.1.0.tar.gz",
    patches = [
        # Bypass checking ios unit test runner when building MP ios applications.
        "@com_google_mediapipe//third_party:build_bazel_rules_apple_bypass_test_runner_check.diff",
        "@//third_party:build_bazel_rules_apple_validation.diff",
    ],
    patch_args = [
        "-p1",
    ],
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

load(
    "@build_bazel_rules_swift//swift:extras.bzl",
    "swift_rules_extra_dependencies",
)
swift_rules_extra_dependencies()

load(
    "@build_bazel_apple_support//lib:repositories.bzl",
    "apple_support_dependencies",
)
apple_support_dependencies()

# GoogleTest/GoogleMock framework. Used by most unit-tests.
# Last updated 2021-07-02.
http_archive(
    name = "com_google_googletest",
    urls = ["https://github.com/google/googletest/archive/4ec4cd23f486bf70efcc5d2caa40f24368f752e3.zip"],
    strip_prefix = "googletest-4ec4cd23f486bf70efcc5d2caa40f24368f752e3",
    sha256 = "de682ea824bfffba05b4e33b67431c247397d6175962534305136aa06f92e049",
)

# Google Benchmark library v1.6.1 released on 2022-01-10.
http_archive(
    name = "com_google_benchmark",
    urls = ["https://github.com/google/benchmark/archive/refs/tags/v1.6.1.tar.gz"],
    strip_prefix = "benchmark-1.6.1",
    sha256 = "6132883bc8c9b0df5375b16ab520fac1a85dc9e4cf5be59480448ece74b278d4",
    build_file = "@com_google_mediapipe//third_party:benchmark.BUILD",
)

# gflags needed by glog
http_archive(
    name = "com_github_gflags_gflags",
    strip_prefix = "gflags-2.2.2",
    sha256 = "19713a36c9f32b33df59d1c79b4958434cb005b5b47dc5400a7a4b078111d9b5",
    url = "https://github.com/gflags/gflags/archive/v2.2.2.zip",
)

# 2020-08-21
http_archive(
    name = "com_github_glog_glog",
    strip_prefix = "glog-0.6.0",
    sha256 = "8a83bf982f37bb70825df71a9709fa90ea9f4447fb3c099e1d720a439d88bad6",
    urls = [
        "https://github.com/google/glog/archive/v0.6.0.tar.gz",
    ],
)
http_archive(
    name = "com_github_glog_glog_no_gflags",
    strip_prefix = "glog-0.6.0",
    sha256 = "8a83bf982f37bb70825df71a9709fa90ea9f4447fb3c099e1d720a439d88bad6",
    build_file = "@com_google_mediapipe//third_party:glog_no_gflags.BUILD",
    urls = [
        "https://github.com/google/glog/archive/v0.6.0.tar.gz",
    ],
    patches = [
        "@com_google_mediapipe//third_party:com_github_glog_glog.diff",
    ],
    patch_args = [
        "-p1",
    ],
)

# 2023-06-05
# This version of Glog is required for Windows support, but currently causes
# crashes on some Android devices.
http_archive(
    name = "com_github_glog_glog_windows",
    strip_prefix = "glog-3a0d4d22c5ae0b9a2216988411cfa6bf860cc372",
    sha256 = "170d08f80210b82d95563f4723a15095eff1aad1863000e8eeb569c96a98fefb",
    urls = [
      "https://github.com/google/glog/archive/3a0d4d22c5ae0b9a2216988411cfa6bf860cc372.zip",
    ],
    patches = [
        "@com_google_mediapipe//third_party:com_github_glog_glog.diff",
        "@com_google_mediapipe//third_party:com_github_glog_glog_windows_patch.diff",
    ],
    patch_args = [
        "-p1",
    ],
)

# easyexif
http_archive(
    name = "easyexif",
    url = "https://github.com/mayanklahiri/easyexif/archive/master.zip",
    strip_prefix = "easyexif-master",
    build_file = "@com_google_mediapipe//third_party:easyexif.BUILD",
)

# libyuv
http_archive(
    name = "libyuv",
    # Error: operand type mismatch for `vbroadcastss' caused by commit 8a13626e42f7fdcf3a6acbb0316760ee54cda7d8.
    urls = ["https://chromium.googlesource.com/libyuv/libyuv/+archive/2525698acba9bf9b701ba6b4d9584291a1f62257.tar.gz"],
    build_file = "@com_google_mediapipe//third_party:libyuv.BUILD",
)

# Note: protobuf-javalite is no longer released as a separate download, it's included in the main Java download.
# ...but the Java download is currently broken, so we use the "source" download.
http_archive(
    name = "com_google_protobuf_javalite",
    sha256 = "87407cd28e7a9c95d9f61a098a53cf031109d451a7763e7dd1253abf8b4df422",
    strip_prefix = "protobuf-3.19.1",
    urls = ["https://github.com/protocolbuffers/protobuf/archive/v3.19.1.tar.gz"],
)

load("@com_google_mediapipe//third_party/flatbuffers:workspace.bzl", flatbuffers = "repo")
flatbuffers()

http_archive(
    name = "com_google_audio_tools",
    strip_prefix = "multichannel-audio-tools-1f6b1319f13282eda6ff1317be13de67f4723860",
    urls = ["https://github.com/google/multichannel-audio-tools/archive/1f6b1319f13282eda6ff1317be13de67f4723860.zip"],
    sha256 = "fe346e1aee4f5069c4cbccb88706a9a2b2b4cf98aeb91ec1319be77e07dd7435",
    repo_mapping = {"@com_github_glog_glog" : "@com_github_glog_glog_no_gflags"},
    # TODO: Fix this in AudioTools directly
    patches = ["@com_google_mediapipe//third_party:com_google_audio_tools_fixes.diff"],
    patch_args = ["-p1"]
)

http_archive(
    name = "pffft",
    strip_prefix = "jpommier-pffft-7c3b5a7dc510",
    urls = ["https://bitbucket.org/jpommier/pffft/get/7c3b5a7dc510.zip"],
    build_file = "@com_google_mediapipe//third_party:pffft.BUILD",
)

# sentencepiece
http_archive(
    name = "com_google_sentencepiece",
    strip_prefix = "sentencepiece-0.1.96",
    sha256 = "8409b0126ebd62b256c685d5757150cf7fcb2b92a2f2b98efb3f38fc36719754",
    urls = [
        "https://github.com/google/sentencepiece/archive/refs/tags/v0.1.96.zip"
    ],
    build_file = "@com_google_mediapipe//third_party:sentencepiece.BUILD",
    patches = ["@com_google_mediapipe//third_party:com_google_sentencepiece.diff"],
    patch_args = ["-p1"],
)

http_archive(
    name = "darts_clone",
    build_file = "@com_google_mediapipe//third_party:darts_clone.BUILD",
    sha256 = "c97f55d05c98da6fcaf7f9ecc6a6dc6bc5b18b8564465f77abff8879d446491c",
    strip_prefix = "darts-clone-e40ce4627526985a7767444b6ed6893ab6ff8983",
    urls = [
        "https://github.com/s-yata/darts-clone/archive/e40ce4627526985a7767444b6ed6893ab6ff8983.zip",
    ],
)

http_archive(
    name = "org_tensorflow_text",
    sha256 = "f64647276f7288d1b1fe4c89581d51404d0ce4ae97f2bcc4c19bd667549adca8",
    strip_prefix = "text-2.2.0",
    urls = [
        "https://github.com/tensorflow/text/archive/v2.2.0.zip",
    ],
    patches = [
        "@com_google_mediapipe//third_party:tensorflow_text_remove_tf_deps.diff",
        "@com_google_mediapipe//third_party:tensorflow_text_a0f49e63.diff",
    ],
    patch_args = ["-p1"],
    repo_mapping = {"@com_google_re2": "@com_googlesource_code_re2"},
)

http_archive(
    name = "com_googlesource_code_re2",
    sha256 = "ef516fb84824a597c4d5d0d6d330daedb18363b5a99eda87d027e6bdd9cba299",
    strip_prefix = "re2-03da4fc0857c285e3a26782f6bc8931c4c950df4",
    urls = [
        "https://github.com/google/re2/archive/03da4fc0857c285e3a26782f6bc8931c4c950df4.tar.gz",
    ],
)

# Point to the commit that deprecates the usage of Eigen::MappedSparseMatrix.
http_archive(
    name = "ceres_solver",
    url = "https://github.com/ceres-solver/ceres-solver/archive/123fba61cf2611a3c8bddc9d91416db26b10b558.zip",
    patches = [
        "@com_google_mediapipe//third_party:ceres_solver_compatibility_fixes.diff"
    ],
    patch_args = [
        "-p1",
    ],
    strip_prefix = "ceres-solver-123fba61cf2611a3c8bddc9d91416db26b10b558",
    sha256 = "8b7b16ceb363420e0fd499576daf73fa338adb0b1449f58bea7862766baa1ac7"
)

http_archive(
    name = "opencv",
    build_file = "@//third_party:opencv.BUILD",
    sha256 = "5e37b791b2fe42ed39b52d9955920b951ee42d5da95f79fbc9765a08ef733399",
    strip_prefix = "opencv-3.4.16",
    urls = ["https://github.com/opencv/opencv/archive/3.4.16.tar.gz"],
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
    path = "/usr/local",
)

new_local_repository(
    name = "macos_arm64_opencv",
    build_file = "@com_google_mediapipe//third_party:opencv_macos.BUILD",
    path = "/opt/homebrew",
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
    sha256 = "7dd536d06f59e6e1156b546bd581523d8df92ce83440002885ec5abc06558de2",
    build_file = "@com_google_mediapipe//third_party:opencv_ios.BUILD",
    type = "zip",
    url = "https://github.com/opencv/opencv/releases/download/3.2.0/opencv-3.2.0-ios-framework.zip",
)

# Building an opencv.xcframework from the OpenCV 4.5.3 sources is necessary for
# MediaPipe iOS Task Libraries to be supported on arm64(M1) Macs. An
# `opencv.xcframework` archive has not been released and it is recommended to
# build the same from source using a script provided in OpenCV 4.5.0 upwards.
# OpenCV is fixed to version to 4.5.3 since swift support can only be disabled
# from 4.5.3 upwards. This is needed to avoid errors when the library is linked
# in Xcode. Swift support will be added in when the final binary MediaPipe iOS
# Task libraries are built.
http_archive(
    name = "ios_opencv_source",
    sha256 = "a61e7a4618d353140c857f25843f39b2abe5f451b018aab1604ef0bc34cd23d5",
    build_file = "@com_google_mediapipe//third_party:opencv_ios_source.BUILD",
    type = "zip",
    url = "https://github.com/opencv/opencv/archive/refs/tags/4.5.3.zip",
)

http_archive(
    name = "stblib",
    strip_prefix = "stb-b42009b3b9d4ca35bc703f5310eedc74f584be58",
    sha256 = "13a99ad430e930907f5611325ec384168a958bf7610e63e60e2fd8e7b7379610",
    urls = ["https://github.com/nothings/stb/archive/b42009b3b9d4ca35bc703f5310eedc74f584be58.tar.gz"],
    build_file = "@com_google_mediapipe//third_party:stblib.BUILD",
    patches = [
        "@com_google_mediapipe//third_party:stb_image_impl.diff"
    ],
    patch_args = [
        "-p1",
    ],
)

# More iOS deps.

http_archive(
    name = "google_toolbox_for_mac",
    url = "https://github.com/google/google-toolbox-for-mac/archive/v2.2.1.zip",
    sha256 = "e3ac053813c989a88703556df4dc4466e424e30d32108433ed6beaec76ba4fdc",
    strip_prefix = "google-toolbox-for-mac-2.2.1",
    build_file = "@com_google_mediapipe//third_party:google_toolbox_for_mac.BUILD",
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

# TensorFlow repo should always go after the other external dependencies.
# TF on 2023-07-26.
_TENSORFLOW_GIT_COMMIT = "e92261fd4cec0b726692081c4d2966b75abf31dd"
# curl -L https://github.com/tensorflow/tensorflow/archive/<TENSORFLOW_GIT_COMMIT>.tar.gz | shasum -a 256
_TENSORFLOW_SHA256 = "478a229bd4ec70a5b568ac23b5ea013d9fca46a47d6c43e30365a0412b9febf4"
http_archive(
    name = "org_tensorflow",
    urls = [
      "https://github.com/tensorflow/tensorflow/archive/%s.tar.gz" % _TENSORFLOW_GIT_COMMIT,
    ],
    patches = [
        "@com_google_mediapipe//third_party:org_tensorflow_compatibility_fixes.diff",
        "@com_google_mediapipe//third_party:org_tensorflow_system_python.diff",
        # Diff is generated with a script, don't update it manually.
        "@com_google_mediapipe//third_party:org_tensorflow_custom_ops.diff",
        # Works around Bazel issue with objc_library.
        # See https://github.com/bazelbuild/bazel/issues/19912
        "@com_google_mediapipe//third_party:org_tensorflow_objc_build_fixes.diff",
    ],
    patch_args = [
        "-p1",
    ],
    strip_prefix = "tensorflow-%s" % _TENSORFLOW_GIT_COMMIT,
    sha256 = _TENSORFLOW_SHA256,
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

# Node dependencies
http_archive(
    name = "build_bazel_rules_nodejs",
    sha256 = "94070eff79305be05b7699207fbac5d2608054dd53e6109f7d00d923919ff45a",
    urls = ["https://github.com/bazelbuild/rules_nodejs/releases/download/5.8.2/rules_nodejs-5.8.2.tar.gz"],
)

load("@build_bazel_rules_nodejs//:repositories.bzl", "build_bazel_rules_nodejs_dependencies")
build_bazel_rules_nodejs_dependencies()

# fetches nodejs, npm, and yarn
load("@build_bazel_rules_nodejs//:index.bzl", "node_repositories", "yarn_install")
node_repositories()
yarn_install(
    name = "npm",
    package_json = "@com_google_mediapipe//:package.json",
    yarn_lock = "@com_google_mediapipe//:yarn.lock",
)

# Protobuf for Node dependencies
http_archive(
    name = "rules_proto_grpc",
    sha256 = "bbe4db93499f5c9414926e46f9e35016999a4e9f6e3522482d3760dc61011070",
    strip_prefix = "rules_proto_grpc-4.2.0",
    urls = ["https://github.com/rules-proto-grpc/rules_proto_grpc/archive/4.2.0.tar.gz"],
)

http_archive(
    name = "com_google_protobuf_javascript",
    sha256 = "35bca1729532b0a77280bf28ab5937438e3dcccd6b31a282d9ae84c896b6f6e3",
    strip_prefix = "protobuf-javascript-3.21.2",
    urls = ["https://github.com/protocolbuffers/protobuf-javascript/archive/refs/tags/v3.21.2.tar.gz"],
)

load("@rules_proto_grpc//:repositories.bzl", "rules_proto_grpc_toolchains", "rules_proto_grpc_repos")
rules_proto_grpc_toolchains()
rules_proto_grpc_repos()

load("@rules_proto//proto:repositories.bzl", "rules_proto_dependencies", "rules_proto_toolchains")
rules_proto_dependencies()
rules_proto_toolchains()

load("@com_google_mediapipe//third_party:external_files.bzl", "external_files")
external_files()

load("@com_google_mediapipe//third_party:wasm_files.bzl", "wasm_files")
wasm_files()

# Halide

new_local_repository(
    name = "halide",
    build_file = "@com_google_mediapipe//third_party/halide:BUILD.bazel",
    path = "third_party/halide",
)

http_archive(
    name = "linux_halide",
    sha256 = "d290fadf3f358c94aacf43c883de6468bb98883e26116920afd491ec0e440cd2",
    strip_prefix = "Halide-15.0.1-x86-64-linux",
    urls = ["https://github.com/halide/Halide/releases/download/v15.0.1/Halide-15.0.1-x86-64-linux-4c63f1befa1063184c5982b11b6a2cc17d4e5815.tar.gz"],
    build_file = "@com_google_mediapipe//third_party:halide.BUILD",
)

http_archive(
    name = "macos_x86_64_halide",
    sha256 = "48ff073ac1aee5c4aca941a4f043cac64b38ba236cdca12567e09d803594a61c",
    strip_prefix = "Halide-15.0.1-x86-64-osx",
    urls = ["https://github.com/halide/Halide/releases/download/v15.0.1/Halide-15.0.1-x86-64-osx-4c63f1befa1063184c5982b11b6a2cc17d4e5815.tar.gz"],
    build_file = "@com_google_mediapipe//third_party:halide.BUILD",
)

http_archive(
    name = "macos_arm_64_halide",
    sha256 = "db5d20d75fa7463490fcbc79c89f0abec9c23991f787c8e3e831fff411d5395c",
    strip_prefix = "Halide-15.0.1-arm-64-osx",
    urls = ["https://github.com/halide/Halide/releases/download/v15.0.1/Halide-15.0.1-arm-64-osx-4c63f1befa1063184c5982b11b6a2cc17d4e5815.tar.gz"],
    build_file = "@com_google_mediapipe//third_party:halide.BUILD",
)

http_archive(
    name = "windows_halide",
    sha256 = "61fd049bd75ee918ac6c30d0693aac6048f63f8d1fc4db31001573e58eae8dae",
    strip_prefix = "Halide-15.0.1-x86-64-windows",
    urls = ["https://github.com/halide/Halide/releases/download/v15.0.1/Halide-15.0.1-x86-64-windows-4c63f1befa1063184c5982b11b6a2cc17d4e5815.zip"],
    build_file = "@com_google_mediapipe//third_party:halide.BUILD",
)
