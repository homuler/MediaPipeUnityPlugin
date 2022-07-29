using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe
{
  public class FloatVectorPacket : Packet<List<float>>
  {
    /// <summary>
    ///   Creates an empty <see cref="FloatVectorPacket" /> instance.
    /// </summary>
    /// 

    private int _vectorLength;


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
      return At<FloatVectorPacket>(timestamp);
    }

    public override List<float> Get()
    {
      if (_vectorLength < 0)
      {
        throw new InvalidOperationException("The array's length is unknown, set Length first");
      }

      var result = new float[_vectorLength];

      unsafe
      {
        var src = (float*)GetArrayPtr();

        for (var i = 0; i < result.Length; i++)
        {
          result[i] = *src++;
        }
      }

      return result.ToList();
    }

    public IntPtr GetArrayPtr()
    {
      UnsafeNativeMethods.mp_Packet__GetFloatVector(mpPtr, out var floatFrameVector).Assert();
      GC.KeepAlive(this);

      return floatFrameVector;
    }

    public override StatusOr<List<float>> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
