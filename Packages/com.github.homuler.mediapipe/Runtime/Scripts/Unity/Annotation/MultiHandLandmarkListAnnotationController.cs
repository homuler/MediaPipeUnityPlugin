using System.Collections.Generic;

namespace Mediapipe.Unity {
  public class MultiHandLandmarkListAnnotationController : AnnotationController<MultiHandLandmarkListAnnotation, IList<NormalizedLandmarkList>> {
    List<ClassificationList> handedness;

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
