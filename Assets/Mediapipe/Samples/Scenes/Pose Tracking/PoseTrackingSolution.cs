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
      graphRunner.OnPoseDetectionOutput.AddListener(_poseDetectionAnnotationController.DrawLater);
      graphRunner.OnPoseLandmarksOutput.AddListener(_poseLandmarksAnnotationController.DrawLater);
      graphRunner.OnPoseWorldLandmarksOutput.AddListener(_poseWorldLandmarksAnnotationController.DrawLater);
      graphRunner.OnRoiFromLandmarksOutput.AddListener(_roiFromLandmarksAnnotationController.DrawLater);

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
      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out var _, out var _, out var _, out var _, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out var _, out var _, out var _, out var _, false));
      }
    }
  }
}
