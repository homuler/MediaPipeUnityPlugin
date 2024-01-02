// Copyright (c) 2023 homuler
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
    public static extern MpReturnCode mp_Packet__GetNormalizedLandmarksVector(IntPtr packet, out NativeNormalizedLandmarksArray value);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api_NormalizedLandmarksArray__delete(IntPtr data, int size);
  }
}
