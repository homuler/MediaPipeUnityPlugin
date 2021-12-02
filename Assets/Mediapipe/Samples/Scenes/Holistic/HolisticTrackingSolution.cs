// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Holistic
{
  public class HolisticTrackingSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private RectTransform _worldAnnotationArea;
    [SerializeField] private DetectionAnnotationController _poseDetectionAnnotationController;
    [SerializeField] private HolisticLandmarkListAnnotationController _holisticAnnotationController;
    [SerializeField] private PoseWorldLandmarkListAnnotationController _poseWorldLandmarksAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _poseRoiAnnotationController;
    [SerializeField] private HolisticTrackingGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

    public RunningMode runningMode;
    public HolisticTrackingGraph.ModelComplexity modelComplexity
    {
      get => _graphRunner.modelComplexity;
      set => _graphRunner.modelComplexity = value;
    }

    public bool smoothLandmarks
    {
      get => _graphRunner.smoothLandmarks;
      set => _graphRunner.smoothLandmarks = value;
    }

    public bool refineFaceLandmarks
    {
      get => _graphRunner.refineFaceLandmarks;
      set => _graphRunner.refineFaceLandmarks = value;
    }

    public long timeoutMillisec
    {
      get => _graphRunner.timeoutMillisec;
      set => _graphRunner.SetTimeoutMillisec(value);
    }

    public override void Play()
    {
      if (_coroutine != null)
      {
        Stop();
      }
      base.Play();
      _coroutine = StartCoroutine(Run());
    }

    public override void Pause()
    {
      base.Pause();
      ImageSourceProvider.ImageSource.Pause();
    }

    public override void Resume()
    {
      base.Resume();
      var _ = StartCoroutine(ImageSourceProvider.ImageSource.Resume());
    }

    public override void Stop()
    {
      base.Stop();
      StopCoroutine(_coroutine);
      ImageSourceProvider.ImageSource.Stop();
      _graphRunner.Stop();
    }

    private IEnumerator Run()
    {
      var graphInitRequest = _graphRunner.WaitForInit();
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }
      // NOTE: The _screen will be resized later, keeping the aspect ratio.
      _screen.Initialize(imageSource);

      _worldAnnotationArea.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Smooth Landmarks = {smoothLandmarks}");
      Logger.LogInfo(TAG, $"Refine Face Landmarks = {refineFaceLandmarks}");
      Logger.LogInfo(TAG, $"Timeout Millisec = {timeoutMillisec}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnPoseDetectionOutput.AddListener(OnPoseDetectionOutput);
        _graphRunner.OnFaceLandmarksOutput.AddListener(OnFaceLandmarksOutput);
        _graphRunner.OnPoseLandmarksOutput.AddListener(OnPoseLandmarksOutput);
        _graphRunner.OnLeftHandLandmarksOutput.AddListener(OnLeftHandLandmarksOutput);
        _graphRunner.OnRightHandLandmarksOutput.AddListener(OnRightHandLandmarksOutput);
        _graphRunner.OnPoseWorldLandmarksOutput.AddListener(OnPoseWorldLandmarksOutput);
        _graphRunner.OnPoseRoiOutput.AddListener(OnPoseRoiOutput);
        _graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        _graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      SetupAnnotationController(_poseDetectionAnnotationController, imageSource);
      SetupAnnotationController(_holisticAnnotationController, imageSource);
      SetupAnnotationController(_poseWorldLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_poseRoiAnnotationController, imageSource);

      while (true)
      {
        yield return new WaitWhile(() => isPaused);

        var textureFrameRequest = _textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        // Copy current image to TextureFrame
        ReadFromImageSource(imageSource, textureFrame);

        _graphRunner.AddTextureFrameToInputStream(textureFrame).AssertOk();

        if (runningMode == RunningMode.Sync)
        {
          // TODO: copy texture before `textureFrame` is released
          _screen.ReadSync(textureFrame);

          // When running synchronously, wait for the outputs here (blocks the main thread).
          var value = _graphRunner.FetchNextValue();
          _poseDetectionAnnotationController.DrawNow(value.poseDetection);
          _holisticAnnotationController.DrawNow(value.faceLandmarks, value.poseLandmarks, value.leftHandLandmarks, value.rightHandLandmarks);
          _poseWorldLandmarksAnnotationController.DrawNow(value.poseWorldLandmarks);
          _poseRoiAnnotationController.DrawNow(value.poseRoi);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnPoseDetectionOutput(Detection poseDetection)
    {
      _poseDetectionAnnotationController.DrawLater(poseDetection);
    }

    private void OnFaceLandmarksOutput(NormalizedLandmarkList faceLandmarks)
    {
      _holisticAnnotationController.DrawFaceLandmarkListLater(faceLandmarks);
    }

    private void OnPoseLandmarksOutput(NormalizedLandmarkList poseLandmarks)
    {
      _holisticAnnotationController.DrawPoseLandmarkListLater(poseLandmarks);
    }

    private void OnLeftHandLandmarksOutput(NormalizedLandmarkList leftHandLandmarks)
    {
      _holisticAnnotationController.DrawLeftHandLandmarkListLater(leftHandLandmarks);
    }

    private void OnRightHandLandmarksOutput(NormalizedLandmarkList rightHandLandmarks)
    {
      _holisticAnnotationController.DrawRightHandLandmarkListLater(rightHandLandmarks);
    }

    private void OnPoseWorldLandmarksOutput(LandmarkList poseWorldLandmarks)
    {
      _poseWorldLandmarksAnnotationController.DrawLater(poseWorldLandmarks);
    }

    private void OnPoseRoiOutput(NormalizedRect roiFromLandmarks)
    {
      _poseRoiAnnotationController.DrawLater(roiFromLandmarks);
    }
  }
}
