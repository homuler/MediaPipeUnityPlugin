// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.InstantMotionTracking
{
  public class InstantMotionTrackingSolution : Solution
  {
    [SerializeField] private Screen _screen;
    [SerializeField] private Anchor3dAnnotationController _trackedAnchorDataAnnotationController;
    [SerializeField] private RegionTrackingGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Coroutine _coroutine;

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

    private void Update()
    {
      if (Input.GetMouseButtonDown(0))
      {
        var rectTransform = _screen.GetComponent<RectTransform>();

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
        {
          if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out var localPoint))
          {
            var normalizedPoint = rectTransform.GetNormalizedPosition(localPoint, _graphRunner.rotation, ImageSourceProvider.ImageSource.isHorizontallyFlipped);
            _graphRunner.ResetAnchor(normalizedPoint.x, normalizedPoint.y);
            _trackedAnchorDataAnnotationController.ResetAnchor();
          }
        }
      }
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

      _graphRunner.ResetAnchor();

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnTrackedAnchorDataOutput.AddListener(OnTrackedAnchorDataOutput);
        _graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        _graphRunner.StartRun(imageSource).AssertOk();
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      SetupAnnotationController(_trackedAnchorDataAnnotationController, imageSource);
      _trackedAnchorDataAnnotationController.ResetAnchor();

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
          _trackedAnchorDataAnnotationController.DrawNow(value);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnTrackedAnchorDataOutput(List<Anchor3d> trackedAnchorData)
    {
      _trackedAnchorDataAnnotationController.DrawLater(trackedAnchorData);
    }
  }
}
