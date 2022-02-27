// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.BoxTracking
{
  public class BoxTrackingSolution : ImageSourceSolution<BoxTrackingGraph>
  {
    [SerializeField] private DetectionListAnnotationController _trackedDetectionsAnnotationController;

    protected override void OnStartRun()
    {
      graphRunner.OnTrackedDetectionsOutput.AddListener(_trackedDetectionsAnnotationController.DrawLater);
      SetupAnnotationController(_trackedDetectionsAnnotationController, ImageSourceProvider.ImageSource);
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
