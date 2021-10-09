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
#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [Pure, DllImport(MediaPipeLibrary)]
    public static extern IntPtr eglGetCurrentContext();
#endif
  }
}
