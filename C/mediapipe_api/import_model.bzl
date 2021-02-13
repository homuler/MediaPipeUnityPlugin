load("@rules_pkg//:pkg.bzl", "pkg_zip")

def pkg_asset(name, srcs = [], **kwargs):
    """Package MediaPipe assets
    This task renames asset files so that they can be added to an AssetBundle (e.g. x.tflte -> x.bytes) and zip them.

    Args:
      name: the name of the output zip file
      srcs: files to be packaged
    """

    rename_target = "normalize_%s_exts" % name
    _normalize_exts(name = rename_target, srcs = srcs)

    pkg_zip(
      name = name,
      srcs = [":" + rename_target],
      **kwargs,
    )

def _normalize_exts_impl(ctx):
    output_files = []

    for src in ctx.files.srcs:
        if src.extension in ctx.attr.bytes_exts:
            dest = ctx.actions.declare_file(src.path[:-1 * len(src.extension)] + "bytes")
            ctx.actions.run_shell(
              inputs = [src],
              outputs = [dest],
              arguments = [src.path, dest.path],
              command = "test $1 != $2 && cp $1 $2",
              progress_message = "Copying %s to %s...".format(src.path, dest.path),
            )
            output_files.append(dest)
        else:
            output_files.append(src)

    return [
      DefaultInfo(files = depset(output_files)),
    ]

_normalize_exts = rule(
    implementation = _normalize_exts_impl,
    attrs = {
        "srcs": attr.label_list(allow_files = True),
        "bytes_exts": attr.string_list(default = ["jpg", "tflite", "uuu"]),
    },
)
