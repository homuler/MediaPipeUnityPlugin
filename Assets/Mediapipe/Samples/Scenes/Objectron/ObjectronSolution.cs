// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.Objectron
{
  public class ObjectronSolution : ImageSourceSolution<ObjectronGraph>
  {
    [SerializeField] private FrameAnnotationController _liftedObjectsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _multiBoxRectsAnnotationController;
    [SerializeField] private NormalizedLandmarkListAnnotationController _multiBoxLandmarksAnnotationController;

    public ObjectronGraph.Category category
    {
      get => graphRunner.category;
      set => graphRunner.category = value;
    }

    public int maxNumObjects
    {
      get => graphRunner.maxNumObjects;
      set => graphRunner.maxNumObjects = value;
    }

    protected override void OnStartRun()
    {
      graphRunner.OnLiftedObjectsOutput.AddListener(_liftedObjectsAnnotationController.DrawLater);
      graphRunner.OnMultiBoxRectsOutput.AddListener(_multiBoxRectsAnnotationController.DrawLater);
      graphRunner.OnMultiBoxLandmarksOutput.AddListener(_multiBoxLandmarksAnnotationController.DrawLater);

      var imageSource = ImageSourceProvider.ImageSource;
      SetupAnnotationController(_liftedObjectsAnnotationController, imageSource);
      _liftedObjectsAnnotationController.focalLength = graphRunner.focalLength;
      _liftedObjectsAnnotationController.principalPoint = graphRunner.principalPoint;

      SetupAnnotationController(_multiBoxRectsAnnotationController, imageSource);
      SetupAnnotationController(_multiBoxLandmarksAnnotationController, imageSource);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out var _, out var _, out var _, true);
        yield return new WaitForEndOfFrame();
      }
      else if (runningMode == RunningMode.SyncNonBlock)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out var _, out var _, out var _, false));
      }
    }
  }
}
