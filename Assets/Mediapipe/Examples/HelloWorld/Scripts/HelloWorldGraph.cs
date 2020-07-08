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

    public HelloWorldGraph() {
      mpCalculatorGraph = MpCalculatorGraphCreate();

      Initialize();
    }

    ~HelloWorldGraph() {
      MpCalculatorGraphDestroy(mpCalculatorGraph);
    }

    public void StartRun() {
      var outputStreamPollerOrStatus = new StatusOrPoller(MpCalculatorGraphAddOutputStreamPoller(mpCalculatorGraph, "out"));

      if (!outputStreamPollerOrStatus.IsOk()) {
        // TODO: ステータスの出力
        Debug.Log("Failed to add output stream: out");
        // StatusをBuildして返す
      }

      outputStreamPoller = outputStreamPollerOrStatus.GetPoller();

      var sidePacket = new SidePacket();
      lastStatus = new Status(MpCalculatorGraphStartRun(mpCalculatorGraph, sidePacket.GetPtr()));
    }

    public Status AddStringToInputStream(string text, int timestamp) {
      return new Status(MpCalculatorGraphAddStringPacketToInputStream(mpCalculatorGraph, "in", text, timestamp));
    }

    public void CloseInputStream() {
      lastStatus = new Status(MpCalculatorGraphCloseInputStream(mpCalculatorGraph, "in"));
    }

    public void WaitUntilDone() {
      lastStatus = new Status(MpCalculatorGraphWaitUntilDone(mpCalculatorGraph));
    }

    public bool HasNextPacket() {
      return outputStreamPoller.HasNextPacket();
    }

    public string GetPacketValue() {
      return outputStreamPoller.GetPacketValue();
    }

    public bool IsOk() {
      return lastStatus == null ? true : lastStatus.IsOk();
    }

    public Status GetLastStatus() {
      return lastStatus;
    }

    private void Initialize() {
      var config = ParseMpCalculatorGraphConfigOrDie(configText);

      lastStatus = new Status(MpCalculatorGraphInitialize(mpCalculatorGraph, config));
    }

    #region Externs

    // CalculatorGraph API
    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpCalculatorGraph MpCalculatorGraphCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpCalculatorGraphDestroy(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpCalculatorGraphConfig ParseMpCalculatorGraphConfigOrDie(string input);

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
