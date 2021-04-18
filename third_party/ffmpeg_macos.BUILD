licenses(["notice"])  # LGPL

exports_files(["LICENSE"])

cc_library(
    name = "libffmpeg",
    srcs = [
        "lib/libavcodec.dylib",
        "lib/libavformat.dylib",
        "lib/libavutil.dylib",
        "lib/libswscale.dylib",
        "lib/libavresample.dylib",
    ],
    hdrs = glob(
        [
            "include/libavcodec/*.h",
            "include/libavformat/*.h",
            "include/libavutil/*.h",
            "include/libswscale/*.h",
            "include/libavresample/*.h",
        ]
    ),
    includes = ["include"],
    linkopts = [
        "-lavcodec",
        "-lavformat",
        "-lavutil",
        "-lswscale",
        "-lavresample",
    ],
    linkstatic = 1,
    visibility = ["//visibility:public"],
)
