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
      get { return graphRunner.timeoutMicrosec / 1000; }
      set { graphRunner.timeoutMicrosec = value * 1000; }
    }

    public override void Play() {
      if (coroutine != null) {
        Stop();
      }
      base.Play();
      graphRunner.Initialize();
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

      var graphRunner = gameObject.GetComponent<HolisticTrackingGraph>();

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Detect Iris = {detectIris}");
      Logger.LogInfo(TAG, $"Timeout Millisec = {timeoutMillisec}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

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

      // Decide which TextureFormat to use
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES) {
        // Use BGRA32 when the input packet is GpuBuffer
        textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.BGRA32);
      } else {
        // Use RGBA32 when the input packet is ImageFrame
        textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);
      }

      poseDetectionAnnotationController.isMirrored = imageSource.isMirrored;
      holisticAnnotationController.isMirrored = imageSource.isMirrored;
      poseWorldLandmarksAnnotationController.isMirrored = imageSource.isMirrored;
      poseRoiAnnotationController.isMirrored = imageSource.isMirrored;

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
