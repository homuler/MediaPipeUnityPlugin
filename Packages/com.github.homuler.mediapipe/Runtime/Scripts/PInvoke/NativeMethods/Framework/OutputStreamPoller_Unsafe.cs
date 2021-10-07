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
    #region OutputStreamPoller
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_OutputStreamPoller__delete(IntPtr poller);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_OutputStreamPoller__Reset(IntPtr poller);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_OutputStreamPoller__Next_Ppacket(IntPtr poller, IntPtr packet, out bool result);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_OutputStreamPoller__SetMaxQueueSize(IntPtr poller, int queueSize);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_OutputStreamPoller__QueueSize(IntPtr poller, out int queueSize);
    #endregion

    #region StatusOrPoller
    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_StatusOrPoller__delete(IntPtr statusOrPoller);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrPoller__status(IntPtr statusOrPoller, out IntPtr status);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode mp_StatusOrPoller__value(IntPtr statusOrPoller, out IntPtr poller);
    #endregion
  }
}
