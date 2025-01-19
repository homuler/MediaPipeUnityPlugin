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

  public sealed class HolisticLandmarkListWithMaskAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private RawImage _screen;
    [SerializeField] private HolisticLandmarkListAnnotation _holisticLandmarkListAnnotation;
    [SerializeField] private MaskOverlayAnnotation _maskOverlayAnnotation;

    public override bool isMirrored
    {
      set
      {
        _holisticLandmarkListAnnotation.isMirrored = value;
        _maskOverlayAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _holisticLandmarkListAnnotation.rotationAngle = value;
        _maskOverlayAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void InitMask(int width, int height) => _maskOverlayAnnotation.Init(_screen, width, height);

    public void SetMaskTexture(Texture2D maskTexture, Color color) => _maskOverlayAnnotation.SetMaskTexture(maskTexture, color);

    public void SetMaskThreshold(float threshold) => _maskOverlayAnnotation.SetThreshold(threshold);

    public void ReadMask(Image segmentationMask, bool isMirrored = false) => _maskOverlayAnnotation.Read(segmentationMask, isMirrored);

    public void Draw(mptcc.NormalizedLandmarks faceLandmarks, mptcc.NormalizedLandmarks poseLandmarks,
                     mptcc.NormalizedLandmarks leftHandLandmarks, mptcc.NormalizedLandmarks rightHandLandmarks, bool visualizeZ = false)
    {
      _holisticLandmarkListAnnotation.Draw(faceLandmarks, poseLandmarks, leftHandLandmarks, rightHandLandmarks, visualizeZ);
      _maskOverlayAnnotation.Draw();
    }
  }
}
