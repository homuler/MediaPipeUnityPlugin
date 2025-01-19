// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

namespace Mediapipe.Tasks.Components.Containers
{
  public static class PacketExtension
  {
    public static void Get(this Packet<Classifications> packet, ref Classifications value)
    {
      UnsafeNativeMethods.mp_Packet__GetClassifications(packet.mpPtr, out var classifications).Assert();
      Classifications.Copy(classifications, ref value);
      classifications.Dispose();
    }

    public static void Get(this Packet<ClassificationResult> packet, ref ClassificationResult value)
    {
      UnsafeNativeMethods.mp_Packet__GetClassificationResult(packet.mpPtr, out var classificationResult).Assert();
      ClassificationResult.Copy(classificationResult, ref value);
      classificationResult.Dispose();
    }

    public static void Get(this Packet<List<Classifications>> packet, List<Classifications> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetClassificationsVector(packet.mpPtr, out var classificationResult).Assert();
      var tmp = new ClassificationResult(outs, null);
      ClassificationResult.Copy(classificationResult, ref tmp);
      classificationResult.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetClassificationsVector(this Packet<List<Classifications>> packet, List<Classifications> outs) => Get(packet, outs);

    public static void Get(this Packet<DetectionResult> packet, ref DetectionResult value)
    {
      UnsafeNativeMethods.mp_Packet__GetDetectionResult(packet.mpPtr, out var detectionResult).Assert();
      DetectionResult.Copy(detectionResult, ref value);
      detectionResult.Dispose();
    }

    public static void Get(this Packet<List<ClassificationResult>> packet, List<ClassificationResult> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetClassificationResultVector(packet.mpPtr, out var classificationResults).Assert();
      outs.FillWith(classificationResults);
      classificationResults.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetDetectionResult(this Packet<DetectionResult> packet, ref DetectionResult value) => Get(packet, ref value);

    public static void Get(this Packet<Landmarks> packet, ref Landmarks outs)
    {
      UnsafeNativeMethods.mp_Packet__GetLandmarks(packet.mpPtr, out var landmarks).Assert();
      Landmarks.Copy(landmarks, ref outs);
      landmarks.Dispose();
    }

    public static void Get(this Packet<List<Landmarks>> packet, List<Landmarks> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetLandmarksVector(packet.mpPtr, out var landmarksArray).Assert();
      outs.FillWith(landmarksArray);
      landmarksArray.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetLandmarksList(this Packet<List<Landmarks>> packet, List<Landmarks> outs) => Get(packet, outs);

    public static void Get(this Packet<NormalizedLandmarks> packet, ref NormalizedLandmarks outs)
    {
      UnsafeNativeMethods.mp_Packet__GetNormalizedLandmarks(packet.mpPtr, out var landmarks).Assert();
      NormalizedLandmarks.Copy(landmarks, ref outs);
      landmarks.Dispose();
    }

    public static void Get(this Packet<List<NormalizedLandmarks>> packet, List<NormalizedLandmarks> outs)
    {
      UnsafeNativeMethods.mp_Packet__GetNormalizedLandmarksVector(packet.mpPtr, out var landmarksArray).Assert();
      outs.FillWith(landmarksArray);
      landmarksArray.Dispose();
    }

    [Obsolete("Use Get instead")]
    public static void GetNormalizedLandmarksList(this Packet<List<NormalizedLandmarks>> packet, List<NormalizedLandmarks> outs) => Get(packet, outs);
  }
}
