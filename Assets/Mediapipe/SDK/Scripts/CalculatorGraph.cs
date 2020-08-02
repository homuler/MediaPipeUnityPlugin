using MpCalculatorGraph = System.IntPtr;
using MpPacket = System.IntPtr;
using MpStatus = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class CalculatorGraph {
    private CalculatorGraphConfig graphConfig;
    public MpCalculatorGraph mpCalculatorGraph;

    public CalculatorGraph(string configText) {
      graphConfig = new CalculatorGraphConfig(configText);
      mpCalculatorGraph = UnsafeNativeMethods.MpCalculatorGraphCreate();

      var status = Initialize(graphConfig);

      if (!status.IsOk()) {
        throw new System.SystemException(status.ToString());
      }
    }

    ~CalculatorGraph() {
      UnsafeNativeMethods.MpCalculatorGraphDestroy(mpCalculatorGraph);
    }

    public Status StartRun(SidePacket sidePacket) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphStartRun(mpCalculatorGraph, sidePacket.GetPtr()));
    }

    public Status WaitUntilDone() {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphWaitUntilDone(mpCalculatorGraph));
    }

    public Status CloseInputStream(string name) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphCloseInputStream(mpCalculatorGraph, name));
    }

    private Status Initialize(CalculatorGraphConfig config) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphInitialize(mpCalculatorGraph, config.GetPtr()));
    }

    protected MpStatusOrPoller AddOutputStreamPoller(string name) {
      return UnsafeNativeMethods.MpCalculatorGraphAddOutputStreamPoller(mpCalculatorGraph, name);
    }

    protected Status AddPacketToInputStream(string name, MpPacket packetPtr) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphAddPacketToInputStream(mpCalculatorGraph, name, packetPtr));
    }
  }
}
