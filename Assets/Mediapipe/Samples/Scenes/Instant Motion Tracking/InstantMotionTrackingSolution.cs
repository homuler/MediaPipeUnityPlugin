using Mediapipe.Unity.CoordinateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.InstantMotionTracking {
  public class InstantMotionTrackingSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] Anchor3dAnnotationController trackedAnchorDataAnnotationController;
    [SerializeField] RegionTrackingGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public long timeoutMillisec {
      get { return graphRunner.timeoutMillisec; }
      set { graphRunner.SetTimeoutMillisec(value); }
    }

    public RunningMode runningMode;

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

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
        var rectTransform = screen.GetComponent<RectTransform>();

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main)) {
          if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out var localPoint)) {
            var normalizedPoint = rectTransform.GetNormalizedPosition(localPoint, graphRunner.rotation, ImageSourceProvider.imageSource.isHorizontallyFlipped);
            graphRunner.ResetAnchor(normalizedPoint.x, normalizedPoint.y);
            trackedAnchorDataAnnotationController.ResetAnchor();
          }
        }
      }
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
      SetupScreen(screen, imageSource);
      screen.texture = imageSource.GetCurrentTexture();

      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError) {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      graphRunner.ResetAnchor();

      if (runningMode == RunningMode.Async) {
        graphRunner.OnTrackedAnchorDataOutput.AddListener(OnTrackedAnchorDataOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      SetupAnnotationController(trackedAnchorDataAnnotationController, imageSource);
      trackedAnchorDataAnnotationController.ResetAnchor();

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
          trackedAnchorDataAnnotationController.DrawNow(value);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnTrackedAnchorDataOutput(List<Anchor3d> trackedAnchorData) {
      trackedAnchorDataAnnotationController.DrawLater(trackedAnchorData);
    }
  }
}
