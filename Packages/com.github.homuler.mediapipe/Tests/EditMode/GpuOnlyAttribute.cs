// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class GpuOnlyAttribute : CategoryAttribute { }
}
