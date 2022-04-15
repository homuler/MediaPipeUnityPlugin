// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public static class ImageSourceProvider
  {
    public static ImageSource ImageSource { get; private set; }

    public static void SwitchSource(ImageSource.SourceType sourceType)
    {
      var obj = GameObject.Find("Image Source");

      switch (sourceType)
      {
        case ImageSource.SourceType.Camera:
          {
            ImageSource = obj.GetComponent<WebCamSource>();
            break;
          }
        case ImageSource.SourceType.Image:
          {
            ImageSource = obj.GetComponent<StaticImageSource>();
            break;
          }
        case ImageSource.SourceType.Video:
          {
            ImageSource = obj.GetComponent<VideoSource>();
            break;
          }
        default:
          {
            throw new System.ArgumentException($"Unknown Image Source: {sourceType}");
          }
      }
    }
  }
}
