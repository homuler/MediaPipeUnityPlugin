// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class Anchor3dAnnotationController : AnnotationController<Anchor3dAnnotation>
  {
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _defaultDepth = 100.0f;
    [SerializeField] private bool _visualizeZ = true;

    private List<Anchor3d> _currentTarget;
    private Gyroscope _gyroscope;
    private Quaternion _defaultRotation = Quaternion.identity;
    private Vector3 _cameraPosition;

    protected override void Start()
    {
      base.Start();

      _cameraPosition = 10 * (transform.worldToLocalMatrix * _mainCamera.transform.position);
      if (SystemInfo.supportsGyroscope)
      {
        Input.gyro.enabled = true;
        _gyroscope = Input.gyro;
      }
    }

    public void ResetAnchor()
    {
      if (_gyroscope != null)
      {
        // Assume Landscape Left mode
        // TODO: consider screen's rotation
        var screenRotation = Quaternion.Euler(90, 0, -90);
        var currentRotation = GyroToUnity(_gyroscope.attitude);
        var defaultYAngle = Quaternion.Inverse(screenRotation * currentRotation).eulerAngles.y;
        _defaultRotation = Quaternion.Euler(90, defaultYAngle, -90);
      }
    }

    public void DrawNow(List<Anchor3d> target)
    {
      _currentTarget = target;
      SyncNow();
    }

    public void DrawLater(List<Anchor3d> target)
    {
      UpdateCurrentTarget(target, ref _currentTarget);
    }

    protected override void SyncNow()
    {
      isStale = false;

      var currentRotation = _gyroscope == null ? Quaternion.identity : GyroToUnity(_gyroscope.attitude);
      var anchor3d = _currentTarget == null || _currentTarget.Count < 1 ? null : (Anchor3d?)_currentTarget[0]; // at most one anchor
      annotation.Draw(anchor3d, Quaternion.Inverse(_defaultRotation * currentRotation), _cameraPosition, _defaultDepth, _visualizeZ);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
      return new Quaternion(q.x, q.y, -q.z, -q.w);
    }
  }
}
