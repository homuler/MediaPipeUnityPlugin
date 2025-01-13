// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Core;
using Mediapipe.Tasks.Audio.Core;
using Mediapipe.Tasks.Audio.AudioClassifier;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Tests.Tasks.Audio
{
  public class AudioClassifierTest
  {
    private const string _ResourcePath = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private const string _TestResourcePath = "Packages/com.github.homuler.mediapipe/Tests/Resources";

    private const int _CallbackTimeoutMillisec = 2000;

    private static readonly IResourceManager _ResourceManager = new LocalResourceManager();
    private readonly Lazy<TextAsset> _audioClassifierModel =
        new Lazy<TextAsset>(() => AssetDatabase.LoadAssetAtPath<TextAsset>($"{_ResourcePath}/yamnet_audio_classifier_with_metadata.bytes"));

    private readonly Lazy<AudioClip> _doorSound =
        new Lazy<AudioClip>(() => AssetDatabase.LoadAssetAtPath<AudioClip>($"{_TestResourcePath}/prison-cell-door.mp3"));

    #region Create
    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelIsNotSpecified()
    {
      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = AudioClassifier.CreateFromOptions(options);
      });
    }

    [Test]
    public void Create_ShouldReturnAudioClassifier_When_AssetModelBufferIsValid()
    {
      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes));

      Assert.DoesNotThrow(() =>
      {
        using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
        {
          audioClassifier.Close();
        }
      });
    }

    [Test]
    public void Create_ShouldThrowBadStatusException_When_AssetModelPathDoesNotExist()
    {
      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "unknown_path.bytes"));

      LogAssert.Expect(LogType.Exception, new Regex("KeyNotFoundException"));

      _ = Assert.Throws<BadStatusException>(() =>
      {
        using var _ = AudioClassifier.CreateFromOptions(options);
      });
    }

    [UnityTest]
    public IEnumerator Create_ShouldReturnAudioClassifier_When_AssetModelPathIsValid()
    {
      yield return _ResourceManager.PrepareAssetAsync("yamnet_audio_classifier_with_metadata.bytes");

      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetPath: "yamnet_audio_classifier_with_metadata.bytes"));

      Assert.DoesNotThrow(() =>
      {
        using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
        {
          audioClassifier.Close();
        }
      });
    }
    #endregion

    #region Classify
    [Test]
    public void Classify_ShouldReturnAudioClassifierResult_When_AudioClipIsEmpty()
    {
      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes), runningMode: RunningMode.AUDIO_CLIPS);

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate];
        var audioClip = new Matrix(sound, 2, sampleRate, Matrix.Layout.ColMajor);
        var results = audioClassifier.Classify(audioClip, sampleRate);

        Assert.IsTrue(results.Count > 1);

        var result1 = results[0];
        Assert.AreEqual(0, result1.timestampMs);
        Assert.IsTrue(result1.classifications.Count > 0);

        var classification = result1.classifications[0];
        Assert.IsTrue(classification.categories.Count > 0);

        var category1 = classification.categories[0];
        Assert.AreEqual("Silence", category1.categoryName);

#pragma warning disable IDE0056
        var lastCategory = classification.categories[classification.categories.Count - 1];
#pragma warning restore IDE0056
        Assert.IsTrue(Mathf.Approximately(0.0f, lastCategory.score));

        var result2 = results[1];
        Assert.AreNotEqual(0, result2.timestampMs);
      }
    }

    [Test]
    public void Classify_ShouldReturnCategoriesLessThanMaxResults()
    {
      var options = new AudioClassifierOptions(
        new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes),
        runningMode: RunningMode.AUDIO_CLIPS,
        maxResults: 1);

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate];
        var audioClip = new Matrix(sound, 2, sampleRate, Matrix.Layout.ColMajor);
        var results = audioClassifier.Classify(audioClip, sampleRate);

        Assert.IsTrue(results.Count > 0);

        var categories = results[0].classifications?[0].categories;
        Assert.AreEqual(1, categories?.Count);
      }
    }

    [Test]
    public void Classify_ShouldReturnCategories_WhoseScoreIsOverTheThreshold()
    {
      var scoreThresold = 0.3f;
      var options = new AudioClassifierOptions(
        new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes),
        runningMode: RunningMode.AUDIO_CLIPS,
        scoreThreshold: scoreThresold);

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate];
        var audioClip = new Matrix(sound, 2, sampleRate, Matrix.Layout.ColMajor);
        var results = audioClassifier.Classify(audioClip, sampleRate);

        Assert.IsTrue(results.Count > 0);

        var categories = results[0].classifications?[0].categories;
        Assert.IsTrue(categories?.Count > 0);
