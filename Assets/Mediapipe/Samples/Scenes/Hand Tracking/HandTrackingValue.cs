// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Unity.HandTracking
{
  public class HandTrackingValue
  {
    public readonly List<Detection> palmDetections;
    public readonly List<NormalizedRect> handRectsFromPalmDetections;
    public readonly List<NormalizedLandmarkList> handLandmarks;
    public readonly List<LandmarkList> handWorldLandmarks;
    public readonly List<NormalizedRect> handRectsFromLandmarks;
    public readonly List<ClassificationList> handedness;

    public HandTrackingValue(List<Detection> palmDetections, List<NormalizedRect> handRectsFromPalmDetections,
                             List<NormalizedLandmarkList> handLandmarks, List<LandmarkList> handWorldLandmarks,
                             List<NormalizedRect> handRectsFromLandmarks, List<ClassificationList> handedness)
    {
      this.palmDetections = palmDetections;
      this.handRectsFromPalmDetections = handRectsFromPalmDetections;
      this.handLandmarks = handLandmarks;
      this.handWorldLandmarks = handWorldLandmarks;
      this.handRectsFromLandmarks = handRectsFromLandmarks;
      this.handedness = handedness;
    }
  }
}
