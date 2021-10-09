// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Unity
{
  public class NormalizedRectListAnnotationController : AnnotationController<RectangleListAnnotation>
  {
    private IList<NormalizedRect> _currentTarget;

    public void DrawNow(IList<NormalizedRect> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(IList<NormalizedRect> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;
      annotation.Draw(_currentTarget);
    }
  }
}
