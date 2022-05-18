// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [SuppressUnmanagedCodeSecurity]
  internal static partial class UnsafeNativeMethods
  {
    internal const string MediaPipeLibrary =
#if UNITY_EDITOR
      "mediapipe_c";
#elif UNITY_IOS || UNITY_WEBGL
      "__Internal";
#elif UNITY_ANDROID
      "mediapipe_jni";
#else
      "mediapipe_c";
#endif

    static UnsafeNativeMethods()
    {
      mp_api__SetFreeHGlobal(FreeHGlobal);
    }

    private delegate void FreeHGlobalDelegate(IntPtr hglobal);

    [AOT.MonoPInvokeCallback(typeof(FreeHGlobalDelegate))]
    private static void FreeHGlobal(IntPtr hglobal)
    {
      Marshal.FreeHGlobal(hglobal);
    }

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    private static extern void mp_api__SetFreeHGlobal([MarshalAs(UnmanagedType.FunctionPtr)] FreeHGlobalDelegate freeHGlobal);
  }
}
