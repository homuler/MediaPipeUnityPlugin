// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.FaceDetection
{
  public class FaceDetectionSolution : ImageSourceSolution<FaceDetectionGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;

    public FaceDetectionGraph.ModelType modelType
    {
      get => graphRunner.modelType;
      set => graphRunner.modelType = value;
    }

    protected override void OnStartRun()
    {
      graphRunner.OnFaceDetectionsOutput.AddListener(_faceDetectionsAnnotationController.DrawLater);
      SetupAnnotationController(_faceDetectionsAnnotationController, ImageSourceProvider.ImageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out var _, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out var _, false));
      }
    }
  }
}
