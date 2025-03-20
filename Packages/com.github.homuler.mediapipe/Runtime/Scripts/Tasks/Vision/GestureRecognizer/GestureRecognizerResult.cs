// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.GestureRecognizer
{
  /// <summary>
  ///   The gesture recognition result from GestureRecognizer, where each vector element represents a single hand detected in the image.
  /// </summary>
  public readonly struct GestureRecognizerResult
  {
    /// <summary>
    ///   Recognized hand gestures of detected hands. Note that the index of
    ///   the gesture is always -1, because the raw indices from multiple gesture
    ///   classifiers cannot consolidate to a meaningful index.
    /// </summary>
    public readonly List<Classifications> gestures;
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

    internal GestureRecognizerResult(List<Classifications> gestures, List<Classifications> handedness,
        List<NormalizedLandmarks> handLandmarks, List<Landmarks> handWorldLandmarks)
    {
      this.gestures = gestures;
      this.handedness = handedness;
      this.handLandmarks = handLandmarks;
      this.handWorldLandmarks = handWorldLandmarks;
    }

    public static GestureRecognizerResult Alloc(int capacity)
    {
      var gestures = new List<Classifications>(capacity);
      var handedness = new List<Classifications>(capacity);
      var handLandmarks = new List<NormalizedLandmarks>(capacity);
      var handWorldLandmarks = new List<Landmarks>(capacity);
      return new GestureRecognizerResult(gestures, handedness, handLandmarks, handWorldLandmarks);
    }

    public void CloneTo(ref GestureRecognizerResult destination)
    {
      if (handLandmarks == null)
      {
        destination = default;
        return;
      }

      var dstGestures = destination.gestures ?? new List<Classifications>(gestures.Count);
      dstGestures.CopyFrom(gestures);

      var dstHandedness = destination.handedness ?? new List<Classifications>(handedness.Count);
      dstHandedness.CopyFrom(handedness);

      var dstHandLandmarks = destination.handLandmarks ?? new List<NormalizedLandmarks>(handLandmarks.Count);
      dstHandLandmarks.CopyFrom(handLandmarks);

      var dstHandWorldLandmarks = destination.handWorldLandmarks ?? new List<Landmarks>(handWorldLandmarks.Count);
      dstHandWorldLandmarks.CopyFrom(handWorldLandmarks);

      destination = new GestureRecognizerResult(dstGestures, dstHandedness, dstHandLandmarks, dstHandWorldLandmarks);
    }

    public override string ToString()
      => $"{{ \"gestures\": {Util.Format(gestures)}, \"handedness\": {Util.Format(handedness)}, \"handLandmarks\": {Util.Format(handLandmarks)}, \"handWorldLandmarks\": {Util.Format(handWorldLandmarks)} }}";
  }
}
