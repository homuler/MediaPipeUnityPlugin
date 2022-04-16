// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.BoxTracking
{
  public class BoxTrackingSolution : ImageSourceSolution<BoxTrackingGraph>
  {
    [SerializeField] private DetectionListAnnotationController _trackedDetectionsAnnotationController;

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnTrackedDetectionsOutput += OnTrackedDetectionsOutput;
      }

      SetupAnnotationController(_trackedDetectionsAnnotationController, ImageSourceProvider.ImageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      List<Detection> trackedDetections = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out trackedDetections, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out trackedDetections, false));
      }

      _trackedDetectionsAnnotationController.DrawNow(trackedDetections);
    }

    private void OnTrackedDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
    {
      _trackedDetectionsAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
