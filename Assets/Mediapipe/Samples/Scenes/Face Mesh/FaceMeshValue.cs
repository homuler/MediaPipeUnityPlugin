// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Unity.FaceMesh
{
  public class FaceMeshValue
  {
    public readonly List<Detection> faceDetections;
    public readonly List<NormalizedLandmarkList> multiFaceLandmarks;
    public readonly List<NormalizedRect> faceRectsFromLandmarks;
    public readonly List<NormalizedRect> faceRectsFromDetections;

    public FaceMeshValue(List<Detection> faceDetections, List<NormalizedLandmarkList> multiFaceLandmarks, List<NormalizedRect> faceRectsFromLandmarks, List<NormalizedRect> faceRectsFromDetections)
    {
      this.faceDetections = faceDetections;
      this.multiFaceLandmarks = multiFaceLandmarks;
      this.faceRectsFromLandmarks = faceRectsFromLandmarks;
      this.faceRectsFromDetections = faceRectsFromDetections;
    }
  }
}
