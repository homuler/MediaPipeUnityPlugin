// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

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

    /// <summary>
    ///   Create a bool Packet.
    /// </summary>
    public static Packet CreateBool(bool value)
    {
      UnsafeNativeMethods.mp__MakeBoolPacket__b(value, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a bool Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateBoolAt(bool value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeBoolPacket_At__b_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a bool vector Packet.
    /// </summary>
    public static Packet CreateBoolVector(bool[] value)
    {
      UnsafeNativeMethods.mp__MakeBoolVectorPacket__Pb_i(value, value.Length, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a bool vector Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateBoolVectorAt(bool[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeBoolVectorPacket_At__Pb_i_ll(value, value.Length, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a double Packet.
    /// </summary>
    public static Packet CreateDouble(double value)
    {
      UnsafeNativeMethods.mp__MakeDoublePacket__d(value, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a double Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateDoubleAt(double value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeDoublePacket_At__d_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a float Packet.
    /// </summary>
    public static Packet CreateFloat(float value)
    {
      UnsafeNativeMethods.mp__MakeFloatPacket__f(value, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a float Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateFloatAt(float value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeFloatPacket_At__f_ll(value, timestampMicrosec, out var ptr).Assert();

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
    ///   Get the content of a bool vector Packet as a <see cref="List{bool}"/>.
    /// </summary>
    public List<bool> GetBoolList()
    {
      var value = new List<bool>();
      GetBoolList(value);

      return value;
    }

    /// <summary>
    ///   Get the content of a bool vector Packet as a <see cref="List{bool}"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="List{bool}"/> to be filled with the content of the <see cref="Packet"/>.
    /// </param>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;bool&gt; data.
    /// </exception>
    public void GetBoolList(List<bool> value)
    {
      UnsafeNativeMethods.mp_Packet__GetBoolVector(mpPtr, out var structArray).Assert();
      GC.KeepAlive(this);

      structArray.CopyTo(value);
      structArray.Dispose();
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a double.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain double data.
    /// </exception>
    public double GetDouble()
    {
      UnsafeNativeMethods.mp_Packet__GetDouble(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a float.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain float data.
    /// </exception>
    public float GetFloat()
    {
      UnsafeNativeMethods.mp_Packet__GetFloat(mpPtr, out var value).Assert();

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

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a std::vector&lt;bool&gt;.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;bool&gt;.
    /// </exception>
    public void ValidateAsBoolVector()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsBoolVector(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a double.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain double data;.
    /// </exception>
    public void ValidateAsDouble()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsDouble(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a float.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain float data;.
    /// </exception>
    public void ValidateAsFloat()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloat(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
