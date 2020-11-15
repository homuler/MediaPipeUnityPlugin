using System;

namespace Mediapipe {
  public class CalculatorGraph : MpResourceHandle {
    public CalculatorGraph() : base() {
      UnsafeNativeMethods.mp_CalculatorGraph__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public CalculatorGraph(string configText) : base() {
      var config = CalculatorGraphConfig.ParseFromString(configText);
      UnsafeNativeMethods.mp_CalculatorGraph__Rconfig(config.mpPtr, out var ptr).Assert();
      GC.KeepAlive(config);
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_CalculatorGraph__delete(ptr);
    }

    public Status Initialize(CalculatorGraphConfig config) {
      UnsafeNativeMethods.mp_CalculatorGraph__Initialize__Rconfig(mpPtr, config.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status Initialize(CalculatorGraphConfig config, SidePacket sidePacket) {
      UnsafeNativeMethods.mp_CalculatorGraph__Initialize__Rconfig_Rsp(mpPtr, config.mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    /// <remarks>Crashes if config is not set</remarks>
    public CalculatorGraphConfig config {
      get {
        UnsafeNativeMethods.mp_CalculatorGraph__Config(mpPtr, out var configPtr).Assert();

        GC.KeepAlive(this);
        return new CalculatorGraphConfig(configPtr);
      }
    }

    public Status StartRun() {
      return StartRun(new SidePacket());
    }

    public Status StartRun(SidePacket sidePacket) {
      UnsafeNativeMethods.mp_CalculatorGraph__StartRun__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status WaitUntilDone() {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilDone(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status CloseInputStream(string name) {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseInputStream__PKc(mpPtr, name, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public StatusOrPoller<T> AddOutputStreamPoller<T>(string name) {
      UnsafeNativeMethods.mp_CalculatorGraph__AddOutputStreamPoller__PKc(mpPtr, name, out var statusOrPollerPtr).Assert();

      GC.KeepAlive(this);
      return new StatusOrPoller<T>(statusOrPollerPtr);
    }

    public Status AddPacketToInputStream<T>(string name, Packet<T> packet) {
      UnsafeNativeMethods.mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mpPtr, name, packet.mpPtr, out var statusPtr).Assert();
      packet.Dispose(); // respect move semantics

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public GpuResources GetGpuResources() {
      UnsafeNativeMethods.mp_CalculatorGraph__GetGpuResources(mpPtr, out var gpuResourcesPtr).Assert();

      GC.KeepAlive(this);
      return new GpuResources(gpuResourcesPtr);
    }

    public Status SetGpuResources(GpuResources gpuResources) {
      UnsafeNativeMethods.mp_CalculatorGraph__SetGpuResources__SPgpu(mpPtr, gpuResources.sharedPtr, out var statusPtr).Assert();

      GC.KeepAlive(gpuResources);
      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
