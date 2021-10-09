// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class TimedModelMatrixProtoListPacket : Packet<TimedModelMatrixProtoList>
  {
    public TimedModelMatrixProtoListPacket() : base() { }
    public TimedModelMatrixProtoListPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public override TimedModelMatrixProtoList Get()
    {
      UnsafeNativeMethods.mp_Packet__GetTimedModelMatrixProtoList(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var matrixProtoList = serializedProto.Deserialize(TimedModelMatrixProtoList.Parser);
      serializedProto.Dispose();

      return matrixProtoList;
    }

    public override StatusOr<TimedModelMatrixProtoList> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
