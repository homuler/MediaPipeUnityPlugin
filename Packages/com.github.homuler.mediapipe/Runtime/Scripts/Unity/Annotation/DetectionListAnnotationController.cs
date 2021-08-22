using System.Collections.Generic;

namespace Mediapipe.Unity {
  public class DetectionListAnnotationController : AnnotationController<DetectionListAnnotation, IList<Detection>> {
    public void Draw(DetectionList detectionList) {
      Draw(detectionList?.Detection);
    }
  }
}
