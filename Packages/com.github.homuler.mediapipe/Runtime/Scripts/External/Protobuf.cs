using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using pb = global::Google.Protobuf;

namespace Mediapipe {
  public static class Protobuf {
    static Protobuf() {
      // UnsafeNativeMethods.google_protobuf__SetLogHandler__PF(protobufLogHandler).Assert();
    }

    public static T DeserializeProto<T>(IntPtr ptr, pb::MessageParser<T> parser) where T : pb::IMessage<T> {
      var serializedProto = Marshal.PtrToStructure<SerializedProto>(ptr);
      var bytes = new byte[serializedProto.length];

      Marshal.Copy(serializedProto.str, bytes, 0, bytes.Length);

      return parser.ParseFrom(bytes);
    }

    public static List<T> DeserializeProtoVector<T>(IntPtr ptr, pb::MessageParser<T> parser) where T : pb::IMessage<T> {
      var serializedProtoVector = Marshal.PtrToStructure<SerializedProtoVector>(ptr);
      var protos = new List<T>(serializedProtoVector.size);

      unsafe {
        byte** protoPtr = (byte**)serializedProtoVector.data;

        for (var i = 0; i < serializedProtoVector.size; i++) {
          protos.Add(Protobuf.DeserializeProto<T>((IntPtr)(*protoPtr++), parser));
        }
      }

      return protos;
    }

    public delegate void ProtobufLogHandler(int level, string filename, int line, string message);
    static readonly ProtobufLogHandler protobufLogHandler = LogProtobufMessage;

    [AOT.MonoPInvokeCallback(typeof(ProtobufLogHandler))]
    static void LogProtobufMessage(int level, string filename, int line, string message) {
      Logger.Log(GetLogLevel(level), $"[libprotobuf {FormatProtobufLogLevel(level)} {filename}:{line}] {message}");
    }

    static string FormatProtobufLogLevel(int level) {
      switch (level) {
        case 1: return "WARNING";
        case 2: return "ERROR";
        case 3: return "FATAL";
        default: return "INFO";
      }
    }

    static Logger.LogLevel GetLogLevel(int level) {
      switch (level) {
        case 1: return Logger.LogLevel.Warn;
        case 2: return Logger.LogLevel.Error;
        case 3: return Logger.LogLevel.Fatal;
        default: return Logger.LogLevel.Info;
      }
    }
  }
}
