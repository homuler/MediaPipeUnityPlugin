// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UnicodeInlineText : Text
{
  private bool _disableDirty = false;
  private readonly Regex _regexp = new Regex(@"\\u(?<Value>[a-zA-Z0-9]+)");

  protected override void OnPopulateMesh(VertexHelper vh)
  {
    var cache = text;
    _disableDirty = true;
    text = Decode(text);
    base.OnPopulateMesh(vh);
    text = cache;
    _disableDirty = false;
  }

  private string Decode(string value)
  {
    return _regexp.Replace(value, m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
  }

  public override void SetLayoutDirty()
  {
    if (_disableDirty)
    {
      return;
    }
    base.SetLayoutDirty();
  }

  public override void SetVerticesDirty()
  {
    if (_disableDirty)
    {
      return;
    }
    base.SetVerticesDirty();
  }

  public override void SetMaterialDirty()
  {
    if (_disableDirty)
    {
      return;
    }
    base.SetMaterialDirty();
  }
}
