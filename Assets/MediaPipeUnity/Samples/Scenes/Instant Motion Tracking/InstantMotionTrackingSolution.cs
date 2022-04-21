// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.InstantMotionTracking
{
  public class InstantMotionTrackingSolution : ImageSourceSolution<RegionTrackingGraph>
  {
    [SerializeField] private Anchor3dAnnotationController _trackedAnchorDataAnnotationController;

    private void Update()
    {
      if (Input.GetMouseButtonDown(0))
      {
        var rectTransform = screen.GetComponent<RectTransform>();

        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, Camera.main))
        {
          if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out var localPoint))
          {
            var isMirrored = ImageSourceProvider.ImageSource.isFrontFacing ^ ImageSourceProvider.ImageSource.isHorizontallyFlipped;
            var normalizedPoint = rectTransform.rect.PointToImageNormalized(localPoint, graphRunner.rotation, isMirrored);
            graphRunner.ResetAnchor(normalizedPoint.x, normalizedPoint.y);
            _trackedAnchorDataAnnotationController.ResetAnchor();
          }
        }
      }
    }

    protected override void OnStartRun()
    {
      graphRunner.ResetAnchor();

      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnTrackedAnchorDataOutput += OnTrackedAnchorDataOutput;
      }

      SetupAnnotationController(_trackedAnchorDataAnnotationController, ImageSourceProvider.ImageSource);
      _trackedAnchorDataAnnotationController.ResetAnchor();
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      List<Anchor3d> trackedAnchorData = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out trackedAnchorData, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out trackedAnchorData, false));
      }

      _trackedAnchorDataAnnotationController.DrawNow(trackedAnchorData);
    }

    private void OnTrackedAnchorDataOutput(object stream, OutputEventArgs<List<Anchor3d>> eventArgs)
    {
      _trackedAnchorDataAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
