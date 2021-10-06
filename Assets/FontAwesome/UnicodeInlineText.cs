using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UnicodeInlineText : Text
{
  private bool disableDirty = false;
  private Regex regexp = new Regex(@"\\u(?<Value>[a-zA-Z0-9]+)");

  protected override void OnPopulateMesh(VertexHelper vh)
  {
    string cache = text;
    disableDirty = true;
    text = Decode(text);
    base.OnPopulateMesh(vh);
    text = cache;
    disableDirty = false;
  }

  private string Decode(string value)
  {
    return regexp.Replace(value, m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
  }

  public override void SetLayoutDirty()
  {
    if (disableDirty) return;
    base.SetLayoutDirty();
  }

  public override void SetVerticesDirty()
  {
    if (disableDirty) return;
    base.SetVerticesDirty();
  }

  public override void SetMaterialDirty()
  {
    if (disableDirty) return;
    base.SetMaterialDirty();
  }
}
