using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.PoseTracking {
  public class PoseTrackingSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] DetectionAnnotationController poseDetectionAnnotationController;
    [SerializeField] NormalizedLandmarkListAnnotationController poseLandmarksAnnotationController;
    [SerializeField] PoseTrackingGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public RunningMode runningMode;
    public PoseTrackingGraph.ModelComplexity modelComplexity {
      get { return graphRunner.modelComplexity; }
      set { graphRunner.modelComplexity = value; }
    }

    public bool smoothLandmarks {
      get { return graphRunner.smoothLandmarks; }
      set { graphRunner.smoothLandmarks = value; }
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
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }

      screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      screen.texture = imageSource.GetCurrentTexture();

      var graphRunner = gameObject.GetComponent<PoseTrackingGraph>();

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      if (runningMode == RunningMode.Async) {
        graphRunner.OnPoseDetectionOutput.AddListener(OnPoseDetectionOutput);
        graphRunner.OnPoseLandmarksOutput.AddListener(OnPoseLandmarksOutput);
        graphRunner.OnPoseWorldLandmarksOutput.AddListener(OnPoseWorldLandmarksOutput);
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

      poseDetectionAnnotationController.isMirrored = imageSource.isMirrored;
      poseLandmarksAnnotationController.isMirrored = imageSource.isMirrored;

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
          poseDetectionAnnotationController.Draw(value.poseDetection);
          poseLandmarksAnnotationController.Draw(value.poseLandmarks);
          // Logger.LogDebug($"Pose World Landmarks: {value.poseWorldLandmarks}");
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnPoseDetectionOutput(Detection poseDetection) {
      poseDetectionAnnotationController.Draw(poseDetection);
    }

    void OnPoseLandmarksOutput(NormalizedLandmarkList poseLandmarks) {
      poseLandmarksAnnotationController.Draw(poseLandmarks);
    }

    void OnPoseWorldLandmarksOutput(LandmarkList poseWorldLandmarks) {
      // Logger.LogDebug($"Pose World Landmarks: {poseWorldLandmarks}");
    }
  }
}
