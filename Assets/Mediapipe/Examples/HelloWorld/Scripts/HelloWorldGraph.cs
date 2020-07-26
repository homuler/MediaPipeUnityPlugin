// Copyright 2019 The MediaPipe Authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Runtime.InteropServices;
using UnityEngine;

using MpCalculatorGraph = System.IntPtr;
using MpCalculatorGraphConfig = System.IntPtr;
using MpSidePacket = System.IntPtr;
using MpStatus = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe
{
  public class HelloWorldGraph
  {
    /// <Summary>
    ///   A simple example to print out "Hello World!" from a MediaPipe graph.
    ///   Original C++ source code is <see cref="https://github.com/google/mediapipe/blob/master/mediapipe/examples/desktop/hello_world/hello_world.cc">HERE</see>
    /// </Summary>

    private const string MediapipeLibrary = "mediapipe_c";

    private const string configText = @"
input_stream: ""in""
output_stream: ""out""
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
";

    private MpCalculatorGraph mpCalculatorGraph;
    private OutputStreamPoller outputStreamPoller;
    private Status lastStatus;

    public HelloWorldGraph()
    {
      mpCalculatorGraph = MpCalculatorGraphCreate();

      Initialize();
    }

    ~HelloWorldGraph() {
      MpCalculatorGraphDestroy(mpCalculatorGraph);
    }

    public void StartRun()
    {
      var outputStreamPollerOrStatus = new StatusOrPoller(MpCalculatorGraphAddOutputStreamPoller(mpCalculatorGraph, "out"));

      if (!outputStreamPollerOrStatus.IsOk()) {
        Debug.Log("Failed to add output stream: out");
        // TODO: build and log Status
      }

      outputStreamPoller = outputStreamPollerOrStatus.GetPoller();

      var sidePacket = new SidePacket();
      lastStatus = new Status(MpCalculatorGraphStartRun(mpCalculatorGraph, sidePacket.GetPtr()));
    }

    public Status AddStringToInputStream(string text, int timestamp)
    {
      return new Status(MpCalculatorGraphAddStringPacketToInputStream(mpCalculatorGraph, "in", text, timestamp));
    }

    public void CloseInputStream()
    {
      lastStatus = new Status(MpCalculatorGraphCloseInputStream(mpCalculatorGraph, "in"));
    }

    public void WaitUntilDone()
    {
      lastStatus = new Status(MpCalculatorGraphWaitUntilDone(mpCalculatorGraph));
    }

    public bool HasNextPacket()
    {
      return outputStreamPoller.HasNextPacket();
    }

    public string GetPacketValue()
    {
      return outputStreamPoller.GetPacketValue();
    }

    public bool IsOk()
    {
      return lastStatus == null ? false : lastStatus.IsOk();
    }

    public Status GetLastStatus()
    {
      return lastStatus;
    }

    private void Initialize()
    {
      SetUpDebug();
      var config = ParseMpCalculatorGraphConfig(configText);

      if (config == System.IntPtr.Zero)
      {
        Debug.Log("Failed to parse graph config");
        return;
      }

      lastStatus = new Status(MpCalculatorGraphInitialize(mpCalculatorGraph, config));
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void DebugProtobufLog(int level, string filename, int line, string message);
    private static readonly DebugProtobufLog debugProtobufLog = DebugProtobufLogImpl;
    private static System.IntPtr functionPointer;

    private static void DebugProtobufLogImpl(int level, string filename, int line, string message)
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

    public static void SetUpDebug()
    {
      if (functionPointer == System.IntPtr.Zero)
      {
        functionPointer = Marshal.GetFunctionPointerForDelegate(debugProtobufLog);
      }

      SetProtobufLogHandler(functionPointer);
    }

    #region Externs

    // CalculatorGraph API
    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpCalculatorGraph MpCalculatorGraphCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpCalculatorGraphDestroy(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe System.IntPtr SetProtobufLogHandler([MarshalAs(UnmanagedType.FunctionPtr)]System.IntPtr debugCal);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpCalculatorGraphConfig ParseMpCalculatorGraphConfig(string input);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphInitialize(MpCalculatorGraph graph, MpCalculatorGraphConfig config);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphStartRun(MpCalculatorGraph graph, MpSidePacket sidePacket);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphWaitUntilDone(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatusOrPoller MpCalculatorGraphAddOutputStreamPoller(MpCalculatorGraph graph, string name);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphAddStringPacketToInputStream(MpCalculatorGraph graph, string name, string packet, int timestamp);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphCloseInputStream(MpCalculatorGraph graph, string name);

    #endregion
  }
}
