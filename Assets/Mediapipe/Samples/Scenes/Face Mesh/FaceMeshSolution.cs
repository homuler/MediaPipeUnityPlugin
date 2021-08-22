using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.FaceMesh {
  public class FaceMeshSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] DetectionListAnnotationController detectionListAnnotationController;
    [SerializeField] NormalizedRectListAnnotationController normalizedRectListAnnotationController;
    [SerializeField] MultiNormalizedLandmarkListAnnotationController multiNormalizedLandmarkListAnnotationController;
    [SerializeField] FaceMeshGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public RunningMode runningMode;
    public int maxNumFaces {
      get { return graphRunner.maxNumFaces; }
      set { graphRunner.maxNumFaces = value; }
    }

    public override void Play() {
      base.Play();
      graphRunner.Initialize();

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
      ImageSourceProvider.imageSource.Stop();
      graphRunner.Stop();
    }

    IEnumerator Run() {
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared) {
        Debug.LogError("Failed to start ImageSource, exiting...");
        yield break;
      }

      screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      screen.texture = imageSource.GetCurrentTexture();

      var graphRunner = gameObject.GetComponent<FaceMeshGraph>();

      Debug.Log($"Max Num Faces: {maxNumFaces}");
      Debug.Log($"Running Mode: {runningMode}");

      if (runningMode == RunningMode.Async) {
        graphRunner.OnFacesDetected.AddListener(OnFacesDetected);
        graphRunner.OnFaceLandmarksDetected.AddListener(OnFaceLandmarksDetected);
        graphRunner.OnFaceRectsDetected.AddListener(OnFaceRectsDetected);
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

      detectionListAnnotationController.isMirrored = imageSource.isMirrored;
      normalizedRectListAnnotationController.isMirrored = imageSource.isMirrored;
      multiNormalizedLandmarkListAnnotationController.isMirrored = imageSource.isMirrored;

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
          detectionListAnnotationController.Draw(value.faceDetections);
          normalizedRectListAnnotationController.Draw(value.faceRectsFromLandmarks);
          multiNormalizedLandmarkListAnnotationController.Draw(value.multiFaceLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnFacesDetected(List<Detection> faceDetections) {
      detectionListAnnotationController.Draw(faceDetections);
    }

    void OnFaceLandmarksDetected(List<NormalizedLandmarkList> multiFaceLandmarks) {
      multiNormalizedLandmarkListAnnotationController.Draw(multiFaceLandmarks);
    }

    void OnFaceRectsDetected(List<NormalizedRect> faceRectsFromLandmarks) {
      normalizedRectListAnnotationController.Draw(faceRectsFromLandmarks);
    }
  }
}
