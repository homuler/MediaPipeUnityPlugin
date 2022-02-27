// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.IrisTracking
{
  public class IrisTrackingSolution : ImageSourceSolution<IrisTrackingGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private NormalizedRectAnnotationController _faceRectAnnotationController;
    [SerializeField] private FaceLandmarkListAnnotationController _faceLandmarksWithIrisAnnotationController;

    protected override void OnStartRun()
    {
      graphRunner.OnFaceDetectionsOutput.AddListener(_faceDetectionsAnnotationController.DrawLater);
      graphRunner.OnFaceRectOutput.AddListener(_faceRectAnnotationController.DrawLater);
      graphRunner.OnFaceLandmarksWithIrisOutput.AddListener(_faceLandmarksWithIrisAnnotationController.DrawLater);

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
      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out var _, out var _, out var _, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out var _, out var _, out var _, false));
      }
    }
  }
}
