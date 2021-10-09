// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class LabelAnnotation : HierarchicalAnnotation
  {
    [SerializeField] private Text _labelText;
    [SerializeField] private Transform _backgroundTransform;

    public void Draw(string text, Vector3 position, Color color, float maxWidth, float maxHeight)
    {
      if (ActivateFor(text))
      {
        // move to the front to show background plane.
        _labelText.transform.localPosition = new Vector3(position.x, position.y, -1);
        _labelText.transform.localRotation = Quaternion.Euler(0, 0, -(int)rotationAngle);
        _labelText.text = text;
        _labelText.color = DecideTextColor(color);
        _labelText.fontSize = GetFontSize(text, maxWidth, Mathf.Min(maxHeight, 48.0f));

        var width = Mathf.Min(_labelText.preferredWidth + 24, maxWidth); // add margin
        var height = _labelText.preferredHeight;
        var rectTransform = _labelText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);

        _backgroundTransform.localScale = new Vector3(width / 10, 1, height / 10);
        _backgroundTransform.gameObject.GetComponent<Renderer>().material.color = color;
      }
    }

    private int GetFontSize(string text, float maxWidth, float maxHeight)
    {
      var ch = Mathf.Min(maxWidth / text.Length, maxHeight);
      return (int)Mathf.Clamp(ch, 24.0f, 72.0f);
    }

    private Color DecideTextColor(Color backgroundColor)
    {
      var lw = CalcContrastRatio(Color.white, backgroundColor);
      var lb = CalcContrastRatio(backgroundColor, Color.black);
      return lw < lb ? Color.black : Color.white;
    }

    private float CalcRelativeLuminance(Color color)
    {
      var r = color.r <= 0.03928f ? color.r / 12.92f : Mathf.Pow((color.r + 0.055f) / 1.055f, 2.4f);
      var g = color.g <= 0.03928f ? color.g / 12.92f : Mathf.Pow((color.g + 0.055f) / 1.055f, 2.4f);
      var b = color.b <= 0.03928f ? color.b / 12.92f : Mathf.Pow((color.b + 0.055f) / 1.055f, 2.4f);
      return (0.2126f * r) + (0.7152f * g) + (0.0722f * b);
    }

    private float CalcContrastRatio(Color lighter, Color darker)
    {
      var l1 = CalcRelativeLuminance(lighter);
      var l2 = CalcRelativeLuminance(darker);
      return (l1 + 0.05f) / (l2 + 0.05f);
    }
  }
}
