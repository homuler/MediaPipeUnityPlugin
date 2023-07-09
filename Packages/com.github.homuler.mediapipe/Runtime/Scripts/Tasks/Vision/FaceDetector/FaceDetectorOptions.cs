// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.FaceDetector
{
  public sealed class FaceDetectorOptions : Tasks.Core.ITaskOptions
  {
    /// <param name="detectionResult">
    ///   face detection result object that contains a list of face detections,
    ///   each detection has a bounding box that is expressed in the unrotated
    ///   input frame of reference coordinates system,
    ///   i.e. in `[0,image_width) x [0,image_height)`, which are the dimensions
    ///   of the underlying image data.
    /// </param>
    /// <param name="image">
    ///   The input image that the face detector runs on.
    /// </param>
    /// <param name="timestampMs">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(Components.Containers.DetectionResult detectionResult, Image image, int timestampMs);

    public Tasks.Core.BaseOptions baseOptions { get; }
    public Core.RunningMode runningMode { get; }
    public float minDetectionConfidence { get; } = 0.5f;
    public float minSuppressionThreshold { get; } = 0.3f;
    public ResultCallback resultCallback { get; }

    public FaceDetectorOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      float minDetectionConfidence = 0.5f,
      float minSuppressionThreshold = 0.3f,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.minDetectionConfidence = minDetectionConfidence;
      this.minSuppressionThreshold = minSuppressionThreshold;
      this.resultCallback = resultCallback;
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
