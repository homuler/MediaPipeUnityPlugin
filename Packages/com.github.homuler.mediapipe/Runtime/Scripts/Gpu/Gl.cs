// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class Gl
  {
    public static uint GL_TEXTURE_2D = 0x0DE1;

    public static void Flush()
    {
      UnsafeNativeMethods.glFlush();
    }

    public static void ReadPixels(int x, int y, int width, int height, uint glFormat, uint glType, IntPtr pixels)
    {
      UnsafeNativeMethods.glReadPixels(x, y, width, height, glFormat, glType, pixels);
    }
  }
}
