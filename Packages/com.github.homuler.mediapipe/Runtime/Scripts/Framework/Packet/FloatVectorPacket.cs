// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe
{
  public class FloatVectorPacket : Packet<float[]>
  {
    /// <summary>
    ///   Creates an empty <see cref="FloatVectorPacket" /> instance.
    /// </summary>
    /// 

    private int _vectorLength = -1;


    public FloatVectorPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FloatVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public FloatVectorPacket(float[] value) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket__PA_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
      _vectorLength = value.Length;
    }

    public FloatVectorPacket(float[] value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket_At__PA_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public FloatVectorPacket At(Timestamp timestamp)
    {
      var packet = At<FloatVectorPacket>(timestamp);
      packet._vectorLength = _vectorLength;
      return packet;
    }

    public override float[] Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFloatVector(mpPtr, out var floatFrameVector, out var size).Assert();
      GC.KeepAlive(this);
      if (size < 0)
      {
        throw new InvalidOperationException("The array's length is unknown, set Length first");
      }

      var result = new float[size];

      unsafe
      {
        var src = (float*)floatFrameVector;

        for (var i = 0; i < result.Length; i++)
        {
          result[i] = *src++;
        }
      }

      return result;
    }

    public override StatusOr<float[]> Consume()
    {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsFloatVector(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
