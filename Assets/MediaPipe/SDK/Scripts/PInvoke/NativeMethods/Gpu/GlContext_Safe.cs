using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    #region GlContext
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_SharedGlContext__get(IntPtr sharedGlContext);
    #endregion

    #region GlSyncToken
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlSyncToken__get(IntPtr glSyncToken);
    #endregion
  }
}
