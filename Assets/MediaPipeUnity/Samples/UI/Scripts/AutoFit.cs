// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

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

    [SerializeField] private FitMode _fitMode;

    private void LateUpdate()
    {
      var rectTransform = GetComponent<RectTransform>();
      if (rectTransform.rect.width == 0 || rectTransform.rect.height == 0)
      {
        return;
      }

      var parentRect = gameObject.transform.parent.gameObject.GetComponent<RectTransform>().rect;
      var (width, height) = GetBoundingBoxSize(rectTransform);

      var ratio = parentRect.width / width;
      var h = height * ratio;

      if (_fitMode == FitMode.FitWidth || (_fitMode == FitMode.Expand && h >= parentRect.height) || (_fitMode == FitMode.Shrink && h <= parentRect.height))
      {
        rectTransform.offsetMin *= ratio;
        rectTransform.offsetMax *= ratio;
        return;
      }

      ratio = parentRect.height / height;

      rectTransform.offsetMin *= ratio;
      rectTransform.offsetMax *= ratio;
    }

    private (float, float) GetBoundingBoxSize(RectTransform rectTransform)
    {
      var rect = rectTransform.rect;
      var center = rect.center;
      var topLeftRel = new Vector2(rect.xMin - center.x, rect.yMin - center.y);
      var topRightRel = new Vector2(rect.xMax - center.x, rect.yMin - center.y);
      var rotatedTopLeftRel = rectTransform.localRotation * topLeftRel;
      var rotatedTopRightRel = rectTransform.localRotation * topRightRel;
      var wMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.x), Mathf.Abs(rotatedTopRightRel.x));
      var hMax = Mathf.Max(Mathf.Abs(rotatedTopLeftRel.y), Mathf.Abs(rotatedTopRightRel.y));
      return (2 * wMax, 2 * hMax);
    }
  }
}
