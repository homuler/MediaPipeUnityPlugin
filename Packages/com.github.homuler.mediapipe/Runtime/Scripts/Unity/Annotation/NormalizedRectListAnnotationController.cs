using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class NormalizedRectListAnnotationController : AnnotationController<NormalizedRectListAnnotation, IList<NormalizedRect>> {
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    protected override void Start() {
      base.Start();
      annotation.SetColor(color);
      annotation.SetLineWidth(lineWidth);
    }
  }
}
