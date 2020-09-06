using System;
using System.Runtime.InteropServices;

using MpBinaryMask = System.IntPtr;
using MpBoundingBox = System.IntPtr;
using MpRelativeBoundingBox = System.IntPtr;
using MpRelativeKeypoint = System.IntPtr;
using MpLocationData = System.IntPtr;

namespace Mediapipe {
  public class LocationData {
    public int format;
    public BoundingBox boundingBox;
    public RelativeBoundingBox relativeBoundingBox;
    public BinaryMask mask;
    public RelativeKeypoint[] relativeKeypoints;

    public unsafe LocationData(MpLocationData locationDataPtr) {
      var inner = Marshal.PtrToStructure<LocationDataInner>(locationDataPtr);

      format = inner.format;

      if (inner.boundingBox != IntPtr.Zero) {
        boundingBox = Marshal.PtrToStructure<BoundingBox>(inner.boundingBox);
      }

      relativeBoundingBox = Marshal.PtrToStructure<RelativeBoundingBox>(inner.relativeBoundingBox);
      mask = new BinaryMask(inner.mask);
      relativeKeypoints = RelativeKeypoint.PtrToArray((IntPtr)inner.relativeKeypoints, inner.relativeKeypointsSize);
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct LocationDataInner {
      public int format;
      public MpBoundingBox boundingBox;
      public MpRelativeBoundingBox relativeBoundingBox;
      public MpBinaryMask mask;
      public MpRelativeKeypoint* relativeKeypoints;
      public int relativeKeypointsSize;
    }
  }
}
