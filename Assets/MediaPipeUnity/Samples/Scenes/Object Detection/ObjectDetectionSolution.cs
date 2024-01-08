// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Sample.ObjectDetection
{
  public class ObjectDetectionSolution : ImageSourceSolution<ObjectDetectionGraph>
  {
    [SerializeField] private DetectionListAnnotationController _outputDetectionsAnnotationController;

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnOutputDetectionsOutput += OnOutputDetectionsOutput;
      }

      SetupAnnotationController(_outputDetectionsAnnotationController, ImageSourceProvider.ImageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      var task = graphRunner.WaitNextAsync();
      yield return new WaitUntil(() => task.IsCompleted);

      _outputDetectionsAnnotationController.DrawNow(task.Result);
    }

    private void OnOutputDetectionsOutput(object stream, OutputStream<List<Detection>>.OutputEventArgs eventArgs)
    {
      var packet = eventArgs.packet;
      var value = packet == null ? default : packet.Get(Detection.Parser);
      _outputDetectionsAnnotationController.DrawLater(value);
    }
  }
}
