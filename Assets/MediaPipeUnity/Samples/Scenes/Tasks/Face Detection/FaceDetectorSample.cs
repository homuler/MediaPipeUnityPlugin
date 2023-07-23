// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

using FaceDetectionResult = Mediapipe.Tasks.Components.Containers.DetectionResult;

namespace Mediapipe.Unity.Sample.FaceDetection
{
  public class FaceDetectorSample : VisionTaskApiRunner
  {
    [SerializeField] private DetectionResultAnnotationController _detectionResultAnnotationController;

    private Tasks.Vision.FaceDetector.FaceDetector _faceDetector;
    private Experimental.TextureFramePool _textureFramePool;

    public readonly FaceDetectionConfig config = new FaceDetectionConfig();

    public override void Stop()
    {
      base.Stop();
      _faceDetector.Close();
      _textureFramePool?.Dispose();
      _textureFramePool = null;
    }

    protected override IEnumerator Run()
    {
      Debug.Log($"Model = {config.ModelName}");
      Debug.Log($"Running Mode = {config.RunningMode}");
      Debug.Log($"MinDetectionConfidence = {config.MinDetectionConfidence}");
      Debug.Log($"MinSuppressionThreshold = {config.MinSuppressionThreshold}");
      Debug.Log($"NumFaces = {config.NumFaces}");

      yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

      var options = new Tasks.Vision.FaceDetector.FaceDetectorOptions(
        new Tasks.Core.BaseOptions(Tasks.Core.BaseOptions.Delegate.GPU, modelAssetPath: config.ModelPath),
        runningMode: config.RunningMode,
        minDetectionConfidence: config.MinDetectionConfidence,
        minSuppressionThreshold: config.MinSuppressionThreshold,
        numFaces: config.NumFaces,
        resultCallback: config.RunningMode == Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnFaceDetectionsOutput : null
      );
      _faceDetector = Tasks.Vision.FaceDetector.FaceDetector.CreateFromOptions(options);
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

      SetupAnnotationController(_detectionResultAnnotationController, imageSource);

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
        var req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), true, false);
        yield return new WaitUntil(() => req.done);

        if (req.hasError)
        {
          Debug.LogError($"Failed to read texture from the image source, exiting...");
          break;
        }

        var image = textureFrame.BuildCPUImage();
        switch (_faceDetector.runningMode)
        {
          case Tasks.Vision.Core.RunningMode.IMAGE:
            var result = _faceDetector.Detect(image);
            screen.texture = textureFrame.texture;
            _detectionResultAnnotationController.DrawNow(result);
            break;
          case Tasks.Vision.Core.RunningMode.VIDEO:
            result = _faceDetector.DetectForVideo(image, (int)GetCurrentTimestampMillisec());
            screen.texture = textureFrame.texture;
            _detectionResultAnnotationController.DrawNow(result);
            break;
          case Tasks.Vision.Core.RunningMode.LIVE_STREAM:
            _faceDetector.DetectAsync(image, (int)GetCurrentTimestampMillisec());
            break;
        }

        textureFrame.Release();
      }
    }

    private void OnFaceDetectionsOutput(FaceDetectionResult result, Image image, int timestamp)
    {
      _detectionResultAnnotationController.DrawLater(result);
    }
  }
}
