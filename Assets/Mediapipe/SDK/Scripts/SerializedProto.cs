using System;
using System.Runtime.InteropServices;

using pb = global::Google.Protobuf;

namespace Mediapipe {
  public class SerializedProto {
    public static T FromPtr<T>(IntPtr ptr, pb::MessageParser<T> parser) where T : pb::IMessage<T> {
      var inner = Marshal.PtrToStructure<SerializedProtoInner>(ptr);
      var bytes = new byte[inner.length];

      unsafe {
        Marshal.Copy((IntPtr)inner.serializedStr, bytes, 0, inner.length);
      }

      return parser.ParseFrom(bytes);
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct SerializedProtoInner {
      public byte* serializedStr;
      public int length;
    }
  }
}
