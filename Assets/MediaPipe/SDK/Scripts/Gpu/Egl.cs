using System;

namespace Mediapipe {
  public class Egl {
    public static IntPtr getCurrentContext() {
      return SafeNativeMethods.eglGetCurrentContext();
    }
  }
}
