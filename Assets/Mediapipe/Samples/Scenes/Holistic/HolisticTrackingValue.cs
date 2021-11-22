// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity
{
  public class HolisticTrackingValue
  {
    public readonly Detection poseDetection;
    public readonly NormalizedLandmarkList poseLandmarks;
    public readonly NormalizedLandmarkList faceLandmarks;
    public readonly NormalizedLandmarkList leftHandLandmarks;
    public readonly NormalizedLandmarkList rightHandLandmarks;
    public readonly LandmarkList poseWorldLandmarks;
    public readonly NormalizedRect poseRoi;

    public HolisticTrackingValue(Detection poseDetection, NormalizedLandmarkList poseLandmarks,
                                 NormalizedLandmarkList faceLandmarks, NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks,
                                 LandmarkList poseWorldLandmarks, NormalizedRect poseRoi)
    {
      this.poseDetection = poseDetection;
      this.poseLandmarks = poseLandmarks;
      this.faceLandmarks = faceLandmarks;
      this.leftHandLandmarks = leftHandLandmarks;
      this.rightHandLandmarks = rightHandLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.poseRoi = poseRoi;
    }
  }
}
