

using Google.Protobuf;
using System;

namespace Mediapipe
{
  public class MatrixPacket : Packet<MatrixData>
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
    ///   Creates an empty <see cref="MatrixPacket
    ///   " /> instance.
    /// </summary>
    public MatrixPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public MatrixPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public MatrixPacket(MatrixData matrixData) : base()
    {
      var value = matrixData.ToByteArray();
      UnsafeNativeMethods.mp__MakeMatrixPacket__PKc_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
      length = value.Length;
    }

    public MatrixPacket(MatrixData matrixData, Timestamp timestamp) : base()
    {
      var value = matrixData.ToByteArray();
      UnsafeNativeMethods.mp__MakeMatrixPacket_At__PKc_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
      length = value.Length;
    }

    public MatrixPacket At(Timestamp timestamp)
    {
      var packet = At<MatrixPacket>(timestamp);
      packet.length = length;
      return packet;
    }

    public override MatrixData Get()
    {
      throw new NotImplementedException();
    }

    public override StatusOr<MatrixData> Consume()
    {
      throw new NotImplementedException();
    }

  }
}
