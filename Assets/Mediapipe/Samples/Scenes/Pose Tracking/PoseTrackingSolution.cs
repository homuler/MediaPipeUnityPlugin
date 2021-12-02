// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.PoseTracking
{
  public class PoseTrackingSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private RectTransform _worldAnnotationArea;
    [SerializeField] private DetectionAnnotationController _poseDetectionAnnotationController;
    [SerializeField] private PoseLandmarkListAnnotationController _poseLandmarksAnnotationController;
    [SerializeField] private PoseWorldLandmarkListAnnotationController _poseWorldLandmarksAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _roiFromLandmarksAnnotationController;
    [SerializeField] private PoseTrackingGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

    public RunningMode runningMode;

    public PoseTrackingGraph.ModelComplexity modelComplexity
    {
      get => _graphRunner.modelComplexity;
      set => _graphRunner.modelComplexity = value;
    }

    public bool smoothLandmarks
    {
      get => _graphRunner.smoothLandmarks;
      set => _graphRunner.smoothLandmarks = value;
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
        _graphRunner.OnPoseLandmarksOutput.AddListener(OnPoseLandmarksOutput);
        _graphRunner.OnPoseWorldLandmarksOutput.AddListener(OnPoseWorldLandmarksOutput);
        _graphRunner.OnRoiFromLandmarksOutput.AddListener(OnRoiFromLandmarksOutput);
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
      SetupAnnotationController(_poseLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_poseWorldLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_roiFromLandmarksAnnotationController, imageSource);

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
          _poseLandmarksAnnotationController.DrawNow(value.poseLandmarks);
          _poseWorldLandmarksAnnotationController.DrawNow(value.poseWorldLandmarks);
          _roiFromLandmarksAnnotationController.DrawNow(value.roiFromLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnPoseDetectionOutput(Detection poseDetection)
    {
      _poseDetectionAnnotationController.DrawLater(poseDetection);
    }

    private void OnPoseLandmarksOutput(NormalizedLandmarkList poseLandmarks)
    {
      _poseLandmarksAnnotationController.DrawLater(poseLandmarks);
    }

    private void OnPoseWorldLandmarksOutput(LandmarkList poseWorldLandmarks)
    {
      _poseWorldLandmarksAnnotationController.DrawLater(poseWorldLandmarks);
    }

    private void OnRoiFromLandmarksOutput(NormalizedRect roiFromLandmarks)
    {
      _roiFromLandmarksAnnotationController.DrawLater(roiFromLandmarks);
    }
  }
}
