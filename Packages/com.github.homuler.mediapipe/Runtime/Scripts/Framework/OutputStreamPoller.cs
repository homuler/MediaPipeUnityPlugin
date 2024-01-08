// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class OutputStreamPoller<T> : MpResourceHandle
  {
    public OutputStreamPoller(IntPtr ptr) : base(ptr) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_OutputStreamPoller__delete(ptr);
    }

    public bool Next(Packet<T> packet)
    {
      UnsafeNativeMethods.mp_OutputStreamPoller__Next_Ppacket(mpPtr, packet.mpPtr, out var result).Assert();

      GC.KeepAlive(this);
      return result;
    }

    public void Reset()
    {
      UnsafeNativeMethods.mp_OutputStreamPoller__Reset(mpPtr).Assert();
    }

    public void SetMaxQueueSize(int queueSize)
    {
      UnsafeNativeMethods.mp_OutputStreamPoller__SetMaxQueueSize(mpPtr, queueSize).Assert();
    }

    public int QueueSize()
    {
      UnsafeNativeMethods.mp_OutputStreamPoller__QueueSize(mpPtr, out var result).Assert();

      GC.KeepAlive(this);
      return result;
    }
  }
}
