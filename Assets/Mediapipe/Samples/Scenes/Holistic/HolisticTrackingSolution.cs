using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Holistic {
  public class HolisticTrackingSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] DetectionAnnotationController poseDetectionAnnotationController;
    [SerializeField] HolisticLandmarkListAnnotationController holisticAnnotationController;
    [SerializeField] PoseWorldLandmarkListAnnotationController poseWorldLandmarksAnnotationController;
    [SerializeField] NormalizedRectAnnotationController poseRoiAnnotationController;
    [SerializeField] HolisticTrackingGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Coroutine coroutine;

    public RunningMode runningMode;
    public HolisticTrackingGraph.ModelComplexity modelComplexity {
      get { return graphRunner.modelComplexity; }
      set { graphRunner.modelComplexity = value; }
    }

    public bool smoothLandmarks {
      get { return graphRunner.smoothLandmarks; }
      set { graphRunner.smoothLandmarks = value; }
    }

    public bool detectIris {
      get { return graphRunner.detectIris; }
      set { graphRunner.detectIris = value; }
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

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Detect Iris = {detectIris}");
      Logger.LogInfo(TAG, $"Timeout Millisec = {timeoutMillisec}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError) {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async) {
        graphRunner.OnPoseDetectionOutput.AddListener(OnPoseDetectionOutput);
        graphRunner.OnFaceLandmarksOutput.AddListener(OnFaceLandmarksOutput);
        graphRunner.OnPoseLandmarksOutput.AddListener(OnPoseLandmarksOutput);
        graphRunner.OnLeftHandLandmarksOutput.AddListener(OnLeftHandLandmarksOutput);
        graphRunner.OnRightHandLandmarksOutput.AddListener(OnRightHandLandmarksOutput);
        graphRunner.OnLeftIrisLandmarksOutput.AddListener(OnLeftIrisLandmarksOutput);
        graphRunner.OnRightIrisLandmarksOutput.AddListener(OnRightIrisLandmarksOutput);
        graphRunner.OnPoseWorldLandmarksOutput.AddListener(OnPoseWorldLandmarksOutput);
        graphRunner.OnPoseRoiOutput.AddListener(OnPoseRoiOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      poseDetectionAnnotationController.isMirrored = imageSource.isMirrored;
      holisticAnnotationController.isMirrored = imageSource.isMirrored;
      poseWorldLandmarksAnnotationController.isMirrored = imageSource.isMirrored;
      poseRoiAnnotationController.isMirrored = imageSource.isMirrored;

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
          poseDetectionAnnotationController.DrawNow(value.poseDetection);
          holisticAnnotationController.DrawNow(value.faceLandmarks, value.poseLandmarks, value.leftHandLandmarks, value.rightHandLandmarks, value.leftIrisLandmarks, value.rightIrisLandmarks);
          poseWorldLandmarksAnnotationController.DrawNow(value.poseWorldLandmarks);
          poseRoiAnnotationController.DrawNow(value.poseRoi);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnPoseDetectionOutput(Detection poseDetection) {
      poseDetectionAnnotationController.DrawLater(poseDetection);
    }

    void OnFaceLandmarksOutput(NormalizedLandmarkList faceLandmarks) {
      holisticAnnotationController.DrawFaceLandmarkListLater(faceLandmarks);
    }

    void OnPoseLandmarksOutput(NormalizedLandmarkList poseLandmarks) {
      holisticAnnotationController.DrawPoseLandmarkListLater(poseLandmarks);
    }

    void OnLeftHandLandmarksOutput(NormalizedLandmarkList leftHandLandmarks) {
      holisticAnnotationController.DrawLeftHandLandmarkListLater(leftHandLandmarks);
    }

    void OnRightHandLandmarksOutput(NormalizedLandmarkList rightHandLandmarks) {
      holisticAnnotationController.DrawRightHandLandmarkListLater(rightHandLandmarks);
    }

    void OnLeftIrisLandmarksOutput(NormalizedLandmarkList leftIrisLandmarks) {
      holisticAnnotationController.DrawLeftIrisLandmarkListLater(leftIrisLandmarks);
    }

    void OnRightIrisLandmarksOutput(NormalizedLandmarkList rightIrisLandmarks) {
      holisticAnnotationController.DrawRightIrisLandmarkListLater(rightIrisLandmarks);
    }

    void OnPoseWorldLandmarksOutput(LandmarkList poseWorldLandmarks) {
      poseWorldLandmarksAnnotationController.DrawLater(poseWorldLandmarks);
    }

    void OnPoseRoiOutput(NormalizedRect roiFromLandmarks) {
      poseRoiAnnotationController.DrawLater(roiFromLandmarks);
    }
  }
}
