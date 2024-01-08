// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Sample.Holistic
{
  public class HolisticTrackingSolution : ImageSourceSolution<HolisticTrackingGraph>
  {
    [SerializeField] private RectTransform _worldAnnotationArea;
    [SerializeField] private DetectionAnnotationController _poseDetectionAnnotationController;
    [SerializeField] private HolisticLandmarkListAnnotationController _holisticAnnotationController;
    [SerializeField] private PoseWorldLandmarkListAnnotationController _poseWorldLandmarksAnnotationController;
    [SerializeField] private MaskAnnotationController _segmentationMaskAnnotationController;
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

    public bool enableSegmentation
    {
      get => graphRunner.enableSegmentation;
      set => graphRunner.enableSegmentation = value;
    }

    public bool smoothSegmentation
    {
      get => graphRunner.smoothSegmentation;
      set => graphRunner.smoothSegmentation = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    protected override void SetupScreen(ImageSource imageSource)
    {
      base.SetupScreen(imageSource);
      _worldAnnotationArea.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnPoseDetectionOutput += OnPoseDetectionOutput;
        graphRunner.OnFaceLandmarksOutput += OnFaceLandmarksOutput;
        graphRunner.OnPoseLandmarksOutput += OnPoseLandmarksOutput;
        graphRunner.OnLeftHandLandmarksOutput += OnLeftHandLandmarksOutput;
        graphRunner.OnRightHandLandmarksOutput += OnRightHandLandmarksOutput;
        graphRunner.OnPoseWorldLandmarksOutput += OnPoseWorldLandmarksOutput;
        graphRunner.OnSegmentationMaskOutput += OnSegmentationMaskOutput;
        graphRunner.OnPoseRoiOutput += OnPoseRoiOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_poseDetectionAnnotationController, imageSource);
      SetupAnnotationController(_holisticAnnotationController, imageSource);
      SetupAnnotationController(_poseWorldLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_segmentationMaskAnnotationController, imageSource);
      _segmentationMaskAnnotationController.InitScreen(imageSource.textureWidth, imageSource.textureHeight);
      SetupAnnotationController(_poseRoiAnnotationController, imageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      var task = graphRunner.WaitNextAsync();
      yield return new WaitUntil(() => task.IsCompleted);

      var result = task.Result;
      _poseDetectionAnnotationController.DrawNow(result.poseDetection);
      _holisticAnnotationController.DrawNow(result.faceLandmarks, result.poseLandmarks, result.leftHandLandmarks, result.rightHandLandmarks);
      _poseWorldLandmarksAnnotationController.DrawNow(result.poseWorldLandmarks);
      _segmentationMaskAnnotationController.DrawNow(result.segmentationMask);
      _poseRoiAnnotationController.DrawNow(result.poseRoi);

      result.segmentationMask?.Dispose();
    }

    private void OnPoseDetectionOutput(object stream, OutputStream<Detection>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(Detection.Parser);
      _poseDetectionAnnotationController.DrawLater(value);
    }

    private void OnFaceLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
      _holisticAnnotationController.DrawFaceLandmarkListLater(value);
    }

    private void OnPoseLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
      _holisticAnnotationController.DrawPoseLandmarkListLater(value);
    }

    private void OnLeftHandLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
      _holisticAnnotationController.DrawLeftHandLandmarkListLater(value);
    }

    private void OnRightHandLandmarksOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
      _holisticAnnotationController.DrawRightHandLandmarkListLater(value);
    }

    private void OnPoseWorldLandmarksOutput(object stream, OutputStream<LandmarkList>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(LandmarkList.Parser);
      _poseWorldLandmarksAnnotationController.DrawLater(value);
    }

    private void OnSegmentationMaskOutput(object stream, OutputStream<ImageFrame>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get();
      _segmentationMaskAnnotationController.DrawLater(value);
      value?.Dispose();
    }

    private void OnPoseRoiOutput(object stream, OutputStream<NormalizedRect>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedRect.Parser);
      _poseRoiAnnotationController.DrawLater(value);
    }
  }
}
