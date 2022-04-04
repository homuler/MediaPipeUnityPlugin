// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.PoseTracking
{
  public class PoseTrackingSolution : ImageSourceSolution<PoseTrackingGraph>
  {
    [SerializeField] private RectTransform _worldAnnotationArea;
    [SerializeField] private DetectionAnnotationController _poseDetectionAnnotationController;
    [SerializeField] private PoseLandmarkListAnnotationController _poseLandmarksAnnotationController;
    [SerializeField] private PoseWorldLandmarkListAnnotationController _poseWorldLandmarksAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _roiFromLandmarksAnnotationController;


    public PoseTrackingGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public bool smoothLandmarks
    {
      get => graphRunner.smoothLandmarks;
      set => graphRunner.smoothLandmarks = value;
    }

    protected override void SetupScreen(ImageSource imageSource)
    {
      base.SetupScreen(imageSource);
      _worldAnnotationArea.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();
    }

    protected override void OnStartRun()
    {
      graphRunner.OnPoseDetectionOutput += OnPoseDetectionOutput;
      graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
      graphRunner.OnPoseWorldLandmarksOutput += OnPoseWorldLandmarksOutput;
      graphRunner.OnRoiFromLandmarksOutput += OnRoiFromLandmarksOutput;

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_poseDetectionAnnotationController, imageSource);
      SetupAnnotationController(_poseLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_poseWorldLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_roiFromLandmarksAnnotationController, imageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      Detection poseDetection = null;
      NormalizedLandmarkList poseLandmarks = null;
      LandmarkList poseWorldLandmarks = null;
      NormalizedRect roiFromLandmarks = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out poseDetection, out poseLandmarks, out poseWorldLandmarks, out roiFromLandmarks, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out poseDetection, out poseLandmarks, out poseWorldLandmarks, out roiFromLandmarks, false));
      }

      _poseDetectionAnnotationController.DrawNow(poseDetection);
      _poseLandmarksAnnotationController.DrawNow(poseLandmarks);
      _poseWorldLandmarksAnnotationController.DrawNow(poseWorldLandmarks);
      _roiFromLandmarksAnnotationController.DrawNow(roiFromLandmarks);
    }

    private void OnPoseDetectionOutput(object stream, OutputEventArgs<Detection> eventArgs)
    {
      _poseDetectionAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnPoseLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
    {
      _poseLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnPoseWorldLandmarksOutput(object stream, OutputEventArgs<LandmarkList> eventArgs)
    {
      _poseWorldLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnRoiFromLandmarksOutput(object stream, OutputEventArgs<NormalizedRect> eventArgs)
    {
      _roiFromLandmarksAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
