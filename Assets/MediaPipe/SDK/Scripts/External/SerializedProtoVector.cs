using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  internal struct SerializedProtoVector {
    public IntPtr data;
    public int size;
  }
}
