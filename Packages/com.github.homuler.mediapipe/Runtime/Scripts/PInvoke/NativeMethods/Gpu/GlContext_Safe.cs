// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class SafeNativeMethods
  {
    #region GlContext
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_SharedGlContext__get(IntPtr sharedGlContext);

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlContext__egl_display(IntPtr glContext);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlContext__egl_config(IntPtr glContext);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlContext__egl_context(IntPtr glContext);
#endif

#if UNITY_IOS
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlContext__eagl_context(IntPtr glContext);
#elif UNITY_STANDALONE_OSX
    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlContext__nsgl_context(IntPtr glContext);

    [Pure, DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlContext__nsgl_pixel_format(IntPtr glContext);
#endif

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool mp_GlContext__IsCurrent(IntPtr glContext);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlContext__gl_major_version(IntPtr glContext);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern int mp_GlContext__gl_minor_version(IntPtr glContext);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern long mp_GlContext__gl_finish_count(IntPtr glContext);
    #endregion

    #region GlSyncToken
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern IntPtr mp_GlSyncToken__get(IntPtr glSyncToken);
    #endregion
  }
}
