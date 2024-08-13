// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mediapipe.Unity.Sample.MediaPipeVideo
{
  public class MediaPipeVideoSolution : LegacySolutionRunner<MediaPipeVideoGraph>
  {
    private Texture2D _outputTexture;
    private Experimental.TextureFramePool _textureFramePool;

    protected override IEnumerator Run()
    {
      var graphInitRequest = graphRunner.WaitForInit(runningMode);
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Debug.LogError("Failed to start ImageSource, exiting...");
        yield break;
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      _textureFramePool = new Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Resize(imageSource.textureWidth, imageSource.textureHeight);
      screen.Rotate(imageSource.rotation.Reverse());

      // NOTE: we can share the GL context of the render thread with MediaPipe (for now, only on Android)
      var canUseGpuImage = graphRunner.configType == GraphRunner.ConfigType.OpenGLES && GpuManager.GpuResources != null;
      using var glContext = canUseGpuImage ? GpuManager.GetGlContext() : null;

      // Setup output texture
      if (canUseGpuImage)
      {
        if (_textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          textureFrame.RemoveAllReleaseListeners();
          graphRunner.SetupOutputPacket(textureFrame, glContext);

          // MediaPipe will write the result to the textureFrame
          screen.texture = Texture2D.CreateExternalTexture(textureFrame.width, textureFrame.height, textureFrame.format, false, false, textureFrame.GetNativeTexturePtr());
        }
        else
        {
          throw new InternalException("Failed to get the output texture");
        }
      }
      else
      {
        _outputTexture = new Texture2D(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, false);
        screen.texture = _outputTexture;
      }

      yield return graphInitRequest;
      if (graphInitRequest.isError)
      {
        Debug.LogError(graphInitRequest.error);
        yield break;
      }

      graphRunner.StartRun(imageSource);

      AsyncGPUReadbackRequest req = default;
      var waitUntilReqDone = new WaitUntil(() => req.done);

      while (true)
      {
        if (isPaused)
        {
          yield return new WaitWhile(() => isPaused);
        }

        if (!_textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          yield return new WaitForEndOfFrame();
          continue;
        }

        // Copy current image to TextureFrame
        if (canUseGpuImage)
        {
          yield return new WaitForEndOfFrame();
          textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture());
        }
        else
        {
          req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture());
          yield return waitUntilReqDone;

          if (req.hasError)
          {
            Debug.LogError($"Failed to read texture from the image source, exiting...");
            break;
          }
        }

        graphRunner.AddTextureFrameToInputStream(textureFrame, glContext);

        if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES)
        {
          continue;
        }

        if (runningMode.IsSynchronous())
        {
          var task = graphRunner.WaitNextAsync();
          yield return new WaitUntil(() => task.IsCompleted);

          var imageFrame = task.Result;
          if (imageFrame != null)
          {
            _outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
            _outputTexture.Apply();
            imageFrame.Dispose();
          }
        }
      }
    }
  }
}
