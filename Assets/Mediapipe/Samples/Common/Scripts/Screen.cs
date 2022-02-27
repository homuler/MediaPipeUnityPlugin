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

    public Texture texture
    {
      private get => _screen.texture;
      set => _screen.texture = value;
    }

    public UnityEngine.Rect uvRect
    {
      set => _screen.uvRect = value;
    }

    public void Initialize(ImageSource imageSource)
    {
      _imageSource = imageSource;

      Resize(_imageSource.textureWidth, _imageSource.textureHeight);
      Rotate(_imageSource.rotation.Reverse());
      uvRect = GetUvRect(RunningMode.Async);
      texture = imageSource.GetCurrentTexture();
    }

    public void Resize(int width, int height)
    {
      _screen.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void Rotate(RotationAngle rotationAngle)
    {
      _screen.rectTransform.localEulerAngles = rotationAngle.GetEulerAngles();
    }

    public void ReadSync(TextureFrame textureFrame)
    {
      if (!(texture is Texture2D))
      {
        texture = new Texture2D(_imageSource.textureWidth, _imageSource.textureHeight, TextureFormat.RGBA32, false);
        uvRect = GetUvRect(RunningMode.Sync);
      }
      textureFrame.CopyTexture(texture);
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
