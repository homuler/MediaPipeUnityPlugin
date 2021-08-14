using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.FaceDetection {
  public class FaceDetectionSolution : SolutionBase {
    [SerializeField] UnityEngine.UI.RawImage screen;
    Coroutine coroutine;

    public override void Play() {
      base.Play();

      if (coroutine != null) {
        StopCoroutine(coroutine);
      }
      coroutine = StartCoroutine(Run());
    }

    public override void Pause() {
      base.Pause();
      ImageSourceProvider.imageSource.Pause();
    }

    public override void Resume() {
      base.Resume();
      StartCoroutine(ImageSourceProvider.imageSource.Resume());
    }

    public override void Stop() {
      base.Stop();
      StopCoroutine(coroutine);
    }

    IEnumerator Run() {
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared) {
        Debug.LogError("Failed to start ImageSource, exiting...");
        yield break;
      }

      screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      screen.texture = new Texture2D(imageSource.textureWidth, imageSource.textureHeight, imageSource.textureFormat, false);

      while (true) {
        yield return new WaitWhile(() => isPaused);

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
