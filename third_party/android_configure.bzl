# Copyright 2019 gRPC authors.
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# Copyright 2016 The TensorFlow Authors. All Rights Reserved.
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#     http://www.apache.org/licenses/LICENSE-2.0
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# Based on https://github.com/gnossen/grpc/blob/19fe1e777c9de7cfa42b04bbe764856f265b6110/third_party/android/android_configure.bzl
# and https://github.com/tensorflow/tensorflow/blob/c858694b46eb777087b57a721f0cbb7e271e4b9e/third_party/android/android_configure.bzl

# Changes
#  - determine ANDROID_NDK_API_LEVEL dynamically
#  - not to set `path` explicitly

"""Repository rule for Android SDK and NDK autoconfiguration.
This rule is a no-op unless the required android environment variables are set.
"""

# Workaround for https://github.com/bazelbuild/bazel/issues/14260

_ANDROID_NDK_HOME = "ANDROID_NDK_HOME"
_ANDROID_NDK_API_LEVEL = "ANDROID_NDK_API_LEVEL"
_ANDROID_SDK_HOME = "ANDROID_HOME"

def _android_autoconf_impl(repository_ctx):
    sdk_home = repository_ctx.os.environ.get(_ANDROID_SDK_HOME)
    ndk_home = repository_ctx.os.environ.get(_ANDROID_NDK_HOME)
    ndk_api_level = repository_ctx.os.environ.get(_ANDROID_NDK_API_LEVEL)

    # version 31.0.0 won't work https://stackoverflow.com/a/68036845
    sdk_rule = ""
    if sdk_home:
        sdk_rule = """native.android_sdk_repository(name="androidsdk")"""

    # Note that Bazel does not support NDK 22 yet.
    ndk_rule = ""
    if ndk_home:
        if not ndk_api_level:
          ndk_rule = """native.android_ndk_repository(name="androidndk")"""
        else:
          ndk_rule = """
    native.android_ndk_repository(
        name="androidndk",
        api_level={},
    )
""".format(ndk_api_level)

    if ndk_rule == "" and sdk_rule == "":
        sdk_rule = "pass"

    repository_ctx.file("BUILD.bazel", "")
    repository_ctx.file("android_configure.bzl", """
def android_workspace():
    {}
    {}
    """.format(sdk_rule, ndk_rule))

android_configure = repository_rule(
    implementation = _android_autoconf_impl,
    environ = [
        _ANDROID_NDK_HOME,
        _ANDROID_NDK_API_LEVEL,
        _ANDROID_SDK_HOME,
    ],
)
