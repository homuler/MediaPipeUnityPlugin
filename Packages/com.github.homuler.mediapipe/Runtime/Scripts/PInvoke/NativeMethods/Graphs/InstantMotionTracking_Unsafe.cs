using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class UnsafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_InstantMotionTrackingAnchorArray__delete(IntPtr anchorVectorData);

    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_Packet__GetInstantMotionTrackingAnchorVector(IntPtr packet, out Mediapipe.InstantMotionTracking.AnchorVector anchorVector);
  }
}
