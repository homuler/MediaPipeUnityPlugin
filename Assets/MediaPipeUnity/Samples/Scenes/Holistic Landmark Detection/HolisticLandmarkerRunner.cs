// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using Mediapipe.Tasks.Vision.HolisticLandmarker;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mediapipe.Unity.Sample.HolisticLandmarkDetection
{
  public class HolisticLandmarkerRunner : VisionTaskApiRunner<HolisticLandmarker>
  {
    [SerializeField] private HolisticLandmarkerResultAnnotationController _holisticLandmarkerResultAnnotationController;
    [SerializeField] private TextAsset _holisticLandmarkerTask;

    private Experimental.TextureFramePool _textureFramePool;

    public readonly HolisticLandmarkDetectionConfig config = new HolisticLandmarkDetectionConfig();

    public override void Stop()
    {
      base.Stop();
      _textureFramePool?.Dispose();
      _textureFramePool = null;
    }

    protected override IEnumerator Run()
    {
      Debug.Log($"Delegate = {config.Delegate}");
      Debug.Log($"Image Read Mode = {config.ImageReadMode}");
      Debug.Log($"Running Mode = {config.RunningMode}");
      Debug.Log($"MinFaceDetectionConfidence = {config.MinFaceDetectionConfidence}");
      Debug.Log($"MinFaceSuppressionThreshold = {config.MinFaceSuppressionThreshold}");
      Debug.Log($"MinFaceLandmarksConfidence = {config.MinFaceLandmarksConfidence}");
      Debug.Log($"MinPoseDetectionConfidence = {config.MinPoseDetectionConfidence}");
      Debug.Log($"MinPoseSuppressionThreshold = {config.MinPoseSuppressionThreshold}");
      Debug.Log($"MinPoseLandmarksConfidence = {config.MinPoseLandmarksConfidence}");
      Debug.Log($"MinHandLandmarksConfidence = {config.MinHandLandmarksConfidence}");
      Debug.Log($"OutputFaceBlendshapes = {config.OutputFaceBlendshapes}");
      Debug.Log($"OutputSegmentationMask = {config.OutputSegmentationMask}");

      yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

      var options = config.GetHolisticLandmarkerOptions(_holisticLandmarkerTask, config.RunningMode == Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnHolisticLandmarkDetectionOutput : null);
      taskApi = HolisticLandmarker.CreateFromOptions(options, GpuManager.GpuResources);
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

      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Initialize(imageSource);

      yield return null; // wait for the screen to be initialized to ensure that the MaskOverlayAnnotation will be initialized correctly

      SetupAnnotationController(_holisticLandmarkerResultAnnotationController, imageSource);
      _holisticLandmarkerResultAnnotationController.InitScreen(imageSource.textureWidth, imageSource.textureHeight);

      var transformationOptions = imageSource.GetTransformationOptions();
      var flipHorizontally = transformationOptions.flipHorizontally;
      var flipVertically = transformationOptions.flipVertically;

      AsyncGPUReadbackRequest req = default;
      var waitUntilReqDone = new WaitUntil(() => req.done);
      var waitForEndOfFrame = new WaitForEndOfFrame();
      var waitUntilTrue = new WaitUntil(() => true);
      var result = new HolisticLandmarkerResult();

      // NOTE: we can share the GL context of the render thread with MediaPipe (for now, only on Android)
      var canUseGpuImage = SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && GpuManager.GpuResources != null;
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
        switch (config.ImageReadMode)
        {
          case ImageReadMode.GPU:
            if (!canUseGpuImage)
            {
              throw new System.Exception("ImageReadMode.GPU is not supported");
            }
            textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
            image = textureFrame.BuildGPUImage(glContext);
            // TODO: Currently we wait here for one frame to make sure the texture is fully copied to the TextureFrame before sending it to MediaPipe.
            // This usually works but is not guaranteed. Find a proper way to do this. See: https://github.com/homuler/MediaPipeUnityPlugin/pull/1311
            yield return waitForEndOfFrame;
            break;
          case ImageReadMode.CPU:
            yield return waitForEndOfFrame;
            textureFrame.ReadTextureOnCPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
            image = textureFrame.BuildCPUImage();
            textureFrame.Release();
            break;
          case ImageReadMode.CPUAsync:
          default:
            req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
            yield return waitUntilReqDone;

            if (req.hasError)
            {
              Debug.LogWarning($"Failed to read texture from the image source");
              continue;
            }
            image = textureFrame.BuildCPUImage();
            textureFrame.Release();
            break;
        }

        switch (taskApi.runningMode)
        {
          case Tasks.Vision.Core.RunningMode.IMAGE:
            if (taskApi.TryDetect(image, ref result))
            {
              _holisticLandmarkerResultAnnotationController.DrawNow(in result);
            }
            else
            {
              _holisticLandmarkerResultAnnotationController.DrawNow(default);
            }
            result.segmentationMask?.Dispose();
            break;
          case Tasks.Vision.Core.RunningMode.VIDEO:
            if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), ref result))
            {
              _holisticLandmarkerResultAnnotationController.DrawNow(in result);
            }
            else
            {
              _holisticLandmarkerResultAnnotationController.DrawNow(default);
            }
            result.segmentationMask?.Dispose();
            break;
          case Tasks.Vision.Core.RunningMode.LIVE_STREAM:
            taskApi.DetectAsync(image, GetCurrentTimestampMillisec());
            break;
        }
      }
    }

    private void OnHolisticLandmarkDetectionOutput(in HolisticLandmarkerResult result, Image image, long timestamp)
    {
      _holisticLandmarkerResultAnnotationController.DrawLater(in result);
      result.segmentationMask?.Dispose();
    }
  }
}
