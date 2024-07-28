// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class PacketMap : MpResourceHandle
  {
    public PacketMap() : base()
    {
      UnsafeNativeMethods.mp_PacketMap__(out var ptr).Assert();
      this.ptr = ptr;
    }

    // TODO: make this constructor internal
    public PacketMap(IntPtr ptr, bool isOwner) : base(ptr, isOwner) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_PacketMap__delete(ptr);
    }

    public int size => SafeNativeMethods.mp_PacketMap__size(mpPtr);

    /// <remarks>
    ///   This method cannot verify that the packet type corresponding to the <paramref name="key" /> is indeed a <typeparamref name="T" />,
    ///   so you must make sure by youreself that it is.
    /// </remarks>
    public Packet<T> At<T>(string key)
    {
      UnsafeNativeMethods.mp_PacketMap__find__PKc(mpPtr, key, out var packetPtr).Assert();

      if (packetPtr == IntPtr.Zero)
      {
        return default; // null
      }
      GC.KeepAlive(this);
      return new Packet<T>(packetPtr, true);
    }

    /// <remarks>
    ///   This method cannot verify that the packet type corresponding to the <paramref name="key" /> is indeed a <typeparamref name="T" />,
    ///   so you must make sure by youreself that it is.
    /// </remarks>
    public bool TryGet<T>(string key, out Packet<T> packet)
    {
      UnsafeNativeMethods.mp_PacketMap__find__PKc(mpPtr, key, out var packetPtr).Assert();

      if (packetPtr == IntPtr.Zero)
      {
        packet = default; // null
        return false;
      }
      GC.KeepAlive(this);
      packet = new Packet<T>(packetPtr, true);
      return true;
    }

    public void Emplace<T>(string key, Packet<T> packet)
    {
      UnsafeNativeMethods.mp_PacketMap__emplace__PKc_Rp(mpPtr, key, packet.mpPtr).Assert();
      packet.Dispose(); // respect move semantics
      GC.KeepAlive(this);
    }

    public int Erase(string key)
    {
      UnsafeNativeMethods.mp_PacketMap__erase__PKc(mpPtr, key, out var count).Assert();

      GC.KeepAlive(this);
      return count;
    }

    public void Clear()
    {
      SafeNativeMethods.mp_PacketMap__clear(mpPtr);
    }
  }
}
