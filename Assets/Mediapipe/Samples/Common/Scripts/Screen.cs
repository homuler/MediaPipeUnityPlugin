// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity
{
  public class Screen : MonoBehaviour
  {
    [SerializeField] private RawImage _screen;

    private ImageSource _imageSource;

    public void Initialize(ImageSource imageSource)
    {
      _imageSource = imageSource;

      _screen.rectTransform.sizeDelta = new Vector2(_imageSource.textureWidth, _imageSource.textureHeight);
      _screen.rectTransform.localEulerAngles = _imageSource.rotation.Reverse().GetEulerAngles();
      _screen.uvRect = GetUvRect(RunningMode.Async);
      _screen.texture = imageSource.GetCurrentTexture();
    }

    public void ReadSync(TextureFrame textureFrame)
    {
      if (!(_screen.texture is Texture2D))
      {
        _screen.texture = new Texture2D(_imageSource.textureWidth, _imageSource.textureHeight, TextureFormat.RGBA32, false);
        _screen.uvRect = GetUvRect(RunningMode.Sync);
      }
      textureFrame.CopyTexture(_screen.texture);
    }

    private UnityEngine.Rect GetUvRect(RunningMode runningMode)
    {
      var rect = new UnityEngine.Rect(0, 0, 1, 1);

      if (_imageSource.isFrontFacing)
      {
        rect.x = 1;
        rect.width = -1;
      }
      if (_imageSource.isVerticallyFlipped && runningMode == RunningMode.Async)
      {
        rect.y = 1;
        rect.height = -1;
      }

      return rect;
    }
  }
}
