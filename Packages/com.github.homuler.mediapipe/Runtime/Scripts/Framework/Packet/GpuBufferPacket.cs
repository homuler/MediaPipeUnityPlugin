// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class GpuBufferPacket : Packet<GpuBuffer>
  {
    public GpuBufferPacket() : base() { }
    public GpuBufferPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public GpuBufferPacket(GpuBuffer gpuBuffer) : base()
    {
      UnsafeNativeMethods.mp__MakeGpuBufferPacket__Rgb(gpuBuffer.mpPtr, out var ptr).Assert();
      gpuBuffer.Dispose(); // respect move semantics

      this.ptr = ptr;
    }

    public GpuBufferPacket(GpuBuffer gpuBuffer, Timestamp timestamp)
    {
      UnsafeNativeMethods.mp__MakeGpuBufferPacket_At__Rgb_Rts(gpuBuffer.mpPtr, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      gpuBuffer.Dispose(); // respect move semantics

      this.ptr = ptr;
    }

    public override GpuBuffer Get()
    {
      UnsafeNativeMethods.mp_Packet__GetGpuBuffer(mpPtr, out var gpuBufferPtr).Assert();

      GC.KeepAlive(this);
      return new GpuBuffer(gpuBufferPtr, false);
    }

    public override StatusOr<GpuBuffer> Consume()
    {
      UnsafeNativeMethods.mp_Packet__ConsumeGpuBuffer(mpPtr, out var statusOrGpuBufferPtr).Assert();

      GC.KeepAlive(this);
      return new StatusOrGpuBuffer(statusOrGpuBufferPtr);
    }

    public override Status ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsGpuBuffer(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
