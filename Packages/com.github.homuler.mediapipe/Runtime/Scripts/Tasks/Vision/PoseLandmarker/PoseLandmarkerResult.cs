// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Linq;
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
    public readonly IReadOnlyList<NormalizedLandmarks> poseLandmarks;
    /// <summary>
    ///   Detected pose landmarks in world coordinates.
    /// </summary>
    public readonly IReadOnlyList<Landmarks> poseWorldLandmarks;
    /// <summary>
    ///   Optional segmentation masks for pose.
    /// </summary>
    public readonly IReadOnlyList<Image> segmentationMasks;

    internal PoseLandmarkerResult(IReadOnlyList<NormalizedLandmarks> poseLandmarks,
      IReadOnlyList<Landmarks> poseWorldLandmarks, IReadOnlyList<Image> segmentationMasks = null)
    {
      this.poseLandmarks = poseLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.segmentationMasks = segmentationMasks;
    }

    // TODO: add parameterless constructors
    internal static PoseLandmarkerResult Empty()
      => new PoseLandmarkerResult(new List<NormalizedLandmarks>(), new List<Landmarks>());

    internal static PoseLandmarkerResult CreateFrom(IReadOnlyList<NormalizedLandmarkList> poseLandmarksProto,
      IReadOnlyList<LandmarkList> poseWorldLandmarksProto, IReadOnlyList<Image> segmentationMasks = null)
    {
      var poseLandmarks = poseLandmarksProto.Select(NormalizedLandmarks.CreateFrom).ToList();
      var poseWorldLandmarks = poseWorldLandmarksProto.Select(Landmarks.CreateFrom).ToList();

      return new PoseLandmarkerResult(poseLandmarks, poseWorldLandmarks, segmentationMasks);
    }

    public override string ToString()
      => $"{{ \"poseLandmarks\": {Util.Format(poseLandmarks)}, \"poseWorldLandmarks\": {Util.Format(poseWorldLandmarks)}, \"segmentationMasks\": {Util.Format(segmentationMasks)} }}";
  }
}
