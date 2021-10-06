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
