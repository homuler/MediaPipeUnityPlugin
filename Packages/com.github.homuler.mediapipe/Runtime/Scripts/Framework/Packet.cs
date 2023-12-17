// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class Packet : MpResourceHandle
  {
    private Packet(IntPtr ptr, bool isOwner) : base(ptr, isOwner) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_Packet__delete(ptr);
    }

    public long TimestampMicroseconds()
    {
      var value = SafeNativeMethods.mp_Packet__TimestampMicroseconds(mpPtr);
      GC.KeepAlive(this);

      return value;
    }

    internal static Packet CreateEmpty()
    {
      UnsafeNativeMethods.mp_Packet__(out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Low-level API to reference the packet that <paramref name="ptr" /> points to.
    /// </summary>
    /// <remarks>
    ///   This method is to be used when you want to reference the packet whose lifetime is managed by native code.
    /// </remarks>
    /// <param name="ptr">
    ///   A pointer to a native Packet instance.
    /// </param>
    public static Packet CreateForReference(IntPtr ptr) => new Packet(ptr, false);

    public static Packet CreateBool(bool value)
    {
      UnsafeNativeMethods.mp__MakeBoolPacket__b(value, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    public static Packet CreateBoolAt(bool value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeBoolPacket_At__b_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a boolean.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown. 
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain bool data.
    /// </exception>
    public bool GetBool()
    {
      UnsafeNativeMethods.mp_Packet__GetBool(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a boolean.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain bool data.
    /// </exception>
    public void ValidateAsBool()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsBool(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
