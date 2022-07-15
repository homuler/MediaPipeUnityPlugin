

using System;

namespace Mediapipe
{
  public class MatrixFramePacket : Packet<byte[]>
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
    ///   Creates an empty <see cref="MatrixFramePacket
    ///   " /> instance.
    /// </summary>
    public MatrixFramePacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public MatrixFramePacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public MatrixFramePacket(byte[] value) : base()
    {
      UnsafeNativeMethods.mp__MakeMatrixFramePacket__PKc_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
      length = value.Length;
    }

    public MatrixFramePacket(byte[] value, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeMatrixFramePacket_At__PA_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
      length = value.Length;
    }

    public MatrixFramePacket At(Timestamp timestamp)
    {
      var packet = At<MatrixFramePacket>(timestamp);
      packet.length = length;
      return packet;
    }

    public override byte[] Get()
    {
      throw new NotImplementedException();
    }

    public override StatusOr<byte[]> Consume()
    {
      throw new NotImplementedException();
    }

  }
}
