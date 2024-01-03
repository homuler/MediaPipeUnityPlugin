// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
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
    public readonly List<Classifications> handedness;
    /// <summary>
    ///   Detected hand landmarks in normalized image coordinates.
    /// </summary>
    public readonly List<NormalizedLandmarks> handLandmarks;
    /// <summary>
    ///   Detected hand landmarks in world coordinates.
    /// </summary>
    public readonly List<Landmarks> handWorldLandmarks;

    internal HandLandmarkerResult(List<Classifications> handedness,
        List<NormalizedLandmarks> handLandmarks, List<Landmarks> handWorldLandmarks)
    {
      this.handedness = handedness;
      this.handLandmarks = handLandmarks;
      this.handWorldLandmarks = handWorldLandmarks;
    }

    public static HandLandmarkerResult Alloc(int capacity)
    {
      var handedness = new List<Classifications>(capacity);
      var handLandmarks = new List<NormalizedLandmarks>(capacity);
      var handWorldLandmarks = new List<Landmarks>(capacity);
      return new HandLandmarkerResult(handedness, handLandmarks, handWorldLandmarks);
    }

    public void CloneTo(ref HandLandmarkerResult destination)
    {
      if (handLandmarks == null)
      {
        destination = default;
        return;
      }

      var dstHandedness = destination.handedness ?? new List<Classifications>(handedness.Count);
      dstHandedness.Clear();
      dstHandedness.AddRange(handedness);

      var dstHandLandmarks = destination.handLandmarks ?? new List<NormalizedLandmarks>(handLandmarks.Count);
      dstHandLandmarks.Clear();
      dstHandLandmarks.AddRange(handLandmarks);

      var dstHandWorldLandmarks = destination.handWorldLandmarks ?? new List<Landmarks>(handWorldLandmarks.Count);
      dstHandWorldLandmarks.Clear();
      dstHandWorldLandmarks.AddRange(handWorldLandmarks);

      destination = new HandLandmarkerResult(dstHandedness, dstHandLandmarks, dstHandWorldLandmarks);
    }

    public override string ToString()
      => $"{{ \"handedness\": {Util.Format(handedness)}, \"handLandmarks\": {Util.Format(handLandmarks)}, \"handWorldLandmarks\": {Util.Format(handWorldLandmarks)} }}";
  }
}
