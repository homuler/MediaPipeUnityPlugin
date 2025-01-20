// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Mediapipe.Tasks.Core;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Tasks.Vision.HolisticLandmarker;
using Mediapipe.Unity;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Tests.Tasks.Vision
{
  public class HolisticLandmarkerTest
  {
    private const string _ResourcePath = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private const string _TestResourcePath = "Packages/com.github.homuler.mediapipe/Tests/Resources";

    private const int _CallbackTimeoutMillisec = 1000;
    private const int _PoseLandmarksCount = 33;

    private static readonly IResourceManager _ResourceManager = new LocalResourceManager();
    private readonly Lazy<TextAsset> _holisticLandmarkerModel =
        new Lazy<TextAsset>(() => AssetDatabase.LoadAssetAtPath<TextAsset>($"{_ResourcePath}/holistic_landmarker.bytes"));

    private readonly Lazy<Texture2D> _facePicture =
        new Lazy<Texture2D>(() => AssetDatabase.LoadAssetAtPath<Texture2D>($"{_TestResourcePath}/lenna.png"));

    #region Create
    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelIsNotSpecified()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = HolisticLandmarker.CreateFromOptions(options);
      });
    }

    [Test]
    public void Create_ShouldReturnHolisticLandmarker_When_AssetModelBufferIsValid()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes));

      Assert.DoesNotThrow(() =>
      {
        using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
        {
          landmarker.Close();
        }
      });
    }

    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelPathDoesNotExist()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "unknown_path.bytes"));

      LogAssert.Expect(LogType.Exception, new Regex("KeyNotFoundException"));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = HolisticLandmarker.CreateFromOptions(options);
      });
    }

    [UnityTest]
    public IEnumerator Create_returns_HolisticLandmarker_when_assetModelPath_is_valid()
    {
      yield return _ResourceManager.PrepareAssetAsync("holistic_landmarker.bytes");

      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "holistic_landmarker.bytes"));

      Assert.DoesNotThrow(() =>
      {
        using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
        {
          landmarker.Close();
        }
      });
    }
    #endregion

    #region Detect
    [Test]
    public void Detect_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = landmarker.Detect(image);
          Assert.IsNull(result.poseLandmarks.landmarks);
        }
      }
    }

    [Test]
    public void Detect_ShouldReturnHolisticLandmarkerResult_When_PoseIsDetected()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = landmarker.Detect(image);
          Assert.AreEqual(_PoseLandmarksCount, result.poseLandmarks.landmarks?.Count);
        }
      }
    }
    #endregion

    #region TryDetect
    [Test]
    public void TryeDetect_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = new HolisticLandmarkerResult();
          var found = landmarker.TryDetect(image, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryeDetect_ShouldReturnTrue_When_PoseIsDetected()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.IMAGE);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = new HolisticLandmarkerResult();
          var found = landmarker.TryDetect(image, ref result);
          Assert.IsTrue(found);
          Assert.AreEqual(_PoseLandmarksCount, result.poseLandmarks.landmarks?.Count);
        }
      }
    }
    #endregion

    #region DetectForVideo
    [Test]
    public void DetectForVideo_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = landmarker.DetectForVideo(image, 1);
          Assert.IsNull(result.poseLandmarks.landmarks);
        }
      }
    }

    [Test]
    public void DetectForVideo_ShouldReturnHolisticLandmarkerResult_When_PoseIsDetected()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = landmarker.DetectForVideo(image, 1);
          Assert.AreEqual(_PoseLandmarksCount, result.poseLandmarks.landmarks?.Count);
        }
      }
    }
    #endregion

    #region TryDetectForVideo
    [Test]
    public void TryDetectForVideo_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = new HolisticLandmarkerResult();
          var found = landmarker.TryDetectForVideo(image, 1, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryDetectForVideo_ShouldReturnTrue_When_PoseIsDetected()
    {
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _holisticLandmarkerModel.Value.bytes), runningMode: RunningMode.VIDEO);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = new HolisticLandmarkerResult();
          var found = landmarker.TryDetectForVideo(image, 1, ref result);
          Assert.IsTrue(found);
          Assert.AreEqual(_PoseLandmarksCount, result.poseLandmarks.landmarks?.Count);
        }
      }
    }
    #endregion

    #region DetectAsync
    [UnityTest]
    public IEnumerator DetectAsync_ShouldInvokeTheCallbackWithAnEmptyResult_When_ImageIsEmpty()
    {
      var isCallbackInvoked = false;
      var result = new HolisticLandmarkerResult();
      void callback(in HolisticLandmarkerResult landmarkerResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = landmarkerResult;
      };
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _holisticLandmarkerModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          landmarker.DetectAsync(image, 1);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.IsNull(result.poseLandmarks.landmarks);
      }
    }


    [UnityTest]
    public IEnumerator DetectAsync_invokes_the_callback_If_pose_is_detected()
    {
      var isCallbackInvoked = false;
      var result = new HolisticLandmarkerResult();
      void callback(in HolisticLandmarkerResult detectionResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = detectionResult;
      };
      var options = new HolisticLandmarkerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _holisticLandmarkerModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var landmarker = HolisticLandmarker.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          landmarker.DetectAsync(image, 1);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.AreEqual(_PoseLandmarksCount, result.poseLandmarks.landmarks?.Count);
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
