// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  public struct GlTextureInfo
  {
    public int glInternalFormat;
    public uint glFormat;
    public uint glType;
    public int downscale;
  }
}
