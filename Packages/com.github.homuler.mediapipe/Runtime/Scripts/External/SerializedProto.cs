// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

using pb = Google.Protobuf;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct SerializedProto
  {
    private readonly IntPtr _str;
    private readonly int _length;

    public void Dispose()
    {
      UnsafeNativeMethods.delete_array__PKc(_str);
    }

    public T Deserialize<T>(pb::MessageParser<T> parser) where T : pb::IMessage<T>
    {
      var bytes = new byte[_length];
      Marshal.Copy(_str, bytes, 0, bytes.Length);
      return parser.ParseFrom(bytes);
    }
  }
}
