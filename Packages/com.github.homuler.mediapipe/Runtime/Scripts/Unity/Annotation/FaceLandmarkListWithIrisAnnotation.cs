using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class FaceLandmarkListWithIrisAnnotation : HierarchicalAnnotation
  {
    [SerializeField] FaceLandmarkListAnnotation faceLandmarkList;
    [SerializeField] IrisLandmarkListAnnotation leftIrisLandmarkList;
    [SerializeField] IrisLandmarkListAnnotation rightIrisLandmarkList;

    const int faceLandmarkCount = 468;
    const int irisLandmarkCount = 5;

    public override bool isMirrored
    {
      set
      {
        faceLandmarkList.isMirrored = value;
        leftIrisLandmarkList.isMirrored = value;
        rightIrisLandmarkList.isMirrored = value;
        base.isMirrored = value;
      }
    }

    public override RotationAngle rotationAngle
    {
      set
      {
        faceLandmarkList.rotationAngle = value;
        leftIrisLandmarkList.rotationAngle = value;
        rightIrisLandmarkList.rotationAngle = value;
        base.rotationAngle = value;
      }
    }

    public void DrawFaceLandmarkList(IList<NormalizedLandmark> target, bool visualizeZ = false)
    {
      if (ActivateFor(target))
      {
        faceLandmarkList.Draw(target, visualizeZ);
      }
    }

    public void DrawLeftIrisLandmarkList(IList<NormalizedLandmark> target, bool visualizeZ = false, int circleVertices = 128)
    {
      // does not deactivate if the target is null as long as face landmarks are present.
      leftIrisLandmarkList.Draw(target, visualizeZ, circleVertices);
    }

    public void DrawRightIrisLandmarkList(IList<NormalizedLandmark> target, bool visualizeZ = false, int circleVertices = 128)
    {
      // does not deactivate if the target is null as long as face landmarks are present.
      rightIrisLandmarkList.Draw(target, visualizeZ, circleVertices);
    }

    public static (IList<NormalizedLandmark>, IList<NormalizedLandmark>, IList<NormalizedLandmark>) PartitionLandmarkList(IList<NormalizedLandmark> landmarks)
    {
      if (landmarks == null)
      {
        return (null, null, null);
      }

      var enumerator = landmarks.GetEnumerator();
      var faceLandmarks = new List<NormalizedLandmark>(faceLandmarkCount);
      for (var i = 0; i < faceLandmarkCount; i++)
      {
        if (enumerator.MoveNext())
        {
          faceLandmarks.Add(enumerator.Current);
        }
      }
      if (faceLandmarks.Count < faceLandmarkCount)
      {
        return (null, null, null);
      }

      var leftIrisLandmarks = new List<NormalizedLandmark>(irisLandmarkCount);
      for (var i = 0; i < irisLandmarkCount; i++)
      {
        if (enumerator.MoveNext())
        {
          leftIrisLandmarks.Add(enumerator.Current);
        }
      }
      if (leftIrisLandmarks.Count < irisLandmarkCount)
      {
        return (faceLandmarks, null, null);
      }

      var rightIrisLandmarks = new List<NormalizedLandmark>(irisLandmarkCount);
      for (var i = 0; i < irisLandmarkCount; i++)
      {
        if (enumerator.MoveNext())
        {
          rightIrisLandmarks.Add(enumerator.Current);
        }
      }
      if (rightIrisLandmarks.Count < irisLandmarkCount)
      {
        return (faceLandmarks, leftIrisLandmarks, null);
      }

      return (faceLandmarks, leftIrisLandmarks, rightIrisLandmarks);
    }
  }
}
