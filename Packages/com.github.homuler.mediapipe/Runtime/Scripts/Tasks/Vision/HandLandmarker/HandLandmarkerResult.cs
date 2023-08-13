// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Linq;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.HandLandmarker
{
  /// <summary>
  ///   The hand landmarks result from HandLandmarker, where each vector element represents a single hand detected in the image.
  /// </summary>
  public readonly struct HandLandmarkerResult
  {
    /// <summary>
    ///   Classification of handedness.
    /// </summary>
    public readonly IReadOnlyList<Classifications> handedness;
    /// <summary>
    ///   Detected hand landmarks in normalized image coordinates.
    /// </summary>
    public readonly IReadOnlyList<NormalizedLandmarks> handLandmarks;
    /// <summary>
    ///   Detected hand landmarks in world coordinates.
    /// </summary>
    public readonly IReadOnlyList<Landmarks> handWorldLandmarks;

    internal HandLandmarkerResult(IReadOnlyList<Classifications> handedness,
        IReadOnlyList<NormalizedLandmarks> handLandmarks, IReadOnlyList<Landmarks> handWorldLandmarks)
    {
      this.handedness = handedness;
      this.handLandmarks = handLandmarks;
      this.handWorldLandmarks = handWorldLandmarks;
    }

    // TODO: add parameterless constructors
    internal static HandLandmarkerResult Empty()
      => new HandLandmarkerResult(new List<Classifications>(), new List<NormalizedLandmarks>(), new List<Landmarks>());

    internal static HandLandmarkerResult CreateFrom(IReadOnlyList<ClassificationList> handednessProto,
        IReadOnlyList<NormalizedLandmarkList> handLandmarksProto, IReadOnlyList<LandmarkList> handWorldLandmarksProto)
    {
      var handedness = handednessProto.Select(x => Classifications.CreateFrom(x)).ToList();
      var handLandmarks = handLandmarksProto.Select(NormalizedLandmarks.CreateFrom).ToList();
      var handWorldLandmarks = handWorldLandmarksProto.Select(Landmarks.CreateFrom).ToList();

      return new HandLandmarkerResult(handedness, handLandmarks, handWorldLandmarks);
    }

    public override string ToString()
      => $"{{ \"handedness\": {Util.Format(handedness)}, \"handLandmarks\": {Util.Format(handLandmarks)}, \"handWorldLandmarks\": {Util.Format(handWorldLandmarks)} }}";
  }
}
