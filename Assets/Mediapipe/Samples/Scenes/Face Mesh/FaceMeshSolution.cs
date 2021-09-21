using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.FaceMesh {
  public class FaceMeshSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] DetectionListAnnotationController faceDetectionsAnnotationController;
    [SerializeField] MultiFaceLandmarkListAnnotationController multiFaceLandmarksAnnotationController;
    [SerializeField] NormalizedRectListAnnotationController faceRectsFromLandmarksAnnotationController;
    [SerializeField] FaceMeshGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public RunningMode runningMode;

    public int maxNumFaces {
      get { return graphRunner.maxNumFaces; }
      set { graphRunner.maxNumFaces = value; }
    }

    public long timeoutMillisec {
      get { return graphRunner.timeoutMillisec; }
      set { graphRunner.SetTimeoutMillisec(value); }
    }

    public override void Play() {
      if (coroutine != null) {
        Stop();
      }
      base.Play();
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
      ImageSourceProvider.imageSource.Stop();
      graphRunner.Stop();
    }

    IEnumerator Run() {
      var graphInitRequest = graphRunner.WaitForInit();
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared) {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      screen.texture = imageSource.GetCurrentTexture();

      Logger.LogInfo(TAG, $"Max Num Faces = {maxNumFaces}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      // Wait for completion of loading of dependent files, etc.
      yield return graphInitRequest;
      if (graphInitRequest.isError) {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async) {
        graphRunner.OnFaceDetectionsOutput.AddListener(OnFaceDetectionsOutput);
        graphRunner.OnMultiFaceLandmarksOutput.AddListener(OnMultiFaceLandmarksOutput);
        graphRunner.OnFaceRectsFromLandmarksOutput.AddListener(OnFaceRectsFromLandmarksOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      faceDetectionsAnnotationController.isMirrored = imageSource.isHorizontallyFlipped;
      faceRectsFromLandmarksAnnotationController.isMirrored = imageSource.isHorizontallyFlipped;
      multiFaceLandmarksAnnotationController.isMirrored = imageSource.isHorizontallyFlipped;

      while (true) {
        yield return new WaitWhile(() => isPaused);

        var textureFrameRequest = textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        // Copy current image to TextureFrame
        ReadFromImageSource(textureFrame, runningMode, graphRunner.configType);

        graphRunner.AddTextureFrameToInputStream(textureFrame).AssertOk();

        if (runningMode == RunningMode.Sync) {
          // When running synchronously, wait for the outputs here (blocks the main thread).
          var value = graphRunner.FetchNextValue();
          faceDetectionsAnnotationController.DrawNow(value.faceDetections);
          faceRectsFromLandmarksAnnotationController.DrawNow(value.faceRectsFromLandmarks);
          multiFaceLandmarksAnnotationController.DrawNow(value.multiFaceLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnFaceDetectionsOutput(List<Detection> faceDetections) {
      faceDetectionsAnnotationController.DrawLater(faceDetections);
    }

    void OnMultiFaceLandmarksOutput(List<NormalizedLandmarkList> multiFaceLandmarks) {
      multiFaceLandmarksAnnotationController.DrawLater(multiFaceLandmarks);
    }

    void OnFaceRectsFromLandmarksOutput(List<NormalizedRect> faceRectsFromLandmarks) {
      faceRectsFromLandmarksAnnotationController.DrawLater(faceRectsFromLandmarks);
    }
  }
}
