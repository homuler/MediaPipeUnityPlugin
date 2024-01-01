// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.FaceDetector
{
  /// <summary>
  ///   Options for the face detector task.
  /// </summary>
  public sealed class FaceDetectorOptions : Tasks.Core.ITaskOptions
  {
    /// <remarks>
    ///   Some field of <paramref name="detectionResult"/> can be reused to reduce GC.Alloc.
    ///   If you need to refer to the data later, copy the data.
    /// </remarks>
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
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(Components.Containers.DetectionResult detectionResult, Image image, long timestampMillisec);

    /// <summary>
    ///   Base options for the face detector task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the image mode.
    ///   Face detector task has three running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The image mode for detecting faces on single image inputs.</description>
    ///     </item>
    ///     <item>
    ///       <description>The video mode for detecting faces on the decoded frames of a video.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The live stream mode or detecting faces on the live stream of input data, such as from camera.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    public Core.RunningMode runningMode { get; }
    /// <summary>
    ///   The minimum confidence score for the face detection to be considered successful.
    /// </summary>
    public float minDetectionConfidence { get; }
    /// <summary>
    ///   The minimum non-maximum-suppression threshold for face detection to be considered overlapped.
    /// </summary>
    public float minSuppressionThreshold { get; }
    /// <summary>
    ///   The maximum number of faces that can be detected by the face detector.
    /// </summary>
    public int numFaces { get; }
    /// <summary>
    ///   The user-defined result callback for processing live stream data.
    ///   The result callback should only be specified when the running mode is set to the live stream mode.
    /// </summary>
    public ResultCallback resultCallback { get; }

    public FaceDetectorOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      float minDetectionConfidence = 0.5f,
      float minSuppressionThreshold = 0.3f,
      int numFaces = 3,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.minDetectionConfidence = minDetectionConfidence;
      this.minSuppressionThreshold = minSuppressionThreshold;
      this.numFaces = numFaces;
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
        NumFaces = numFaces,
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
