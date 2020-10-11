load("@rules_pkg//:pkg.bzl", "pkg_zip")

def pkg_model(name, srcs = [], **kwargs):
    """Package MediaPipe models
    This task renames model files so that they can be added to an AssetBundle (e.g. x.tflte -> x.bytes) and zip them.

    Args:
      name: the name of the output zip file
      srcs: files to be packaged
    """

    for src in srcs:
      _rename_file(src)

    pkg_zip(
      name = name,
      srcs = [_export_file_path(src) for src in srcs],
      **kwargs,
    )

def _rename_file(src):
    export_file = _export_file_path(src)

    native.genrule(
        name = "export_" + export_file,
        srcs = [src],
        outs = [export_file],
        cmd = "cp $< $@",
    )

def _export_file_path(src):
    [prefix, base_name] = src.split(":") # src must contain one colon
    name_arr = base_name.split(".")
    [name, ext] = [base_name, ""] if len(name_arr) == 1 else ["".join(name_arr[:-1]), name_arr[-1]]
    export_file_ext = "bytes" if ext == "tflite" else "txt"

    return "{}.{}".format(name, export_file_ext)
