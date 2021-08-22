using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class RectListAnnotation : Annotation<IList<Rect>> {
    [SerializeField] GameObject rectAnnotationPrefab;
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    List<RectAnnotation> _rects;
    List<RectAnnotation> rects {
      get {
        if (_rects == null) {
          _rects = new List<RectAnnotation>();
        }
        return _rects;
      }
    }

    public override bool isMirrored {
      set {
        foreach (var rect in rects) {
          rect.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void Destroy() {
      foreach (var rect in rects) {
        Destroy(rect);
      }
      _rects = null;
    }

    public void SetLineWidth(float lineWidth) {
      this.lineWidth = lineWidth;

      foreach (var rect in rects) {
        if (rect == null) {
          break;
        }
        rect.SetLineWidth(lineWidth);
      }
    }

    public void SetColor(Color color) {
      this.color = color;

      foreach (var rect in rects) {
        if (rect == null) {
          break;
        }
        rect.SetColor(color);
      }
    }

    protected override void Draw(IList<Rect> target) {
      SetTargetAll(rects, target, InitializeRectAnnotation);
    }

    protected RectAnnotation InitializeRectAnnotation() {
      var annotation = InstantiateChild<RectAnnotation, Rect>(rectAnnotationPrefab);
      annotation.SetLineWidth(lineWidth);
      annotation.SetColor(color);
      return annotation;
    }
  }
}
