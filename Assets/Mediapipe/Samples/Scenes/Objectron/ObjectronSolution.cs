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
      graphRunner.Initialize().AssertOk();
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
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared) {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }

      screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      screen.texture = imageSource.GetCurrentTexture();

      Logger.LogInfo(TAG, $"Category = {category}");
      Logger.LogInfo(TAG, $"Max Num Objects = {maxNumObjects}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      if (runningMode == RunningMode.Async) {
        graphRunner.OnLiftedObjectsOutput.AddListener(OnLiftedObjectsOutput);
        graphRunner.OnMultiBoxRectsOutput.AddListener(OnMultiBoxRectsOutput);
        graphRunner.OnMultiBoxLandmarksOutput.AddListener(OnMultiBoxLandmarksOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Decide which TextureFormat to use
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES) {
        // Use BGRA32 when the input packet is GpuBuffer
        textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.BGRA32);
      } else {
        // Use RGBA32 when the input packet is ImageFrame
        textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);
      }

      liftedObjectsAnnotationController.focalLength = graphRunner.focalLength;
      liftedObjectsAnnotationController.principalPoint = graphRunner.principalPoint;
      liftedObjectsAnnotationController.dimension = graphRunner.inputDimension;
      liftedObjectsAnnotationController.isMirrored = imageSource.isMirrored;

      multiBoxRectsAnnotationController.isMirrored = imageSource.isMirrored;
      multiBoxLandmarksAnnotationController.isMirrored = imageSource.isMirrored;

      while (true) {
        yield return new WaitWhile(() => isPaused);

        var textureFrameRequest = textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        var currentTexture = imageSource.GetCurrentTexture();

        // Copy currentTexture to textureFrame's texture
        if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES) {
          textureFrame.ReadTextureFromOnGPU(currentTexture);
        } else {
          var textureType = currentTexture.GetType();

          if (textureType == typeof(WebCamTexture)) {
            textureFrame.ReadTextureFromOnCPU((WebCamTexture)currentTexture);
          } else if (textureType == typeof(Texture2D)) {
            textureFrame.ReadTextureFromOnCPU((Texture2D)currentTexture);
          } else {
            textureFrame.ReadTextureFromOnCPU(currentTexture);
          }
        }

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
