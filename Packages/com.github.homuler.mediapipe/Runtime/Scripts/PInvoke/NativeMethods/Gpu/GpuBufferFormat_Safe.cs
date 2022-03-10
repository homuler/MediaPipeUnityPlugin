// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  internal static partial class SafeNativeMethods
  {
    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern ImageFormat.Types.Format mp__ImageFormatForGpuBufferFormat__ui(GpuBufferFormat format);

    [Pure, DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern ImageFormat.Types.Format mp__GpuBufferFormatForImageFormat__ui(ImageFormat.Types.Format format);
  }
}
