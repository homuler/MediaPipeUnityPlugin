using UnityEngine;

namespace Mediapipe.Unity {
  public static class ImageSourceProvider {
    public static ImageSource imageSource { get; private set; }

    public static void SwitchSource(ImageSource.SourceType sourceType) {
      var obj = GameObject.Find(GetSourceObjectName(sourceType));

      imageSource = obj.GetComponent<ImageSource>();
    }

    static string GetSourceObjectName(ImageSource.SourceType sourceType) {
      switch (sourceType) {
        case ImageSource.SourceType.Image: {
          return "Static Image Source";
        }
        case ImageSource.SourceType.Video: {
          return "Video Source";
        }
        default: {
          return "WebCam Source";
        }
      }
    }
  }
}
