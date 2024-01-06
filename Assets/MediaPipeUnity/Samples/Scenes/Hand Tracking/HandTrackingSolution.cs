// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Sample.HandTracking
{
  public class HandTrackingSolution : ImageSourceSolution<HandTrackingGraph>
  {
    [SerializeField] private DetectionListAnnotationController _palmDetectionsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromPalmDetectionsAnnotationController;
    [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromLandmarksAnnotationController;

    public HandTrackingGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
      get => graphRunner.maxNumHands;
      set => graphRunner.maxNumHands = value;
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

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnPalmDetectectionsOutput += OnPalmDetectionsOutput;
        graphRunner.OnHandRectsFromPalmDetectionsOutput += OnHandRectsFromPalmDetectionsOutput;
        graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
        // TODO: render HandWorldLandmarks annotations
        graphRunner.OnHandRectsFromLandmarksOutput += OnHandRectsFromLandmarksOutput;
        graphRunner.OnHandednessOutput += OnHandednessOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_palmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromPalmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromLandmarksAnnotationController, imageSource, true);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      var task = graphRunner.WaitNext();
      yield return new WaitUntil(() => task.IsCompleted);

      var result = task.Result;
      _palmDetectionsAnnotationController.DrawNow(result.palmDetections);
      _handRectsFromPalmDetectionsAnnotationController.DrawNow(result.handRectsFromPalmDetections);
      _handLandmarksAnnotationController.DrawNow(result.handLandmarks, result.handedness);
      // TODO: render HandWorldLandmarks annotations
      _handRectsFromLandmarksAnnotationController.DrawNow(result.handRectsFromLandmarks);
    }

    private void OnPalmDetectionsOutput(object stream, OutputStream.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet.IsEmpty() ? default : packet.GetProtoList(Detection.Parser);
      _palmDetectionsAnnotationController.DrawLater(value);
    }

    private void OnHandRectsFromPalmDetectionsOutput(object stream, OutputStream.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet.IsEmpty() ? default : packet.GetProtoList(NormalizedRect.Parser);
      _handRectsFromPalmDetectionsAnnotationController.DrawLater(value);
    }

    private void OnHandLandmarksOutput(object stream, OutputStream.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet.IsEmpty() ? default : packet.GetProtoList(NormalizedLandmarkList.Parser);
      _handLandmarksAnnotationController.DrawLater(value);
    }

    private void OnHandRectsFromLandmarksOutput(object stream, OutputStream.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet.IsEmpty() ? default : packet.GetProtoList(NormalizedRect.Parser);
      _handRectsFromLandmarksAnnotationController.DrawLater(value);
    }

    private void OnHandednessOutput(object stream, OutputStream.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet.IsEmpty() ? default : packet.GetProtoList(ClassificationList.Parser);
      _handLandmarksAnnotationController.DrawLater(value);
    }
  }
}
