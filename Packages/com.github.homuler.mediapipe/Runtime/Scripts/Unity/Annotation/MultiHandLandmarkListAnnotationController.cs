using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class MultiHandLandmarkListAnnotationController : AnnotationController<MultiHandLandmarkListAnnotation, IList<NormalizedLandmarkList>> {
    [SerializeField] float landmarkRadius = 15.0f;
    [SerializeField] bool visualizeZ = false;

    List<ClassificationList> handedness;

    protected override void Start() {
      base.Start();
      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.VisualizeZ(visualizeZ);
    }

    protected override void LateUpdate() {
      if (isStale) {
        isStale = false;
        annotation.SetTarget(target);

        if (handedness != null) {
          annotation.SetClassificationList(handedness);
          handedness = null;
        }
      }
    }

    public void SetClassificationList(List<ClassificationList> handedness) {
      this.handedness = handedness;
      isStale = true;
    }
  }
}
