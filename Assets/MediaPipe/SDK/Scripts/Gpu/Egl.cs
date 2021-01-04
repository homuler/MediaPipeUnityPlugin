using System;

#if UNITY_STANDALONE_LINUX || UNITY_ANDROID
namespace Mediapipe {
  public class Egl {
    public static IntPtr getCurrentContext() {
      return SafeNativeMethods.eglGetCurrentContext();
    }
  }
}
#endif
