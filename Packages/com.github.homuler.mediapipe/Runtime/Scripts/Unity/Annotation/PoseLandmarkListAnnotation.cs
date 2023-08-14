// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public sealed class PoseLandmarkListAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private PointListAnnotation _landmarkListAnnotation;
    [SerializeField] private ConnectionListAnnotation _connectionListAnnotation;
    [SerializeField] private Color _leftLandmarkColor = Color.green;
    [SerializeField] private Color _rightLandmarkColor = Color.green;

    [Flags]
    public enum BodyParts : short
    {
      None = 0,
      Face = 1,
      // Torso = 2,
      LeftArm = 4,
      LeftHand = 8,
      RightArm = 16,
      RightHand = 32,
      LowerBody = 64,
      All = 127,
    }

    private const int _LandmarkCount = 33;
    private static readonly int[] _LeftLandmarks = new int[] {
      1, 2, 3, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31
    };
    private static readonly int[] _RightLandmarks = new int[] {
      4, 5, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32
    };
    private static readonly List<(int, int)> _Connections = new List<(int, int)> {
      // Left Eye
      (0, 1),
      (1, 2),
      (2, 3),
      (3, 7),
      // Right Eye
      (0, 4),
      (4, 5),
      (5, 6),
      (6, 8),
      // Lips
      (9, 10),
      // Left Arm
      (11, 13),
      (13, 15),
      // Left Hand
      (15, 17),
      (15, 19),
      (15, 21),
      (17, 19),
      // Right Arm
      (12, 14),
      (14, 16),
      // Right Hand
      (16, 18),
      (16, 20),
      (16, 22),
      (18, 20),
      // Torso
      (11, 12),
      (12, 24),
      (24, 23),
      (23, 11),
      // Left Leg
      (23, 25),
      (25, 27),
      (27, 29),
      (27, 31),
      (29, 31),
      // Right Leg
      (24, 26),
      (26, 28),
      (28, 30),
      (28, 32),
      (30, 32),
    };

    public override bool isMirrored
    {
      set
      {
        _landmarkListAnnotation.isMirrored = value;
        _connectionListAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _landmarkListAnnotation.rotationAngle = value;
        _connectionListAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public PointAnnotation this[int index] => _landmarkListAnnotation[index];

    private void Start()
    {
      _landmarkListAnnotation.Fill(_LandmarkCount);
      ApplyLeftLandmarkColor(_leftLandmarkColor);
      ApplyRightLandmarkColor(_rightLandmarkColor);

      _connectionListAnnotation.Fill(_Connections, _landmarkListAnnotation);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
      if (!UnityEditor.PrefabUtility.IsPartOfAnyPrefab(this))
      {
        ApplyLeftLandmarkColor(_leftLandmarkColor);
        ApplyRightLandmarkColor(_rightLandmarkColor);
      }
    }
#endif

    public void SetLeftLandmarkColor(Color leftLandmarkColor)
    {
      _leftLandmarkColor = leftLandmarkColor;
      ApplyLeftLandmarkColor(_leftLandmarkColor);
    }

    public void SetRightLandmarkColor(Color rightLandmarkColor)
    {
      _rightLandmarkColor = rightLandmarkColor;
      ApplyRightLandmarkColor(_rightLandmarkColor);
    }

    public void SetLandmarkRadius(float landmarkRadius)
    {
      _landmarkListAnnotation.SetRadius(landmarkRadius);
    }

    public void SetConnectionColor(Color connectionColor)
    {
      _connectionListAnnotation.SetColor(connectionColor);
    }

    public void SetConnectionWidth(float connectionWidth)
    {
      _connectionListAnnotation.SetLineWidth(connectionWidth);
    }

    public void Draw(IReadOnlyList<Landmark> target, Vector3 scale, bool visualizeZ = false)
    {
      if (ActivateFor(target))
      {
        _landmarkListAnnotation.Draw(target, scale, visualizeZ);
        // Draw explicitly because connection annotation's targets remain the same.
        _connectionListAnnotation.Redraw();
      }
    }

    public void Draw(LandmarkList target, Vector3 scale, bool visualizeZ = false)
    {
      Draw(target?.Landmark, scale, visualizeZ);
    }

    public void Draw(IReadOnlyList<NormalizedLandmark> target, BodyParts mask, bool visualizeZ = false)
    {
      if (ActivateFor(target))
      {
        _landmarkListAnnotation.Draw(target, visualizeZ);
        ApplyMask(mask);
        // Draw explicitly because connection annotation's targets remain the same.
        _connectionListAnnotation.Redraw();
      }
    }

    public void Draw(NormalizedLandmarkList target, BodyParts mask, bool visualizeZ = false)
    {
      Draw(target?.Landmark, mask, visualizeZ);
    }

    public void Draw(IReadOnlyList<mptcc.NormalizedLandmark> target, BodyParts mask, bool visualizeZ = false)
    {
      if (ActivateFor(target))
      {
        _landmarkListAnnotation.Draw(target, visualizeZ);
        ApplyMask(mask);
        // Draw explicitly because connection annotation's targets remain the same.
        _connectionListAnnotation.Redraw();
      }
    }

    public void Draw(mptcc.NormalizedLandmarks target, BodyParts mask, bool visualizeZ = false)
    {
      Draw(target.landmarks, mask, visualizeZ);
    }

    public void Draw(IReadOnlyList<NormalizedLandmark> target, bool visualizeZ = false)
    {
      Draw(target, BodyParts.All, visualizeZ);
    }

    public void Draw(NormalizedLandmarkList target, bool visualizeZ = false)
    {
      Draw(target?.Landmark, BodyParts.All, visualizeZ);
    }

    public void Draw(IReadOnlyList<mptcc.NormalizedLandmark> target, bool visualizeZ = false)
    {
      Draw(target, BodyParts.All, visualizeZ);
    }

    public void Draw(mptcc.NormalizedLandmarks target, bool visualizeZ = false)
    {
      Draw(target.landmarks, BodyParts.All, visualizeZ);
    }

    private void ApplyLeftLandmarkColor(Color color)
    {
      var annotationCount = _landmarkListAnnotation == null ? 0 : _landmarkListAnnotation.count;
      if (annotationCount >= _LandmarkCount)
      {
        foreach (var index in _LeftLandmarks)
        {
          _landmarkListAnnotation[index].SetColor(color);
        }
      }
    }

    private void ApplyRightLandmarkColor(Color color)
    {
      var annotationCount = _landmarkListAnnotation == null ? 0 : _landmarkListAnnotation.count;
      if (annotationCount >= _LandmarkCount)
      {
        foreach (var index in _RightLandmarks)
        {
          _landmarkListAnnotation[index].SetColor(color);
        }
      }
    }

    private void ApplyMask(BodyParts mask)
    {
      if (mask == BodyParts.All)
      {
        return;
      }

      if (!mask.HasFlag(BodyParts.Face))
      {
        // deactivate face landmarks
        for (var i = 0; i <= 10; i++)
        {
          _landmarkListAnnotation[i].SetActive(false);
        }
      }
      if (!mask.HasFlag(BodyParts.LeftArm))
      {
        // deactivate left elbow to hide left arm
        _landmarkListAnnotation[13].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.LeftHand))
      {
        // deactive left wrist, thumb, index and pinky to hide left hand
        _landmarkListAnnotation[15].SetActive(false);
        _landmarkListAnnotation[17].SetActive(false);
        _landmarkListAnnotation[19].SetActive(false);
        _landmarkListAnnotation[21].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.RightArm))
      {
        // deactivate right elbow to hide right arm
        _landmarkListAnnotation[14].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.RightHand))
      {
        // deactivate right wrist, thumb, index and pinky to hide right hand
        _landmarkListAnnotation[16].SetActive(false);
        _landmarkListAnnotation[18].SetActive(false);
        _landmarkListAnnotation[20].SetActive(false);
        _landmarkListAnnotation[22].SetActive(false);
      }
      if (!mask.HasFlag(BodyParts.LowerBody))
      {
        // deactivate lower body landmarks
        for (var i = 25; i <= 32; i++)
        {
          _landmarkListAnnotation[i].SetActive(false);
        }
      }
    }
  }
}
