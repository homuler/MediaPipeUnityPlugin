using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.FaceDetection {
  public class FaceDetectionSolution : SolutionBase {
    [SerializeField] UnityEngine.UI.RawImage screen;
    Coroutine coroutine;

    protected override IEnumerator Start() {
      yield return base.Start();

      coroutine = StartCoroutine(Run());
    }

    IEnumerator Run() {
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared) {
        Debug.LogError("Failed to start ImageSource, exiting...");
        yield break;
      }

      screen.rectTransform.sizeDelta = new Vector2(imageSource.width, imageSource.height);
      screen.texture = new Texture2D((int)imageSource.width, (int)imageSource.height, imageSource.format, false);

      while (true) {
        var textureFrameRequest = imageSource.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        textureFrame.CopyTexture(screen.texture);
        yield return new WaitForEndOfFrame();

        textureFrame.Release();
      }
    }
  }
}
