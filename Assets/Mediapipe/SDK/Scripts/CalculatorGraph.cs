using System.Runtime.InteropServices;

using MpCalculatorGraph = System.IntPtr;
using MpCalculatorGraphConfig = System.IntPtr;
using MpPacket = System.IntPtr;
using MpSidePacket = System.IntPtr;
using MpStatus = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe
{
  public class CalculatorGraph
  {
    private const string MediapipeLibrary = "mediapipe_c";

    private CalculatorGraphConfig graphConfig;
    public MpCalculatorGraph mpCalculatorGraph;

    public CalculatorGraph(string configText)
    {
      graphConfig = new CalculatorGraphConfig(configText);
      mpCalculatorGraph = MpCalculatorGraphCreate();

      var status = Initialize(graphConfig);

      if (!status.IsOk())
      {
        throw new System.SystemException(status.ToString());
      }
    }

    ~CalculatorGraph() {
      MpCalculatorGraphDestroy(mpCalculatorGraph);
    }

    public Status StartRun(SidePacket sidePacket)
    {
      return new Status(MpCalculatorGraphStartRun(mpCalculatorGraph, sidePacket.GetPtr()));
    }

    public Status WaitUntilDone()
    {
      return new Status(MpCalculatorGraphWaitUntilDone(mpCalculatorGraph));
    }

    public StatusOrPoller AddOutputStreamPoller(string name)
    {
      return new StatusOrPoller(MpCalculatorGraphAddOutputStreamPoller(mpCalculatorGraph, name));
    }

    public Status AddPacketToInputStream(string name, Packet packet)
    {
      return new Status(MpCalculatorGraphAddPacketToInputStream(mpCalculatorGraph, name, packet.GetPtr()));
    }

    public Status CloseInputStream(string name)
    {
      return new Status(MpCalculatorGraphCloseInputStream(mpCalculatorGraph, name));
    }

    private Status Initialize(CalculatorGraphConfig config)
    {
      return new Status(MpCalculatorGraphInitialize(mpCalculatorGraph, config.GetPtr()));
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpCalculatorGraph MpCalculatorGraphCreate();

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpCalculatorGraphDestroy(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphInitialize(MpCalculatorGraph graph, MpCalculatorGraphConfig config);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphStartRun(MpCalculatorGraph graph, MpSidePacket sidePacket);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphWaitUntilDone(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatusOrPoller MpCalculatorGraphAddOutputStreamPoller(MpCalculatorGraph graph, string name);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphAddPacketToInputStream(MpCalculatorGraph graph, string name, MpPacket packet);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpCalculatorGraphCloseInputStream(MpCalculatorGraph graph, string name);

    #endregion
  }
}
