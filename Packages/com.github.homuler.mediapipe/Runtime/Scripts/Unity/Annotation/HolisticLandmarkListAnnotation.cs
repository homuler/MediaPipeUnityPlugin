using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public sealed class HolisticLandmarkListAnnotation : HierarchicalAnnotation {
    [SerializeField] FaceLandmarkListAnnotation faceLandmarkList;
    [SerializeField] PoseLandmarkListAnnotation poseLandmarkList;
    [SerializeField] HandLandmarkListAnnotation leftHandLandmarkList;
    [SerializeField] HandLandmarkListAnnotation rightHandLandmarkList;
    [SerializeField] IrisLandmarkListAnnotation leftIrisLandmarkList;
    [SerializeField] IrisLandmarkListAnnotation rightIrisLandmarkList;
    [SerializeField] ConnectionListAnnotation connectionList;

    public override bool isMirrored {
      set {
        faceLandmarkList.isMirrored = value;
        poseLandmarkList.isMirrored = value;
        leftHandLandmarkList.isMirrored = value;
        rightHandLandmarkList.isMirrored = value;
        leftIrisLandmarkList.isMirrored = value;
        rightIrisLandmarkList.isMirrored = value;
        connectionList.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle {
      set {
        faceLandmarkList.rotationAngle = value;
        poseLandmarkList.rotationAngle = value;
        leftHandLandmarkList.rotationAngle = value;
        rightHandLandmarkList.rotationAngle = value;
        leftIrisLandmarkList.rotationAngle = value;
        rightIrisLandmarkList.rotationAngle = value;
        connectionList.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    void Start() {
      leftHandLandmarkList.SetHandedness(HandLandmarkListAnnotation.Hand.Left);
      rightHandLandmarkList.SetHandedness(HandLandmarkListAnnotation.Hand.Right);
      connectionList.Fill(2); // left/right wrist joint
    }

    public void Draw(IList<NormalizedLandmark> faceLandmarks, IList<NormalizedLandmark> poseLandmarks,
                     IList<NormalizedLandmark> leftHandLandmarks, IList<NormalizedLandmark> rightHandLandmarks,
                     IList<NormalizedLandmark> leftIrisLandmarks, IList<NormalizedLandmark> rightIrisLandmarks, bool visualizeZ = false, int circleVertices = 128) {
      var mask = PoseLandmarkListAnnotation.BodyParts.All;
      if (faceLandmarks != null) {
        mask ^= PoseLandmarkListAnnotation.BodyParts.Face;
      }
      if (leftHandLandmarks != null) {
        mask ^= PoseLandmarkListAnnotation.BodyParts.LeftHand;
      }
      if (rightHandLandmarks != null) {
        mask ^= PoseLandmarkListAnnotation.BodyParts.RightHand;
      }
      faceLandmarkList.Draw(faceLandmarks, visualizeZ);
      poseLandmarkList.Draw(poseLandmarks, mask, visualizeZ);
      leftHandLandmarkList.Draw(leftHandLandmarks, visualizeZ);
      rightHandLandmarkList.Draw(rightHandLandmarks, visualizeZ);
      leftIrisLandmarkList.Draw(leftIrisLandmarks, visualizeZ);
      rightIrisLandmarkList.Draw(rightIrisLandmarks, visualizeZ);
      RedrawWristJoints();
    }

    public void Draw(NormalizedLandmarkList faceLandmarks, NormalizedLandmarkList poseLandmarks,
                     NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks,
                     NormalizedLandmarkList leftIrisLandmarks, NormalizedLandmarkList rightIrisLandmarks, bool visualizeZ = false, int circleVertices = 128) {
      Draw(
        faceLandmarks?.Landmark, poseLandmarks?.Landmark, leftHandLandmarks?.Landmark, rightHandLandmarks?.Landmark, leftIrisLandmarks?.Landmark, rightIrisLandmarks?.Landmark, visualizeZ
      );
    }

    void RedrawWristJoints() {
      if (connectionList[0].isEmpty) {
        // connect left elbow and wrist
        connectionList[0].Draw(new Connection(poseLandmarkList[13], leftHandLandmarkList[0]));
      }
      if (connectionList[1].isEmpty) {
        // connect right elbow and wrist
        connectionList[1].Draw(new Connection(poseLandmarkList[14], rightHandLandmarkList[0]));
      }
      connectionList.Redraw();
    }
  }
}
