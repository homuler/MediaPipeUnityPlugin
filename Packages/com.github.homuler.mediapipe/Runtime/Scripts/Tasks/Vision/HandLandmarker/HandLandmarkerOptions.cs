// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.HandLandmarker
{
  /// <summary>
  ///   Options for the hand landmarker task.
  /// </summary>
  public sealed class HandLandmarkerOptions : Tasks.Core.ITaskOptions
  {
    /// <param name="handLandmarksResult">
    ///   The hand landmarks detection results.
    /// </param>
    /// <param name="image">
    ///   The input image that the hand landmarker runs on.
    /// </param>
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(HandLandmarkerResult handLandmarksResult, Image image, long timestampMillisec);

    /// <summary>
    ///   Base options for the hand landmarker task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the image mode.
    ///   HandLandmarker has three running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The image mode for detecting hand landmarks on single image inputs.</description>
    ///     </item>
    ///     <item>
    ///       <description>The video mode for detecting hand landmarks on the decoded frames of a video.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The live stream mode or detecting hand landmarks on the live stream of input data, such as from camera.
    ///         In this mode, the <see cref="resultCallback" /> below must be specified to receive the detection results asynchronously.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    public Core.RunningMode runningMode { get; }
    /// <summary>
    ///   The maximum number of hands can be detected by the hand landmarker.
    /// </summary>
    public int numHands { get; }
    /// <summary>
    ///   The minimum confidence score for the hand detection to be considered successful.
    /// </summary>
    public float minHandDetectionConfidence { get; }
    /// <summary>
    ///   The minimum confidence score of hand presence score in the hand landmark detection.
    /// </summary>
    public float minHandPresenceConfidence { get; }
    /// <summary>
    ///   The minimum confidence score for the hand tracking to be considered successful.
    /// </summary>
    public float minTrackingConfidence { get; }
    /// <summary>
    ///   The user-defined result callback for processing live stream data.
    ///   The result callback should only be specified when the running mode is set to the live stream mode.
    /// </summary>
    public ResultCallback resultCallback { get; }

    public HandLandmarkerOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      int numHands = 1,
      float minHandDetectionConfidence = 0.5f,
      float minHandPresenceConfidence = 0.5f,
      float minTrackingConfidence = 0.5f,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.numHands = numHands;
      this.minHandDetectionConfidence = minHandDetectionConfidence;
      this.minHandPresenceConfidence = minHandPresenceConfidence;
      this.minTrackingConfidence = minTrackingConfidence;
      this.resultCallback = resultCallback;
    }

    internal Proto.HandLandmarkerGraphOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.IMAGE;

      return new Proto.HandLandmarkerGraphOptions
      {
        BaseOptions = baseOptionsProto,
        HandDetectorGraphOptions = new HandDetector.Proto.HandDetectorGraphOptions
        {
          NumHands = numHands,
          MinDetectionConfidence = minHandDetectionConfidence,
        },
        HandLandmarksDetectorGraphOptions = new Proto.HandLandmarksDetectorGraphOptions
        {
          MinDetectionConfidence = minHandPresenceConfidence,
        },
        MinTrackingConfidence = minTrackingConfidence,
      };
    }

    CalculatorOptions Tasks.Core.ITaskOptions.ToCalculatorOptions()
    {
      var options = new CalculatorOptions();
      options.SetExtension(Proto.HandLandmarkerGraphOptions.Extensions.Ext, ToProto());
      return options;
    }
  }
}
