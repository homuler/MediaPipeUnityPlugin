// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.PoseLandmarker
{
  /// <summary>
  ///   The pose landmarks detection result from PoseLandmarker, where each vector element represents a single pose detected in the image.
  /// </summary>
  public readonly struct PoseLandmarkerResult
  {
    /// <summary>
    ///   Detected pose landmarks in normalized image coordinates.
    /// </summary>
    public readonly List<NormalizedLandmarks> poseLandmarks;
    /// <summary>
    ///   Detected pose landmarks in world coordinates.
    /// </summary>
    public readonly List<Landmarks> poseWorldLandmarks;
    /// <summary>
    ///   Optional segmentation masks for pose.
    /// </summary>
    public readonly List<Image> segmentationMasks;

    internal PoseLandmarkerResult(List<NormalizedLandmarks> poseLandmarks,
      List<Landmarks> poseWorldLandmarks, List<Image> segmentationMasks = null)
    {
      this.poseLandmarks = poseLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.segmentationMasks = segmentationMasks;
    }

    public static PoseLandmarkerResult Alloc(int capacity, bool outputSegmentationMasks = false)
    {
      var poseLandmarks = new List<NormalizedLandmarks>(capacity);
      var poseWorldLandmarks = new List<Landmarks>(capacity);
      var segmentationMasks = outputSegmentationMasks ? new List<Image>(capacity) : null;
      return new PoseLandmarkerResult(poseLandmarks, poseWorldLandmarks, segmentationMasks);
    }

    public override string ToString()
      => $"{{ \"poseLandmarks\": {Util.Format(poseLandmarks)}, \"poseWorldLandmarks\": {Util.Format(poseWorldLandmarks)}, \"segmentationMasks\": {Util.Format(segmentationMasks)} }}";
  }
}
