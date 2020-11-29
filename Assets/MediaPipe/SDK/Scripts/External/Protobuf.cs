using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using pb = global::Google.Protobuf;

namespace Mediapipe {
  class Protobuf {
    static Protobuf() {
      UnsafeNativeMethods.google_protobuf__SetLogHandler__PF(protobufLogHandler).Assert();
    }

    /// <exception cref="MediaPipeException">Thrown when an error occured in unmanaged code</exception>
    /// <exception cref="FormatException">Thrown when failed to parse <paramref name="configText" /></exception>
    public static CalculatorGraphConfig ParseFromStringAsCalculatorGraphConfig(string configText) {
      UnsafeNativeMethods.google_protobuf_TextFormat__ParseFromStringAsCalculatorGraphConfig__PKc(configText, out var configPtr).Assert();

      if (configPtr == IntPtr.Zero) {
        throw new FormatException("Failed to parse the text as CalculatorGraphConfig");
      }

      return new CalculatorGraphConfig(configPtr);
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
    private static readonly ProtobufLogHandler protobufLogHandler = LogProtobufMessage;

    private static void LogProtobufMessage(int level, string filename, int line, string message) {
      Debug.Log($"[libprotobuf {FormatProtobufLogLevel(level)} {filename}:{line}] {message}");
    }

    private static string FormatProtobufLogLevel(int level) {
      switch (level) {
        case 1: return "WARNING";
        case 2: return "ERROR";
        case 3: return "FATAL";
        default: return "INFO";
      }
    }
  }
}
