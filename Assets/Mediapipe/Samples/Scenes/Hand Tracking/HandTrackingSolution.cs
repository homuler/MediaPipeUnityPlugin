using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.HandTracking {
  public class HandTrackingSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] DetectionListAnnotationController palmDetectionsAnnotationController;
    [SerializeField] NormalizedRectListAnnotationController handRectsFromPalmDetectionsAnnotationController;
    [SerializeField] MultiHandLandmarkListAnnotationController handLandmarksAnnotationController;
    [SerializeField] NormalizedRectListAnnotationController handRectsFromLandmarksAnnotationController;
    [SerializeField] HandTrackingGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public RunningMode runningMode;

    public int maxNumHands {
      get { return graphRunner.maxNumHands; }
      set { graphRunner.maxNumHands = value; }
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

      Logger.LogInfo(TAG, $"Max Num Hands = {maxNumHands}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError) {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async) {
        graphRunner.OnPalmDetectectionsOutput.AddListener(OnPalmDetectectionsOutput);
        graphRunner.OnHandRectsFromPalmDetectionsOutput.AddListener(OnHandRectsFromPalmDetectionsOutput);
        graphRunner.OnHandLandmarksOutput.AddListener(OnHandLandmarksOutput);
        graphRunner.OnHandRectsFromLandmarksOutput.AddListener(OnHandRectsFromLandmarksOutput);
        graphRunner.OnHandednessOutput.AddListener(OnHandednessOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      // The input image is flipped if it's **not** mirrored
      palmDetectionsAnnotationController.isMirrored = !imageSource.isHorizontallyFlipped;
      handRectsFromPalmDetectionsAnnotationController.isMirrored = !imageSource.isHorizontallyFlipped;
      handLandmarksAnnotationController.isMirrored = !imageSource.isHorizontallyFlipped;
      handRectsFromLandmarksAnnotationController.isMirrored = !imageSource.isHorizontallyFlipped;

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
          palmDetectionsAnnotationController.DrawNow(value.palmDetections);
          handRectsFromPalmDetectionsAnnotationController.DrawNow(value.handRectsFromPalmDetections);
          handLandmarksAnnotationController.DrawNow(value.handLandmarks, value.handedness);
          handRectsFromLandmarksAnnotationController.DrawNow(value.handRectsFromLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnPalmDetectectionsOutput(List<Detection> palmDetections) {
      palmDetectionsAnnotationController.DrawLater(palmDetections);
    }

    void OnHandRectsFromPalmDetectionsOutput(List<NormalizedRect> handRectsFromPalmDetections) {
      handRectsFromPalmDetectionsAnnotationController.DrawLater(handRectsFromPalmDetections);
    }

    void OnHandLandmarksOutput(List<NormalizedLandmarkList> handLandmarks) {
      handLandmarksAnnotationController.DrawLater(handLandmarks);
    }

    void OnHandRectsFromLandmarksOutput(List<NormalizedRect> handRectsFromLandmarks) {
      handRectsFromLandmarksAnnotationController.DrawLater(handRectsFromLandmarks);
    }

    void OnHandednessOutput(List<ClassificationList> handedness) {
      handLandmarksAnnotationController.DrawLater(handedness);
    }
  }
}
