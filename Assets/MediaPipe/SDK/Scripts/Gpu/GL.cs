using System;

namespace Mediapipe {
  public class Gl {
    public static void Flush() {
      UnsafeNativeMethods.glFlush();
    }

    public static void ReadPixels(int x, int y, int width, int height, UInt32 glFormat, UInt32 glType, IntPtr pixels) {
      UnsafeNativeMethods.glReadPixels(x, y, width, height, glFormat, glType, pixels);
    }
  }
}
