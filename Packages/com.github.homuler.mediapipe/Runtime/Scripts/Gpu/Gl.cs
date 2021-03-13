using System;

namespace Mediapipe {
  public class Gl {
    public static UInt32 GL_TEXTURE_2D = 0x0DE1;

    public static void Flush() {
      UnsafeNativeMethods.glFlush();
    }

    public static void ReadPixels(int x, int y, int width, int height, UInt32 glFormat, UInt32 glType, IntPtr pixels) {
      UnsafeNativeMethods.glReadPixels(x, y, width, height, glFormat, glType, pixels);
    }
  }
}
