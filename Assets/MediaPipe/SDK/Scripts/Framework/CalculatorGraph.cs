using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public class CalculatorGraph : MpResourceHandle {
    public delegate IntPtr NativePacketCallback(IntPtr packetPtr);
    public delegate Status PacketCallback<T, U>(T packet) where T : Packet<U>;

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

    public Status ObserveOutputStream<T, U>(string streamName, PacketCallback<T, U> packetCallback, out GCHandle callbackHandle) where T : Packet<U> {
      NativePacketCallback nativePacketCallback = (IntPtr packetPtr) => {
        Status status = null;
        try {
          T packet = (T)Activator.CreateInstance(typeof(T), packetPtr, false);
          status = packetCallback(packet);
        } catch (Exception e) {
          status = Status.FailedPrecondition(e.ToString());
        }
        return status.mpPtr;
      };
      callbackHandle = GCHandle.Alloc(nativePacketCallback, GCHandleType.Pinned);

      UnsafeNativeMethods.mp_CalculatorGraph__ObserveOutputStream__PKc_PF(mpPtr, streamName, nativePacketCallback, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public StatusOrPoller<T> AddOutputStreamPoller<T>(string streamName) {
      UnsafeNativeMethods.mp_CalculatorGraph__AddOutputStreamPoller__PKc(mpPtr, streamName, out var statusOrPollerPtr).Assert();

      GC.KeepAlive(this);
      return new StatusOrPoller<T>(statusOrPollerPtr);
    }

    public Status Run() {
      return Run(new SidePacket());
    }

    public Status Run(SidePacket sidePacket) {
      UnsafeNativeMethods.mp_CalculatorGraph__Run__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status StartRun() {
      return StartRun(new SidePacket());
    }

    public Status StartRun(SidePacket sidePacket) {
      UnsafeNativeMethods.mp_CalculatorGraph__StartRun__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status WaitUntilIdle() {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilIdle(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status WaitUntilDone() {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilDone(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public bool HasError() {
      return SafeNativeMethods.mp_CalculatorGraph__HasError(mpPtr);
    }

    public Status AddPacketToInputStream<T>(string streamName, Packet<T> packet) {
      UnsafeNativeMethods.mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mpPtr, streamName, packet.mpPtr, out var statusPtr).Assert();
      packet.Dispose(); // respect move semantics

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status SetInputStreamMaxQueueSize(string streamName, int maxQueueSize) {
      UnsafeNativeMethods.mp_CalculatorGraph__SetInputStreamMaxQueueSize__PKc_i(mpPtr, streamName, maxQueueSize, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status CloseInputStream(string streamName) {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseInputStream__PKc(mpPtr, streamName, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status CloseAllPacketSources() {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseAllPacketSources(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public void Cancel() {
      UnsafeNativeMethods.mp_CalculatorGraph__Cancel(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public bool GraphInputStreamsClosed() {
      return SafeNativeMethods.mp_CalculatorGraph__GraphInputStreamsClosed(mpPtr);
    }

    public bool IsNodeThrottled(int nodeId) {
      return SafeNativeMethods.mp_CalculatorGraph__IsNodeThrottled__i(mpPtr, nodeId);
    }

    public bool UnthrottleSources() {
      return SafeNativeMethods.mp_CalculatorGraph__UnthrottleSources(mpPtr);
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
