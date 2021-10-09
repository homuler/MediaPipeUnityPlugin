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
    [DllImport(MediaPipeLibrary)]
    public static extern void glFlush();

    [DllImport(MediaPipeLibrary)]
    public static extern void glReadPixels(int x, int y, int width, int height, uint glFormat, uint glType, IntPtr pixels);
  }
}
