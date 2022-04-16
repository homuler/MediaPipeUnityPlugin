// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity
{
  public static class ImageSourceProvider
  {
    private static ImageSource _ImageSource;
    public static ImageSource ImageSource
    {
      get => _ImageSource;
      set
      {
        if (value != null && !value.enabled)
        {
          value.enabled = true;
        }
        _ImageSource = value;
      }
    }

    public static ImageSourceType CurrentSourceType
    {
      get
      {
        if (_ImageSource is WebCamSource)
        {
          return ImageSourceType.WebCamera;
        }
        if (_ImageSource is StaticImageSource)
        {
          return ImageSourceType.Image;
        }
        if (_ImageSource is VideoSource)
        {
          return ImageSourceType.Video;
        }
        return ImageSourceType.Unknown;
      }
    }
  }
}
