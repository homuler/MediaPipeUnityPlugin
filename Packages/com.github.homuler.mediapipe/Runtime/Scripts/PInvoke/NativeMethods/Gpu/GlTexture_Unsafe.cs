// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class UnsafeNativeMethods
  {
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__(out IntPtr glTexture);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_GlTexture__delete(IntPtr glTexture);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__Release(IntPtr glTexture);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_GlTexture__GetGpuBufferFrame(IntPtr glTexture, out IntPtr gpuBuffer);
  }
}
