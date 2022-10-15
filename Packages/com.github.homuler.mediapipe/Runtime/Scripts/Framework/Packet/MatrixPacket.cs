// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Google.Protobuf;
using System;

namespace Mediapipe
{
  public class MatrixPacket : Packet<MatrixData>
  {
    /// <summary>
    ///   Creates an empty <see cref="MatrixPacket" /> instance.
    /// </summary>
    public MatrixPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public MatrixPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public MatrixPacket(MatrixData matrixData) : base()
    {
      var value = matrixData.ToByteArray();
      UnsafeNativeMethods.mp__MakeMatrixPacket__PKc_i(value, value.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public MatrixPacket(MatrixData matrixData, Timestamp timestamp) : base()
    {
      var value = matrixData.ToByteArray();
      UnsafeNativeMethods.mp__MakeMatrixPacket_At__PKc_i_Rt(value, value.Length, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      this.ptr = ptr;
    }

    public MatrixPacket At(Timestamp timestamp)
    {
      return At<MatrixPacket>(timestamp);
    }

    public override MatrixData Get()
    {
      UnsafeNativeMethods.mp_Packet__GetMatrix(mpPtr, out var serializedMatrixData).Assert();
      GC.KeepAlive(this);

      var matrixData = serializedMatrixData.Deserialize(MatrixData.Parser);
      serializedMatrixData.Dispose();

      return matrixData;
    }

    public override StatusOr<MatrixData> Consume()
    {
      throw new NotSupportedException();
    }

    public override Status ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsMatrix(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
