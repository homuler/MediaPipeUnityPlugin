// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Core
{
  internal interface ITaskOptions
  {
    CalculatorOptions ToCalculatorOptions() => null;

    Google.Protobuf.WellKnownTypes.Any ToAnyOptions() => null;
  }
}
