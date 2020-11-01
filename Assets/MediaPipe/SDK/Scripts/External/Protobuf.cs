using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mediapipe {
  class Protobuf {
    static Protobuf() {
      UnsafeNativeMethods.google_protobuf__SetLogHandler__PF(Marshal.GetFunctionPointerForDelegate(protobufLogHandler)).Assert();
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

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void ProtobufLogHandler(int level, string filename, int line, string message);
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
