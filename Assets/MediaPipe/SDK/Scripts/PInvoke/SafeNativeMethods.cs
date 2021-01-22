using System.Security;

namespace Mediapipe {
  [SuppressUnmanagedCodeSecurityAttribute]
  internal static partial class SafeNativeMethods {
    private const string MediaPipeLibrary =
#if UNITY_EDITOR
      "mediapipe_c";
#elif UNITY_IOS
      "__Internal";
#elif UNITY_ANDROID
      "mediapipe_jni";
#else
      "mediapipe_c";
#endif
  }
}
