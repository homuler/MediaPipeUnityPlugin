namespace Mediapipe {
  public class GpuBufferPacket : Packet<GpuBuffer> {
    public GpuBufferPacket() : base() {}

    public GpuBufferPacket(GpuBuffer gpuBuffer, int timestamp) :
      base(UnsafeNativeMethods.MpMakeGpuBufferPacketAt(gpuBuffer.GetPtr(), timestamp), gpuBuffer) {}

    public override GpuBuffer GetValue() {
      throw new System.NotImplementedException();
    }

    public override GpuBuffer ConsumeValue() {
      var statusOrGpuBuffer = new StatusOrGpuBuffer(UnsafeNativeMethods.MpPacketConsumeGpuBuffer(GetPtr()));

      if (!statusOrGpuBuffer.IsOk()) {
        throw new System.SystemException(statusOrGpuBuffer.status.ToString());
      }

      ReleaseValue();

      return statusOrGpuBuffer.ConsumeValue();
    }

    public override void ReleasePtr() {
      ReleaseValue();
      base.ReleasePtr();
    }

    private void ReleaseValue() {
      if (valueHandle.IsAllocated) {
        var gpuBuffer = (GpuBuffer)valueHandle.Target;
        gpuBuffer.ReleasePtr();
      }
    }
  }
}
