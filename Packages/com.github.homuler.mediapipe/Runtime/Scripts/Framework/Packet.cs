// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace Mediapipe
{
  public static class Packet
  {
    /// <summary>
    ///   Create a bool Packet.
    /// </summary>
    public static Packet<bool> CreateBool(bool value)
    {
      UnsafeNativeMethods.mp__MakeBoolPacket__b(value, out var ptr).Assert();

      return new Packet<bool>(ptr, true);
    }

    /// <summary>
    ///   Create a bool Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<bool> CreateBoolAt(bool value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeBoolPacket_At__b_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet<bool>(ptr, true);
    }

    /// <summary>
    ///   Create a bool vector Packet.
    /// </summary>
    public static Packet<List<bool>> CreateBoolVector(bool[] value)
    {
      UnsafeNativeMethods.mp__MakeBoolVectorPacket__Pb_i(value, value.Length, out var ptr).Assert();

      return new Packet<List<bool>>(ptr, true);
    }

    /// <summary>
    ///   Create a bool vector Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<List<bool>> CreateBoolVectorAt(bool[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeBoolVectorPacket_At__Pb_i_ll(value, value.Length, timestampMicrosec, out var ptr).Assert();

      return new Packet<List<bool>>(ptr, true);
    }

    /// <summary>
    ///   Create a double Packet.
    /// </summary>
    public static Packet<double> CreateDouble(double value)
    {
      UnsafeNativeMethods.mp__MakeDoublePacket__d(value, out var ptr).Assert();

      return new Packet<double>(ptr, true);
    }

    /// <summary>
    ///   Create a double Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<double> CreateDoubleAt(double value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeDoublePacket_At__d_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet<double>(ptr, true);
    }

    /// <summary>
    ///   Create a float Packet.
    /// </summary>
    public static Packet<float> CreateFloat(float value)
    {
      UnsafeNativeMethods.mp__MakeFloatPacket__f(value, out var ptr).Assert();

      return new Packet<float>(ptr, true);
    }

    /// <summary>
    ///   Create a float Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<float> CreateFloatAt(float value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeFloatPacket_At__f_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet<float>(ptr, true);
    }

    /// <summary>
    ///   Create a float array Packet.
    /// </summary>
    public static Packet<float[]> CreateFloatArray(float[] value)
    {
      UnsafeNativeMethods.mp__MakeFloatArrayPacket__Pf_i(value, value.Length, out var ptr).Assert();

      return new Packet<float[]>(ptr, true);
    }

    /// <summary>
    ///   Create a float array Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<float[]> CreateFloatArrayAt(float[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeFloatArrayPacket_At__Pf_i_ll(value, value.Length, timestampMicrosec, out var ptr).Assert();

      return new Packet<float[]>(ptr, true);
    }

    /// <summary>
    ///   Create a float vector Packet.
    /// </summary>
    public static Packet<List<float>> CreateFloatVector(float[] value)
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket__Pf_i(value, value.Length, out var ptr).Assert();

      return new Packet<List<float>>(ptr, true);
    }

    /// <summary>
    ///   Create a float vector Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<List<float>> CreateFloatVectorAt(float[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket_At__Pf_i_ll(value, value.Length, timestampMicrosec, out var ptr).Assert();

      return new Packet<List<float>>(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="GpuBuffer"/> Packet.
    /// </summary>
    public static Packet<GpuBuffer> CreateGpuBuffer(GpuBuffer value)
    {
      UnsafeNativeMethods.mp__MakeGpuBufferPacket__Rgb(value.mpPtr, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet<GpuBuffer>(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="GpuBuffer"> Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<GpuBuffer> CreateGpuBufferAt(GpuBuffer value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeGpuBufferPacket_At__Rgb_ll(value.mpPtr, timestampMicrosec, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet<GpuBuffer>(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="Image"/> Packet.
    /// </summary>
    public static Packet<Image> CreateImage(Image value)
    {
      UnsafeNativeMethods.mp__MakeImagePacket__PI(value.mpPtr, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet<Image>(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="Image"> Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<Image> CreateImageAt(Image value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeImagePacket_At__PI_ll(value.mpPtr, timestampMicrosec, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet<Image>(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="ImageFrame"/> Packet.
    /// </summary>
    public static Packet<ImageFrame> CreateImageFrame(ImageFrame value)
    {
      UnsafeNativeMethods.mp__MakeImageFramePacket__Pif(value.mpPtr, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet<ImageFrame>(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="ImageFrame"/> Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<ImageFrame> CreateImageFrameAt(ImageFrame value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeImageFramePacket_At__Pif_ll(value.mpPtr, timestampMicrosec, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet<ImageFrame>(ptr, true);
    }

    /// <summary>
    ///   Create an int Packet.
    /// </summary>
    public static Packet<int> CreateInt(int value)
    {
      UnsafeNativeMethods.mp__MakeIntPacket__i(value, out var ptr).Assert();

      return new Packet<int>(ptr, true);
    }

    /// <summary>
    ///   Create a int Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<int> CreateIntAt(int value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeIntPacket_At__i_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet<int>(ptr, true);
    }

    /// <summary>
    ///   Create a MediaPipe protobuf message Packet.
    /// </summary>
    public static Packet<TMessage> CreateProto<TMessage>(TMessage value) where TMessage : IMessage<TMessage>
    {
      unsafe
      {
        var size = value.CalculateSize();
        var arr = stackalloc byte[size];
        value.WriteTo(new Span<byte>(arr, size));

        UnsafeNativeMethods.mp__PacketFromDynamicProto__PKc_PKc_i(value.Descriptor.FullName, arr, size, out var statusPtr, out var ptr).Assert();

        Status.UnsafeAssertOk(statusPtr);
        return new Packet<TMessage>(ptr, true);
      }
    }

    /// <summary>
    ///   Create a MediaPipe protobuf message Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<TMessage> CreateProtoAt<TMessage>(TMessage value, long timestampMicrosec) where TMessage : IMessage<TMessage>
    {
      unsafe
      {
        var size = value.CalculateSize();
        var arr = stackalloc byte[size];
        value.WriteTo(new Span<byte>(arr, size));

        UnsafeNativeMethods.mp__PacketFromDynamicProto_At__PKc_PKc_i_ll(value.Descriptor.FullName, arr, size, timestampMicrosec, out var statusPtr, out var ptr).Assert();
        Status.UnsafeAssertOk(statusPtr);

        return new Packet<TMessage>(ptr, true);
      }
    }

    /// <summary>
    ///   Create a string Packet.
    /// </summary>
    public static Packet<string> CreateString(string value)
    {
      UnsafeNativeMethods.mp__MakeStringPacket__PKc(value ?? "", out var ptr).Assert();

      return new Packet<string>(ptr, true);
    }

    /// <summary>
    ///   Create a string Packet.
    /// </summary>
    public static Packet<string> CreateString(byte[] value)
    {
      UnsafeNativeMethods.mp__MakeStringPacket__PKc_i(value, value?.Length ?? 0, out var ptr).Assert();

      return new Packet<string>(ptr, true);
    }

    /// <summary>
    ///   Create a string Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<string> CreateStringAt(string value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeStringPacket_At__PKc_ll(value ?? "", timestampMicrosec, out var ptr).Assert();

      return new Packet<string>(ptr, true);
    }

    /// <summary>
    ///   Create a string Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet<string> CreateStringAt(byte[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeStringPacket_At__PKc_i_ll(value, value?.Length ?? 0, timestampMicrosec, out var ptr).Assert();

      return new Packet<string>(ptr, true);
    }
  }

  public partial class Packet<TValue> : MpResourceHandle
  {
    public Packet() : base(true)
    {
      UnsafeNativeMethods.mp_Packet__(out var ptr).Assert();
      this.ptr = ptr;
    }

    internal Packet(IntPtr ptr, bool isOwner) : base(ptr, isOwner) { }

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

    public bool IsEmpty() => SafeNativeMethods.mp_Packet__IsEmpty(mpPtr);

    internal void SwitchNativePtr(IntPtr packetPtr)
    {
      if (isOwner)
      {
        throw new InvalidOperationException("This operation is permitted only when the packet instance is for reference");
      }
      ptr = packetPtr;
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
    public static Packet<TValue> CreateForReference(IntPtr ptr) => new Packet<TValue>(ptr, false);
  }
}
