// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.IrisTracking
{
  public class IrisTrackingSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _faceRectAnnotationController;
    [SerializeField] private FaceLandmarkListAnnotationController _faceLandmarksWithIrisAnnotationController;
    [SerializeField] private IrisTrackingGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

    public RunningMode runningMode;

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

      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnFaceDetectionsOutput.AddListener(OnFaceDetectionsOutput);
        _graphRunner.OnFaceRectOutput.AddListener(OnFaceRectOutput);
        _graphRunner.OnFaceLandmarksWithIrisOutput.AddListener(OnFaceLandmarksWithIrisOutput);
        _graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        _graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      SetupAnnotationController(_faceDetectionsAnnotationController, imageSource);
      SetupAnnotationController(_faceRectAnnotationController, imageSource);
      SetupAnnotationController(_faceLandmarksWithIrisAnnotationController, imageSource);

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
          _faceDetectionsAnnotationController.DrawNow(value.faceDetections);
          _faceRectAnnotationController.DrawNow(value.faceRect);
          _faceLandmarksWithIrisAnnotationController.DrawNow(value.faceLandmarksWithIris);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnFaceDetectionsOutput(List<Detection> faceDetections)
    {
      _faceDetectionsAnnotationController.DrawLater(faceDetections);
    }

    private void OnFaceRectOutput(NormalizedRect faceRect)
    {
      _faceRectAnnotationController.DrawLater(faceRect);
    }

    private void OnFaceLandmarksWithIrisOutput(NormalizedLandmarkList faceLandmarkListWithIris)
    {
      _faceLandmarksWithIrisAnnotationController.DrawLater(faceLandmarkListWithIris);
    }
  }
}
