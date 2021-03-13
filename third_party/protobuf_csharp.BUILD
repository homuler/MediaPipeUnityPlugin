licenses(["notice"])

package(
    default_visibility = ["//visibility:public"],
)

load("@mediapipe_api//mediapipe_api:dotnet.bzl", "dotnet_library")

exports_files(
    srcs = glob(["csharp/**", "benchmarks/**"]),
)

dotnet_library(
    name = "protobuf_dlls",
    srcs = glob(["csharp/**", "benchmarks/**"]),
    solution_file = ":csharp/src/Google.Protobuf.sln",
    outs = [
      "csharp/src/Google.Protobuf/bin/Release/net45/Google.Protobuf.dll",
      "csharp/src/Google.Protobuf/bin/Release/net45/System.Buffers.dll",
      "csharp/src/Google.Protobuf/bin/Release/net45/System.Memory.dll",
      "csharp/src/Google.Protobuf/bin/Release/net45/System.Runtime.CompilerServices.Unsafe.dll",
    ],
)
