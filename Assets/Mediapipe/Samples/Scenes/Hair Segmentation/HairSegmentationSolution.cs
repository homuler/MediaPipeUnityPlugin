// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.HairSegmentation
{
  public class HairSegmentationSolution : ImageSourceSolution<HairSegmentationGraph>
  {
    [SerializeField] private MaskAnnotationController _hairMaskAnnotationController;

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnHairMaskOutput += OnHairMaskOutput;
      }

      SetupAnnotationController(_hairMaskAnnotationController, ImageSourceProvider.ImageSource);
      _hairMaskAnnotationController.InitScreen(512, 512);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      ImageFrame hairMask = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out hairMask, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out hairMask, false));
      }

      _hairMaskAnnotationController.DrawNow(hairMask);
    }

    private void OnHairMaskOutput(object stream, OutputEventArgs<ImageFrame> eventArgs)
    {
      _hairMaskAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
