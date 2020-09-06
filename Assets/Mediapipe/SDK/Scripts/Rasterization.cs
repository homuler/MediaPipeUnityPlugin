using System;
using System.Runtime.InteropServices;

using MpInterval = System.IntPtr;
using MpRasterization = System.IntPtr;

namespace Mediapipe {
  public class Rasterization {
    public Interval[] intervals;

    public Rasterization(MpRasterization ptr) : this(Marshal.PtrToStructure<RasterizationInner>(ptr)) {}

    private unsafe Rasterization(RasterizationInner inner) {
      var intervalsSize = inner.intervalsSize;
      intervals = new Interval[intervalsSize];

      if (intervalsSize == 0) { return; }

      Interval** arr = (Interval**)inner.intervals;

      for (var i = 0; i < intervalsSize; i++) {
        intervals[i] = Marshal.PtrToStructure<Interval>((IntPtr)(*arr++));
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct RasterizationInner {
      public MpInterval* intervals;
      public int intervalsSize;
    }
  }
}
