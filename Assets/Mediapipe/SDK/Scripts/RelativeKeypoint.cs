using System;
using System.Runtime.InteropServices;

using MpRelativeKeypointArray = System.IntPtr;

namespace Mediapipe {
  public class RelativeKeypoint {
    public float x;
    public float y;
    public string keypointLabel;
    public float score;

    public RelativeKeypoint() {}

    private unsafe RelativeKeypoint(RelativeKeypointInner inner) {
      x = inner.x;
      y = inner.y;
      keypointLabel = Marshal.PtrToStringAnsi((IntPtr)inner.keypointLabel);
      score = inner.score;
    }

    public static unsafe RelativeKeypoint[] PtrToArray(MpRelativeKeypointArray ptr, int size) {
      var keypoints = new RelativeKeypoint[size];

      if (size == 0) { return keypoints; }

      RelativeKeypointInner** arr = (RelativeKeypointInner**)ptr;

      for (var i = 0; i < size; i++) {
        var inner = Marshal.PtrToStructure<RelativeKeypointInner>((IntPtr)(*arr++));
        keypoints[i] = new RelativeKeypoint(inner);
      }

      return keypoints;
    }

    [StructLayout(LayoutKind.Sequential)]
    private unsafe struct RelativeKeypointInner {
      public float x;
      public float y;
      public char* keypointLabel;
      public float score;
    }
  }
}
