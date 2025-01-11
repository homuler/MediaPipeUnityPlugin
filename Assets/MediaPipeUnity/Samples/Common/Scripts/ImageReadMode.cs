// Copyright (c) 2025 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System.ComponentModel;

namespace Mediapipe.Unity
{
  [System.Serializable]
  public enum ImageReadMode
  {
    CPU,
    [Description("CPU Async")]
    CPUAsync,
    GPU,
  }
}
