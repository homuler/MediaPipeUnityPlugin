// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Mediapipe.Tasks.Core;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Tasks.Vision.GestureRecognizer;
using Mediapipe.Unity;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Tests.Tasks.Vision
{
  public class GestureRecognizerTest
  {
    private const string _ResourcePath = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private const string _TestResourcePath = "Packages/com.github.homuler.mediapipe/Tests/Resources";
    private const int _CallbackTimeoutMillisec = 1000;

    private static readonly IResourceManager _ResourceManager = new LocalResourceManager();
    private readonly Lazy<TextAsset> _gestureRecognizerModel =
        new Lazy<TextAsset>(() => AssetDatabase.LoadAssetAtPath<TextAsset>($"{_ResourcePath}/gesture_recognizer.bytes"));

    private readonly Lazy<Texture2D> _thumbUpPicture =
        new Lazy<Texture2D>(() => AssetDatabase.LoadAssetAtPath<Texture2D>($"{_TestResourcePath}/thumb-up.jpg"));

    #region Create
    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelIsNotSpecified()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = GestureRecognizer.CreateFromOptions(options);
      });
    }

    [Test]
    public void Create_ShouldReturnGestureRecognizer_When_AssetModelBufferIsValid()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _gestureRecognizerModel.Value.bytes));

      Assert.DoesNotThrow(() =>
      {
        using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
        {
          gestureRecognizer.Close();
        }
      });
    }


    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelPathDoesNotExist()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "unknown_path.bytes"));

      LogAssert.Expect(LogType.Exception, new Regex("KeyNotFoundException"));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = GestureRecognizer.CreateFromOptions(options);
      });
    }

    [UnityTest]
    public IEnumerator Create_returns_ObjectDetector_when_assetModelPath_is_valid()
    {
      yield return _ResourceManager.PrepareAssetAsync("gesture_recognizer.bytes");

      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "gesture_recognizer.bytes"));

      Assert.DoesNotThrow(() =>
      {
        using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
        {
          gestureRecognizer.Close();
        }
      });
    }
    #endregion

    #region Recognize
    [Test]
    public void Recognize_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.IMAGE);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = gestureRecognizer.Recognize(image, null);
          Assert.IsNull(result.gestures);
        }
      }
    }

    [Test]
    public void Recognize_ShouldReturnGestureRecognitionResult_When_HandGesturesAreDetected()
    {
      var options = new GestureRecognizerOptions(
        new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        numHands: 2);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          var result = gestureRecognizer.Recognize(image, null);
          Assert.IsNotNull(result.gestures);
          Assert.IsTrue(result.gestures.Count > 0);
          Assert.IsTrue(result.gestures[0].categories.Count > 0);
          Assert.AreEqual("Thumb_Up", result.gestures[0].categories[0].categoryName);
        }
      }
    }

    [Test]
    public void Recognize_ShouldReturnCategoryInAllowList()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        cannedGestureClassifierOptions: new ClassifierOptions()
        {
          categoryAllowlist = new List<string> { "Thumb_Down" },
        });

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          var result = gestureRecognizer.Recognize(image, null);
          Assert.IsNotNull(result.gestures);
          Assert.IsTrue(result.gestures.Count > 0);
          Assert.IsTrue(result.gestures[0].categories.Count > 0);
          Assert.AreEqual("Thumb_Down", result.gestures[0].categories[0].categoryName);
        }
      }
    }

    [Test]
    public void Recognize_ShouldNotReturnCategoryInDenyList()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        cannedGestureClassifierOptions: new ClassifierOptions()
        {
          categoryDenylist = new List<string> { "Thumb_Up" },
        });

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          var result = gestureRecognizer.Recognize(image, null);
          Assert.IsNotNull(result.gestures);
          Assert.IsTrue(result.gestures.Count > 0);
          Assert.IsTrue(result.gestures[0].categories.Count > 0);
          Assert.AreNotEqual("Thumb_Up", result.gestures[0].categories[0].categoryName);
        }
      }
    }
    #endregion

    #region TryRecognize
    [Test]
    public void TryRecognize_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.IMAGE);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = GestureRecognizerResult.Alloc(0);
          var found = gestureRecognizer.TryRecognize(image, null, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryRecognize_ShouldReturnTrue_When_HandGesturesAreDetected()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.IMAGE);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          var result = GestureRecognizerResult.Alloc(0);
          var found = gestureRecognizer.TryRecognize(image, null, ref result);
          Assert.IsTrue(found);
          Assert.IsNotNull(result.gestures);
          Assert.IsTrue(result.gestures.Count > 0);
        }
      }
    }
    #endregion

    #region RecognizeForVideo
    [Test]
    public void RecognizeForVideo_ShouldReturnAnEmptyResult_When_ImageIsEmpty()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.VIDEO);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = gestureRecognizer.RecognizeForVideo(image, 1, null);
          Assert.IsNull(result.gestures);
        }
      }
    }

    [Test]
    public void RecognizeForVideo_ShouldReturnGestureRecognitionResult_When_HandGesturesAreDetected()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.VIDEO);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          var result = gestureRecognizer.RecognizeForVideo(image, 1, null);
          Assert.IsNotNull(result.gestures);
          Assert.IsTrue(result.gestures.Count > 0);
        }
      }
    }
    #endregion

    #region TryRecognizeForVideo
    [Test]
    public void TryRecognizeForVideo_ShouldReturnFalse_When_ImageIsEmpty()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.VIDEO);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          var result = GestureRecognizerResult.Alloc(0);
          var found = gestureRecognizer.TryRecognizeForVideo(image, 1, null, ref result);
          Assert.IsFalse(found);
        }
      }
    }

    [Test]
    public void TryRecognizeForVideo_ShouldReturnTrue_When_HandGesturesAreDetected()
    {
      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
        runningMode: RunningMode.VIDEO);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          var result = GestureRecognizerResult.Alloc(0);
          var found = gestureRecognizer.TryRecognizeForVideo(image, 1, null, ref result);
          Assert.IsTrue(found);
          Assert.IsNotNull(result.gestures);
          Assert.IsTrue(result.gestures.Count > 0);
        }
      }
    }
    #endregion

    #region RecognizeAsync
    [UnityTest]
    public IEnumerator RecognizeAsync_ShouldInvokeTheCallbackWithAnEmptyResult_When_ImageIsEmpty()
    {
      var isCallbackInvoked = false;
      var result = default(GestureRecognizerResult);
      void callback(GestureRecognizerResult recognizerResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = recognizerResult;
      };

      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        var width = 32;
        var height = 32;
        var pixelData = BuildSolidColorData(width, height, UnityEngine.Color.gray);
        using (var image = new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData))
        {
          gestureRecognizer.RecognizeAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.IsNull(result.gestures);
      }
    }

    [UnityTest]
    public IEnumerator RecognizeAsync_invokes_the_callback_if_gestures_are_detected()
    {
      var isCallbackInvoked = false;
      var result = default(GestureRecognizerResult);
      void callback(GestureRecognizerResult recognizerResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = recognizerResult;
      };

      var options = new GestureRecognizerOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _gestureRecognizerModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var gestureRecognizer = GestureRecognizer.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_thumbUpPicture.Value))
        {
          gestureRecognizer.RecognizeAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.IsTrue(result.gestures?.Count > 0);
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
