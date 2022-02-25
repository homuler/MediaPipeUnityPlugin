// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity
{
  public abstract class ImageSourceSolution<T> : Solution where T : GraphRunner
  {
    [SerializeField] protected Screen screen;
    [SerializeField] protected T graphRunner;
    [SerializeField] protected TextureFramePool textureFramePool;

    private Coroutine _coroutine;

    public RunningMode runningMode;

    public long timeoutMillisec
    {
      get => graphRunner.timeoutMillisec;
      set => graphRunner.timeoutMillisec = value;
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
      graphRunner.Stop();
    }

    private IEnumerator Run()
    {
      var graphInitRequest = graphRunner.WaitForInit(runningMode);
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }

      SetupScreen(imageSource);

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);
      graphRunner.StartRun(imageSource).AssertOk();

      OnStartRun();

      var waitWhilePausing = new WaitWhile(() => isPaused);

      while (true)
      {
        if (isPaused)
        {
          yield return waitWhilePausing;
        }

        if (!textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          yield return new WaitForEndOfFrame();
          continue;
        }

        // Copy current image to TextureFrame
        ReadFromImageSource(imageSource, textureFrame);
        graphRunner.AddTextureFrameToInputStream("input_video", textureFrame).AssertOk();

        if (runningMode.IsSynchronous())
        {
          screen.ReadSync(textureFrame);
          yield return WaitForNextValue();
        }
        else
        {
          yield return new WaitForEndOfFrame();
        }
      }
    }

    protected virtual void SetupScreen(ImageSource imageSource)
    {
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Initialize(imageSource);
    }

    protected abstract void OnStartRun();

    protected abstract IEnumerator WaitForNextValue();
  }
}