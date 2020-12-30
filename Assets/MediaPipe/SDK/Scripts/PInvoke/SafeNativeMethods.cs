using System.Security;

namespace Mediapipe {
  [SuppressUnmanagedCodeSecurityAttribute]
  internal static partial class SafeNativeMethods {
#if UNITY_ANDROID
    private const string MediaPipeLibrary = "mediapipe_jni";
#else
    private const string MediaPipeLibrary = "mediapipe_c";
#endif
  }
}
