// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Holistic
{
  public class HolisticTrackingSolution : ImageSourceSolution<HolisticTrackingGraph>
  {
    [SerializeField] private RectTransform _worldAnnotationArea;
    [SerializeField] private DetectionAnnotationController _poseDetectionAnnotationController;
    [SerializeField] private HolisticLandmarkListAnnotationController _holisticAnnotationController;
    [SerializeField] private PoseWorldLandmarkListAnnotationController _poseWorldLandmarksAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _poseRoiAnnotationController;

    public HolisticTrackingGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public bool smoothLandmarks
    {
      get => graphRunner.smoothLandmarks;
      set => graphRunner.smoothLandmarks = value;
    }

    public bool refineFaceLandmarks
    {
      get => graphRunner.refineFaceLandmarks;
      set => graphRunner.refineFaceLandmarks = value;
    }

    protected override void SetupScreen(ImageSource imageSource)
    {
      base.SetupScreen(imageSource);
      _worldAnnotationArea.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();
    }

    protected override void OnStartRun()
    {
      graphRunner.OnPoseDetectionOutput += OnPoseDetectionOutput;
      graphRunner.OnFaceLandmarksOutput += OnFaceLandmarksOutput;
      graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
      graphRunner.OnLeftHandLandmarksOutput += OnLeftHandLandmarksOutput;
      graphRunner.OnRightHandLandmarksOutput += OnRightHandLandmarksOutput;
      graphRunner.OnPoseWorldLandmarksOutput += OnPoseWorldLandmarksOutput;
      graphRunner.OnPoseRoiOutput += OnPoseRoiOutput;

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_poseDetectionAnnotationController, imageSource);
      SetupAnnotationController(_holisticAnnotationController, imageSource);
      SetupAnnotationController(_poseWorldLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_poseRoiAnnotationController, imageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      Detection poseDetection = null;
      NormalizedLandmarkList faceLandmarks = null;
      NormalizedLandmarkList poseLandmarks = null;
      NormalizedLandmarkList leftHandLandmarks = null;
      NormalizedLandmarkList rightHandLandmarks = null;
      LandmarkList poseWorldLandmarks = null;
      NormalizedRect poseRoi = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out poseDetection, out poseLandmarks, out faceLandmarks, out leftHandLandmarks, out rightHandLandmarks, out poseWorldLandmarks, out poseRoi, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out poseDetection, out poseLandmarks, out faceLandmarks, out leftHandLandmarks, out rightHandLandmarks, out poseWorldLandmarks, out poseRoi, false));
      }

      _poseDetectionAnnotationController.DrawNow(poseDetection);
      _holisticAnnotationController.DrawNow(faceLandmarks, poseLandmarks, leftHandLandmarks, rightHandLandmarks);
      _poseWorldLandmarksAnnotationController.DrawNow(poseWorldLandmarks);
      _poseRoiAnnotationController.DrawNow(poseRoi);
    }

    private void OnPoseDetectionOutput(object stream, OutputEventArgs<Detection> eventArgs)
    {
      _poseDetectionAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnFaceLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
    {
      _holisticAnnotationController.DrawFaceLandmarkListLater(eventArgs.value);
    }

    private void OnPoseLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
    {
      _holisticAnnotationController.DrawPoseLandmarkListLater(eventArgs.value);
    }

    private void OnLeftHandLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
    {
      _holisticAnnotationController.DrawLeftHandLandmarkListLater(eventArgs.value);
    }

    private void OnRightHandLandmarksOutput(object stream, OutputEventArgs<NormalizedLandmarkList> eventArgs)
    {
      _holisticAnnotationController.DrawRightHandLandmarkListLater(eventArgs.value);
    }

    private void OnPoseWorldLandmarksOutput(object stream, OutputEventArgs<LandmarkList> eventArgs)
    {
      _poseWorldLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnPoseRoiOutput(object stream, OutputEventArgs<NormalizedRect> eventArgs)
    {
      _poseRoiAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
