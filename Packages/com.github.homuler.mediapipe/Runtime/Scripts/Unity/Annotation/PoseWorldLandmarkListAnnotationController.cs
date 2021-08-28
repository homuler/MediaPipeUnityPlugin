using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class PoseWorldLandmarkListAnnotationController : AnnotationController<PoseWorldLandmarkListAnnotation, IList<Landmark>> {
    [SerializeField] float hipHeightMeter = 0.9f;
    [SerializeField] float landmarkRadius = 15.0f;
    [SerializeField] Vector3 scale = new Vector3(100, 100, 100);
    [SerializeField] bool visualizeZ = true;

    protected override void Start() {
      base.Start();
      transform.localPosition = new Vector3(0, hipHeightMeter * scale.y, 0);

      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.SetScale(scale);
      annotation.VisualizeZ(visualizeZ);
    }

    public void Draw(LandmarkList landmarkList) {
      Draw(landmarkList?.Landmark);
    }
  }
}
