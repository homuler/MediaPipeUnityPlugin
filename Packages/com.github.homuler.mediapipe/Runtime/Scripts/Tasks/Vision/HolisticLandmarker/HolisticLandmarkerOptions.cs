// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Vision.HolisticLandmarker
{
  /// <summary>
  ///   Options for the holistic landmarker task.
  /// </summary>
  public sealed class HolisticLandmarkerOptions : Tasks.Core.ITaskOptions
  {
    /// <param name="holisticLandmarkerResult">
    ///   The holistic landmarker detection results.
    /// </param>
    /// <param name="image">
    ///   The input image that the holistic landmarker runs on.
    /// </param>
    /// <param name="timestampMillisec">
    ///   The input timestamp in milliseconds.
    /// </param>
    public delegate void ResultCallback(in HolisticLandmarkerResult holisticLandmarkerResult, Image image, long timestampMillisec);

    /// <summary>
    ///   Base options for the holistic landmarker task.
    /// </summary>
    public Tasks.Core.BaseOptions baseOptions { get; }
    /// <summary>
    ///   The running mode of the task. Default to the image mode.
    ///   PoseLandmarker has three running modes:
    ///   <list type="number">
    ///     <item>
    ///       <description>The image mode for detecting holistic landmarks on single image inputs.</description>
    ///     </item>
    ///     <item>
    ///       <description>The video mode for detecting holistic landmarks on the decoded frames of a video.</description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         The live stream mode or detecting holistic landmarks on the live stream of input data, such as from camera.
    ///         In this mode, the <see cref="resultCallback" /> below must be specified to receive the detection results asynchronously.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    public Core.RunningMode runningMode { get; }
    /// <summary>
    ///   The minimum confidence score for the face detection to be considered successful.
    /// </summary>
    public float minFaceDetectionConfidence { get; }
    /// <summary>
    ///   The minimum non-maximum-suppression threshold for face detection to be considered overlapped.
    /// </summary>
    public float minFaceSuppressionThreshold { get; }
    /// <summary>
    ///   The minimum confidence score for the face landmark detection to be considered successful.
    /// </summary>
    public float minFaceLandmarksConfidence { get; }
    /// <summary>
    ///   The minimum confidence score for the pose detection to be considered successful.
    /// </summary>
    public float minPoseDetectionConfidence { get; }
    /// <summary>
    ///   The minimum non-maximum-suppression threshold for pose detection to be considered overlapped.
    /// </summary>
    public float minPoseSuppressionThreshold { get; }
    /// <summary>
    ///   The minimum confidence score for the pose landmark detection to be considered successful.
    /// </summary>
    public float minPoseLandmarksConfidence { get; }
    /// <summary>
    ///   The minimum confidence score for the hand landmark detection to be considered successful.
    /// </summary>
    public float minHandLandmarksConfidence { get; }
    /// <summary>
    ///   Whether HolisticLandmarker outputs face blendshapes classification. Face blendshapes are used for rendering the 3D face model.
    /// </summary>
    public bool outputFaceBlendshapes { get; }
    /// <summary>
    ///   whether to output segmentation masks.
    /// </summary>
    public bool outputSegmentationMask { get; }
    /// <summary>
    ///   The user-defined result callback for processing live stream data.
    ///   The result callback should only be specified when the running mode is set to the live stream mode.
    /// </summary>
    public ResultCallback resultCallback { get; }

    public HolisticLandmarkerOptions(
      Tasks.Core.BaseOptions baseOptions,
      Core.RunningMode runningMode = Core.RunningMode.IMAGE,
      float minFaceDetectionConfidence = 0.5f,
      float minFaceSuppressionThreshold = 0.5f,
      float minFaceLandmarksConfidence = 0.5f,
      float minPoseDetectionConfidence = 0.5f,
      float minPoseSuppressionThreshold = 0.5f,
      float minPoseLandmarksConfidence = 0.5f,
      float minHandLandmarksConfidence = 0.5f,
      bool outputFaceBlendshapes = false,
      bool outputSegmentationMask = false,
      ResultCallback resultCallback = null)
    {
      this.baseOptions = baseOptions;
      this.runningMode = runningMode;
      this.minFaceDetectionConfidence = minFaceDetectionConfidence;
      this.minFaceSuppressionThreshold = minFaceSuppressionThreshold;
      this.minFaceLandmarksConfidence = minFaceLandmarksConfidence;
      this.minPoseDetectionConfidence = minPoseDetectionConfidence;
      this.minPoseSuppressionThreshold = minPoseSuppressionThreshold;
      this.minPoseLandmarksConfidence = minPoseLandmarksConfidence;
      this.minHandLandmarksConfidence = minHandLandmarksConfidence;
      this.outputFaceBlendshapes = outputFaceBlendshapes;
      this.outputSegmentationMask = outputSegmentationMask;
      this.resultCallback = resultCallback;
    }

    internal Proto.HolisticLandmarkerGraphOptions ToProto()
    {
      var baseOptionsProto = baseOptions.ToProto();
      baseOptionsProto.UseStreamMode = runningMode != Core.RunningMode.IMAGE;

      return new Proto.HolisticLandmarkerGraphOptions
      {
        BaseOptions = baseOptionsProto,
        FaceDetectorGraphOptions = new FaceDetector.Proto.FaceDetectorGraphOptions
        {
          MinDetectionConfidence = minFaceDetectionConfidence,
          MinSuppressionThreshold = minFaceSuppressionThreshold,
        },
        FaceLandmarksDetectorGraphOptions = new FaceLandmarker.Proto.FaceLandmarksDetectorGraphOptions
        {
          MinDetectionConfidence = minFaceLandmarksConfidence,
        },
        PoseDetectorGraphOptions = new PoseDetector.Proto.PoseDetectorGraphOptions
        {
          MinDetectionConfidence = minPoseDetectionConfidence,
          MinSuppressionThreshold = minPoseSuppressionThreshold,
        },
        PoseLandmarksDetectorGraphOptions = new PoseLandmarker.Proto.PoseLandmarksDetectorGraphOptions
        {
          MinDetectionConfidence = minPoseLandmarksConfidence,
        },
        HandLandmarksDetectorGraphOptions = new HandLandmarker.Proto.HandLandmarksDetectorGraphOptions
        {
          MinDetectionConfidence = minHandLandmarksConfidence,
        },
      };
    }

    Google.Protobuf.WellKnownTypes.Any Tasks.Core.ITaskOptions.ToAnyOptions() => Google.Protobuf.WellKnownTypes.Any.Pack(ToProto());
  }
}
