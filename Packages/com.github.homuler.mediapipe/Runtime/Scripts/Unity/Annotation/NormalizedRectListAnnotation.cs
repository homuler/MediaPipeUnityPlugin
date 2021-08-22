using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class NormalizedRectListAnnotation : Annotation<IList<NormalizedRect>> {
    [SerializeField] GameObject normalizedRectAnnotationPrefab;
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    List<NormalizedRectAnnotation> _normalizedRects;
    List<NormalizedRectAnnotation> normalizedRects {
      get {
        if (_normalizedRects == null) {
          _normalizedRects = new List<NormalizedRectAnnotation>();
        }
        return _normalizedRects;
      }
    }

    public override bool isMirrored {
      set {
        foreach (var normalizedRect in normalizedRects) {
          normalizedRect.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void Destroy() {
      foreach (var normalizedRect in normalizedRects) {
        Destroy(normalizedRect);
      }
      _normalizedRects = null;
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;

      foreach (var normalizedRect in normalizedRects) {
        if (normalizedRect == null) {
          break;
        }
        normalizedRect.SetLineWidth(lineWidth);
      }
    }

    public void SetColor(Color color) {
      this.color = color;

      foreach (var normalizedRect in normalizedRects) {
        if (normalizedRect == null) {
          break;
        }
        normalizedRect.SetColor(color);
      }
    }

    protected override void Draw(IList<NormalizedRect> target) {
      SetTargetAll(normalizedRects, target, InitializeNormalizedRectAnnotation);
    }

    protected NormalizedRectAnnotation InitializeNormalizedRectAnnotation() {
      var annotation = InstantiateChild<NormalizedRectAnnotation, NormalizedRect>(normalizedRectAnnotationPrefab);
      annotation.SetLineWidth(lineWidth);
      annotation.SetColor(color);
      return annotation;
    }
  }
}
