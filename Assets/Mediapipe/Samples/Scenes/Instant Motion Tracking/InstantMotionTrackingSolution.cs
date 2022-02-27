// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity.CoordinateSystem;
using System.Collections;
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
            var normalizedPoint = rectTransform.GetNormalizedPosition(localPoint, graphRunner.rotation, ImageSourceProvider.ImageSource.isHorizontallyFlipped);
            graphRunner.ResetAnchor(normalizedPoint.x, normalizedPoint.y);
            _trackedAnchorDataAnnotationController.ResetAnchor();
          }
        }
      }
    }

    protected override void OnStartRun()
    {
      graphRunner.ResetAnchor();
      graphRunner.OnTrackedAnchorDataOutput.AddListener(_trackedAnchorDataAnnotationController.DrawLater);

      SetupAnnotationController(_trackedAnchorDataAnnotationController, ImageSourceProvider.ImageSource);
      _trackedAnchorDataAnnotationController.ResetAnchor();
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
