// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class SidePacket : MpResourceHandle
  {
    public SidePacket() : base()
    {
      UnsafeNativeMethods.mp_SidePacket__(out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_SidePacket__delete(ptr);
    }

    public int size => SafeNativeMethods.mp_SidePacket__size(mpPtr);

    /// <remarks>
    ///   This method cannot verify that the packet type corresponding to the <paramref name="key" /> is indeed a <typeparamref name="TPacket" />,
    ///   so you must make sure by youreself that it is.
    /// </remarks>
    public TPacket At<TPacket, TValue>(string key) where TPacket : Packet<TValue>, new()
    {
      UnsafeNativeMethods.mp_SidePacket__at__PKc(mpPtr, key, out var packetPtr).Assert();

      if (packetPtr == IntPtr.Zero)
      {
        return default; // null
      }
      GC.KeepAlive(this);
      return Packet<TValue>.Create<TPacket>(packetPtr, true);
    }

    public void Emplace<T>(string key, Packet<T> packet)
    {
      UnsafeNativeMethods.mp_SidePacket__emplace__PKc_Rp(mpPtr, key, packet.mpPtr).Assert();
      packet.Dispose(); // respect move semantics
      GC.KeepAlive(this);
    }

    public int Erase(string key)
    {
      UnsafeNativeMethods.mp_SidePacket__erase__PKc(mpPtr, key, out var count).Assert();

      GC.KeepAlive(this);
      return count;
    }

    public void Clear()
    {
      SafeNativeMethods.mp_SidePacket__clear(mpPtr);
    }
  }
}
