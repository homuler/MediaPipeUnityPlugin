using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using pb = global::Google.Protobuf;

namespace Mediapipe {
  public class SerializedProtoVector {
    public static List<T> FromPtr<T>(IntPtr ptr, pb::MessageParser<T> parser) where T : pb::IMessage<T> {
      var inner = Marshal.PtrToStructure<SerializedProtoVectorInner>(ptr);
      var protos = new List<T>(inner.size);

      unsafe {
        var arr = inner.data;

        for (var i = 0; i < inner.size; i++) {
          protos.Add(SerializedProto.FromPtr<T>((IntPtr)(*arr++), parser));
        }
      }

      return protos;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct SerializedProtoVectorInner {
      public IntPtr* data; // SerializedProto**
      public int size;
    }
  }
}
