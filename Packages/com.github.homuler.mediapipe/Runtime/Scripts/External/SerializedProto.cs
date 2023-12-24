// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;
using Google.Protobuf;

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

    public T Deserialize<T>(MessageParser<T> parser) where T : IMessage<T>
    {
      unsafe
      {
        var bytes = new ReadOnlySpan<byte>((byte*)_str, _length);
        return parser.ParseFrom(bytes);
      }
    }

    public void WriteTo<T>(T proto) where T : IMessage<T>
    {
      unsafe
      {
        var bytes = new ReadOnlySpan<byte>((byte*)_str, _length);
        proto.MergeFrom(bytes);
      }
    }
  }
}
