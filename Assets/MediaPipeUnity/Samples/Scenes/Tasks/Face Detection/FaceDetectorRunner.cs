// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using FaceDetectionResult = Mediapipe.Tasks.Components.Containers.DetectionResult;

namespace Mediapipe.Unity.Sample.FaceDetection
{
  public class FaceDetectorRunner : VisionTaskApiRunner<Tasks.Vision.FaceDetector.FaceDetector>
  {
    [SerializeField] private DetectionResultAnnotationController _detectionResultAnnotationController;

    private Experimental.TextureFramePool _textureFramePool;

    public readonly FaceDetectionConfig config = new FaceDetectionConfig();

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
      Debug.Log($"MinDetectionConfidence = {config.MinDetectionConfidence}");
      Debug.Log($"MinSuppressionThreshold = {config.MinSuppressionThreshold}");
      Debug.Log($"NumFaces = {config.NumFaces}");

      yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

      var options = config.GetFaceDetectorOptions(config.RunningMode == Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnFaceDetectionsOutput : null);
      taskApi = Tasks.Vision.FaceDetector.FaceDetector.CreateFromOptions(options);
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Debug.LogError("Failed to start ImageSource, exiting...");
        yield break;
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
      _textureFramePool = new Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Initialize(imageSource);

      SetupAnnotationController(_detectionResultAnnotationController, imageSource);

      var transformationOptions = imageSource.GetTransformationOptions();
      var flipHorizontally = transformationOptions.flipHorizontally;
      var flipVertically = transformationOptions.flipVertically;
      var imageProcessingOptions = new Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: (int)transformationOptions.rotationAngle);

      AsyncGPUReadbackRequest req = default;
      var waitUntilReqDone = new WaitUntil(() => req.done);
      var result = FaceDetectionResult.Alloc(options.numFaces);

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
        req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
        yield return waitUntilReqDone;

        if (req.hasError)
        {
          Debug.LogError($"Failed to read texture from the image source, exiting...");
          break;
        }

        var image = textureFrame.BuildCPUImage();
        switch (taskApi.runningMode)
        {
          case Tasks.Vision.Core.RunningMode.IMAGE:
            if (taskApi.TryDetect(image, imageProcessingOptions, ref result))
            {
              _detectionResultAnnotationController.DrawNow(result);
            }
            else
            {
              // clear the annotation
              _detectionResultAnnotationController.DrawNow(default);
            }
            break;
          case Tasks.Vision.Core.RunningMode.VIDEO:
            if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
            {
              _detectionResultAnnotationController.DrawNow(result);
            }
            else
            {
              // clear the annotation
              _detectionResultAnnotationController.DrawNow(default);
            }
            break;
          case Tasks.Vision.Core.RunningMode.LIVE_STREAM:
            taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
            break;
        }

        textureFrame.Release();
      }
    }

    private void OnFaceDetectionsOutput(FaceDetectionResult result, Image image, long timestamp)
    {
      _detectionResultAnnotationController.DrawLater(result);
    }
  }
}
