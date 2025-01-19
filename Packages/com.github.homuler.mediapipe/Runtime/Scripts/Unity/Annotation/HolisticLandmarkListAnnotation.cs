// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

using mptcc = Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Unity
{
  public sealed class HolisticLandmarkListAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private FaceLandmarkListWithIrisAnnotation _faceLandmarkListAnnotation;
    [SerializeField] private PoseLandmarkListAnnotation _poseLandmarkListAnnotation;
    [SerializeField] private HandLandmarkListAnnotation _leftHandLandmarkListAnnotation;
    [SerializeField] private HandLandmarkListAnnotation _rightHandLandmarkListAnnotation;
    [SerializeField] private ConnectionListAnnotation _connectionListAnnotation;

    public override bool isMirrored
    {
      set
      {
        _faceLandmarkListAnnotation.isMirrored = value;
        _poseLandmarkListAnnotation.isMirrored = value;
        _leftHandLandmarkListAnnotation.isMirrored = value;
        _rightHandLandmarkListAnnotation.isMirrored = value;
        _connectionListAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        _faceLandmarkListAnnotation.rotationAngle = value;
        _poseLandmarkListAnnotation.rotationAngle = value;
        _leftHandLandmarkListAnnotation.rotationAngle = value;
        _rightHandLandmarkListAnnotation.rotationAngle = value;
        _connectionListAnnotation.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    private void Start()
    {
      _leftHandLandmarkListAnnotation.SetHandedness(HandLandmarkListAnnotation.Hand.Left);
      _rightHandLandmarkListAnnotation.SetHandedness(HandLandmarkListAnnotation.Hand.Right);
      _connectionListAnnotation.Fill(2); // left/right wrist joint
    }

    public void Draw(IReadOnlyList<NormalizedLandmark> faceLandmarks, IReadOnlyList<NormalizedLandmark> poseLandmarks,
                     IReadOnlyList<NormalizedLandmark> leftHandLandmarks, IReadOnlyList<NormalizedLandmark> rightHandLandmarks, bool visualizeZ = false, int circleVertices = 128)
    {
      var mask = PoseLandmarkListAnnotation.BodyParts.All;
      if (faceLandmarks != null)
      {
        mask ^= PoseLandmarkListAnnotation.BodyParts.Face;
      }
      if (leftHandLandmarks != null)
      {
        mask ^= PoseLandmarkListAnnotation.BodyParts.LeftHand;
      }
      if (rightHandLandmarks != null)
      {
        mask ^= PoseLandmarkListAnnotation.BodyParts.RightHand;
      }
      _faceLandmarkListAnnotation.Draw(faceLandmarks, visualizeZ, circleVertices);
      _poseLandmarkListAnnotation.Draw(poseLandmarks, mask, visualizeZ);
      _leftHandLandmarkListAnnotation.Draw(leftHandLandmarks, visualizeZ);
      _rightHandLandmarkListAnnotation.Draw(rightHandLandmarks, visualizeZ);
      RedrawWristJoints();
    }

    public void Draw(NormalizedLandmarkList faceLandmarks, NormalizedLandmarkList poseLandmarks,
                     NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks, bool visualizeZ = false, int circleVertices = 128)
    {
      Draw(
        faceLandmarks?.Landmark,
        poseLandmarks?.Landmark,
        leftHandLandmarks?.Landmark,
        rightHandLandmarks?.Landmark,
        visualizeZ,
        circleVertices
      );
    }

    public void Draw(mptcc.NormalizedLandmarks faceLandmarks, mptcc.NormalizedLandmarks poseLandmarks,
                     mptcc.NormalizedLandmarks leftHandLandmarks, mptcc.NormalizedLandmarks rightHandLandmarks, bool visualizeZ = false, int circleVertices = 128)
    {
      var mask = PoseLandmarkListAnnotation.BodyParts.All;
      if (faceLandmarks.landmarks != null)
      {
        mask ^= PoseLandmarkListAnnotation.BodyParts.Face;
      }
      if (leftHandLandmarks.landmarks != null)
      {
        mask ^= PoseLandmarkListAnnotation.BodyParts.LeftHand;
      }
      if (rightHandLandmarks.landmarks != null)
      {
        mask ^= PoseLandmarkListAnnotation.BodyParts.RightHand;
      }
      _faceLandmarkListAnnotation.Draw(faceLandmarks, visualizeZ, circleVertices);
      _poseLandmarkListAnnotation.Draw(poseLandmarks, mask, visualizeZ);
      _leftHandLandmarkListAnnotation.Draw(leftHandLandmarks, visualizeZ);
      _rightHandLandmarkListAnnotation.Draw(rightHandLandmarks, visualizeZ);

      if (leftHandLandmarks.landmarks != null)
      {
        RedrawLeftWristJoint();
      }
      if (rightHandLandmarks.landmarks != null)
      {
        RedrawRightWristJoint();
      }
    }

    private void RedrawWristJoints()
    {
      if (_connectionListAnnotation[0].isEmpty && _poseLandmarkListAnnotation.count > 0 && _leftHandLandmarkListAnnotation.count > 0)
      {
        // connect left elbow and wrist
        _connectionListAnnotation[0].Draw(new Connection(_poseLandmarkListAnnotation[13], _leftHandLandmarkListAnnotation[0]));
      }
      if (_connectionListAnnotation[1].isEmpty && _poseLandmarkListAnnotation.count > 0 && _rightHandLandmarkListAnnotation.count > 0)
      {
        // connect right elbow and wrist
        _connectionListAnnotation[1].Draw(new Connection(_poseLandmarkListAnnotation[14], _rightHandLandmarkListAnnotation[0]));
      }
      _connectionListAnnotation.Redraw();
    }

    private void RedrawLeftWristJoint()
    {
      if (_connectionListAnnotation[0].isEmpty && _poseLandmarkListAnnotation.count > 0 && _leftHandLandmarkListAnnotation.count > 0)
      {
        // connect left elbow and wrist
        _connectionListAnnotation[0].Draw(new Connection(_poseLandmarkListAnnotation[13], _leftHandLandmarkListAnnotation[0]));
      }
      _connectionListAnnotation[0].Redraw();
    }

    private void RedrawRightWristJoint()
    {
      if (_connectionListAnnotation[1].isEmpty && _poseLandmarkListAnnotation.count > 0 && _rightHandLandmarkListAnnotation.count > 0)
      {
        // connect right elbow and wrist
        _connectionListAnnotation[1].Draw(new Connection(_poseLandmarkListAnnotation[14], _rightHandLandmarkListAnnotation[0]));
      }
      _connectionListAnnotation[1].Redraw();
    }
  }
}
