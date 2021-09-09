using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using pb = global::Google.Protobuf;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  internal struct SerializedProtoVector {
    public IntPtr data;
    public int size;

    public void Dispose() {
      UnsafeNativeMethods.mp_api_SerializedProtoArray__delete(data);
    }

    public List<T> Deserialize<T>(pb::MessageParser<T> parser) where T : pb::IMessage<T> {
      var protos = new List<T>(size);

      unsafe {
        SerializedProto* protoPtr = (SerializedProto*)data;

        for (var i = 0; i < size; i++) {
          var serializedProto = Marshal.PtrToStructure<SerializedProto>((IntPtr)protoPtr);
          protos.Add(serializedProto.Deserialize<T>(parser));
        }
      }

      return protos;
    }
  }
}
