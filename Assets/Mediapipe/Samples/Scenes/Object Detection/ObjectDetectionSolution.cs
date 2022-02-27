// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.ObjectDetection
{
  public class ObjectDetectionSolution : ImageSourceSolution<ObjectDetectionGraph>
  {
    [SerializeField] private DetectionListAnnotationController _outputDetectionsAnnotationController;

    protected override void OnStartRun()
    {
      graphRunner.OnOutputDetectionsOutput.AddListener(_outputDetectionsAnnotationController.DrawLater);
      SetupAnnotationController(_outputDetectionsAnnotationController, ImageSourceProvider.ImageSource);
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
