using System;

namespace Mediapipe {
  public class GpuBufferPacket : Packet<GpuBuffer> {
    public GpuBufferPacket() : base() {}

    public GpuBufferPacket(GpuBuffer gpuBuffer, int timestamp) :
      base(UnsafeNativeMethods.MpMakeGpuBufferPacketAt(gpuBuffer.GetPtr(), timestamp), gpuBuffer) {}

    public override GpuBuffer GetValue() {
      throw new NotSupportedException();
    }

    public override GpuBuffer ConsumeValue() {
      var gpuBuffer = new StatusOrGpuBuffer(UnsafeNativeMethods.MpPacketConsumeGpuBuffer(GetPtr())).ConsumeValue();

      ReleaseValue();

      return gpuBuffer;
    }

    public override IntPtr ReleasePtr() {
      ReleaseValue();
      return base.ReleasePtr();
    }

    private void ReleaseValue() {
      if (valueHandle.IsAllocated) {
        var gpuBuffer = (GpuBuffer)valueHandle.Target;
        gpuBuffer.ReleasePtr();
      }
    }
  }
}
