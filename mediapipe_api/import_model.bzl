# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

"""Asset Packager

Macros to zip dependent assets (e.g. *.tflite) in a format compatible with Unity.
"""

load("@rules_pkg//:pkg.bzl", "pkg_zip")

def copy_file(name, src, out):
    native.genrule(
        name = name,
        srcs = [src],
        outs = [out],
        cmd = "cp $< $@",
    )

def pkg_asset(name, srcs = [], **kwargs):
    """Package MediaPipe assets

    This task renames asset files so that they can be added to an AssetBundle (e.g. x.tflte -> x.bytes) and zip them.

    Args:
      name: the name of the output zip file
      srcs: files to be packaged
      **kwargs: other arguments for pkg_zip
    """

    rename_target = "normalize_%s_exts" % name
    _normalize_exts(name = rename_target, srcs = srcs)

    pkg_zip(
        name = name,
        srcs = [":" + rename_target],
        **kwargs
    )

def _normalize_exts_impl(ctx):
    output_files = []

    for src in ctx.files.srcs:
        ext = "bytes" if src.extension in ctx.attr.bytes_exts else ("txt" if src.extension in ctx.attr.txt_exts else src.extension)

        if ext == src.extension:
            output_files.append(src)
        else:
            dest = ctx.actions.declare_file(src.path[:-1 * len(src.extension)] + ext)
            ctx.actions.run_shell(
                inputs = [src],
                outputs = [dest],
                arguments = [src.path, dest.path],
                command = "test $1 != $2 && cp $1 $2",
                progress_message = "Copying {} to {}...".format(src.path, dest.path),
            )
            output_files.append(dest)

    return [
        DefaultInfo(files = depset(output_files)),
    ]

_normalize_exts = rule(
    implementation = _normalize_exts_impl,
    attrs = {
        "srcs": attr.label_list(allow_files = True),
        "bytes_exts": attr.string_list(default = ["binarypb", "jpg", "png", "tflite", "uuu"]),
        "txt_exts": attr.string_list(default = ["pbtxt"]),
    },
)
