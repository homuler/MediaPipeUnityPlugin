// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Objectron
{
  public class ObjectronSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private ObjectronGraph _graphRunner;
    [SerializeField] private FrameAnnotationController _liftedObjectsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _multiBoxRectsAnnotationController;
    [SerializeField] private NormalizedLandmarkListAnnotationController _multiBoxLandmarksAnnotationController;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

    public ObjectronGraph.Category category
    {
      get => _graphRunner.category;
      set => _graphRunner.category = value;
    }

    public int maxNumObjects
    {
      get => _graphRunner.maxNumObjects;
      set => _graphRunner.maxNumObjects = value;
    }

    public long timeoutMillisec
    {
      get => _graphRunner.timeoutMillisec;
      set => _graphRunner.SetTimeoutMillisec(value);
    }

    public RunningMode runningMode;

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

      Logger.LogInfo(TAG, $"Category = {category}");
      Logger.LogInfo(TAG, $"Max Num Objects = {maxNumObjects}");
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnLiftedObjectsOutput.AddListener(OnLiftedObjectsOutput);
        _graphRunner.OnMultiBoxRectsOutput.AddListener(OnMultiBoxRectsOutput);
        _graphRunner.OnMultiBoxLandmarksOutput.AddListener(OnMultiBoxLandmarksOutput);
        _graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        _graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      SetupAnnotationController(_liftedObjectsAnnotationController, imageSource);
      _liftedObjectsAnnotationController.focalLength = _graphRunner.focalLength;
      _liftedObjectsAnnotationController.principalPoint = _graphRunner.principalPoint;

      SetupAnnotationController(_multiBoxRectsAnnotationController, imageSource);
      SetupAnnotationController(_multiBoxLandmarksAnnotationController, imageSource);

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
          _liftedObjectsAnnotationController.DrawNow(value.liftedObjects);
          _multiBoxRectsAnnotationController.DrawNow(value.multiBoxRects);
          _multiBoxLandmarksAnnotationController.DrawNow(value.multiBoxLandmarks);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnLiftedObjectsOutput(FrameAnnotation liftedObjects)
    {
      _liftedObjectsAnnotationController.DrawLater(liftedObjects);
    }

    private void OnMultiBoxRectsOutput(List<NormalizedRect> multiBoxRects)
    {
      _multiBoxRectsAnnotationController.DrawLater(multiBoxRects);
    }

    private void OnMultiBoxLandmarksOutput(List<NormalizedLandmarkList> multiBoxLandmarks)
    {
      _multiBoxLandmarksAnnotationController.DrawLater(multiBoxLandmarks);
    }
  }
}
