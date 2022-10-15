// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct FloatVector
  {
    private readonly IntPtr _data;
    private readonly int _size;

    public void Dispose()
    {
      UnsafeNativeMethods.delete_array__Pf(_data);
    }

    public List<float> Copy()
    {
      var data = new List<float>(_size);

      unsafe
      {
        var floatPtr = (float*)_data;

        for (var i = 0; i < _size; i++)
        {
          data.Add(*floatPtr++);
        }
      }
      return data;
    }
  }

  public class FloatVectorPacket : Packet<List<float>>
  {
    /// <summary>
    ///   Creates an empty <see cref="FloatVectorPacket" /> instance.
    /// </summary>
    /// 
    public FloatVectorPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public FloatVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }


    public FloatVectorPacket(float[] value) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket__Pf_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public FloatVectorPacket(float[] value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket_At__Pf_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public FloatVectorPacket(List<float> value) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket__Pf_i(value.ToArray(), value.Count, out var ptr).Assert();
      this.ptr = ptr;
    }

    public FloatVectorPacket(List<float> value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeFloatVectorPacket_At__Pf_i_Rt(value.ToArray(), value.Count, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public FloatVectorPacket At(Timestamp timestamp)
    {
      return At<FloatVectorPacket>(timestamp);
    }

    public override List<float> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetFloatVector(mpPtr, out var floatVector).Assert();
      GC.KeepAlive(this);

      var result = floatVector.Copy();
      floatVector.Dispose();
      return result;
    }

    public override StatusOr<List<float>> Consume()
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
