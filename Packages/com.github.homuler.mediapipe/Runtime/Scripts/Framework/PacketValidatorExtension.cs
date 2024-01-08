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
  public static class PacketValidatorExtension
  {
    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a boolean.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain bool data.
    /// </exception>
    public static void Validate(this Packet<bool> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsBool(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsBool(this Packet<bool> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a std::vector&lt;bool&gt;.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;bool&gt;.
    /// </exception>
    public static void Validate(this Packet<List<bool>> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsBoolVector(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsBoolVector(this Packet<List<bool>> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a double.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain double data.
    /// </exception>
    public static void Validate(this Packet<double> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsDouble(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsDouble(this Packet<double> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a float.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain float data.
    /// </exception>
    public static void Validate(this Packet<float> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloat(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsFloat(this Packet<float> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a float array.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain a float array.
    /// </exception>
    public static void Validate(this Packet<float[]> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloatArray(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsFloatArray(this Packet<float[]> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is std::vector&lt;float&gt;.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain std::vector&lt;bool&gt;.
    /// </exception>
    public static void Validate(this Packet<List<float>> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloatVector(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsFloatVector(this Packet<List<float>> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see cref="GpuBuffer"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="GpuBuffer"/>.
    /// </exception>
    public static void Validate(this Packet<GpuBuffer> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsGpuBuffer(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsGpuBuffer(this Packet<GpuBuffer> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see cref="Image"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="Image"/>.
    /// </exception>
    public static void Validate(this Packet<Image> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsImage(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsImage(this Packet<Image> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see cref="ImageFrame"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see cref="ImageFrame"/>.
    /// </exception>
    public static void Validate(this Packet<ImageFrame> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsImageFrame(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsImageFrame(this Packet<ImageFrame> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is an <see langword="int"/>.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain <see langword="int"/>.
    /// </exception>
    public static void Validate(this Packet<int> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsInt(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsInt(this Packet<int> packet) => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a proto message.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain proto messages.
    /// </exception>
    public static void Validate<T>(this Packet<T> packet) where T : IMessage<T>
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsProtoMessageLite(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsProtoMessageLite<T>(this Packet<T> packet) where T : IMessage<T> => Validate(packet);

    /// <summary>
    ///   Validate if the content of the <see cref="Packet"/> is a string.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the <see cref="Packet"/> doesn't contain string.
    /// </exception>
    public static void Validate(this Packet<string> packet)
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsString(packet.mpPtr, out var statusPtr).Assert();
      GC.KeepAlive(packet);

      Status.UnsafeAssertOk(statusPtr);
    }

    [Obsolete("Use Validate instead")]
    public static void ValidateAsString(this Packet<string> packet) => Validate(packet);
  }
}
