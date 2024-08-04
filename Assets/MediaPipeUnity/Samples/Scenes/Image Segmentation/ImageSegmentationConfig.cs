// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.ComponentModel;
using Mediapipe.Tasks.Vision.ImageSegmenter;

namespace Mediapipe.Unity.Sample.ImageSegmentation
{
  public enum ModelType : int
  {
    [Description("HairSegmenter")]
    HairSegmenter = 0,
    [Description("SelfieSegmenter (square)")]
    SelfieSegmenterSquare = 1,
    [Description("SelfieSegmenter (landscape)")]
    SelfieSegmenterLandscape = 2,
  }

  public enum SelfieModelCategory : int
  {
    Person = 0,
  }

  public enum HairModelCategory : int
  {
    Background = 0,
    Hair = 1,
  }

  public class ImageSegmentationConfig
  {
    public Tasks.Core.BaseOptions.Delegate Delegate { get; set; } =
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
      Tasks.Core.BaseOptions.Delegate.CPU;
#else
    Tasks.Core.BaseOptions.Delegate.GPU;
#endif

    public ModelType Model { get; set; } = ModelType.HairSegmenter;
    public Tasks.Vision.Core.RunningMode RunningMode { get; set; } = Tasks.Vision.Core.RunningMode.LIVE_STREAM;

    public int CategoryIndex {
      get
      {
        switch (Model)
        {
          case ModelType.SelfieSegmenterSquare:
          case ModelType.SelfieSegmenterLandscape:
            return (int)SelfieModelCategory.Person;
          case ModelType.HairSegmenter:
            return (int)HairModelCategory.Hair;
          default:
            return 0;
        }
      }
    }

    public string ModelName => Model.GetDescription() ?? Model.ToString();
    public string ModelPath
    {
      get
      {
        switch (Model)
        {
          case ModelType.SelfieSegmenterSquare:
            return "selfie_segmentation.bytes";
          case ModelType.SelfieSegmenterLandscape:
            return "selfie_segmentation_landscape.bytes";
          case ModelType.HairSegmenter:
            return "hair_segmentation.bytes";
          default:
            return null;
        }
      }
    }

    public ImageSegmenterOptions GetImageSegmenterOptions(ImageSegmenterOptions.ResultCallback resultCallback = null)
    {
      return new ImageSegmenterOptions(
        new Tasks.Core.BaseOptions(Delegate, modelAssetPath: ModelPath),
        runningMode: RunningMode,
        resultCallback: resultCallback
      );
    }
  }
}
