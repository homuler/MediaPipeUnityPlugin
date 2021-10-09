// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class UnsafeNativeMethods
  {
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp__GlTextureInfoForGpuBufferFormat__ui_i_ui(
        GpuBufferFormat format, int plane, GlVersion glVersion, out GlTextureInfo glTextureInfo);
  }
}
