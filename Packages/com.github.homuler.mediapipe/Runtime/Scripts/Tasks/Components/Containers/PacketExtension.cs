// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Components.Containers
{
  public static class PacketExtension
  {
    public static void GetClassificationsVector(this Packet packet, List<Classifications> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetClassificationsVector(packet.mpPtr, out var classificationResult).Assert();
      var tmp = new ClassificationResult(outs, null);
      ClassificationResult.Copy(classificationResult, ref tmp);
      classificationResult.Dispose();
    }

    public static void GetDetectionResult(this Packet packet, ref DetectionResult value)
    {
      UnsafeNativeMethods.mp_Packet__GetDetectionResult(packet.mpPtr, out var detectionResult).Assert();
      DetectionResult.Copy(detectionResult, ref value);
      detectionResult.Dispose();
    }

    public static void GetLandmarksList(this Packet packet, List<Landmarks> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetLandmarksVector(packet.mpPtr, out var landmarksArray).Assert();
      outs.FillWith(landmarksArray);
      landmarksArray.Dispose();
    }

    public static void GetNormalizedLandmarksList(this Packet packet, List<NormalizedLandmarks> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetNormalizedLandmarksVector(packet.mpPtr, out var landmarksArray).Assert();
      outs.FillWith(landmarksArray);
      landmarksArray.Dispose();
    }
  }
}
