using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  internal struct SerializedProto {
    public IntPtr str;
    public int length;
  }
}
