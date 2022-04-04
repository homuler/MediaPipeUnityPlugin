// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.MediaPipeVideo
{
  public class MediaPipeVideoSolution : ImageSourceSolution<MediaPipeVideoGraph>
  {
    private Texture2D _outputTexture;

    protected override void SetupScreen(ImageSource imageSource)
    {
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Resize(imageSource.textureWidth, imageSource.textureHeight);
      screen.Rotate(imageSource.rotation.Reverse());

      // Setup output texture
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES)
      {
        if (textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          textureFrame.RemoveAllReleaseListeners();
          graphRunner.SetupOutputPacket(textureFrame);

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
    }

    protected override void OnStartRun()
    {
      // Do nothing
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override void RenderCurrentFrame(TextureFrame textureFrame)
    {
      // Do nothing because the screen will be updated later in `DrawNow`. 
    }

    protected override IEnumerator WaitForNextValue()
    {
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES)
      {
        yield break;
      }

      ImageFrame outputVideo = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out outputVideo, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out outputVideo, false));
      }

      DrawNow(outputVideo);
    }

    private void DrawNow(ImageFrame imageFrame)
    {
      if (imageFrame != null)
      {
        _outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
        _outputTexture.Apply();
      }
    }
  }
}
