// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.FaceLandmarker
{
  public sealed class FaceLandmarkerOptions : Tasks.Core.ITaskOptions
  {
    /// <param name="faceLandmarksResult">
    ///   The face landmarks detection results.
    /// </param>
    /// <param name="image">
    ///   The input image that the face landmarker runs on.
    /// </param>
    /// <param name="timestampMs">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(FaceLandmarkerResult faceLandmarksResult, Image image, int timestampMs);

    public Tasks.Core.BaseOptions baseOptions { get; }
    public Core.RunningMode runningMode { get; }
    public int numFaces { get; }
    public float minFaceDetectionConfidence { get; }
    public float minFacePresenceConfidence { get; }
    public float minTrackingConfidence { get; }
    public bool outputFaceBlendshapes { get; }
    public bool outputFaceTransformationMatrixes { get; }
    public ResultCallback resultCallback { get; }

    public FaceLandmarkerOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      int numFaces = 1,
      float minFaceDetectionConfidence = 0.5f,
      float minFacePresenceConfidence = 0.5f,
      float minTrackingConfidence = 0.5f,
      bool outputFaceBlendshapes = false,
      bool outputFaceTransformationMatrixes = false,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.numFaces = numFaces;
      this.minFaceDetectionConfidence = minFaceDetectionConfidence;
      this.minFacePresenceConfidence = minFacePresenceConfidence;
      this.minTrackingConfidence = minTrackingConfidence;
      this.outputFaceBlendshapes = outputFaceBlendshapes;
      this.outputFaceTransformationMatrixes = outputFaceTransformationMatrixes;
      this.resultCallback = resultCallback;
    }

    internal Proto.FaceLandmarkerGraphOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.IMAGE;

      return new Proto.FaceLandmarkerGraphOptions
      {
        BaseOptions = baseOptionsProto,
        FaceDetectorGraphOptions = new FaceDetector.Proto.FaceDetectorGraphOptions
        {
          MinDetectionConfidence = minFaceDetectionConfidence,
          NumFaces = numFaces,
        },
        FaceLandmarksDetectorGraphOptions = new Proto.FaceLandmarksDetectorGraphOptions
        {
          MinDetectionConfidence = minFacePresenceConfidence,
        },
        MinTrackingConfidence = minTrackingConfidence,
      };
    }

    CalculatorOptions Tasks.Core.ITaskOptions.ToCalculatorOptions()
    {
      var options = new CalculatorOptions();
      options.SetExtension(Proto.FaceLandmarkerGraphOptions.Extensions.Ext, ToProto());
      return options;
    }
  }
}
