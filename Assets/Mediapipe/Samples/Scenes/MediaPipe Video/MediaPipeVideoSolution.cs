// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.MediaPipeVideo
{
  public class MediaPipeVideoSolution : Solution
  {
    [SerializeField] private RawImage _screen;
    [SerializeField] private MediaPipeVideoGraph _graphRunner;
    [SerializeField] private TextureFramePool _textureFramePool;

    private Texture2D _outputTexture;
    private static ImageFrame _CurrentOutput;

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

    private void Update()
    {
      if (_CurrentOutput != null)
      {
        var outputVideo = _CurrentOutput;
        _CurrentOutput = null;

        DrawNow(outputVideo);
        outputVideo.Dispose();
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
      _screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      _screen.rectTransform.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();

      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      // Setup output texture
      if (_graphRunner.configType == GraphRunner.ConfigType.OpenGLES)
      {
        var textureFrameRequest = _textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var _outputTexture = textureFrameRequest.result;

        // Exclude from TextureFramePool
        _outputTexture.RemoveAllReleaseListeners();
        _graphRunner.SetupOutputPacket(_outputTexture);

        _screen.texture = Texture2D.CreateExternalTexture(_outputTexture.width, _outputTexture.height, _outputTexture.format, false, false, _outputTexture.GetNativeTexturePtr());
      }
      else
      {
        _outputTexture = new Texture2D(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, false);
        _screen.texture = _outputTexture;
      }

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnOutput.AddListener(OnOutput);
        _graphRunner.StartRunAsync(imageSource).AssertOk();
      }
      else
      {
        _graphRunner.StartRun(imageSource).AssertOk();
      }

      while (true)
      {
        yield return new WaitWhile(() => isPaused);

        var textureFrameRequest = _textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        // Copy current image to TextureFrame
        ReadFromImageSource(imageSource, textureFrame);

        _graphRunner.AddTextureFrameToInputStream(textureFrame).AssertOk();

        if (runningMode == RunningMode.Sync && _graphRunner.configType != GraphRunner.ConfigType.OpenGLES)
        {
          // When running synchronously, wait for the outputs here (blocks the main thread).
          var output = _graphRunner.FetchNextValue();
          DrawNow(output);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnOutput(ImageFrame outputVideo)
    {
      if (outputVideo != null)
      {
        _CurrentOutput = outputVideo;
      }
    }

    private void DrawNow(ImageFrame imageFrame)
    {
      _outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
      _outputTexture.Apply();
    }
  }
}
