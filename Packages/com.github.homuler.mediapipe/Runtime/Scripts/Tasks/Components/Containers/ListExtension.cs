// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Components.Containers
{
  internal static class ListExtension
  {
    public static void CopyFrom(this List<Classifications> target, List<Classifications> source)
    {
      target.ResizeTo(source.Count);

      var i = 0;
      foreach (var x in source)
      {
        var v = target[i];
        x.CloneTo(ref v);
        target[i++] = v;
      }
    }

    public static void CopyFrom(this List<Detection> target, List<Detection> source)
    {
      target.ResizeTo(source.Count);

      var i = 0;
      foreach (var x in source)
      {
        var v = target[i];
        x.CloneTo(ref v);
        target[i++] = v;
      }
    }

    public static void CopyFrom(this List<Landmarks> target, List<Landmarks> source)
    {
      target.ResizeTo(source.Count);

      var i = 0;
      foreach (var x in source)
      {
        var v = target[i];
        x.CloneTo(ref v);
        target[i++] = v;
      }
    }

    public static void CopyFrom(this List<NormalizedLandmarks> target, List<NormalizedLandmarks> source)
    {
      target.ResizeTo(source.Count);

      var i = 0;
      foreach (var x in source)
      {
        var v = target[i];
        x.CloneTo(ref v);
        target[i++] = v;
      }
    }
  }
}
