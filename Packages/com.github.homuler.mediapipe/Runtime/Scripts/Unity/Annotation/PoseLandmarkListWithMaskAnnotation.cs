// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public sealed class PoseLandmarkListWithMaskAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private PoseLandmarkListAnnotation _poseLandmarkListAnnotation;
    [SerializeField] private MaskOverlayAnnotation _maskOverlayAnnotation;

    public override bool isMirrored
    {
      set
      {
        _poseLandmarkListAnnotation.isMirrored = value;
        _maskOverlayAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _poseLandmarkListAnnotation.rotationAngle = value;
        _maskOverlayAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void InitMask(RawImage screen, int width, int height) => _maskOverlayAnnotation.Init(screen, width, height);

    public void SetLeftLandmarkColor(Color leftLandmarkColor) => _poseLandmarkListAnnotation.SetLeftLandmarkColor(leftLandmarkColor);

    public void SetRightLandmarkColor(Color rightLandmarkColor) => _poseLandmarkListAnnotation.SetRightLandmarkColor(rightLandmarkColor);

    public void SetLandmarkRadius(float landmarkRadius) => _poseLandmarkListAnnotation.SetLandmarkRadius(landmarkRadius);

    public void SetConnectionColor(Color connectionColor) => _poseLandmarkListAnnotation.SetConnectionColor(connectionColor);

    public void SetConnectionWidth(float connectionWidth) => _poseLandmarkListAnnotation.SetConnectionWidth(connectionWidth);

    public void SetMaskTexture(Texture2D maskTexture, Color color) => _maskOverlayAnnotation.SetMaskTexture(maskTexture, color);

    public void SetMaskThreshold(float threshold) => _maskOverlayAnnotation.SetThreshold(threshold);

    public void ReadMask(Image segmentationMask, bool isMirrored = false) => _maskOverlayAnnotation.Read(segmentationMask, isMirrored);

    public void Draw(mptcc.NormalizedLandmarks poseLandmarks, bool visualizeZ = false)
    {
      if (ActivateFor(poseLandmarks.landmarks))
      {
        _poseLandmarkListAnnotation.Draw(poseLandmarks, visualizeZ);
        _maskOverlayAnnotation.Draw();
      }
    }
  }
}
