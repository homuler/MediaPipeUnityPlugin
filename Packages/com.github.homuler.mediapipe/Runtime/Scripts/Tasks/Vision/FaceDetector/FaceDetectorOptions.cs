// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.FaceDetector
{
  public sealed class FaceDetectorOptions : Tasks.Core.ITaskOptions
  {
    public delegate void ResultCallback();

    public Tasks.Core.BaseOptions baseOptions { get; }
    public Core.RunningMode runningMode { get; }
    public float minDetectionConfidence { get; } = 0.5f;
    public float minSuppressionThreshold { get; } = 0.3f;

    public FaceDetectorOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      float minDetectionConfidence = 0.5f,
      float minSuppressionThreshold = 0.3f)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.minDetectionConfidence = minDetectionConfidence;
      this.minSuppressionThreshold = minSuppressionThreshold;
    }

    internal Proto.FaceDetectorGraphOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.IMAGE;

      return new Proto.FaceDetectorGraphOptions
      {
        BaseOptions = baseOptionsProto,
        MinDetectionConfidence = minDetectionConfidence,
        MinSuppressionThreshold = minSuppressionThreshold,
      };
    }

    CalculatorOptions Tasks.Core.ITaskOptions.ToCalculatorOptions()
    {
      var options = new CalculatorOptions();
      options.SetExtension(Proto.FaceDetectorGraphOptions.Extensions.Ext, ToProto());
      return options;
    }
  }
}
