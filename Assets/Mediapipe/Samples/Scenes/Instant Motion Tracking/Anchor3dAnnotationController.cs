using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class Anchor3dAnnotationController : AnnotationController<Anchor3dAnnotation>
  {
    [SerializeField] Camera mainCamera;
    [SerializeField] float defaultDepth = 100.0f;
    [SerializeField] bool visualizeZ = true;

    List<Anchor3d> currentTarget;
    Gyroscope gyroscope;
    Quaternion defaultRotation = Quaternion.identity;
    Vector3 cameraPosition;

    protected override void Start()
    {
      base.Start();

      cameraPosition = 10 * (transform.worldToLocalMatrix * mainCamera.transform.position);
      if (SystemInfo.supportsGyroscope)
      {
        Input.gyro.enabled = true;
        gyroscope = Input.gyro;
      }
    }

    public void ResetAnchor()
    {
      if (gyroscope != null)
      {
        // Assume Landscape Left mode
        // TODO: consider screen's rotation
        var screenRotation = Quaternion.Euler(90, 0, -90);
        var currentRotation = GyroToUnity(gyroscope.attitude);
        var defaultYAngle = Quaternion.Inverse(screenRotation * currentRotation).eulerAngles.y;
        defaultRotation = Quaternion.Euler(90, defaultYAngle, -90);
      }
    }

    public void DrawNow(List<Anchor3d> target)
    {
      currentTarget = target;
      SyncNow();
    }

    public void DrawLater(List<Anchor3d> target)
    {
      UpdateCurrentTarget(target, ref currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;

      var currentRotation = gyroscope == null ? Quaternion.identity : GyroToUnity(gyroscope.attitude);
      var anchor3d = currentTarget == null || currentTarget.Count < 1 ? null : (Anchor3d?)currentTarget[0]; // at most one anchor
      annotation.Draw(anchor3d, Quaternion.Inverse(defaultRotation * currentRotation), cameraPosition, defaultDepth, visualizeZ);
    }

    void ApplyTranslateZ(float translateZ)
    {
      if (visualizeZ)
      {
        annotation.transform.localPosition = new Vector3(0, 0, translateZ);
      }
      else
      {
        annotation.transform.localPosition = Vector3.zero;
      }
    }

    static Quaternion GyroToUnity(Quaternion q)
    {
      return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
  }
}
