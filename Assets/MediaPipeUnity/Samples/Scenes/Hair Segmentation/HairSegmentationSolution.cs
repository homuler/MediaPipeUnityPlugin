// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Sample.HairSegmentation
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
      var task = graphRunner.WaitNext();
      yield return new WaitUntil(() => task.IsCompleted);

      _hairMaskAnnotationController.DrawNow(task.Result);
      task.Result?.Dispose();
    }

    private void OnHairMaskOutput(object stream, OutputStream<ImageFrame>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get();
      _hairMaskAnnotationController.DrawLater(value);
      value?.Dispose();
    }
  }
}