#pragma warning disable IDE0056
        Assert.IsTrue(categories[categories.Count - 1].score >= scoreThresold);
#pragma warning restore IDE0056
      }
    }

    [Test]
    public void Classify_ShouldReturnCategoryInAllowList()
    {
      var options = new AudioClassifierOptions(
        new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes),
        runningMode: RunningMode.AUDIO_CLIPS,
        categoryAllowList: new List<string> { "Sound effect" });

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate];
        var audioClip = new Matrix(sound, 2, sampleRate, Matrix.Layout.ColMajor);
        var results = audioClassifier.Classify(audioClip, sampleRate);

        Assert.IsTrue(results.Count > 0);

        var categories = results[0].classifications?[0].categories;
        Assert.AreEqual(1, categories.Count);

        var category = categories[0];
        Assert.AreEqual("Sound effect", category.categoryName);
      }
    }

    [Test]
    public void Classify_ShouldNotReturnCategoryInDenyList()
    {
      var options = new AudioClassifierOptions(
        new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes),
        runningMode: RunningMode.AUDIO_CLIPS,
        categoryDenyList: new List<string> { "Silence" });

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate];
        var audioClip = new Matrix(sound, 2, sampleRate, Matrix.Layout.ColMajor);
        var results = audioClassifier.Classify(audioClip, sampleRate);

        Assert.IsTrue(results.Count > 0);

        var category = results[0].classifications?[0].categories?[0];
        Assert.AreNotEqual("Silence", category?.categoryName);
      }
    }

    [Test]
    public void Classify_ShouldReturnAudioClassifierResult_When_AudioClipIsNotEmpty()
    {
      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes), runningMode: RunningMode.AUDIO_CLIPS);

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var source = _doorSound.Value;
        var sound = new float[source.channels * source.samples];
        Assert.True(_doorSound.Value.GetData(sound, 0));

        var audioClip = new Matrix(sound, source.channels, source.samples, Matrix.Layout.ColMajor);
        var results = audioClassifier.Classify(audioClip, source.frequency);

        Assert.IsTrue(results.Count > 0);
      }
    }
    #endregion

    #region TryClassify
    [Test]
    public void TryClassify_ShouldReturnAudioClassifierResult_When_AudioClipIsNotEmpty()
    {
      var options = new AudioClassifierOptions(
        new BaseOptions(BaseOptions.Delegate.CPU, modelAssetBuffer: _audioClassifierModel.Value.bytes),
        runningMode: RunningMode.AUDIO_CLIPS,
        maxResults: 5);

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate];
        var audioClip = new Matrix(sound, 2, sampleRate, Matrix.Layout.ColMajor);

        var results = new List<ClassificationResult>();
        Assert.IsTrue(audioClassifier.TryClassify(audioClip, sampleRate, results));

        var category = results?[0].classifications?[0].categories?[0];
        Assert.AreEqual("Silence", category?.categoryName);
      }
    }
    #endregion

    #region ClassifyAsync
    [UnityTest]
    public IEnumerator ClassifyAsync_ShouldInvokeTheCallback()
    {
      var invokeCount = 0;
      var successCount = 0;
      void callback(ClassificationResult classificationResult, long timestamp)
      {
        invokeCount++;

        var category = classificationResult.classifications?[0].categories?[0];
        Assert.AreEqual("Silence", category?.categoryName);

        successCount++;
      };

      var options = new AudioClassifierOptions(new BaseOptions(BaseOptions.Delegate.CPU,
          modelAssetBuffer: _audioClassifierModel.Value.bytes),
          runningMode: RunningMode.AUDIO_STREAM,
          resultCallback: callback);

      using (var audioClassifier = AudioClassifier.CreateFromOptions(options))
      {
        var sampleRate = 16000;
        var sound = new float[2 * sampleRate * 3];
        var audioClip = new Matrix(sound, 2, sampleRate * 3, Matrix.Layout.ColMajor);

        audioClassifier.ClassifyAsync(audioClip, sampleRate, 0);

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        yield return new WaitUntil(() =>
        {
          // wait for the callback to be invoked 3 times
          return invokeCount > 2 || stopwatch.ElapsedMilliseconds > _CallbackTimeoutMillisec;
        });

        Assert.IsTrue(invokeCount > 2);
        Assert.AreEqual(successCount, invokeCount);
      }
    }
    #endregion
  }
}
