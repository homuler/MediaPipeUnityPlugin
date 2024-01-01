// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.PoseLandmarker
{
  /// <summary>
  ///   Options for the pose landmarker task.
  /// </summary>
  public sealed class PoseLandmarkerOptions : Tasks.Core.ITaskOptions
  {
    /// <param name="poseLandmarksResult">
    ///   The pose landmarker detection results.
    /// </param>
    /// <param name="image">
    ///   The input image that the pose landmarker runs on.
    /// </param>
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(PoseLandmarkerResult poseLandmarksResult, Image image, long timestampMillisec);

    /// <summary>
    ///   Base options for the pose landmarker task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the image mode.
    ///   PoseLandmarker has three running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The image mode for detecting pose landmarks on single image inputs.</description>
    ///     </item>
    ///     <item>
    ///       <description>The video mode for detecting pose landmarks on the decoded frames of a video.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The live stream mode or detecting pose landmarks on the live stream of input data, such as from camera.
    ///         In this mode, the <see cref="resultCallback" /> below must be specified to receive the detection results asynchronously.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    public Core.RunningMode runningMode { get; }
    /// <summary>
    ///   The maximum number of poses can be detected by the pose landmarker.
    /// </summary>
    public int numPoses { get; }
    /// <summary>
    ///   The minimum confidence score for the pose detection to be considered successful.
    /// </summary>
    public float minPoseDetectionConfidence { get; }
    /// <summary>
    ///   The minimum confidence score of pose presence score in the pose landmark detection.
    /// </summary>
    public float minPosePresenceConfidence { get; }
    /// <summary>
    ///   The minimum confidence score for the pose tracking to be considered successful.
    /// </summary>
    public float minTrackingConfidence { get; }
    /// <summary>
    ///   whether to output segmentation masks.
    /// </summary>
    public bool outputSegmentationMasks { get; }
    /// <summary>
    ///   The user-defined result callback for processing live stream data.
    ///   The result callback should only be specified when the running mode is set to the live stream mode.
    /// </summary>
    public ResultCallback resultCallback { get; }

    public PoseLandmarkerOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      int numPoses = 1,
      float minPoseDetectionConfidence = 0.5f,
      float minPosePresenceConfidence = 0.5f,
      float minTrackingConfidence = 0.5f,
      bool outputSegmentationMasks = false,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.numPoses = numPoses;
      this.minPoseDetectionConfidence = minPoseDetectionConfidence;
      this.minPosePresenceConfidence = minPosePresenceConfidence;
      this.minTrackingConfidence = minTrackingConfidence;
      this.outputSegmentationMasks = outputSegmentationMasks;
      this.resultCallback = resultCallback;
    }

    internal Proto.PoseLandmarkerGraphOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.IMAGE;

      return new Proto.PoseLandmarkerGraphOptions
      {
        BaseOptions = baseOptionsProto,
        PoseDetectorGraphOptions = new PoseDetector.Proto.PoseDetectorGraphOptions
        {
          NumPoses = numPoses,
          MinDetectionConfidence = minPoseDetectionConfidence,
        },
        PoseLandmarksDetectorGraphOptions = new Proto.PoseLandmarksDetectorGraphOptions
        {
          MinDetectionConfidence = minPosePresenceConfidence,
        },
        MinTrackingConfidence = minTrackingConfidence,
      };
    }

    CalculatorOptions Tasks.Core.ITaskOptions.ToCalculatorOptions()
    {
      var options = new CalculatorOptions();
      options.SetExtension(Proto.PoseLandmarkerGraphOptions.Extensions.Ext, ToProto());
      return options;
    }
  }
}
