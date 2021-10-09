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
    public static extern MpReturnCode google_InitGoogleLogging__PKc(string name);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern MpReturnCode google_ShutdownGoogleLogging();

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_FLAGS_logtostderr([MarshalAs(UnmanagedType.I1)] bool value);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_FLAGS_stderrthreshold(int threshold);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_FLAGS_minloglevel(int level);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_FLAGS_log_dir(string dir);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_FLAGS_v(int v);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_LOG_INFO__PKc(string str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_LOG_WARNING__PKc(string str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_LOG_ERROR__PKc(string str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void glog_LOG_FATAL__PKc(string str);

    [DllImport(MediaPipeLibrary, ExactSpelling = true)]
    public static extern void google_FlushLogFiles(Glog.Severity severity);
  }
}
