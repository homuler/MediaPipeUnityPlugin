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
using Mediapipe.Tasks.Vision.ImageSegmenter;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Tests.Tasks.Vision
{
  public class ImageSegmenterTest
  {
    private const string _ResourcePath = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private const string _TestResourcePath = "Packages/com.github.homuler.mediapipe/Tests/Resources";

    private const int _CallbackTimeoutMillisec = 1000;

    private static readonly IResourceManager _ResourceManager = new LocalResourceManager();
    private readonly Lazy<TextAsset> _hairSegmenterModel =
        new Lazy<TextAsset>(() => AssetDatabase.LoadAssetAtPath<TextAsset>($"{_ResourcePath}/hair_segmentation.bytes"));

    private readonly Lazy<Texture2D> _facePicture =
        new Lazy<Texture2D>(() => AssetDatabase.LoadAssetAtPath<Texture2D>($"{_TestResourcePath}/lenna.png"));

    #region CreateFromOptions
    [Test]
    public void CreateFromOptions_ShouldThrowBadStatusException_When_AssetModelIsNotSpecified()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = ImageSegmenter.CreateFromOptions(options);
      });
    }

    [Test]
    public void CreateFromOptions_ShouldReturnImageSegmenter_When_AssetModelBufferIsValid()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _hairSegmenterModel.Value.bytes));

      Assert.DoesNotThrow(() =>
      {
        using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
        {
          imageSegmenter.Close();
        }
      });
    }

    [Test]
    public void CreateFromOptions_ShouldThrowBadStatusException_When_AssetModelPathDoesNotExist()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "unknown_path.bytes"));

      LogAssert.Expect(LogType.Exception, new Regex("KeyNotFoundException"));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = ImageSegmenter.CreateFromOptions(options);
      });
    }

    [UnityTest]
    public IEnumerator Create_returns_ImageSegmenter_when_assetModelPath_is_valid()
    {
      yield return _ResourceManager.PrepareAssetAsync("hair_segmentation.bytes");

      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "hair_segmentation.bytes"));

      Assert.DoesNotThrow(() =>
      {
        using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
        {
          imageSegmenter.Close();
        }
      });
    }

    [Test]
    public void CreateFromOptions_ShouldThrowBadStatusException_When_NeitherOutputSegmentationMasksNorOutputCategoryMaskIsTrue()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _hairSegmenterModel.Value.bytes),
        outputConfidenceMasks: false,
        outputCategoryMask: false);

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = ImageSegmenter.CreateFromOptions(options);
      });
    }
    #endregion

    #region Segment
    [Test]
    public void Segment_ShouldReturnConfidenceMasks_With_DefaultOptions()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _hairSegmenterModel.Value.bytes),
        runningMode: RunningMode.IMAGE);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = imageSegmenter.Segment(image, null);
          Assert.AreEqual(2, result.confidenceMasks?.Count); // background, hair
          Assert.Null(result.categoryMask);
        }
      }
    }

    [Test]
    public void Segment_ShouldReturnCategoryMask_When_OutputCategoryMaskIsTrue()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _hairSegmenterModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        outputCategoryMask: true);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = imageSegmenter.Segment(image, null);
          Assert.AreEqual(2, result.confidenceMasks?.Count); // background, hair
          Assert.NotNull(result.categoryMask);
        }
      }
    }

    [Test]
    public void Segment_ShouldNotReturnConfidenceMasks_When_OutputConfidenceMasksIsFalse()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _hairSegmenterModel.Value.bytes),
        runningMode: RunningMode.IMAGE,
        outputConfidenceMasks: false,
        outputCategoryMask: true);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = imageSegmenter.Segment(image, null);
          Assert.Null(result.confidenceMasks);
          Assert.NotNull(result.categoryMask);
        }
      }
    }
    #endregion


    #region SegmentForVideo
    [Test]
    public void SegmentForVideo_ShouldReturnConfidenceMasks_With_DefaultOptions()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _hairSegmenterModel.Value.bytes),
        runningMode: RunningMode.VIDEO);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = imageSegmenter.SegmentForVideo(image, 1, null);
          Assert.AreEqual(2, result.confidenceMasks?.Count); // background, hair
          Assert.Null(result.categoryMask);
        }
      }
    }

    [Test]
    public void SegmentForVideo_ShouldReturnCategoryMask_When_OutputCategoryMaskIsTrue()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
        modelAssetBuffer: _hairSegmenterModel.Value.bytes),
        runningMode: RunningMode.VIDEO,
        outputCategoryMask: true);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          var result = imageSegmenter.SegmentForVideo(image, 1, null);
          Assert.AreEqual(2, result.confidenceMasks?.Count); // background, hair
          Assert.NotNull(result.categoryMask);
        }
      }
    }
    #endregion

    #region SegmentAsync
    [UnityTest]
    public IEnumerator SegmentAsync_invokes_the_callback_with_default_options()
    {
      var isCallbackInvoked = false;
      var result = ImageSegmenterResult.Alloc(true);
      void callback(ImageSegmenterResult segmentationResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = segmentationResult;
      };
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _hairSegmenterModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          resultCallback: callback);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          imageSegmenter.SegmentAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.AreEqual(2, result.confidenceMasks?.Count); // background, hair
        Assert.Null(result.categoryMask);
      }
    }

    [UnityTest]
    public IEnumerator SegmentAsync_invokes_the_callback_when_outputCategoryMask_is_true()
    {
      var isCallbackInvoked = false;
      var result = ImageSegmenterResult.Alloc(true);
      void callback(ImageSegmenterResult segmentationResult, Image image, long timestamp)
      {
        isCallbackInvoked = true;
        result = segmentationResult;
      };
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _hairSegmenterModel.Value.bytes),
          runningMode: RunningMode.LIVE_STREAM,
          outputCategoryMask: true,
          resultCallback: callback);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        using (var image = CopyAsImage(_facePicture.Value))
        {
          imageSegmenter.SegmentAsync(image, 1, null);
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          return isCallbackInvoked || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(isCallbackInvoked);
        Assert.AreEqual(2, result.confidenceMasks?.Count); // background, hair
        Assert.NotNull(result.categoryMask);
      }
    }
    #endregion

    #region Labels
    [Test]
    public void Labels_should_return_an_empty_list()
    {
      var options = new ImageSegmenterOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _hairSegmenterModel.Value.bytes),
          runningMode: RunningMode.IMAGE);

      using (var imageSegmenter = ImageSegmenter.CreateFromOptions(options))
      {
        Assert.AreEqual(0, imageSegmenter.labels.Count);
      }
    }
    #endregion

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
