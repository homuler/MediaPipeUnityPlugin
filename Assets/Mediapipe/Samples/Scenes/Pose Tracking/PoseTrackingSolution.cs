using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.PoseTracking
{
  public class PoseTrackingSolution : Solution
  {
    [SerializeField] RawImage screen;
    [SerializeField] RectTransform worldAnnotationArea;
    [SerializeField] DetectionAnnotationController poseDetectionAnnotationController;
    [SerializeField] PoseLandmarkListAnnotationController poseLandmarksAnnotationController;
    [SerializeField] PoseWorldLandmarkListAnnotationController poseWorldLandmarksAnnotationController;
    [SerializeField] NormalizedRectAnnotationController roiFromLandmarksAnnotationController;
    [SerializeField] PoseTrackingGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public RunningMode runningMode;

    public PoseTrackingGraph.ModelComplexity modelComplexity
    {
      get { return graphRunner.modelComplexity; }
      set { graphRunner.modelComplexity = value; }
    }

    public bool smoothLandmarks
    {
      get { return graphRunner.smoothLandmarks; }
      set { graphRunner.smoothLandmarks = value; }
    }

    public long timeoutMillisec
    {
      get { return graphRunner.timeoutMillisec; }
      set { graphRunner.SetTimeoutMillisec(value); }
    }

    public override void Play()
    {
      if (coroutine != null)
      {
        Stop();
      }
      base.Play();
      coroutine = StartCoroutine(Run());
    }

    public override void Pause()
    {
      base.Pause();
      ImageSourceProvider.imageSource.Pause();
    }

    public override void Resume()
    {
      base.Resume();
      StartCoroutine(ImageSourceProvider.imageSource.Resume());
    }

    public override void Stop()
    {
      base.Stop();
      StopCoroutine(coroutine);
      ImageSourceProvider.imageSource.Stop();
      graphRunner.Stop();
    }

    IEnumerator Run()
    {
      var graphInitRequest = graphRunner.WaitForInit();
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      SetupScreen(screen, imageSource);
      screen.texture = imageSource.GetCurrentTexture();
      worldAnnotationArea.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        graphRunner.OnPoseDetectionOutput.AddListener(OnPoseDetectionOutput);
        graphRunner.OnPoseLandmarksOutput.AddListener(OnPoseLandmarksOutput);
        graphRunner.OnPoseWorldLandmarksOutput.AddListener(OnPoseWorldLandmarksOutput);
        graphRunner.OnRoiFromLandmarksOutput.AddListener(OnRoiFromLandmarksOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      SetupAnnotationController(poseDetectionAnnotationController, imageSource);
      SetupAnnotationController(poseLandmarksAnnotationController, imageSource);
      SetupAnnotationController(poseWorldLandmarksAnnotationController, imageSource);
      SetupAnnotationController(roiFromLandmarksAnnotationController, imageSource);

      while (true)
      {
        yield return new WaitWhile(() => isPaused);

        var textureFrameRequest = textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        // Copy current image to TextureFrame
        ReadFromImageSource(textureFrame, runningMode, graphRunner.configType);

        graphRunner.AddTextureFrameToInputStream(textureFrame).AssertOk();

        if (runningMode == RunningMode.Sync)
        {
          // When running synchronously, wait for the outputs here (blocks the main thread).
          var value = graphRunner.FetchNextValue();
          poseDetectionAnnotationController.DrawNow(value.poseDetection);
          poseLandmarksAnnotationController.DrawNow(value.poseLandmarks);
          poseWorldLandmarksAnnotationController.DrawNow(value.poseWorldLandmarks);
          roiFromLandmarksAnnotationController.DrawNow(value.roiFromLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnPoseDetectionOutput(Detection poseDetection)
    {
      poseDetectionAnnotationController.DrawLater(poseDetection);
    }

    void OnPoseLandmarksOutput(NormalizedLandmarkList poseLandmarks)
    {
      poseLandmarksAnnotationController.DrawLater(poseLandmarks);
    }

    void OnPoseWorldLandmarksOutput(LandmarkList poseWorldLandmarks)
    {
      poseWorldLandmarksAnnotationController.DrawLater(poseWorldLandmarks);
    }

    void OnRoiFromLandmarksOutput(NormalizedRect roiFromLandmarks)
    {
      roiFromLandmarksAnnotationController.DrawLater(roiFromLandmarks);
    }
  }
}
