using System;
using System.Runtime.InteropServices;
using UnityEngine;

using MpCalculatorGraphConfig = System.IntPtr;
using ProtobufLogHandlerPtr = System.IntPtr;

namespace Mediapipe {
  public class CalculatorGraphConfig : ResourceHandle {
    private bool _disposed = false;
    public MpCalculatorGraphConfig mpCalculatorGraphConfig;

    static CalculatorGraphConfig() {
      UnsafeNativeMethods.SetProtobufLogHandler(Marshal.GetFunctionPointerForDelegate(protobufLogHandler));
    }

    public CalculatorGraphConfig(string configText) : base(UnsafeNativeMethods.ParseMpCalculatorGraphConfig(configText)) {
      if (ptr == IntPtr.Zero) {
        throw new FormatException("Failed to parse the text as graph config");
      }
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpCalculatorGraphConfigDestroy(ptr);
      }

      ptr = IntPtr.Zero;

      _disposed = true;
    }

    // Protobuf Logger
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void ProtobufLogHandler(int level, string filename, int line, string message);
    private static readonly ProtobufLogHandler protobufLogHandler = LogProtobufMessage;
    private static ProtobufLogHandlerPtr protobufLogHandlerPtr;

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
