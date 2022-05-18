// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using UnityEngine;

namespace Mediapipe.Unity.CoordinateSystem.Tests
{
  public class ImageCoordinateTest
  {
    #region ImageToPoint
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
    public void ImageToPoint_ShouldReturnLocalPoint_When_ImageSizeIsSameAsScreenSize(int width, int height, int x, int y, int z, RotationAngle imageRotation, bool isMirrored,
                                                                                     float expectedX, float expectedY, float expectedZ)
    {
      var rect = BuildRect(-width / 2, width / 2, -height / 2, height / 2);
      var result = ImageCoordinate.ImageToPoint(rect, x, y, z, width, height, imageRotation, isMirrored);
      Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
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
    public void ImageToPoint_ShouldReturnLocalPoint_When_ImageSizeIsNotSameAsScreenSize(int imageWidth, int imageHeight, int width, int height, int x, int y, int z,
                                                                                        RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY, float expectedZ)
    {
      var rect = BuildRect(-width / 2, width / 2, -height / 2, height / 2);
      var result = ImageCoordinate.ImageToPoint(rect, x, y, z, imageWidth, imageHeight, imageRotation, isMirrored);
      Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
    }

    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation0, false, 0, 480)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation0, true, 640, 480)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation90, false, 640, 480)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation90, true, 640, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation180, false, 640, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation180, true, 0, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation270, false, 0, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation270, true, 0, 480)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation0, false, -320, 480)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation0, true, 320, 480)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation90, false, 320, 480)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation90, true, 320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation180, false, 320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation180, true, -320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation270, false, -320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation270, true, -320, 480)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation0, false, -640, 480)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation0, true, 0, 480)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation90, false, 0, 480)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation90, true, 0, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation180, false, 0, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation180, true, -640, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation270, false, -640, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation270, true, -640, 480)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation0, false, 0, 240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation0, true, 640, 240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation90, false, 640, 240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation90, true, 640, -240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation180, false, 640, -240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation180, true, 0, -240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation270, false, 0, -240)]
    [TestCase(640, 480, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation270, true, 0, 240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation0, false, -320, 240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation0, true, 320, 240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation90, false, 320, 240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation90, true, 320, -240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation180, false, 320, -240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation180, true, -320, -240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation270, false, -320, -240)]
    [TestCase(640, 480, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation270, true, -320, 240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation0, false, -640, 240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation0, true, 0, 240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation90, false, 0, 240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation90, true, 0, -240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation180, false, 0, -240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation180, true, -640, -240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation270, false, -640, -240)]
    [TestCase(640, 480, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation270, true, -640, 240)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation0, false, 0, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation0, true, 640, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation90, false, 640, 0)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation90, true, 640, -480)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation180, false, 640, -480)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation180, true, 0, -480)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation270, false, 0, -480)]
    [TestCase(640, 480, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation270, true, 0, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation0, false, -320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation0, true, 320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation90, false, 320, 0)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation90, true, 320, -480)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation180, false, 320, -480)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation180, true, -320, -480)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation270, false, -320, -480)]
    [TestCase(640, 480, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation270, true, -320, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation0, false, -640, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation0, true, 0, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation90, false, 0, 0)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation90, true, 0, -480)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation180, false, 0, -480)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation180, true, -640, -480)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation270, false, -640, -480)]
    [TestCase(640, 480, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation270, true, -640, 0)]
    public void ImageToPoint_ShouldReturnLocalPoint_When_TheAnchorOfRectIsNotAtTheCenter(int width, int height, int x, int y, float xMin, float xMax, float yMin, float yMax,
                                                                                         RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY)
    {
      var rect = BuildRect(xMin, xMax, yMin, yMax);
      var result = ImageCoordinate.ImageToPoint(rect, x, y, width, height, imageRotation, isMirrored);
      Assert.AreEqual(new Vector3(expectedX, expectedY, 0), result);
    }
    #endregion

    #region ImageNormalizedToPoint
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
    public void ImageNormalizedToPoint_ShouldReturnLocalPoint_When_TheAnchorOfRectIsAtTheCenter(int width, int height, float normalizedX, float normalizedY, float normalizedZ,
                                                                                                RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY, float expectedZ)
    {
      var rect = BuildRect(-width / 2, width / 2, -height / 2, height / 2);
      var result = ImageCoordinate.ImageNormalizedToPoint(rect, normalizedX, normalizedY, normalizedZ, imageRotation, isMirrored);
      Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
    }

    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation0, false, 0, 480, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation0, true, 640, 480, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation90, false, 640, 480, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation90, true, 640, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation180, false, 640, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation180, true, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation270, false, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, 0, 480, RotationAngle.Rotation270, true, 0, 480, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation0, false, -320, 480, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation0, true, 320, 480, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation90, false, 320, 480, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation90, true, 320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation180, false, 320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation180, true, -320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation270, false, -320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, 0, 480, RotationAngle.Rotation270, true, -320, 480, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation0, false, -640, 480, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation0, true, 0, 480, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation90, false, 0, 480, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation90, true, 0, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation180, false, 0, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation180, true, -640, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation270, false, -640, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, 0, 480, RotationAngle.Rotation270, true, -640, 480, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation0, false, 0, 240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation0, true, 640, 240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation90, false, 640, 240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation90, true, 640, -240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation180, false, 640, -240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation180, true, 0, -240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation270, false, 0, -240, 0)]
    [TestCase(0, 0, 0, 0, 640, -240, 240, RotationAngle.Rotation270, true, 0, 240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation0, false, -320, 240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation0, true, 320, 240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation90, false, 320, 240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation90, true, 320, -240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation180, false, 320, -240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation180, true, -320, -240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation270, false, -320, -240, 0)]
    [TestCase(0, 0, 0, -320, 320, -240, 240, RotationAngle.Rotation270, true, -320, 240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation0, false, -640, 240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation0, true, 0, 240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation90, false, 0, 240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation90, true, 0, -240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation180, false, 0, -240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation180, true, -640, -240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation270, false, -640, -240, 0)]
    [TestCase(0, 0, 0, -640, 0, -240, 240, RotationAngle.Rotation270, true, -640, 240, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation0, false, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation0, true, 640, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation90, false, 640, 0, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation90, true, 640, -480, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation180, false, 640, -480, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation180, true, 0, -480, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation270, false, 0, -480, 0)]
    [TestCase(0, 0, 0, 0, 640, -480, 0, RotationAngle.Rotation270, true, 0, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation0, false, -320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation0, true, 320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation90, false, 320, 0, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation90, true, 320, -480, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation180, false, 320, -480, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation180, true, -320, -480, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation270, false, -320, -480, 0)]
    [TestCase(0, 0, 0, -320, 320, -480, 0, RotationAngle.Rotation270, true, -320, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation0, false, -640, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation0, true, 0, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation90, false, 0, 0, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation90, true, 0, -480, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation180, false, 0, -480, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation180, true, -640, -480, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation270, false, -640, -480, 0)]
    [TestCase(0, 0, 0, -640, 0, -480, 0, RotationAngle.Rotation270, true, -640, 0, 0)]
    public void ImageNormalizedToPoint_ShouldReturnLocalPoint_When_TheAnchorOfRectIsNotAtTheCenter(float normalizedX, float normalizedY, float normalizedZ, float xMin, float xMax, float yMin, float yMax,
                                                                                                   RotationAngle imageRotation, bool isMirrored, float expectedX, float expectedY, float expectedZ)
    {
      var rect = BuildRect(xMin, xMax, yMin, yMax);
      var result = ImageCoordinate.ImageNormalizedToPoint(rect, normalizedX, normalizedY, normalizedZ, imageRotation, isMirrored);
      Assert.AreEqual(new Vector3(expectedX, expectedY, expectedZ), result);
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
    public void ImageNormalizedToPoint_ShouldReturnLocalPoint_When_ZScaleIsSpecified(int width, int height, float normalizedX, float normalizedY, float normalizedZ, float zScale,
                                                                                     RotationAngle imageRotation, bool isMirrored, float expectedZ)
    {
      var rect = BuildRect(-width / 2, width / 2, -height / 2, height / 2);
      var result = ImageCoordinate.ImageNormalizedToPoint(rect, normalizedX, normalizedY, normalizedZ, zScale, imageRotation, isMirrored);
      Assert.AreEqual(expectedZ, result.z);
    }
    #endregion

    private UnityEngine.Rect BuildRect(float xMin, float xMax, float yMin, float yMax)
    {
      var x = xMax < 0 ? xMin : -xMax;
      var y = yMax < 0 ? yMin : -yMax;
      var rect = new UnityEngine.Rect(x, y, -2 * x, -2 * y);

      if (xMax < 0)
      {
        rect.xMax = xMax;
      }
      else
      {
        rect.xMin = xMin;
      }

      if (yMax < 0)
      {
        rect.yMax = yMax;
      }
      else
      {
        rect.yMin = yMin;
      }

      return rect;
    }
  }
}
