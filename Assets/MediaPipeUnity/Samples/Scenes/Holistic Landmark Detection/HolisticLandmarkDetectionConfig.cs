// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Tasks.Vision.HolisticLandmarker;
using UnityEngine;

namespace Mediapipe.Unity.Sample.HolisticLandmarkDetection
{
  public class HolisticLandmarkDetectionConfig
  {
    public Tasks.Core.BaseOptions.Delegate Delegate { get; set; } =
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
      Tasks.Core.BaseOptions.Delegate.CPU;
#else
    Tasks.Core.BaseOptions.Delegate.GPU;
#endif

    public ImageReadMode ImageReadMode { get; set; } = ImageReadMode.CPUAsync;

    public Tasks.Vision.Core.RunningMode RunningMode { get; set; } = Tasks.Vision.Core.RunningMode.LIVE_STREAM;

    public float MinFaceDetectionConfidence { get; set; } = 0.5f;
    public float MinFaceSuppressionThreshold { get; set; } = 0.5f;
    public float MinFaceLandmarksConfidence { get; set; } = 0.5f;
    public float MinPoseDetectionConfidence { get; set; } = 0.5f;
    public float MinPoseSuppressionThreshold { get; set; } = 0.5f;
    public float MinPoseLandmarksConfidence { get; set; } = 0.5f;
    public float MinHandLandmarksConfidence { get; set; } = 0.5f;
    public bool OutputFaceBlendshapes { get; set; } = false;
    public bool OutputSegmentationMask { get; set; } = false;
    public string ModelPath => "holistic_landmarker.bytes";

    public HolisticLandmarkerOptions GetHolisticLandmarkerOptions(TextAsset modelAsset, HolisticLandmarkerOptions.ResultCallback resultCallback = null)
    {
      return new HolisticLandmarkerOptions(
        new Tasks.Core.BaseOptions(Delegate, modelAssetPath: ModelPath),
        runningMode: RunningMode,
        minFaceDetectionConfidence: MinFaceDetectionConfidence,
        minFaceSuppressionThreshold: MinFaceSuppressionThreshold,
        minFaceLandmarksConfidence: MinFaceLandmarksConfidence,
        minPoseDetectionConfidence: MinPoseDetectionConfidence,
        minPoseSuppressionThreshold: MinPoseSuppressionThreshold,
        minPoseLandmarksConfidence: MinPoseLandmarksConfidence,
        minHandLandmarksConfidence: MinHandLandmarksConfidence,
        outputFaceBlendshapes: OutputFaceBlendshapes,
        outputSegmentationMask: OutputSegmentationMask,
        resultCallback: resultCallback
      );
    }
  }
}
