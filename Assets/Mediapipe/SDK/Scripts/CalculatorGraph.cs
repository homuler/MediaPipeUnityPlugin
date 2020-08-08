using System;

using MpPacket = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class CalculatorGraph : ResourceHandle {
    private bool _disposed = false;
    private CalculatorGraphConfig graphConfig;

    public CalculatorGraph(string configText) : base(UnsafeNativeMethods.MpCalculatorGraphCreate()) {
      graphConfig = new CalculatorGraphConfig(configText);

      var status = Initialize(graphConfig);

      if (!status.IsOk()) {
        throw new System.SystemException(status.ToString());
      }
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      if (OwnsResource()) {
        UnsafeNativeMethods.MpCalculatorGraphDestroy(ptr);
      }

      ptr = IntPtr.Zero;
      graphConfig = null;

      _disposed = true;
    }

    public Status StartRun(SidePacket sidePacket) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphStartRun(ptr, sidePacket.GetPtr()));
    }

    public Status WaitUntilDone() {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphWaitUntilDone(ptr));
    }

    public Status CloseInputStream(string name) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphCloseInputStream(ptr, name));
    }

    private Status Initialize(CalculatorGraphConfig config) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphInitialize(ptr, config.GetPtr()));
    }

    protected MpStatusOrPoller AddOutputStreamPoller(string name) {
      return UnsafeNativeMethods.MpCalculatorGraphAddOutputStreamPoller(ptr, name);
    }

    protected Status AddPacketToInputStream(string name, MpPacket packetPtr) {
      return new Status(UnsafeNativeMethods.MpCalculatorGraphAddPacketToInputStream(ptr, name, packetPtr));
    }
  }
}
