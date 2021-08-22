using System.Collections.Generic;

namespace Mediapipe.Unity {
  public class NormalizedLandmarkListAnnotationController : AnnotationController<Annotation<IList<NormalizedLandmark>>, IList<NormalizedLandmark>> {
    public void Draw(NormalizedLandmarkList normalizedLandmarkList) {
      Draw(normalizedLandmarkList.Landmark);
    }
  }
}
