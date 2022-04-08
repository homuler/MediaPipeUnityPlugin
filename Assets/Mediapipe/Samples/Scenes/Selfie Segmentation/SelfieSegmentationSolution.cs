// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.SelfieSegmentation
{
  public class SelfieSegmentationSolution : ImageSourceSolution<SelfieSegmentationGraph>
  {
    [SerializeField] private MaskAnnotationController _segmentationMaskAnnotationController;

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnSegmentationMaskOutput += OnSegmentationMaskOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_segmentationMaskAnnotationController, imageSource);
      _segmentationMaskAnnotationController.InitScreen(imageSource.textureWidth, imageSource.textureHeight);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      ImageFrame segmentationMask = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out segmentationMask, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out segmentationMask, false));
      }

      _segmentationMaskAnnotationController.DrawNow(segmentationMask);
    }

    private void OnSegmentationMaskOutput(object stream, OutputEventArgs<ImageFrame> eventArgs)
    {
      _segmentationMaskAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
