using UnityEngine;

namespace Mediapipe.Unity {
  public class AutoFit : MonoBehaviour {
    [System.Serializable]
    public enum FitMode {
      Expand,
      Shrink,
      FitWidth,
      FitHeight,
    }

    [SerializeField] FitMode fitMode;

    void LateUpdate() {
      var rectTransform = GetComponent<RectTransform>();
      var rect = GetComponent<RectTransform>().rect;
      var parentRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>().rect;

      if (rect.width == 0 || rect.height == 0) {
        return;
      }

      var ratio = parentRect.width / rect.width;
      var w = parentRect.width;
      var h = rect.height * ratio;

      if (fitMode == FitMode.FitWidth || (fitMode == FitMode.Expand && h >= parentRect.height) || (fitMode == FitMode.Shrink && h <= parentRect.height)) {
        rectTransform.offsetMin *= ratio;
        rectTransform.offsetMax *= ratio;
        return;
      }

      ratio = parentRect.height / rect.height;
      w = rect.width * ratio;
      h = parentRect.height;

      rectTransform.offsetMin *= ratio;
      rectTransform.offsetMax *= ratio;
    }
  }
}