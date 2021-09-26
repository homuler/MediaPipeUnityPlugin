using UnityEngine;

namespace Mediapipe.Unity {
  public static class ImageSourceProvider {
    public static ImageSource imageSource { get; private set; }

    public static void SwitchSource(ImageSource.SourceType sourceType) {
      var obj = GameObject.Find("Image Source");

      switch (sourceType) {
        case ImageSource.SourceType.Camera: {
          imageSource = obj.GetComponent<WebCamSource>();
          break;
        }
        case ImageSource.SourceType.Image: {
          imageSource = obj.GetComponent<StaticImageSource>();
          break;
        }
        case ImageSource.SourceType.Video: {
          imageSource = obj.GetComponent<VideoSource>();
          break;
        }
      }
    }
  }
}
