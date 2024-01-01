// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.FaceLandmarker
{
  /// <summary>
  ///   Options for the face landmarker task.
  /// </summary>
  public sealed class FaceLandmarkerOptions : Tasks.Core.ITaskOptions
  {
    /// <param name="faceLandmarksResult">
    ///   The face landmarks detection results.
    /// </param>
    /// <param name="image">
    ///   The input image that the face landmarker runs on.
    /// </param>
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(FaceLandmarkerResult faceLandmarksResult, Image image, long timestampMillisec);

    /// <summary>
    ///   Base options for the hand landmarker task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the image mode.
    ///   FaceLandmarker has three running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The image mode for detecting face landmarks on single image inputs.</description>
    ///     </item>
    ///     <item>
    ///       <description>The video mode for detecting face landmarks on the decoded frames of a video.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The live stream mode or detecting face landmarks on the live stream of input data, such as from camera.
    ///         In this mode, the <see cref="resultCallback" /> below must be specified to receive the detection results asynchronously.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    public Core.RunningMode runningMode { get; }
    /// <summary>
    ///   The maximum number of faces that can be detected by the face detector.
    /// </summary>
    public int numFaces { get; }
    /// <summary>
    ///   The minimum confidence score for the face detection to be considered successful.
    /// </summary>
    public float minFaceDetectionConfidence { get; }
    /// <summary>
    ///   The minimum confidence score of face presence score in the face landmark detection.
    /// </summary>
    public float minFacePresenceConfidence { get; }
    /// <summary>
    ///   The minimum confidence score for the face tracking to be considered successful.
    /// </summary>
    public float minTrackingConfidence { get; }
    /// <summary>
    ///   Whether FaceLandmarker outputs face blendshapes classification.
    ///   Face blendshapes are used for rendering the 3D face model.
    /// </summary>
    public bool outputFaceBlendshapes { get; }
    /// <summary>
    ///   Whether FaceLandmarker outputs facial transformation_matrix.
    ///   Facial transformation matrix is used to transform the face landmarks in canonical face to the detected face,
    ///   so that users can apply face effects on the detected landmarks.
    /// </summary>
    public bool outputFaceTransformationMatrixes { get; }
    /// <summary>
    ///   The user-defined result callback for processing live stream data.
    ///   The result callback should only be specified when the running mode is set to the live stream mode.
    /// </summary>
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
