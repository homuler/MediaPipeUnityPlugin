using System;

namespace Mediapipe {
  public class GL {
    public static void Flush() {
      UnsafeNativeMethods.GlFlush();
    }

    public static void ReadPixels(int x, int y, int width, int height, UInt32 glFormat, UInt32 glType, IntPtr pixels) {
      UnsafeNativeMethods.GlReadPixels(x, y, width, height, glFormat, glType, pixels);
    }
  }
}
