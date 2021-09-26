using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity {
  public class LabelAnnotation : HierarchicalAnnotation {
    [SerializeField] Text labelText;
    [SerializeField] Transform background;

    public void Draw(string text, Vector3 position, Color color, float maxWidth, float maxHeight) {
      if (ActivateFor(text)) {
        // move to the front to show background plane.
        labelText.transform.localPosition = new Vector3(position.x, position.y, -1);
        labelText.transform.localRotation = Quaternion.Euler(0, 0, -(int)rotationAngle);
        labelText.text = text;
        labelText.color = DecideTextColor(color);
        labelText.fontSize = GetFontSize(text, maxWidth, Mathf.Min(maxHeight, 48.0f));

        var width = Mathf.Min(labelText.preferredWidth + 24, maxWidth); // add margin
        var height = labelText.preferredHeight;
        var rectTransform = labelText.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(width, height);

        background.localScale = new Vector3(width / 10, 1, height / 10);
        background.gameObject.GetComponent<Renderer>().material.color = color;
      }
    }

    int GetFontSize(string text, float maxWidth, float maxHeight) {
      var ch = Mathf.Min(maxWidth / text.Length, maxHeight);
      return (int)Mathf.Clamp(ch, 24.0f, 72.0f);
    }

    Color DecideTextColor(Color backgroundColor) {
      var lw = CalcContrastRatio(Color.white, backgroundColor);
      var lb = CalcContrastRatio(backgroundColor, Color.black);
      return lw < lb ? Color.black : Color.white;
    }

    float CalcRelativeLuminance(Color color) {
      var r =  color.r <= 0.03928f ? color.r / 12.92f : Mathf.Pow((color.r + 0.055f) / 1.055f, 2.4f);
      var g =  color.g <= 0.03928f ? color.g / 12.92f : Mathf.Pow((color.g + 0.055f) / 1.055f, 2.4f);
      var b =  color.b <= 0.03928f ? color.b / 12.92f : Mathf.Pow((color.b + 0.055f) / 1.055f, 2.4f);
      return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }

    float CalcContrastRatio(Color lighter, Color darker) {
      var l1 = CalcRelativeLuminance(lighter);
      var l2 = CalcRelativeLuminance(darker);
      return (l1 + 0.05f) / (l2 + 0.05f);
    }
  }
}
