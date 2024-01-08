// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Sample.SelfieSegmentation
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
      var task = graphRunner.WaitNextAsync();
      yield return new WaitUntil(() => task.IsCompleted);

      _segmentationMaskAnnotationController.DrawNow(task.Result);
      task.Result?.Dispose();
    }

    private void OnSegmentationMaskOutput(object stream, OutputStream<ImageFrame>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get();
      _segmentationMaskAnnotationController.DrawLater(value);
      value?.Dispose();
    }
  }
}
