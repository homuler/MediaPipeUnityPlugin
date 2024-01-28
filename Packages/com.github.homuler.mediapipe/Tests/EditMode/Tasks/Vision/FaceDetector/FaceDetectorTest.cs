using System;
using System.Collections;
using NUnit.Framework;
using Mediapipe.Tasks.Core;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Tasks.Vision.FaceDetector;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using System.Text.RegularExpressions;
using Mediapipe.Tasks.Components.Containers;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Tests.Tasks.Vision
{
  public class FaceDetectorTest
  {
    private const string _ResourcePath = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private const string _TestResourcePath = "Packages/com.github.homuler.mediapipe/Tests/Resources";

    private const int _CallbackTimeoutMillisec = 1000;

    // private static readonly ResourceManager _ResourceManager = new LocalResourceManager();
    private readonly Lazy<TextAsset> _faceDetectorModel =
        new Lazy<TextAsset>(() => AssetDatabase.LoadAssetAtPath<TextAsset>($"{_ResourcePath}/blaze_face_short_range.bytes"));

    private readonly Lazy<Texture2D> _facePicture =
        new Lazy<Texture2D>(() => AssetDatabase.LoadAssetAtPath<Texture2D>($"{_TestResourcePath}/lenna.png"));

    #region Create
    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelIsNotSpecified()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = FaceDetector.CreateFromOptions(options);
      });
    }

    [Test]
    public void Create_ShouldReturnFaceDetector_When_AssetModelBufferIsValid()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes));

      Assert.DoesNotThrow(() =>
      {
        using (var faceDetector = FaceDetector.CreateFromOptions(options))
        {
          faceDetector.Close();
        }
      });
    }

    [Test, Ignore("TODO: enable to initialize ResourceManager twice")]
    public void Create_ShouldThrowBadStatusException_When_AssetModelPathDoesNotExist()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "unknown_path.bytes"));

      LogAssert.Expect(LogType.Exception, new Regex("FileNotFoundException"));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = FaceDetector.CreateFromOptions(options);
      });
    }

    [UnityTest, Ignore("TODO: enable to initialize ResourceManager twice")]
    public IEnumerator Create_returns_FaceLandmarker_when_assetModelPath_is_valid()
    {
      // yield return _ResourceManager.PrepareAssetAsync("blaze_face_short_range.bytes");
      yield return null;

      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "blaze_face_short_range.bytes"));

      Assert.DoesNotThrow(() =>
      {
        using (var faceLandmarker = FaceDetector.CreateFromOptions(options))
        {
          faceLandmarker.Close();
        }
      });
    }
    #endregion

    #region Detect
    [Test]
    public void Detect_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = faceDetector.Detect(image, null);
          Assert.IsNull(result.detections);
        }
      }
    }

    [Test]
    public void Detect_ShouldReturnFaceDetectionResult_When_FacesAreDetected()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = faceDetector.Detect(image, null);
          Assert.AreEqual(1, result.detections?.Count);
        }
      }
    }
    #endregion

    #region TryDetect
    [Test]
    public void TryeDetect_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = DetectionResult.Alloc(0);
          var found = faceDetector.TryDetect(image, null, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryeDetect_ShouldReturnTrue_When_FacesAreDetected()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = DetectionResult.Alloc(0);
          var found = faceDetector.TryDetect(image, null, ref result);
          Assert.IsTrue(found);
          Assert.AreEqual(1, result.detections?.Count);
        }
      }
    }
    #endregion

    #region DetectForVideo
    [Test]
    public void DetectForVideo_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = faceDetector.DetectForVideo(image, 1, null);
          Assert.IsNull(result.detections);
        }
      }
    }

    [Test]
    public void DetectForVideo_ShouldReturnFaceDetectionResult_When_FacesAreDetected()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = faceDetector.DetectForVideo(image, 1, null);
          Assert.AreEqual(1, result.detections?.Count);
        }
      }
    }
    #endregion

    #region TryDetectForVideo
    [Test]
    public void TryDetectForVideo_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = DetectionResult.Alloc(0);
          var found = faceDetector.TryDetectForVideo(image, 1, null, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryDetectForVideo_ShouldReturnTrue_When_FacesAreDetected()
    {
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _faceDetectorModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = DetectionResult.Alloc(0);
          var found = faceDetector.TryDetectForVideo(image, 1, null, ref result);
          Assert.IsTrue(found);
          Assert.AreEqual(1, result.detections.Count);
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
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _faceDetectorModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          faceDetector.DetectAsync(image, 1, null);
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
    public IEnumerator DetectAsync_invokes_the_callback_If_faces_are_detected()
    {
      var isCallbackInvoked = false;
      var result = DetectionResult.Alloc(0);
      void callback(DetectionResult detectionResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = detectionResult;
      };
      var options = new FaceDetectorOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _faceDetectorModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var faceDetector = FaceDetector.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          faceDetector.DetectAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.AreEqual(1, result.detections?.Count);
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
