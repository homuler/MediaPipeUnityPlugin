using System.Runtime.InteropServices;
using UnityEngine;

using MpCalculatorGraphConfig = System.IntPtr;
using ProtobufLogHandlerPtr = System.IntPtr;

namespace Mediapipe
{
  public class CalculatorGraphConfig
  {
    private const string MediapipeLibrary = "mediapipe_c";

    public MpCalculatorGraphConfig mpCalculatorGraphConfig;

    static CalculatorGraphConfig()
    {
      SetProtobufLogHandler(Marshal.GetFunctionPointerForDelegate(protobufLogHandler));
    }

    public CalculatorGraphConfig(string configText)
    {
      mpCalculatorGraphConfig = ParseMpCalculatorGraphConfig(configText);

      if (mpCalculatorGraphConfig == System.IntPtr.Zero)
      {
        throw new System.SystemException("Failed to parse the text as graph config");
      }
    }

    ~CalculatorGraphConfig()
    {
      MpCalculatorGraphConfigDestroy(mpCalculatorGraphConfig);
    }

    public MpCalculatorGraphConfig GetPtr()
    {
      return mpCalculatorGraphConfig;
    }

    // Protobuf Logger
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void ProtobufLogHandler(int level, string filename, int line, string message);
    private static readonly ProtobufLogHandler protobufLogHandler = LogProtobufMessage;
    private static ProtobufLogHandlerPtr protobufLogHandlerPtr;

    private static void LogProtobufMessage(int level, string filename, int line, string message)
    {
      Debug.Log($"[libprotobuf {FormatProtobufLogLevel(level)} {filename}:{line}] {message}");
    }

    private static string FormatProtobufLogLevel(int level)
    {
      switch (level)
      {
        case 1: return "WARNING";
        case 2: return "ERROR";
        case 3: return "FATAL";
        default: return "INFO";
      }
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpCalculatorGraphConfig ParseMpCalculatorGraphConfig(string input);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpCalculatorGraphConfigDestroy(MpCalculatorGraphConfig config);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe ProtobufLogHandlerPtr SetProtobufLogHandler([MarshalAs(UnmanagedType.FunctionPtr)]ProtobufLogHandlerPtr logHandler);

    #endregion
  }
}
