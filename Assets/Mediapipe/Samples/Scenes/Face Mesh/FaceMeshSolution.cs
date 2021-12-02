// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.FaceMesh
{
  public class FaceMeshSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromDetectionsAnnotationController;
    [SerializeField] private FaceMeshGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

    public RunningMode runningMode;

    public int maxNumFaces
    {
      get => _graphRunner.maxNumFaces;
      set => _graphRunner.maxNumFaces = value;
    }

    public bool refineLandmarks
    {
      get => _graphRunner.refineLandmarks;
      set => _graphRunner.refineLandmarks = value;
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

      Logger.LogInfo(TAG, $"Max Num Faces = {maxNumFaces}");
      Logger.LogInfo(TAG, $"Refine Landmarks = {refineLandmarks}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      // Wait for completion of loading of dependent files, etc.
      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnFaceDetectionsOutput.AddListener(OnFaceDetectionsOutput);
        _graphRunner.OnMultiFaceLandmarksOutput.AddListener(OnMultiFaceLandmarksOutput);
        _graphRunner.OnFaceRectsFromLandmarksOutput.AddListener(OnFaceRectsFromLandmarksOutput);
        _graphRunner.OnFaceRectsFromDetectionsOutput.AddListener(OnFaceRectsFromDetectionsOutput);
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
      SetupAnnotationController(_faceRectsFromLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_multiFaceLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromDetectionsAnnotationController, imageSource);

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
          _faceRectsFromLandmarksAnnotationController.DrawNow(value.faceRectsFromLandmarks);
          _multiFaceLandmarksAnnotationController.DrawNow(value.multiFaceLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnFaceDetectionsOutput(List<Detection> faceDetections)
    {
      _faceDetectionsAnnotationController.DrawLater(faceDetections);
    }

    private void OnMultiFaceLandmarksOutput(List<NormalizedLandmarkList> multiFaceLandmarks)
    {
      _multiFaceLandmarksAnnotationController.DrawLater(multiFaceLandmarks);
    }

    private void OnFaceRectsFromLandmarksOutput(List<NormalizedRect> faceRectsFromLandmarks)
    {
      _faceRectsFromLandmarksAnnotationController.DrawLater(faceRectsFromLandmarks);
    }

    private void OnFaceRectsFromDetectionsOutput(List<NormalizedRect> faceRectsFromDetections)
    {
      _faceRectsFromDetectionsAnnotationController.DrawLater(faceRectsFromDetections);
    }
  }
}
