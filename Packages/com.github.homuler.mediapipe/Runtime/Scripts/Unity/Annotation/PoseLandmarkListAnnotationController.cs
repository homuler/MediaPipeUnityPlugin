using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class PoseLandmarkListAnnotationController : AnnotationController<PoseLandmarkListAnnotation, IList<NormalizedLandmark>> {
    [SerializeField] float landmarkRadius = 15.0f;
    [SerializeField] bool visualizeZ = false;

    protected override void Start() {
      base.Start();
      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.VisualizeZ(visualizeZ);
    }

    public void Draw(NormalizedLandmarkList normalizedLandmarkList) {
      Draw(normalizedLandmarkList?.Landmark);
    }
  }
}
