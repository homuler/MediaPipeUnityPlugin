// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
namespace Mediapipe
{
  public class Egl
  {
    public static IntPtr GetCurrentContext()
    {
      return SafeNativeMethods.eglGetCurrentContext();
    }
  }
}
#endif
