// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.HandTracking
{
  public class HandTrackingSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private DetectionListAnnotationController _palmDetectionsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromPalmDetectionsAnnotationController;
    [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromLandmarksAnnotationController;
    [SerializeField] private HandTrackingGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

    public RunningMode runningMode;

    public HandTrackingGraph.ModelComplexity modelComplexity
    {
      get => _graphRunner.modelComplexity;
      set => _graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
      get => _graphRunner.maxNumHands;
      set => _graphRunner.maxNumHands = value;
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

      Logger.LogInfo(TAG, $"Model Complexity = {modelComplexity}");
      Logger.LogInfo(TAG, $"Max Num Hands = {maxNumHands}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnPalmDetectectionsOutput.AddListener(OnPalmDetectectionsOutput);
        _graphRunner.OnHandRectsFromPalmDetectionsOutput.AddListener(OnHandRectsFromPalmDetectionsOutput);
        _graphRunner.OnHandLandmarksOutput.AddListener(OnHandLandmarksOutput);
        _graphRunner.OnHandWorldLandmarksOutput.AddListener(OnHandWorldLandmarksOutput);
        _graphRunner.OnHandRectsFromLandmarksOutput.AddListener(OnHandRectsFromLandmarksOutput);
        _graphRunner.OnHandednessOutput.AddListener(OnHandednessOutput);
        _graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        _graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      // The input image is flipped if it's **not** mirrored
      SetupAnnotationController(_palmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromPalmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromLandmarksAnnotationController, imageSource, true);

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
          _palmDetectionsAnnotationController.DrawNow(value.palmDetections);
          _handRectsFromPalmDetectionsAnnotationController.DrawNow(value.handRectsFromPalmDetections);
          _handLandmarksAnnotationController.DrawNow(value.handLandmarks, value.handedness);
          _handRectsFromLandmarksAnnotationController.DrawNow(value.handRectsFromLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnPalmDetectectionsOutput(List<Detection> palmDetections)
    {
      _palmDetectionsAnnotationController.DrawLater(palmDetections);
    }

    private void OnHandRectsFromPalmDetectionsOutput(List<NormalizedRect> handRectsFromPalmDetections)
    {
      _handRectsFromPalmDetectionsAnnotationController.DrawLater(handRectsFromPalmDetections);
    }

    private void OnHandLandmarksOutput(List<NormalizedLandmarkList> handLandmarks)
    {
      _handLandmarksAnnotationController.DrawLater(handLandmarks);
    }

    private void OnHandWorldLandmarksOutput(List<LandmarkList> handWorldLandmarks)
    {
      // TODO: render annotations
    }

    private void OnHandRectsFromLandmarksOutput(List<NormalizedRect> handRectsFromLandmarks)
    {
      _handRectsFromLandmarksAnnotationController.DrawLater(handRectsFromLandmarks);
    }

    private void OnHandednessOutput(List<ClassificationList> handedness)
    {
      _handLandmarksAnnotationController.DrawLater(handedness);
    }
  }
}
