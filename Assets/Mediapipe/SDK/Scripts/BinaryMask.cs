using System.Runtime.InteropServices;

using MpBinaryMask = System.IntPtr;
using MpRasterization = System.IntPtr;

namespace Mediapipe {
  public class BinaryMask {
    public int width;
    public int height;
    public Rasterization rasterization;

    public BinaryMask(MpBinaryMask ptr) : this(Marshal.PtrToStructure<BinaryMaskInner>(ptr)) {}

    private BinaryMask(BinaryMaskInner inner) {
      width = inner.width;
      height = inner.height;
      rasterization = new Rasterization(inner.rasterization);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BinaryMaskInner {
      public int width;
      public int height;
      public MpRasterization rasterization;
    }
  }
}
