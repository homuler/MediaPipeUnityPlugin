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
  public static class PacketGetterExtension
  {
    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a boolean.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain bool data.
    /// </exception>
    public static bool Get(this Packet<bool> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetBool(packet.mpPtr, out var value).Assert();
      GC.KeepAlive(packet);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static bool GetBool(this Packet<bool> packet) => Get(packet);

    /// <summary>
    ///   Get the content of a bool vector Packet as a <see cref="List{bool}"/>.
    /// </summary>
    public static List<bool> Get(this Packet<List<bool>> packet)
    {
      var value = new List<bool>();
      Get(packet, value);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static List<bool> GetBoolList(this Packet<List<bool>> packet) => Get(packet);

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
    public static void Get(this Packet<List<bool>> packet, List<bool> value)
    {
      UnsafeNativeMethods.mp_Packet__GetBoolVector(packet.mpPtr, out var structArray).Assert();
      GC.KeepAlive(packet);

      structArray.CopyTo(value);
      structArray.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetBoolList(this Packet<List<bool>> packet, List<bool> value) => Get(packet, value);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as <see cref="byte[]"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="string"/> data.
    /// </exception>
    public static byte[] GetBytes(this Packet<string> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetByteString(packet.mpPtr, out var strPtr, out var size).Assert();
      GC.KeepAlive(packet);

      var bytes = new byte[size];
      Marshal.Copy(strPtr, bytes, 0, size);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      return bytes;
    }

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as <see cref="byte[]"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="byte[]"/> to be filled with the content of the <see cref="Packet"/>.
    ///   If the length of <paramref name="value"/> is not enough to store the content of the <see cref="Packet"/>,
    ///   the rest of the content will be discarded.
    /// </param>
    /// <returns>
    ///   The number of written elements in <paramref name="value" />.
    /// </returns>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="string"/> data.
    /// </exception>
    public static int GetBytes(this Packet<string> packet, byte[] value)
    {
      UnsafeNativeMethods.mp_Packet__GetByteString(packet.mpPtr, out var strPtr, out var size).Assert();
      GC.KeepAlive(packet);

      var length = Math.Min(size, value.Length);
      Marshal.Copy(strPtr, value, 0, length);
      UnsafeNativeMethods.delete_array__PKc(strPtr);

      return length;
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
    public static double Get(this Packet<double> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetDouble(packet.mpPtr, out var value).Assert();
      GC.KeepAlive(packet);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static double GetDouble(this Packet<double> packet) => Get(packet);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a float.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain float data.
    /// </exception>
    public static float Get(this Packet<float> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetFloat(packet.mpPtr, out var value).Assert();
      GC.KeepAlive(packet);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static float GetFloat(this Packet<float> packet) => Get(packet);

    /// <summary>
    ///   Get the content of a float array Packet as a <see cref="float[]"/>.
    /// </summary>
    public static float[] Get(this Packet<float[]> packet, int length)
    {
      var value = new float[length];
      Get(packet, value);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static float[] GetFloatArray(this Packet<float[]> packet, int length) => Get(packet, length);

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
    public static void Get(this Packet<float[]> packet, float[] value)
    {
      UnsafeNativeMethods.mp_Packet__GetFloatArray_i(packet.mpPtr, value.Length, out var arrayPtr).Assert();
      GC.KeepAlive(packet);

      Marshal.Copy(arrayPtr, value, 0, value.Length);
      UnsafeNativeMethods.delete_array__Pf(arrayPtr);
    }

    [Obsolete("Use Get instead")]
    public static void GetFloatArray(this Packet<float[]> packet, float[] value) => Get(packet, value);

    /// <summary>
    ///   Get the content of a float vector Packet as a <see cref="List{float}"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;float&gt; data.
    /// </exception>
    public static List<float> Get(this Packet<List<float>> packet)
    {
      var value = new List<float>();
      Get(packet, value);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static List<float> GetFloatList(this Packet<List<float>> packet) => Get(packet);

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
    public static void Get(this Packet<List<float>> packet, List<float> value)
    {
      UnsafeNativeMethods.mp_Packet__GetFloatVector(packet.mpPtr, out var structArray).Assert();
      GC.KeepAlive(packet);

      structArray.CopyTo(value);
      structArray.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetFloatList(this Packet<List<float>> packet, List<float> value) => Get(packet, value);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as an <see cref="Image"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="Image"/>.
    /// </exception>
    public static Image Get(this Packet<Image> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetImage(packet.mpPtr, out var ptr).Assert();
      GC.KeepAlive(packet);

      return new Image(ptr, false);
    }

    [Obsolete("Use Get instead")]
    public static Image GetImage(this Packet<Image> packet) => Get(packet);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a list of <see cref="Image"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;Image&gt;.
    /// </exception>
    public static List<Image> Get(this Packet<List<Image>> packet)
    {
      var value = new List<Image>();

      Get(packet, value);
      return value;
    }

    [Obsolete("Use Get instead")]
    public static List<Image> GetImageList(this Packet<List<Image>> packet) => Get(packet);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a list of <see cref="Image"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="List{Image}"/> to be filled with the content of the <see cref="Packet"/>.
    /// </param>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;Image&gt;.
    /// </exception>
    public static void Get(this Packet<List<Image>> packet, List<Image> value)
    {
      UnsafeNativeMethods.mp_Packet__GetImageVector(packet.mpPtr, out var imageArray).Assert();
      GC.KeepAlive(packet);

      foreach (var image in value)
      {
        image.Dispose();
      }
      value.Clear();

      foreach (var imagePtr in imageArray.AsReadOnlySpan())
      {
        value.Add(new Image(imagePtr, false));
      }
    }

    [Obsolete("Use Get instead")]
    public static void GetImageList(this Packet<List<Image>> packet, List<Image> value) => Get(packet, value);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as an <see cref="ImageFrame"/>.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="ImageFrame"/>.
    /// </exception>
    public static ImageFrame Get(this Packet<ImageFrame> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetImageFrame(packet.mpPtr, out var ptr).Assert();
      GC.KeepAlive(packet);

      return new ImageFrame(ptr, false);
    }

    [Obsolete("Use Get instead")]
    public static ImageFrame GetImageFrame(this Packet<ImageFrame> packet) => Get(packet);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as an integer.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain <see langword="int"/> data.
    /// </exception>
    public static int Get(this Packet<int> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetInt(packet.mpPtr, out var value).Assert();
      GC.KeepAlive(packet);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static int GetInt(this Packet<int> packet) => Get(packet);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a proto message.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain proto messages.
    /// </exception>
    public static T Get<T>(this Packet<T> packet, MessageParser<T> parser) where T : IMessage<T>
    {
      UnsafeNativeMethods.mp_Packet__GetProtoMessageLite(packet.mpPtr, out var value).Assert();
      GC.KeepAlive(packet);

      var proto = value.Deserialize(parser);
      value.Dispose();

      return proto;
    }

    [Obsolete("Use Get instead")]
    public static T GetProto<T>(this Packet<T> packet, MessageParser<T> parser) where T : IMessage<T> => Get(packet, parser);

    /// <summary>
    ///   Get the content of the <see cref="Packet"/> as a proto message list.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain a proto message list.
    /// </exception>
    public static List<T> Get<T>(this Packet<List<T>> packet, MessageParser<T> parser) where T : IMessage<T>
    {
      var value = new List<T>();
      Get(packet, parser, value);

      return value;
    }

    [Obsolete("Use Get instead")]
    public static List<T> GetProtoList<T>(this Packet<List<T>> packet, MessageParser<T> parser) where T : IMessage<T> => Get(packet, parser);

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
    public static void Get<T>(this Packet<List<T>> packet, MessageParser<T> parser, List<T> value) where T : IMessage<T>
    {
      UnsafeNativeMethods.mp_Packet__GetVectorOfProtoMessageLite(packet.mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(packet);

      serializedProtoVector.Deserialize(parser, value);
      serializedProtoVector.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetProtoList<T>(this Packet<List<T>> packet, MessageParser<T> parser, List<T> value) where T : IMessage<T> => Get(packet, parser, value);

    /// <summary>
    ///   Write the content of the <see cref="Packet"/> as a proto message list.
    /// </summary>
    /// <remarks>
    ///   On some platforms (e.g. Windows), it will abort the process when <see cref="MediaPipeException"/> should be thrown.
    /// </remarks>
    /// <param name="value">
    ///   The <see cref="List{T}"/> to be filled with the content of the <see cref="Packet"/>.
    /// </param>
    /// <returns>
    ///   The number of written elements in <paramref name="value" />.
    /// </returns>
    /// <exception cref="MediaPipeException">
    ///   If the <see cref="Packet"/> doesn't contain a proto message list.
    /// </exception>
    public static int WriteTo<T>(this Packet<List<T>> packet, MessageParser<T> parser, List<T> value) where T : IMessage<T>
    {
      UnsafeNativeMethods.mp_Packet__GetVectorOfProtoMessageLite(packet.mpPtr, out var serializedProtoVector).Assert();
      GC.KeepAlive(packet);

      var size = serializedProtoVector.WriteTo(parser, value);
      serializedProtoVector.Dispose();

      return size;
    }

    [Obsolete("Use WriteTo instead")]
    public static int WriteProtoListTo<T>(this Packet<List<T>> packet, MessageParser<T> parser, List<T> value) where T : IMessage<T> => WriteTo(packet, parser, value);

    public static string Get(this Packet<string> packet)
    {
      UnsafeNativeMethods.mp_Packet__GetString(packet.mpPtr, out var ptr).Assert();
      GC.KeepAlive(packet);

      if (ptr == IntPtr.Zero)
      {
        return string.Empty;
      }
      var str = Marshal.PtrToStringAnsi(ptr);
      UnsafeNativeMethods.delete_array__PKc(ptr);

      return str;
    }

    [Obsolete("Use Get instead")]
    public static string GetString(this Packet<string> packet) => Get(packet);

    public static T Get<T>(this Packet<T> packet) => throw new NotImplementedException();
  }
}
