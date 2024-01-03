// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Tasks.Components.Containers
{
  internal static class Util
  {
    public static string Format<T>(T value) => value == null ? "null" : $"{value}";

    public static string Format(string value) => value == null ? "null" : $"\"{value}\"";

    public static string Format<T>(List<T> list)
    {
      if (list == null)
      {
        return "null";
      }
      var str = string.Join(", ", list.Select(x => x.ToString()));
      return $"[{str}]";
    }
  }
}
