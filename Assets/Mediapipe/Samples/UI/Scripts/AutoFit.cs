using UnityEngine;

namespace Mediapipe.Unity
{
  public class AutoFit : MonoBehaviour
  {
    [System.Serializable]
    public enum FitMode
    {
      Expand,
      Shrink,
      FitWidth,
      FitHeight,
    }

    [SerializeField] FitMode fitMode;

    void LateUpdate()
    {
      var rectTransform = GetComponent<RectTransform>();
      if (rectTransform.rect.width == 0 || rectTransform.rect.height == 0)
      {
        return;
      }

      var parentRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>().rect;
      var (width, height) = GetBoundingBoxSize(rectTransform);

      var ratio = parentRect.width / width;
      var w = parentRect.width;
      var h = height * ratio;

      if (fitMode == FitMode.FitWidth || (fitMode == FitMode.Expand && h >= parentRect.height) || (fitMode == FitMode.Shrink && h <= parentRect.height))
      {
        rectTransform.offsetMin *= ratio;
        rectTransform.offsetMax *= ratio;
        return;
      }

      ratio = parentRect.height / height;
      w = width * ratio;
      h = parentRect.height;

      rectTransform.offsetMin *= ratio;
      rectTransform.offsetMax *= ratio;
    }

    (float, float) GetBoundingBoxSize(RectTransform rectTransform)
    {
      var rect = rectTransform.rect;
      var center = rect.center;
      var topLeftRel = new Vector2(rect.xMin - center.x, rect.yMin - center.y);
      var topRightRel = new Vector2(rect.xMax - center.x, rect.yMin - center.y);
      var rotatedTopLeftRel = rectTransform.rotation * topLeftRel;
      var rotatedTopRightRel = rectTransform.rotation * topRightRel;
      var wMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.x), Mathf.Abs(rotatedTopRightRel.x));
      var hMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.y), Mathf.Abs(rotatedTopRightRel.y));
      return (2 * wMax, 2 * hMax);
    }
  }
}
