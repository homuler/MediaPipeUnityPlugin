using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class FaceLandmarkListWithIrisAnnotation : Annotation<IList<NormalizedLandmark>>, IAnnotatable<NormalizedLandmarkList> {
    [SerializeField] GameObject faceLandmarkListAnnotationPrefab;
    [SerializeField] GameObject irisLandmarkListAnnotationPrefab;

    FaceLandmarkListAnnotation faceLandmarkListAnnotation;
    IrisLandmarkListAnnotation leftIrisLandmarkListAnnotation;
    IrisLandmarkListAnnotation rightIrisLandmarkListAnnotation;

    const int faceLandmarkCount = 468;

    void Start() {
      faceLandmarkListAnnotation = InstantiateChild<FaceLandmarkListAnnotation, IList<NormalizedLandmark>>(faceLandmarkListAnnotationPrefab);
      leftIrisLandmarkListAnnotation = InstantiateChild<IrisLandmarkListAnnotation, IList<NormalizedLandmark>>(irisLandmarkListAnnotationPrefab);
      rightIrisLandmarkListAnnotation = InstantiateChild<IrisLandmarkListAnnotation, IList<NormalizedLandmark>>(irisLandmarkListAnnotationPrefab);
    }

    public void SetTarget(NormalizedLandmarkList target) {
      SetTarget(target?.Landmark);
    }

    public override bool isMirrored {
      set {
        faceLandmarkListAnnotation.isMirrored = value;
        leftIrisLandmarkListAnnotation.isMirrored = value;
        rightIrisLandmarkListAnnotation.isMirrored = value;
        base.isMirrored = value;
      }
    }

    protected override void Draw(IList<NormalizedLandmark> target) {
      faceLandmarkListAnnotation.SetTarget(target);

      var offset = faceLandmarkCount;
      var leftIrisLandmarkList = new List<NormalizedLandmark> { target[offset + 0], target[offset + 1], target[offset + 2], target[offset + 3], target[offset + 4] };
      var rightIrisLandmarkList = new List<NormalizedLandmark> { target[offset + 5], target[offset + 6], target[offset + 7], target[offset + 8], target[offset + 9] };
      leftIrisLandmarkListAnnotation.SetTarget(leftIrisLandmarkList);
      rightIrisLandmarkListAnnotation.SetTarget(rightIrisLandmarkList);
    }
  }
}
