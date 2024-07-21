// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Core;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Tasks.Vision.ObjectDetector;
using Mediapipe.Unity;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Tests.Tasks.Vision
{
  public class ObjectDetectorTest
  {
    private const string _ResourcePath = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private const string _TestResourcePath = "Packages/com.github.homuler.mediapipe/Tests/Resources";

    private const int _CallbackTimeoutMillisec = 1000;

    private static readonly IResourceManager _ResourceManager = new LocalResourceManager();
    private readonly Lazy<TextAsset> _objectDetectorModel =
        new Lazy<TextAsset>(() => AssetDatabase.LoadAssetAtPath<TextAsset>($"{_ResourcePath}/efficientdet_lite0_float16.bytes"));

    private readonly Lazy<Texture2D> _facePicture =
        new Lazy<Texture2D>(() => AssetDatabase.LoadAssetAtPath<Texture2D>($"{_TestResourcePath}/lenna.png"));

    #region Create
    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelIsNotSpecified()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = ObjectDetector.CreateFromOptions(options);
      });
    }

    [Test]
    public void Create_ShouldReturnFaceDetector_When_AssetModelBufferIsValid()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _objectDetectorModel.Value.bytes));

      Assert.DoesNotThrow(() =>
      {
        using (var objectDetector = ObjectDetector.CreateFromOptions(options))
        {
          objectDetector.Close();
        }
      });
    }

    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelPathDoesNotExist()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "unknown_path.bytes"));

      LogAssert.Expect(LogType.Exception, new Regex("KeyNotFoundException"));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = ObjectDetector.CreateFromOptions(options);
      });
    }

    [UnityTest]
    public IEnumerator Create_returns_ObjectDetector_when_assetModelPath_is_valid()
    {
      yield return _ResourceManager.PrepareAssetAsync("efficientdet_lite0_float16.bytes");

      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "efficientdet_lite0_float16.bytes"));

      Assert.DoesNotThrow(() =>
      {
        using (var objectDetector = ObjectDetector.CreateFromOptions(options))
        {
          objectDetector.Close();
        }
      });
    }
    #endregion

    #region Detect
    [Test]
    public void Detect_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        scoreThreshold: 0.1f); // specify the score threshold to avoid crash, maybe caused by DetectionsDeduplicateCalculator.

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = objectDetector.Detect(image, null);
          Assert.IsNull(result.detections);
        }
      }
    }

    [Test]
    public void Detect_ShouldReturnObjectDetectionResult_When_ObjectsAreDetected()
    {
      var options = new ObjectDetectorOptions(
        new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        maxResults: 10);

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = objectDetector.Detect(image, null);
          Assert.IsTrue(result.detections?.Count > 0);
          Assert.IsTrue(result.detections?.Count <= 10);
        }
      }
    }
    #endregion

    #region TryDetect
    [Test]
    public void TryDetect_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        scoreThreshold: 0.1f); // specify the score threshold to avoid crash, maybe caused by DetectionsDeduplicateCalculator.

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = DetectionResult.Alloc(0);
          var found = objectDetector.TryDetect(image, null, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryDetect_ShouldReturnTrue_When_ObjectsAreDetected()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        maxResults: 10);

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = DetectionResult.Alloc(0);
          var found = objectDetector.TryDetect(image, null, ref result);
          Assert.IsTrue(found);
          Assert.IsTrue(result.detections?.Count > 0);
          Assert.IsTrue(result.detections?.Count <= 10);
        }
      }
    }
    #endregion

    #region DetectForVideo
    [Test]
    public void DetectForVideo_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.VIDEO,
        scoreThreshold: 0.1f); // specify the score threshold to avoid crash, maybe caused by DetectionsDeduplicateCalculator.

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = objectDetector.DetectForVideo(image, 1, null);
          Assert.IsNull(result.detections);
        }
      }
    }

    [Test]
    public void DetectForVideo_ShouldReturnObjectDetectionResult_When_ObjectsAreDetected()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.VIDEO,
        maxResults: 10);

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = objectDetector.DetectForVideo(image, 1, null);
          Assert.IsTrue(result.detections?.Count > 0);
          Assert.IsTrue(result.detections?.Count <= 10);
        }
      }
    }
    #endregion

    #region TryDetectForVideo
    [Test]
    public void TryDetectForVideo_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.VIDEO,
        scoreThreshold: 0.1f); // specify the score threshold to avoid crash, maybe caused by DetectionsDeduplicateCalculator.

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = DetectionResult.Alloc(0);
          var found = objectDetector.TryDetectForVideo(image, 1, null, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryDetectForVideo_ShouldReturnTrue_When_ObjectsAreDetected()
    {
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _objectDetectorModel.Value.bytes),
        runningMode: RunningMode.VIDEO,
        maxResults: 10);

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = DetectionResult.Alloc(0);
          var found = objectDetector.TryDetectForVideo(image, 1, null, ref result);
          Assert.IsTrue(found);
          Assert.IsTrue(result.detections?.Count > 0);
          Assert.IsTrue(result.detections?.Count <= 10);
        }
      }
    }
    #endregion

    #region DetectAsync
    [UnityTest]
    public IEnumerator DetectAsync_ShouldInvokeTheCallbackWithAnEmptyResult_When_ImageIsEmpty()
    {
      var isCallbackInvoked = false;
      var result = DetectionResult.Alloc(0);
      void callback(DetectionResult detectionResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = detectionResult;
      };
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _objectDetectorModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          scoreThreshold: 0.1f, // specify the score threshold to avoid crash, maybe caused by DetectionsDeduplicateCalculator.
          resultCallback: callback);

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          objectDetector.DetectAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.IsNull(result.detections);
      }
    }


    [UnityTest]
    public IEnumerator DetectAsync_invokes_the_callback_if_objects_are_detected()
    {
      var isCallbackInvoked = false;
      var result = DetectionResult.Alloc(0);
      void callback(DetectionResult detectionResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = detectionResult;
      };
      var options = new ObjectDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _objectDetectorModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          maxResults: 10,
          resultCallback: callback);

      using (var objectDetector = ObjectDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          objectDetector.DetectAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.IsTrue(result.detections?.Count > 0);
        Assert.IsTrue(result.detections?.Count <= 10);
      }
    }
    #endregion

    private NativeArray<byte> BuildSolidColorData(int width, int height, Color32 color)
    {
      var srcBytes = new byte[width * height * 4];
      for (var i = 0; i < srcBytes.Length; i += 4)
      {
        srcBytes[i] = color.r;
        srcBytes[i + 1] = color.g;
        srcBytes[i + 2] = color.b;
        srcBytes[i + 3] = color.a;
      }
      return BuildPixelData(srcBytes);
    }

    private NativeArray<byte> BuildPixelData(byte[] bytes)
    {
      var pixelData = new NativeArray<byte>(bytes.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      pixelData.CopyFrom(bytes);

      return pixelData;
    }

    private Image CopyAsImage(Texture2D src)
    {
      var srcData = src.GetPixels32();
      var dst = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);

      var dstData = dst.GetPixels32();
      var w = src.width;
      var h = src.height;

      for (var x = 0; x < w; x++)
      {
        for (var y = 0; y < h; y++)
        {
          dstData[x + (y * w)] = srcData[x + ((h - y - 1) * w)];
        }
      }

      dst.SetPixels32(dstData);
      dst.Apply();

      return new Image(ImageFormat.Types.Format.Srgba, dst);
    }
  }
}
