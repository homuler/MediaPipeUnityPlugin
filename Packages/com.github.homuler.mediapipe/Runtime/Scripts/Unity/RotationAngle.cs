// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity
{
  public enum RotationAngle
  {
    Rotation0 = 0,
    Rotation90 = 90,
    Rotation180 = 180,
    Rotation270 = 270,
  }

  public static class RotationAngleExtension
  {
    public static RotationAngle Add(this RotationAngle rotationAngle, RotationAngle angle)
    {
      return (RotationAngle)(((int)rotationAngle + (int)angle) % 360);
    }

    public static RotationAngle Subtract(this RotationAngle rotationAngle, RotationAngle angle)
    {
      return (RotationAngle)(((int)rotationAngle - (int)angle) % 360);
    }

    public static RotationAngle Reverse(this RotationAngle rotationAngle)
    {
      return (RotationAngle)((360 - (int)rotationAngle) % 360);
    }

    public static Vector3 GetEulerAngles(this RotationAngle rotationAngle)
    {
      return new Vector3(0, 0, (int)rotationAngle);
    }
  }
}
