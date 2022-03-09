# Copyright (c) 2021 homuler
#
# Use of this source code is governed by an MIT-style
# license that can be found in the LICENSE file or at
# https://opensource.org/licenses/MIT.

"""Proto compiler

Macro for generating C# source files corresponding to .proto files
"""

def csharp_proto_src(name, proto_src, deps):
    """Generate C# source code for *.proto

    Args:
      name: target name
      deps: label list of dependent targets
      proto_src: target .proto file path
    """

    base_name = proto_src.split("/")[-1]
    csharp_out = _camelize(base_name.split(".")[0]) + ".cs"
    outdir = "$(GENDIR)"

    native.genrule(
        name = name,
        srcs = deps + [
            "@com_google_protobuf//:well_known_protos",
        ],
        outs = [csharp_out],
        cmd = """
mkdir -p {outdir}
$(location @com_google_protobuf//:protoc) \
    --proto_path=. \
    --proto_path={outdir} \
    --proto_path=$$(pwd)/external/com_google_protobuf/src \
    --proto_path=$$(pwd)/external/com_google_mediapipe \
    --csharp_out={outdir} {}
mv {outdir}/{outfile} $$(dirname $(location {outfile}))
""".format(proto_src, outdir = outdir, outfile = csharp_out),
        tools = [
            "@com_google_protobuf//:protoc",
        ],
    )

def _camelize(str):
    res = ""
    need_capitalize = True

    for s in str.elems():
        if not s.isalnum():
            need_capitalize = True
            continue

        if need_capitalize:
            res += s.capitalize()
        else:
            res += s

        need_capitalize = s.isdigit()

    return res

def _replace_suffix(string, old, new):
    """Returns a string with an old suffix replaced by a new suffix."""
    return string.endswith(old) and string[:-len(old)] + new or string
