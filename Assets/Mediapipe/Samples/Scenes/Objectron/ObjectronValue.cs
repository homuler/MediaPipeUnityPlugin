// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Unity.Objectron
{
  public class ObjectronValue
  {
    public readonly FrameAnnotation liftedObjects;
    public readonly List<NormalizedRect> multiBoxRects;
    public readonly List<NormalizedLandmarkList> multiBoxLandmarks;

    public ObjectronValue(FrameAnnotation liftedObjects, List<NormalizedRect> multiBoxRects, List<NormalizedLandmarkList> multiBoxLandmarks)
    {
      this.liftedObjects = liftedObjects;
      this.multiBoxRects = multiBoxRects;
      this.multiBoxLandmarks = multiBoxLandmarks;
    }
  }
}
