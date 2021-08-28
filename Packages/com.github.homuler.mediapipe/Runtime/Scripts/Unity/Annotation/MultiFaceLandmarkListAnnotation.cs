using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class MultiFaceLandmarkListAnnotation : Annotation<IList<NormalizedLandmarkList>>, I3DAnnotatable {
    [SerializeField] GameObject faceLandmarkListAnnotationPrefab;
    [SerializeField] Color landmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 10.0f;
    [SerializeField] Color connectionColor = Color.red;
    [SerializeField, Range(0, 1)] float connectionWidth = 1.0f;
    [SerializeField] bool visualizeZ = false;

    List<FaceLandmarkListAnnotation> _faceLandmarkLists;
    List<FaceLandmarkListAnnotation> faceLandmarkLists {
      get {
        if (_faceLandmarkLists == null) {
          _faceLandmarkLists = new List<FaceLandmarkListAnnotation>();
        }
        return _faceLandmarkLists;
      }
    }

    public override bool isMirrored {
      set {
        foreach (var faceLandmarkList in faceLandmarkLists) {
          faceLandmarkList.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void Destroy() {
      foreach (var faceLandmarkList in faceLandmarkLists) {
        Destroy(faceLandmarkList);
      }
      _faceLandmarkLists = null;
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      this.landmarkRadius = landmarkRadius;
      foreach (var faceLandmarkList in faceLandmarkLists) {
        faceLandmarkList.SetLandmarkRadius(landmarkRadius);
      }
    }

    public void SetLandmarkColor(Color landmarkColor) {
      this.landmarkColor = landmarkColor;
      foreach (var faceLandmarkList in faceLandmarkLists) {
        faceLandmarkList.SetLandmarkColor(landmarkColor);
      }
    }

    public void SetConnectionWidth(float connectionWidth) {
      this.connectionWidth = connectionWidth;
      foreach (var faceLandmarkList in faceLandmarkLists) {
        faceLandmarkList.SetConnectionWidth(connectionWidth);
      }
    }

    public void SetConnectionColor(Color connectionColor) {
      this.connectionColor = connectionColor;
      foreach (var faceLandmarkList in faceLandmarkLists) {
        faceLandmarkList.SetConnectionColor(connectionColor);
      }
    }

    public void VisualizeZ(bool flag) {
      this.visualizeZ = flag;
      foreach (var faceLandmarkList in faceLandmarkLists) {
        faceLandmarkList.VisualizeZ(flag);
      }
    }

    protected override void Draw(IList<NormalizedLandmarkList> target) {
      SetTargetAll(faceLandmarkLists, target, InitializeFaceLandmarkListAnnotation);
    }

    protected FaceLandmarkListAnnotation InitializeFaceLandmarkListAnnotation() {
      var annotation = InstantiateChild<FaceLandmarkListAnnotation, IList<NormalizedLandmark>>(faceLandmarkListAnnotationPrefab);
      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.SetLandmarkColor(landmarkColor);
      annotation.SetConnectionWidth(connectionWidth);
      annotation.SetConnectionColor(connectionColor);
      annotation.VisualizeZ(visualizeZ);
      return annotation;
    }
  }
}
