// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using Mediapipe.Tasks.Vision.ImageSegmenter;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mediapipe.Unity.Sample.ImageSegmentation
{
  public class ImageSegmenterRunner : VisionTaskApiRunner<ImageSegmenter>
  {
    [SerializeField] private ImageSegmenterResultAnnotationController _imageSegmenterResultAnnotationController;

    private Experimental.TextureFramePool _textureFramePool;

    public readonly ImageSegmentationConfig config = new ImageSegmentationConfig();

    public override void Stop()
    {
      base.Stop();
      _textureFramePool?.Dispose();
      _textureFramePool = null;
    }

    protected override IEnumerator Run()
    {
      Debug.Log($"Delegate = {config.Delegate}");
      Debug.Log($"Model = {config.ModelName}");
      Debug.Log($"Running Mode = {config.RunningMode}");
      Debug.Log($"Category Index = {config.CategoryIndex}");

      yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

      var options = config.GetImageSegmenterOptions(config.RunningMode == Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnImageSegmentationOutput : null);
      taskApi = ImageSegmenter.CreateFromOptions(options, GpuManager.GpuResources);
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
      _textureFramePool = new Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

      screen.Initialize(imageSource);
      yield return null; // Wait a frame for the screen to be resized, keeping the aspect ratio.

      SetupAnnotationController(_imageSegmenterResultAnnotationController, imageSource);
      _imageSegmenterResultAnnotationController.InitScreen(imageSource.textureWidth, imageSource.textureHeight);
      _imageSegmenterResultAnnotationController.SelectMask(config.CategoryIndex);

      var transformationOptions = imageSource.GetTransformationOptions();
      var flipHorizontally = transformationOptions.flipHorizontally;
      var flipVertically = transformationOptions.flipVertically;

      // Always setting rotationDegrees to 0 to avoid the issue that the output mask is rotated when the model is SelfieSegmenter.
      // NOTE: Depending on the rotation degrees, the accuracy may significantly decrease.
      var imageProcessingOptions = new Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: 0);

      AsyncGPUReadbackRequest req = default;
      var waitUntilReqDone = new WaitUntil(() => req.done);
      var result = ImageSegmenterResult.Alloc();

      // NOTE: we can share the GL context of the render thread with MediaPipe (for now, only on Android)
      var canUseGpuImage = options.baseOptions.delegateCase == Tasks.Core.BaseOptions.Delegate.GPU &&
        SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 &&
        GpuManager.GpuResources != null;
      using var glContext = canUseGpuImage ? GpuManager.GetGlContext() : null;

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

        // Build the input Image
        Image image;
        if (glContext != null)
        {
          yield return new WaitForEndOfFrame();
          textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
          image = textureFrame.BuildGpuImage(glContext);
        }
        else
        {
          req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
          yield return waitUntilReqDone;

          if (req.hasError)
          {
            Debug.LogError($"Failed to read texture from the image source, exiting...");
            break;
          }
          image = textureFrame.BuildCPUImage();
          textureFrame.Release();
        }

        switch (taskApi.runningMode)
        {
          case Tasks.Vision.Core.RunningMode.IMAGE:
            if (taskApi.TrySegment(image, imageProcessingOptions, ref result))
            {
              _imageSegmenterResultAnnotationController.DrawNow(result);
            }
            else
            {
              _imageSegmenterResultAnnotationController.DrawNow(default);
            }
            break;
          case Tasks.Vision.Core.RunningMode.VIDEO:
            if (taskApi.TrySegmentForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
            {
              _imageSegmenterResultAnnotationController.DrawNow(result);
            }
            else
            {
              _imageSegmenterResultAnnotationController.DrawNow(default);
            }
            break;
          case Tasks.Vision.Core.RunningMode.LIVE_STREAM:
            taskApi.SegmentAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
            break;
        }
      }
    }

    private void OnImageSegmentationOutput(ImageSegmenterResult result, Image image, long timestamp) => _imageSegmenterResultAnnotationController.DrawLater(result);
  }
}
