// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Google.Protobuf;

namespace Mediapipe
{
  public class Packet : MpResourceHandle
  {
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
    ///   Create a float array Packet.
    /// </summary>
    public static Packet CreateFloatArray(float[] value)
    {
      UnsafeNativeMethods.mp__MakeFloatArrayPacket__Pf_i(value, value.Length, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a float array Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateFloatArrayAt(float[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeFloatArrayPacket_At__Pf_i_ll(value, value.Length, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a float vector Packet.
    /// </summary>
    public static Packet CreateFloatVector(float[] value)
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket__Pf_i(value, value.Length, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a float vector Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateFloatVectorAt(float[] value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket_At__Pf_i_ll(value, value.Length, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="Image"/> Packet.
    /// </summary>
    public static Packet CreateImage(Image value)
    {
      UnsafeNativeMethods.mp__MakeImagePacket__PI(value.mpPtr, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="Image"> Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateImageAt(Image value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeImagePacket_At__PI_ll(value.mpPtr, timestampMicrosec, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="ImageFrame"/> Packet.
    /// </summary>
    public static Packet CreateImageFrame(ImageFrame value)
    {
      UnsafeNativeMethods.mp__MakeImageFramePacket__Pif(value.mpPtr, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create an <see cref="ImageFrame"/> Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateImageFrameAt(ImageFrame value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeImageFramePacket_At__Pif_ll(value.mpPtr, timestampMicrosec, out var ptr).Assert();
      value.Dispose(); // respect move semantics

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create an int Packet.
    /// </summary>
    public static Packet CreateInt(int value)
    {
      UnsafeNativeMethods.mp__MakeIntPacket__i(value, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a int Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateIntAt(int value, long timestampMicrosec)
    {
      UnsafeNativeMethods.mp__MakeIntPacket_At__i_ll(value, timestampMicrosec, out var ptr).Assert();

      return new Packet(ptr, true);
    }

    /// <summary>
    ///   Create a MediaPipe protobuf message Packet.
    /// </summary>
    public static Packet CreateProto<T>(T value) where T : IMessage<T>
    {
      unsafe
      {
        var size = value.CalculateSize();
        var arr = stackalloc byte[size];
        value.WriteTo(new Span<byte>(arr, size));

        UnsafeNativeMethods.mp__PacketFromDynamicProto__PKc_PKc_i(value.Descriptor.FullName, arr, size, out var statusPtr, out var ptr).Assert();

        AssertStatusOk(statusPtr);
        return new Packet(ptr, true);
      }
    }

    /// <summary>
    ///   Create a MediaPipe protobuf message Packet.
    /// </summary>
    /// <param name="timestampMicrosec">
    ///   The timestamp of the packet.
    /// </param>
    public static Packet CreateProtoAt<T>(T value, long timestampMicrosec) where T : IMessage<T>
    {
      unsafe
      {
        var size = value.CalculateSize();
        var arr = stackalloc byte[size];
        value.WriteTo(new Span<byte>(arr, size));

        UnsafeNativeMethods.mp__PacketFromDynamicProto_At__PKc_PKc_i_ll(value.Descriptor.FullName, arr, size, timestampMicrosec, out var statusPtr, out var ptr).Assert();
        AssertStatusOk(statusPtr);

        return new Packet(ptr, true);
      }
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
    ///   Get the content of a float array Packet as a <see cref="float[]"/>.
    /// </summary>
    public float[] GetFloatArray(int length)
    {
      var value = new float[length];
      GetFloatArray(value);

      return value;
    }

    /// <summary>
    ///   Get the content of a float array Packet as a <see cref="float[]"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="float[]"/> to be filled with the content of the <see cref="Packet"/>.
    /// </param>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain a float array.
    /// </exception>
    public void GetFloatArray(float[] value)
    {
      UnsafeNativeMethods.mp_Packet__GetFloatArray_i(mpPtr, value.Length, out var arrayPtr).Assert();
      GC.KeepAlive(this);

      Marshal.Copy(arrayPtr, value, 0, value.Length);
      UnsafeNativeMethods.delete_array__Pf(arrayPtr);
    }

    /// <summary>
    ///   Get the content of a float vector Packet as a <see cref="List{float}"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;float&gt; data.
    /// </exception>
    public List<float> GetFloatList()
    {
      var value = new List<float>();
      GetFloatList(value);

      return value;
    }

    /// <summary>
    ///   Get the content of a float vector Packet as a <see cref="List{float}"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="List{bool}"/> to be filled with the content of the <see cref="Packet"/>.
    /// </param>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;float&gt; data.
    /// </exception>
    public void GetFloatList(List<float> value)
    {
      UnsafeNativeMethods.mp_Packet__GetFloatVector(mpPtr, out var structArray).Assert();
      GC.KeepAlive(this);

      structArray.CopyTo(value);
      structArray.Dispose();
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as an <see cref="Image"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="Image"/>.
    /// </exception>
    public Image GetImage()
    {
      UnsafeNativeMethods.mp_Packet__GetImage(mpPtr, out var ptr).Assert();

      GC.KeepAlive(this);
      return new Image(ptr, false);
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as an <see cref="ImageFrame"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="ImageFrame"/>.
    /// </exception>
    public ImageFrame GetImageFrame()
    {
      UnsafeNativeMethods.mp_Packet__GetImageFrame(mpPtr, out var ptr).Assert();

      GC.KeepAlive(this);
      return new ImageFrame(ptr, false);
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as an integer.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see langword="int"/> data.
    /// </exception>
    public int GetInt()
    {
      UnsafeNativeMethods.mp_Packet__GetInt(mpPtr, out var value).Assert();

      GC.KeepAlive(this);
      return value;
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a proto message.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain proto messages.
    /// </exception>
    public T GetProto<T>(MessageParser<T> parser) where T : IMessage<T>
    {
      UnsafeNativeMethods.mp_Packet__GetProtoMessageLite(mpPtr, out var value).Assert();

      GC.KeepAlive(this);

      var proto = value.Deserialize(parser);
      value.Dispose();

      return proto;
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a proto message list.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain a proto message list.
    /// </exception>
    public List<T> GetProtoList<T>(MessageParser<T> parser) where T : IMessage<T>
    {
      var value = new List<T>();
      GetProtoList(parser, value);

      return value;
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a proto message list.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="List{T}"/> to be filled with the content of the <see cref="Packet"/>.
    /// </param>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain a proto message list.
    /// </exception>
    public void GetProtoList<T>(MessageParser<T> parser, List<T> value) where T : IMessage<T>
    {
      UnsafeNativeMethods.mp_Packet__GetVectorOfProtoMessageLite(mpPtr, out var serializedProtoVector).Assert();

      GC.KeepAlive(this);

      serializedProtoVector.Deserialize(parser, value);
      serializedProtoVector.Dispose();
    }

    public void GetDetectionList(List<Detection> detections)
    {
      UnsafeNativeMethods.mp_Packet__GetVectorOfProtoMessageLite(mpPtr, out var serializedProtoVector).Assert();

      GC.KeepAlive(this);

      foreach (var detection in detections)
      {
        detection.Clear();
      }
      var size = serializedProtoVector.WriteTo(Detection.Parser, detections);
      serializedProtoVector.Dispose();

      detections.RemoveRange(size, detections.Count - size);
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
    ///   If the <see cref="Packet"/> doesn't contain double data.
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
    ///   If the <see cref="Packet"/> doesn't contain float data.
    /// </exception>
    public void ValidateAsFloat()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloat(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a float array.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain a float array.
    /// </exception>
    public void ValidateAsFloatArray()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloatArray(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is std::vector&lt;float&gt;.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;bool&gt;.
    /// </exception>
    public void ValidateAsFloatVector()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloatVector(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see cref="Image"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="Image"/>.
    /// </exception>
    public void ValidateAsImage()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsImage(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see cref="ImageFrame"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="ImageFrame"/>.
    /// </exception>
    public void ValidateAsImageFrame()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsImageFrame(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see langword="int"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see langword="int"/>.
    /// </exception>
    public void ValidateAsInt()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsInt(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a proto message.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain proto messages.
    /// </exception>
    public void ValidateAsProtoMessageLite()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsProtoMessageLite(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
