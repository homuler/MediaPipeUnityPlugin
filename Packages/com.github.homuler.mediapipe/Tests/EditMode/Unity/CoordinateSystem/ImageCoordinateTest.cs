// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
  public class ImageCoordinateTest
  {
    #region GetLocalPosition
    [TestCase(640, 480, -160, 0, 10, RotationAngle.Rotation0, false, -480, 240, 10)]
    [TestCase(640, 480, 0, -160, 10, RotationAngle.Rotation0, false, -320, 400, 10)]
    [TestCase(640, 480, 800, 0, 10, RotationAngle.Rotation0, false, 480, 240, 10)]
    [TestCase(640, 480, 640, -160, 10, RotationAngle.Rotation0, false, 320, 400, 10)]
    [TestCase(640, 480, -160, 480, -10, RotationAngle.Rotation0, false, -480, -240, -10)]
    [TestCase(640, 480, 0, 640, -10, RotationAngle.Rotation0, false, -320, -400, -10)]
    [TestCase(640, 480, 800, 480, -10, RotationAngle.Rotation0, false, 480, -240, -10)]
    [TestCase(640, 480, 640, 640, -10, RotationAngle.Rotation0, false, 320, -400, -10)]
    [TestCase(640, 480, -160, 0, -10, RotationAngle.Rotation0, true, 480, 240, -10)]
    [TestCase(640, 480, 0, -160, -10, RotationAngle.Rotation0, true, 320, 400, -10)]
    [TestCase(640, 480, 800, 0, -10, RotationAngle.Rotation0, true, -480, 240, -10)]
    [TestCase(640, 480, 640, -160, -10, RotationAngle.Rotation0, true, -320, 400, -10)]
    [TestCase(640, 480, -160, 480, 10, RotationAngle.Rotation0, true, 480, -240, 10)]
    [TestCase(640, 480, 0, 640, 10, RotationAngle.Rotation0, true, 320, -400, 10)]
    [TestCase(640, 480, 800, 480, 10, RotationAngle.Rotation0, true, -480, -240, 10)]
    [TestCase(640, 480, 640, 640, 10, RotationAngle.Rotation0, true, -320, -400, 10)]
    [TestCase(640, 480, -160, 0, 10, RotationAngle.Rotation90, false, 320, 400, 10)]
    [TestCase(640, 480, 0, -160, 10, RotationAngle.Rotation90, false, 480, 240, 10)]
    [TestCase(640, 480, 640, 0, 10, RotationAngle.Rotation90, false, 320, -400, 10)]
    [TestCase(640, 480, 480, -160, 10, RotationAngle.Rotation90, false, 480, -240, 10)]
    [TestCase(640, 480, -160, 640, -10, RotationAngle.Rotation90, false, -320, 400, -10)]
    [TestCase(640, 480, 0, 800, -10, RotationAngle.Rotation90, false, -480, 240, -10)]
    [TestCase(640, 480, 640, 640, -10, RotationAngle.Rotation90, false, -320, -400, -10)]
    [TestCase(640, 480, 480, 800, -10, RotationAngle.Rotation90, false, -480, -240, -10)]
    [TestCase(640, 480, -160, 0, -10, RotationAngle.Rotation90, true, 320, -400, -10)]
    [TestCase(640, 480, 0, -160, -10, RotationAngle.Rotation90, true, 480, -240, -10)]
    [TestCase(640, 480, 640, 0, -10, RotationAngle.Rotation90, true, 320, 400, -10)]
    [TestCase(640, 480, 480, -160, -10, RotationAngle.Rotation90, true, 480, 240, -10)]
    [TestCase(640, 480, -160, 640, 10, RotationAngle.Rotation90, true, -320, -400, 10)]
    [TestCase(640, 480, 0, 800, 10, RotationAngle.Rotation90, true, -480, -240, 10)]
    [TestCase(640, 480, 640, 640, 10, RotationAngle.Rotation90, true, -320, 400, 10)]
    [TestCase(640, 480, 480, 800, 10, RotationAngle.Rotation90, true, -480, 240, 10)]
    [TestCase(640, 480, -160, 0, 10, RotationAngle.Rotation180, false, 480, -240, 10)]
    [TestCase(640, 480, 0, -160, 10, RotationAngle.Rotation180, false, 320, -400, 10)]
    [TestCase(640, 480, 800, 0, 10, RotationAngle.Rotation180, false, -480, -240, 10)]
    [TestCase(640, 480, 640, -160, 10, RotationAngle.Rotation180, false, -320, -400, 10)]
    [TestCase(640, 480, -160, 480, -10, RotationAngle.Rotation180, false, 480, 240, -10)]
    [TestCase(640, 480, 0, 640, -10, RotationAngle.Rotation180, false, 320, 400, -10)]
    [TestCase(640, 480, 800, 480, -10, RotationAngle.Rotation180, false, -480, 240, -10)]
    [TestCase(640, 480, 640, 640, -10, RotationAngle.Rotation180, false, -320, 400, -10)]
    [TestCase(640, 480, -160, 0, -10, RotationAngle.Rotation180, true, -480, -240, -10)]
    [TestCase(640, 480, 0, -160, -10, RotationAngle.Rotation180, true, -320, -400, -10)]
    [TestCase(640, 480, 800, 0, -10, RotationAngle.Rotation180, true, 480, -240, -10)]
    [TestCase(640, 480, 640, -160, -10, RotationAngle.Rotation180, true, 320, -400, -10)]
    [TestCase(640, 480, -160, 480, 10, RotationAngle.Rotation180, true, -480, 240, 10)]
    [TestCase(640, 480, 0, 640, 10, RotationAngle.Rotation180, true, -320, 400, 10)]
    [TestCase(640, 480, 800, 480, 10, RotationAngle.Rotation180, true, 480, 240, 10)]
    [TestCase(640, 480, 640, 640, 10, RotationAngle.Rotation180, true, 320, 400, 10)]
    [TestCase(640, 480, -160, 0, 10, RotationAngle.Rotation270, false, -320, -400, 10)]
    [TestCase(640, 480, 0, -160, 10, RotationAngle.Rotation270, false, -480, -240, 10)]
    [TestCase(640, 480, 640, 0, 10, RotationAngle.Rotation270, false, -320, 400, 10)]
    [TestCase(640, 480, 480, -160, 10, RotationAngle.Rotation270, false, -480, 240, 10)]
    [TestCase(640, 480, -160, 640, -10, RotationAngle.Rotation270, false, 320, -400, -10)]
    [TestCase(640, 480, 0, 800, -10, RotationAngle.Rotation270, false, 480, -240, -10)]
    [TestCase(640, 480, 640, 640, -10, RotationAngle.Rotation270, false, 320, 400, -10)]
    [TestCase(640, 480, 480, 800, -10, RotationAngle.Rotation270, false, 480, 240, -10)]
    [TestCase(640, 480, -160, 0, -10, RotationAngle.Rotation270, true, -320, 400, -10)]
    [TestCase(640, 480, 0, -160, -10, RotationAngle.Rotation270, true, -480, 240, -10)]
    [TestCase(640, 480, 640, 0, -10, RotationAngle.Rotation270, true, -320, -400, -10)]
    [TestCase(640, 480, 480, -160, -10, RotationAngle.Rotation270, true, -480, -240, -10)]
    [TestCase(640, 480, -160, 640, 10, RotationAngle.Rotation270, true, 320, 400, 10)]
    [TestCase(640, 480, 0, 800, 10, RotationAngle.Rotation270, true, 480, 240, 10)]
    [TestCase(640, 480, 640, 640, 10, RotationAngle.Rotation270, true, 320, -400, 10)]
    [TestCase(640, 480, 480, 800, 10, RotationAngle.Rotation270, true, 480, -240, 10)]
    public void GetLocalPosition_ShouldReturnLocalPosition_When_ImageSizeIsNotSpecified(int width, int height, int x, int y, int z, RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY, float expectedZ)
    {
      WithRectTransform((rectTransform) =>
      {
        rectTransform.sizeDelta = new Vector2(width, height);
        var result = ImageCoordinate.GetLocalPosition(rectTransform, x, y, z, imageRotation, isMirrored);
        Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
      });
    }

    [TestCase(640, 480, 720, 540, -160, 0, 0, RotationAngle.Rotation0, false, -540, 270, 0)]
    [TestCase(640, 480, 720, 540, 0, -160, 0, RotationAngle.Rotation0, false, -360, 450, 0)]
    [TestCase(640, 480, 720, 540, 800, 0, 0, RotationAngle.Rotation0, false, 540, 270, 0)]
    [TestCase(640, 480, 720, 540, 640, -160, 0, RotationAngle.Rotation0, false, 360, 450, 0)]
    [TestCase(640, 480, 720, 540, -160, 480, 0, RotationAngle.Rotation0, false, -540, -270, 0)]
    [TestCase(640, 480, 720, 540, 0, 640, 0, RotationAngle.Rotation0, false, -360, -450, 0)]
    [TestCase(640, 480, 720, 540, 800, 480, 0, RotationAngle.Rotation0, false, 540, -270, 0)]
    [TestCase(640, 480, 720, 540, 640, 640, 0, RotationAngle.Rotation0, false, 360, -450, 0)]
    [TestCase(640, 480, 960, 720, -160, 0, 0, RotationAngle.Rotation90, true, 480, -600, 0)]
    [TestCase(640, 480, 960, 720, 0, -160, 0, RotationAngle.Rotation90, true, 720, -360, 0)]
    [TestCase(640, 480, 960, 720, 640, 0, 0, RotationAngle.Rotation90, true, 480, 600, 0)]
    [TestCase(640, 480, 960, 720, 480, -160, 0, RotationAngle.Rotation90, true, 720, 360, 0)]
    [TestCase(640, 480, 960, 720, -160, 640, 0, RotationAngle.Rotation90, true, -480, -600, 0)]
    [TestCase(640, 480, 960, 720, 0, 800, 0, RotationAngle.Rotation90, true, -720, -360, 0)]
    [TestCase(640, 480, 960, 720, 640, 640, 0, RotationAngle.Rotation90, true, -480, 600, 0)]
    [TestCase(640, 480, 960, 720, 480, 800, 0, RotationAngle.Rotation90, true, -720, 360, 0)]
    [TestCase(640, 480, 480, 360, -160, 0, 0, RotationAngle.Rotation180, true, -360, -180, 0)]
    [TestCase(640, 480, 480, 360, 0, -160, 0, RotationAngle.Rotation180, true, -240, -300, 0)]
    [TestCase(640, 480, 480, 360, 800, 0, 0, RotationAngle.Rotation180, true, 360, -180, 0)]
    [TestCase(640, 480, 480, 360, 640, -160, 0, RotationAngle.Rotation180, true, 240, -300, 0)]
    [TestCase(640, 480, 480, 360, -160, 480, 0, RotationAngle.Rotation180, true, -360, 180, 0)]
    [TestCase(640, 480, 480, 360, 0, 640, 0, RotationAngle.Rotation180, true, -240, 300, 0)]
    [TestCase(640, 480, 480, 360, 800, 480, 0, RotationAngle.Rotation180, true, 360, 180, 0)]
    [TestCase(640, 480, 480, 360, 640, 640, 0, RotationAngle.Rotation180, true, 240, 300, 0)]
    [TestCase(640, 480, 320, 240, -160, 0, 0, RotationAngle.Rotation270, false, -160, -200, 0)]
    [TestCase(640, 480, 320, 240, 0, -160, 0, RotationAngle.Rotation270, false, -240, -120, 0)]
    [TestCase(640, 480, 320, 240, 640, 0, 0, RotationAngle.Rotation270, false, -160, 200, 0)]
    [TestCase(640, 480, 320, 240, 480, -160, 0, RotationAngle.Rotation270, false, -240, 120, 0)]
    [TestCase(640, 480, 320, 240, -160, 640, 0, RotationAngle.Rotation270, false, 160, -200, 0)]
    [TestCase(640, 480, 320, 240, 0, 800, 0, RotationAngle.Rotation270, false, 240, -120, 0)]
    [TestCase(640, 480, 320, 240, 640, 640, 0, RotationAngle.Rotation270, false, 160, 200, 0)]
    [TestCase(640, 480, 320, 240, 480, 800, 0, RotationAngle.Rotation270, false, 240, 120, 0)]
    public void GetLocalPosition_ShouldReturnLocalPosition_When_ImageSizeIsSpecified(int imageWidth, int imageHeight, int width, int height, int x, int y, int z, RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY, float expectedZ)
    {
      WithRectTransform((rectTransform) =>
      {
        rectTransform.sizeDelta = new Vector2(width, height);
        var result = ImageCoordinate.GetLocalPosition(rectTransform, x, y, z, new Vector2(imageWidth, imageHeight), imageRotation, isMirrored);
        Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
      });
    }
    #endregion

    #region GetLocalPositionNormalized
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation0, false, -480, 240, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation0, false, -320, 360, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation0, false, 480, 240, 640)]
    [TestCase(640, 480, 1f, -0.25f, 1, RotationAngle.Rotation0, false, 320, 360, 640)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation0, false, -480, -240, -640)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation0, false, -320, -360, -640)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation0, false, 480, -240, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation0, false, 320, -360, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation0, true, 480, 240, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation0, true, 320, 360, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation0, true, -480, 240, 640)]
    [TestCase(640, 480, 1f, -0.25f, 1, RotationAngle.Rotation0, true, -320, 360, 640)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation0, true, 480, -240, -640)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation0, true, 320, -360, -640)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation0, true, -480, -240, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation0, true, -320, -360, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation90, false, 320, 360, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation90, false, 480, 240, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation90, false, 320, -360, 480)]
    [TestCase(640, 480, 1, -0.25f, 1, RotationAngle.Rotation90, false, 480, -240, 480)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation90, false, -320, 360, -480)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation90, false, -480, 240, -480)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation90, false, -320, -360, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation90, false, -480, -240, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation90, true, 320, -360, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation90, true, 480, -240, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation90, true, 320, 360, 480)]
    [TestCase(640, 480, 1, -0.25f, 1, RotationAngle.Rotation90, true, 480, 240, 480)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation90, true, -320, -360, -480)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation90, true, -480, -240, -480)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation90, true, -320, 360, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation90, true, -480, 240, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation180, false, 480, -240, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation180, false, 320, -360, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation180, false, -480, -240, 640)]
    [TestCase(640, 480, 1, -0.25f, 1, RotationAngle.Rotation180, false, -320, -360, 640)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation180, false, 480, 240, -640)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation180, false, 320, 360, -640)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation180, false, -480, 240, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation180, false, -320, 360, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation180, true, -480, -240, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation180, true, -320, -360, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation180, true, 480, -240, 640)]
    [TestCase(640, 480, 1, -0.25f, 1, RotationAngle.Rotation180, true, 320, -360, 640)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation180, true, -480, 240, -640)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation180, true, -320, 360, -640)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation180, true, 480, 240, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation180, true, 320, 360, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation270, false, -320, -360, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation270, false, -480, -240, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation270, false, -320, 360, 480)]
    [TestCase(640, 480, 1, -0.25f, 1, RotationAngle.Rotation270, false, -480, 240, 480)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation270, false, 320, -360, -480)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation270, false, 480, -240, -480)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation270, false, 320, 360, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation270, false, 480, 240, 0)]
    [TestCase(640, 480, -0.25f, 0, 0, RotationAngle.Rotation270, true, -320, 360, 0)]
    [TestCase(640, 480, 0, -0.25f, 0, RotationAngle.Rotation270, true, -480, 240, 0)]
    [TestCase(640, 480, 1.25f, 0, 1, RotationAngle.Rotation270, true, -320, -360, 480)]
    [TestCase(640, 480, 1, -0.25f, 1, RotationAngle.Rotation270, true, -480, -240, 480)]
    [TestCase(640, 480, -0.25f, 1, -1, RotationAngle.Rotation270, true, 320, 360, -480)]
    [TestCase(640, 480, 0, 1.25f, -1, RotationAngle.Rotation270, true, 480, 240, -480)]
    [TestCase(640, 480, 1.25f, 1, 0, RotationAngle.Rotation270, true, 320, -360, 0)]
    [TestCase(640, 480, 1, 1.25f, 0, RotationAngle.Rotation270, true, 480, -240, 0)]
    public void GetLocalPositionNormalized_ShouldReturnLocalPosition_When_ImageRotationAndIsMirroredAreSpecified(int width, int height, float normalizedX, float normalizedY, float normalizedZ, RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY, float expectedZ)
    {
      WithRectTransform((rectTransform) =>
      {
        rectTransform.sizeDelta = new Vector2(width, height);
        var result = ImageCoordinate.GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, normalizedZ, imageRotation, isMirrored);
        Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
      });
    }

    [TestCase(640, 480, 0, 0, 1, 100, RotationAngle.Rotation0, false, 100)]
    [TestCase(640, 480, 0, 0, 0.5f, 100, RotationAngle.Rotation0, false, 50)]
    [TestCase(640, 480, 0, 0, -1, 100, RotationAngle.Rotation0, false, -100)]
    [TestCase(640, 480, 0, 0, -0.5f, 100, RotationAngle.Rotation0, false, -50)]
    [TestCase(640, 480, 0, 0, 1, -100, RotationAngle.Rotation0, true, -100)]
    [TestCase(640, 480, 0, 0, 0.5f, -100, RotationAngle.Rotation0, true, -50)]
    [TestCase(640, 480, 0, 0, -1, -100, RotationAngle.Rotation0, true, 100)]
    [TestCase(640, 480, 0, 0, -0.5f, -100, RotationAngle.Rotation0, true, 50)]
    [TestCase(640, 480, 0, 0, 1, 100, RotationAngle.Rotation90, false, 100)]
    [TestCase(640, 480, 0, 0, 0.5f, 100, RotationAngle.Rotation90, false, 50)]
    [TestCase(640, 480, 0, 0, -1, 100, RotationAngle.Rotation90, false, -100)]
    [TestCase(640, 480, 0, 0, -0.5f, 100, RotationAngle.Rotation90, false, -50)]
    [TestCase(640, 480, 0, 0, 1, -100, RotationAngle.Rotation90, true, -100)]
    [TestCase(640, 480, 0, 0, 0.5f, -100, RotationAngle.Rotation90, true, -50)]
    [TestCase(640, 480, 0, 0, -1, -100, RotationAngle.Rotation90, true, 100)]
    [TestCase(640, 480, 0, 0, -0.5f, -100, RotationAngle.Rotation90, true, 50)]
    [TestCase(640, 480, 0, 0, 1, -100, RotationAngle.Rotation180, false, -100)]
    [TestCase(640, 480, 0, 0, 0.5f, -100, RotationAngle.Rotation180, false, -50)]
    [TestCase(640, 480, 0, 0, -1, -100, RotationAngle.Rotation180, false, 100)]
    [TestCase(640, 480, 0, 0, -0.5f, -100, RotationAngle.Rotation180, false, 50)]
    [TestCase(640, 480, 0, 0, 1, 100, RotationAngle.Rotation180, true, 100)]
    [TestCase(640, 480, 0, 0, 0.5f, 100, RotationAngle.Rotation180, true, 50)]
    [TestCase(640, 480, 0, 0, -1, 100, RotationAngle.Rotation180, true, -100)]
    [TestCase(640, 480, 0, 0, -0.5f, 100, RotationAngle.Rotation180, true, -50)]
    [TestCase(640, 480, 0, 0, 1, -100, RotationAngle.Rotation270, false, -100)]
    [TestCase(640, 480, 0, 0, 0.5f, -100, RotationAngle.Rotation270, false, -50)]
    [TestCase(640, 480, 0, 0, -1, -100, RotationAngle.Rotation270, false, 100)]
    [TestCase(640, 480, 0, 0, -0.5f, -100, RotationAngle.Rotation270, false, 50)]
    [TestCase(640, 480, 0, 0, 1, 100, RotationAngle.Rotation270, true, 100)]
    [TestCase(640, 480, 0, 0, 0.5f, 100, RotationAngle.Rotation270, true, 50)]
    [TestCase(640, 480, 0, 0, -1, 100, RotationAngle.Rotation270, true, -100)]
    [TestCase(640, 480, 0, 0, -0.5f, 100, RotationAngle.Rotation270, true, -50)]
    public void GetLocalPositionNormalized_ShouldScaleZ_When_ZScaleIsSpecified(int width, int height, float normalizedX, float normalizedY, float normalizedZ, float zScale, RotationAngle imageRotation, bool isMirrored, float expectedZ)
    {
      WithRectTransform((rectTransform) =>
      {
        rectTransform.sizeDelta = new Vector2(width, height);
        var result = ImageCoordinate.GetLocalPositionNormalized(rectTransform, normalizedX, normalizedY, normalizedZ, zScale, imageRotation, isMirrored);
        Assert.AreEqual(expectedZ, result.z);
      });
    }
    #endregion

    private void WithRectTransform(System.Action<RectTransform> action)
    {
      var gameObject = new GameObject();
      var rectTransform = gameObject.AddComponent<RectTransform>();
      rectTransform.pivot = 0.5f * Vector2.one;
      rectTransform.anchorMin = 0.5f * Vector2.one;
      rectTransform.anchorMax = 0.5f * Vector2.one;

      action(rectTransform);
#pragma warning disable IDE0002
      GameObject.DestroyImmediate(gameObject);
#pragma warning restore IDE0002
    }
  }
}
