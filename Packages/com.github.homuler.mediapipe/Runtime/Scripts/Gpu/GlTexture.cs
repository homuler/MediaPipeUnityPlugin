// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class GlTexture : MpResourceHandle
  {
    public GlTexture() : base()
    {
      UnsafeNativeMethods.mp_GlTexture__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public GlTexture(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_GlTexture__delete(ptr);
    }

    public int width => SafeNativeMethods.mp_GlTexture__width(mpPtr);

    public int height => SafeNativeMethods.mp_GlTexture__height(mpPtr);

    public uint target => SafeNativeMethods.mp_GlTexture__target(mpPtr);

    public uint name => SafeNativeMethods.mp_GlTexture__name(mpPtr);

    public void Release()
    {
      UnsafeNativeMethods.mp_GlTexture__Release(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public GpuBuffer GetGpuBufferFrame()
    {
      UnsafeNativeMethods.mp_GlTexture__GetGpuBufferFrame(mpPtr, out var gpuBufferPtr).Assert();

      GC.KeepAlive(this);
      return new GpuBuffer(gpuBufferPtr);
    }
  }
}
