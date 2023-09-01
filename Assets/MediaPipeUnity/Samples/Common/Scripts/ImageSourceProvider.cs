// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity.Sample
{
  public static class ImageSourceProvider
  {
    private static WebCamSource _WebCamSource;
    private static StaticImageSource _StaticImageSource;
    private static VideoSource _VideoSource;

    public static ImageSource ImageSource { get; private set; }

    public static ImageSourceType CurrentSourceType
    {
      get
      {
        if (ImageSource is WebCamSource)
        {
          return ImageSourceType.WebCamera;
        }
        if (ImageSource is StaticImageSource)
        {
          return ImageSourceType.Image;
        }
        if (ImageSource is VideoSource)
        {
          return ImageSourceType.Video;
        }
        return ImageSourceType.Unknown;
      }
    }

    internal static void Initialize(WebCamSource webCamSource, StaticImageSource staticImageSource, VideoSource videoSource)
    {
      _WebCamSource = webCamSource;
      _StaticImageSource = staticImageSource;
      _VideoSource = videoSource;
    }

    public static void Switch(ImageSourceType imageSourceType)
    {
      switch (imageSourceType)
      {
        case ImageSourceType.WebCamera:
          {
            ImageSource = _WebCamSource;
            break;
          }
        case ImageSourceType.Image:
          {
            ImageSource = _StaticImageSource;
            break;
          }
        case ImageSourceType.Video:
          {
            ImageSource = _VideoSource;
            break;
          }
        case ImageSourceType.Unknown:
        default:
          {
            throw new System.ArgumentException($"Unsupported source type: {imageSourceType}");
          }
      }
    }
  }
}
