using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mediapipe.InstantMotionTracking {
  [StructLayout(LayoutKind.Sequential)]
  internal struct AnchorVector {
    public IntPtr data;
    public int size;

    public void Dispose() {
      UnsafeNativeMethods.mp_InstantMotionTrackingAnchorArray__delete(data);
    }

    public List<Anchor> ToList() {
      var anchors = new List<Anchor>(size);

      unsafe {
        Anchor* anchorPtr = (Anchor*)data;

        for (var i = 0; i < size; i++) {
          anchors.Add(Marshal.PtrToStructure<Anchor>((IntPtr)anchorPtr));
        }
      }

      return anchors;
    }
  }
}
