using System;
using System.Runtime.InteropServices;

using pb = global::Google.Protobuf;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  internal struct SerializedProto {
    public IntPtr str;
    public int length;

    public void Dispose() {
      UnsafeNativeMethods.delete_array__PKc(str);
    }

    public T Deserialize<T>(pb::MessageParser<T> parser) where T : pb::IMessage<T> {
      var bytes = new byte[length];
      Marshal.Copy(str, bytes, 0, bytes.Length);
      return parser.ParseFrom(bytes);
    }
  }
}
