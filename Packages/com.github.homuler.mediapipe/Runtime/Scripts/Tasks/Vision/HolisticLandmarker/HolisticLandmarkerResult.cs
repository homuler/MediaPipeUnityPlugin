// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.HolisticLandmarker
{
  /// <summary>
  ///   The holistic landmarks detection result from HolisticLandmarker, where each vector element represents a single holistic detected in the image.
  /// </summary>
  public readonly struct HolisticLandmarkerResult
  {
    /// <summary>
    ///   Detected face landmarks in normalized image coordinates.
    /// </summary>
    public readonly NormalizedLandmarks faceLandmarks;
    /// <summary>
    ///   Detected pose landmarks in normalized image coordinates.
    /// </summary>
    public readonly NormalizedLandmarks poseLandmarks;
    /// <summary>
    ///   Detected pose landmarks in world coordinates.
    /// </summary>
    public readonly Landmarks poseWorldLandmarks;
    /// <summary>
    ///   Detected left hand landmarks in normalized image coordinates.
    /// </summary>
    public readonly NormalizedLandmarks leftHandLandmarks;
    /// <summary>
    ///   Detected left hand landmarks in world coordinates.
    /// </summary>
    public readonly Landmarks leftHandWorldLandmarks;
    /// <summary>
    ///   Detected right hand landmarks in normalized image coordinates.
    /// </summary>
    public readonly NormalizedLandmarks rightHandLandmarks;
    /// <summary>
    ///   Detected right hand landmarks in world coordinates.
    /// </summary>
    public readonly Landmarks rightHandWorldLandmarks;
    /// <summary>
    ///   Optional face blendshapes.
    /// </summary>
    public readonly Classifications faceBlendshapes;
    /// <summary>
    ///   Optional segmentation masks for pose.
    ///   It must be disposed after use.
    /// </summary>
    public readonly Image segmentationMask;

    internal HolisticLandmarkerResult(NormalizedLandmarks faceLandmarks,
      NormalizedLandmarks poseLandmarks, Landmarks poseWorldLandmarks,
      NormalizedLandmarks leftHandLandmarks, Landmarks leftHandWorldLandmarks,
      NormalizedLandmarks rightHandLandmarks, Landmarks rightHandWorldLandmarks,
      Classifications faceBlendshapes = default, Image segmentationMask = null)
    {
      this.faceLandmarks = faceLandmarks;
      this.poseLandmarks = poseLandmarks;
      this.poseWorldLandmarks = poseWorldLandmarks;
      this.leftHandLandmarks = leftHandLandmarks;
      this.leftHandWorldLandmarks = leftHandWorldLandmarks;
      this.rightHandLandmarks = rightHandLandmarks;
      this.rightHandWorldLandmarks = rightHandWorldLandmarks;
      this.faceBlendshapes = faceBlendshapes;
      this.segmentationMask = segmentationMask;
    }

    /// <remarks>
    ///   Each <see cref="Image"/> in <see cref="segmentationMasks"/> will be moved to <paramref name="destination"/>.
    /// </remarks>
    public void CloneTo(ref HolisticLandmarkerResult destination)
    {
      var dstFaceLandmarks = destination.faceLandmarks;
      faceLandmarks.CloneTo(ref dstFaceLandmarks);

      var dstPoseLandmarks = destination.poseLandmarks;
      poseLandmarks.CloneTo(ref dstPoseLandmarks);

      var dstPoseWorldLandmarks = destination.poseWorldLandmarks;
      poseWorldLandmarks.CloneTo(ref dstPoseWorldLandmarks);

      var dstLeftHandLandmarks = destination.leftHandLandmarks;
      leftHandLandmarks.CloneTo(ref dstLeftHandLandmarks);

      var dstLeftHandWorldLandmarks = destination.leftHandWorldLandmarks;
      leftHandWorldLandmarks.CloneTo(ref dstLeftHandWorldLandmarks);

      var dstRightHandLandmarks = destination.rightHandLandmarks;
      rightHandLandmarks.CloneTo(ref dstRightHandLandmarks);

      var dstRightHandWorldLandmarks = destination.rightHandWorldLandmarks;
      rightHandWorldLandmarks.CloneTo(ref dstRightHandWorldLandmarks);

      var dstFaceBlendshapes = destination.faceBlendshapes;
      faceBlendshapes.CloneTo(ref dstFaceBlendshapes);

      var dstSegmentationMask = segmentationMask;

      destination = new HolisticLandmarkerResult(
        dstFaceLandmarks,
        dstPoseLandmarks,
        dstPoseWorldLandmarks,
        dstLeftHandLandmarks,
        dstLeftHandWorldLandmarks,
        dstRightHandLandmarks,
        dstRightHandWorldLandmarks,
        dstFaceBlendshapes,
        dstSegmentationMask
      );
    }

    public override string ToString()
      => $"{{ \"faceLandmarks\": {Util.Format(faceLandmarks)}, \"poseLandmarks\": {Util.Format(poseLandmarks)}, \"poseWorldLandmarks\": {Util.Format(poseWorldLandmarks)}, \"leftHandLandmarks\": {Util.Format(leftHandLandmarks)}, \"leftHandWorldLandmarks\": {Util.Format(leftHandWorldLandmarks)}, \"rightHandLandmarks\": {Util.Format(rightHandLandmarks)}, \"rightHandWorldLandmarks\": {Util.Format(rightHandWorldLandmarks)}, \"faceBlendshapes\": {Util.Format(faceBlendshapes)}, \"segmentationMask\": {Util.Format(segmentationMask)} }}";
  }
}
