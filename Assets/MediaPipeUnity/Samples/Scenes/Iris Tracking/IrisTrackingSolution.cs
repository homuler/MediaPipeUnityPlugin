// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Sample.IrisTracking
{
  public class IrisTrackingSolution : ImageSourceSolution<IrisTrackingGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _faceRectAnnotationController;
    [SerializeField] private FaceLandmarkListAnnotationController _faceLandmarksWithIrisAnnotationController;

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
        graphRunner.OnFaceRectOutput += OnFaceRectOutput;
        graphRunner.OnFaceLandmarksWithIrisOutput += OnFaceLandmarksWithIrisOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_faceDetectionsAnnotationController, imageSource);
      SetupAnnotationController(_faceRectAnnotationController, imageSource);
      SetupAnnotationController(_faceLandmarksWithIrisAnnotationController, imageSource);
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
      _faceDetectionsAnnotationController.DrawNow(result.faceDetections);
      _faceRectAnnotationController.DrawNow(result.faceRect);
      _faceLandmarksWithIrisAnnotationController.DrawNow(result.faceLandmarksWithIris);
    }

    private void OnFaceDetectionsOutput(object stream, OutputStream<List<Detection>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(Detection.Parser);
      _faceDetectionsAnnotationController.DrawLater(value);
    }

    private void OnFaceRectOutput(object stream, OutputStream<NormalizedRect>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedRect.Parser);
      _faceRectAnnotationController.DrawLater(value);
    }

    private void OnFaceLandmarksWithIrisOutput(object stream, OutputStream<NormalizedLandmarkList>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(NormalizedLandmarkList.Parser);
      _faceLandmarksWithIrisAnnotationController.DrawLater(value);
    }
  }
}
