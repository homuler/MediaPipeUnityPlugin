using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Objectron {
  public class ObjectronSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] ObjectronGraph graphRunner;
    [SerializeField] FrameAnnotationController liftedObjectsAnnotationController;
    [SerializeField] NormalizedRectListAnnotationController multiBoxRectsAnnotationController;
    [SerializeField] NormalizedLandmarkListAnnotationController multiBoxLandmarksAnnotationController;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public ObjectronGraph.Category category {
      get { return graphRunner.category; }
      set { graphRunner.category = value; }
    }

    public int maxNumObjects {
      get { return graphRunner.maxNumObjects; }
      set { graphRunner.maxNumObjects = value; }
    }

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

      Logger.LogInfo(TAG, $"Category = {category}");
      Logger.LogInfo(TAG, $"Max Num Objects = {maxNumObjects}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError) {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async) {
        graphRunner.OnLiftedObjectsOutput.AddListener(OnLiftedObjectsOutput);
        graphRunner.OnMultiBoxRectsOutput.AddListener(OnMultiBoxRectsOutput);
        graphRunner.OnMultiBoxLandmarksOutput.AddListener(OnMultiBoxLandmarksOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      liftedObjectsAnnotationController.focalLength = graphRunner.focalLength;
      liftedObjectsAnnotationController.principalPoint = graphRunner.principalPoint;
      liftedObjectsAnnotationController.isMirrored = imageSource.isHorizontallyFlipped;

      multiBoxRectsAnnotationController.isMirrored = imageSource.isHorizontallyFlipped;
      multiBoxLandmarksAnnotationController.isMirrored = imageSource.isHorizontallyFlipped;

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
          liftedObjectsAnnotationController.DrawNow(value.liftedObjects);
          multiBoxRectsAnnotationController.DrawNow(value.multiBoxRects);
          multiBoxLandmarksAnnotationController.DrawNow(value.multiBoxLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnLiftedObjectsOutput(FrameAnnotation liftedObjects) {
      liftedObjectsAnnotationController.DrawLater(liftedObjects);
    }

    void OnMultiBoxRectsOutput(List<NormalizedRect> multiBoxRects) {
      multiBoxRectsAnnotationController.DrawLater(multiBoxRects);
    }

    void OnMultiBoxLandmarksOutput(List<NormalizedLandmarkList> multiBoxLandmarks) {
      multiBoxLandmarksAnnotationController.DrawLater(multiBoxLandmarks);
    }
  }
}
