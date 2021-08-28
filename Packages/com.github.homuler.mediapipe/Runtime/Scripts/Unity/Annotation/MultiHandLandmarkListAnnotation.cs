using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {
  public class MultiHandLandmarkListAnnotation : Annotation<IList<NormalizedLandmarkList>>, I3DAnnotatable {
    [SerializeField] GameObject handLandmarkListAnnotationPrefab;
    [SerializeField] Color landmarkColor = Color.green;
    [SerializeField] Color leftLandmarkColor = Color.green;
    [SerializeField] Color rightLandmarkColor = Color.green;
    [SerializeField] float landmarkRadius = 15.0f;
    [SerializeField] Color connectionColor = Color.white;
    [SerializeField, Range(0, 1)] float connectionWidth = 1.0f;
    [SerializeField] bool visualizeZ = false;

    List<HandLandmarkListAnnotation> _handLandmarkLists;
    List<HandLandmarkListAnnotation> handLandmarkLists {
      get {
        if (_handLandmarkLists == null) {
          _handLandmarkLists = new List<HandLandmarkListAnnotation>();
        }
        return _handLandmarkLists;
      }
    }

    public override bool isMirrored {
      set {
        foreach (var handLandmarkList in handLandmarkLists) {
          handLandmarkList.isMirrored = value;
        }
        base.isMirrored = value;
      }
    }

    void Destroy() {
      foreach (var handLandmarkList in handLandmarkLists) {
        Destroy(handLandmarkList);
      }
      _handLandmarkLists = null;
    }

    public void SetLandmarkRadius(float landmarkRadius) {
      this.landmarkRadius = landmarkRadius;
      foreach (var handLandmarkList in handLandmarkLists) {
        handLandmarkList.SetLandmarkRadius(landmarkRadius);
      }
    }

    public void SetConnectionWidth(float connectionWidth) {
      this.connectionWidth = connectionWidth;
      foreach (var handLandmarkList in handLandmarkLists) {
        handLandmarkList.SetConnectionWidth(connectionWidth);
      }
    }

    public void SetClassificationList(List<ClassificationList> handedness) {
      var count = handedness == null ? 0 : handedness.Count;
      for (var i = 0; i < Mathf.Min(count, handLandmarkLists.Count); i++) {
        SetClassificationListAt(i, handedness[i].Classification);
      }
      for (var i = count; i < handLandmarkLists.Count; i++) {
        SetClassificationListAt(i, null);
      }
    }

    public void VisualizeZ(bool flag) {
      this.visualizeZ = flag;
      foreach (var handLandmarkList in handLandmarkLists) {
        handLandmarkList.VisualizeZ(flag);
      }
    }

    protected override void Draw(IList<NormalizedLandmarkList> target) {
      SetTargetAll(handLandmarkLists, target, InitializeHandLandmarkListAnnotation);
    }

    void SetClassificationListAt(int index, IList<Classification> handedness) {
      var annotation = handLandmarkLists[index];
      if (handedness == null || handedness.Count == 0) {
        annotation.SetLandmarkColor(landmarkColor);
      } else if (handedness[0].Label == "Left") {
        annotation.SetLandmarkColor(leftLandmarkColor);
      } else if (handedness[0].Label == "Right") {
        annotation.SetLandmarkColor(rightLandmarkColor);
      }
      // ignore unknown label
    }

    HandLandmarkListAnnotation InitializeHandLandmarkListAnnotation() {
      var annotation = InstantiateChild<HandLandmarkListAnnotation, IList<NormalizedLandmark>>(handLandmarkListAnnotationPrefab);
      annotation.SetLandmarkRadius(landmarkRadius);
      annotation.SetLandmarkColor(landmarkColor);
      annotation.SetConnectionWidth(connectionWidth);
      annotation.SetConnectionColor(connectionColor);
      annotation.VisualizeZ(visualizeZ);
      return annotation;
    }
  }
}
