// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class FloatArrayPacket : Packet<float[]>
  {
    private int _length = -1;

    public int length
    {
      get => _length;
      set
      {
        if (_length >= 0)
        {
          throw new InvalidOperationException("Length is already set and cannot be changed");
        }

        _length = value;
      }
    }

    /// <summary>
    ///   Creates an empty <see cref="FloatArrayPacket" /> instance.
    /// </summary>
    public FloatArrayPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FloatArrayPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public FloatArrayPacket(float[] value) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatArrayPacket__Pf_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
      length = value.Length;
    }

    public FloatArrayPacket(float[] value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatArrayPacket_At__Pf_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
      length = value.Length;
    }

    public FloatArrayPacket At(Timestamp timestamp)
    {
      var packet = At<FloatArrayPacket>(timestamp);
      packet.length = length;
      return packet;
    }

    public override float[] Get()
    {
      if (length < 0)
      {
        throw new InvalidOperationException("The array's length is unknown, set Length first");
      }

      var result = new float[length];
      UnsafeNativeMethods.mp_Packet__GetFloatArray_i(mpPtr, length, out var arrayPtr).Assert();
      GC.KeepAlive(this);

      unsafe
      {
        var src = (float*)arrayPtr;

        for (var i = 0; i < result.Length; i++)
        {
          result[i] = *src++;
        }
      }

      UnsafeNativeMethods.delete_array__Pf(arrayPtr);
      return result;
    }

    public override StatusOr<float[]> Consume()
    {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloatArray(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
