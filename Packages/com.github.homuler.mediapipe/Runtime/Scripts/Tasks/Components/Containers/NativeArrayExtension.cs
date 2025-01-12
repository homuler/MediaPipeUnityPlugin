// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Components.Containers
{
  internal static class NativeContainerArrayExtension
  {
    public static void FillWith(this List<ClassificationResult> target, NativeClassificationResultArray source)
    {
      target.ResizeTo(source.size);

      var i = 0;
      foreach (var nativeClassificationResult in source.AsReadOnlySpan())
      {
        var classificationResult = target[i];
        ClassificationResult.Copy(nativeClassificationResult, ref classificationResult);
        target[i++] = classificationResult;
      }
    }

    public static void FillWith(this List<Landmarks> target, NativeLandmarksArray source)
    {
      target.ResizeTo(source.size);

      var i = 0;
      foreach (var nativeLandmarks in source.AsReadOnlySpan())
      {
        var landmarks = target[i];
        Landmarks.Copy(nativeLandmarks, ref landmarks);
        target[i++] = landmarks;
      }
    }

    public static void FillWith(this List<NormalizedLandmarks> target, NativeNormalizedLandmarksArray source)
    {
      target.ResizeTo(source.size);

      var i = 0;
      foreach (var nativeLandmarks in source.AsReadOnlySpan())
      {
        var landmarks = target[i];
        NormalizedLandmarks.Copy(nativeLandmarks, ref landmarks);
        target[i++] = landmarks;
      }
    }
  }
}
