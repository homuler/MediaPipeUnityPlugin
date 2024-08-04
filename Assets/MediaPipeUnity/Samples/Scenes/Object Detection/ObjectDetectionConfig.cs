// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.ComponentModel;

namespace Mediapipe.Unity.Sample.ObjectDetection
{
  public enum ModelType : int
  {
    [Description("EfficientDet-Lite0 (float 16)")]
    EfficientDetLite0Float16 = 0,

    [Description("EfficientDet-Lite0 (float 32)")]
    EfficientDetLite0Float32 = 1,

    [Description("EfficientDet-Lite0 (int8)")]
    EfficientDetLite0Int8 = 2,

    [Description("EfficientDet-Lite2 (float 16)")]
    EfficientDetLite2Float16 = 3,

    [Description("EfficientDet-Lite2 (float 32)")]
    EfficientDetLite2Float32 = 4,

    [Description("EfficientDet-Lite2 (int8)")]
    EfficientDetLite2Int8 = 5,

    [Description("SSDMobileNet-V2 (float 16)")]
    SSDMobileNetV2Float16 = 6,

    [Description("SSDMobileNet-V2 (float 32)")]
    SSDMobileNetV2Float32 = 7,
  }

  public class ObjectDetectionConfig
  {
    public Tasks.Core.BaseOptions.Delegate Delegate { get; set; } =
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
      Tasks.Core.BaseOptions.Delegate.CPU;
#else
    Tasks.Core.BaseOptions.Delegate.GPU;
#endif

    public ModelType Model { get; set; } = ModelType.EfficientDetLite0Float16;

    public Tasks.Vision.Core.RunningMode RunningMode { get; set; } = Tasks.Vision.Core.RunningMode.LIVE_STREAM;

    public int MaxResults { get; set; } = -1;

    public float ScoreThreshold { get; set; } = 0.5f;

    public string ModelName => Model.GetDescription() ?? Model.ToString();
    public string ModelPath
    {
      get
      {
        switch (Model)
        {
          case ModelType.EfficientDetLite0Float16:
            return "efficientdet_lite0_float16.bytes";
          case ModelType.EfficientDetLite0Float32:
            return "efficientdet_lite0_float32.bytes";
          case ModelType.EfficientDetLite0Int8:
            return "efficientdet_lite0_int8.bytes";
          case ModelType.EfficientDetLite2Float16:
            return "efficientdet_lite2_float16.bytes";
          case ModelType.EfficientDetLite2Float32:
            return "efficientdet_lite2_float32.bytes";
          case ModelType.EfficientDetLite2Int8:
            return "efficientdet_lite2_int8.bytes";
          case ModelType.SSDMobileNetV2Float16:
            return "ssd_mobilenet_v2_float16.bytes";
          case ModelType.SSDMobileNetV2Float32:
            return "ssd_mobilenet_v2_float32.bytes";
          default:
            return null;
        }
      }
    }

    public Tasks.Vision.ObjectDetector.ObjectDetectorOptions GetObjectDetectorOptions(Tasks.Vision.ObjectDetector.ObjectDetectorOptions.ResultCallback resultCallback = null)
    {
      return new Tasks.Vision.ObjectDetector.ObjectDetectorOptions(
        new Tasks.Core.BaseOptions(Delegate, modelAssetPath: ModelPath),
        runningMode: RunningMode,
        maxResults: MaxResults,
        scoreThreshold: ScoreThreshold,
        resultCallback: resultCallback
      );
    }
  }
}
