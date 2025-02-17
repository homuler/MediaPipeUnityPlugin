// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Unity.Experimental.Tests
{
  public class ImageTransformationOptionsTest
  {
    //     raw       ->    unrotated    ->      flip       ->   input image   ->   rotated
    // a---------b        a---------b        c---------d        a---------b       a---------b
    // |         |        |         |        |       v |        |         |       |         |
    // |       ^ |        |       ^ |        |         |        |       ^ |       |       ^ |
    // c---------d        c---------d        a---------b        c---------d       c---------d
    [TestCase(false, false, RotationAngle.Rotation0, false, true, RotationAngle.Rotation0)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->   rotated
    // a---------b        a---------b        a---------b        a---------b       a---------b
    // |         |        |         |        |         |        |         |       |         |
    // |       ^ |        |       ^ |        |       ^ |        |       ^ |       |       ^ |
    // c---------d        c---------d        c---------d        c---------d       c---------d
    [TestCase(false, true, RotationAngle.Rotation0, false, false, RotationAngle.Rotation0)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->   rotated
    // a---------b        a---------b        d---------c        b---------a       b---------a
    // |         |        |         |        | v       |        |         |       |         |
    // |       ^ |        |       ^ |        |         |        | ^       |       | ^       |
    // c---------d        c---------d        b---------a        d---------c       d---------c
    [TestCase(true, false, RotationAngle.Rotation0, true, true, RotationAngle.Rotation0)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->   rotated
    // a---------b        a---------b        b---------a        b---------a       b---------a
    // |         |        |         |        |         |        |         |       |         |
    // |       ^ |        |       ^ |        | ^       |        | ^       |       | ^       |
    // c---------d        c---------d        d---------c        d---------c       d---------c
    [TestCase(true, true, RotationAngle.Rotation0, true, false, RotationAngle.Rotation0)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // b-----d        a---------b        c---------d        a---------b        b-----d
    // |     |        |         |        | >       |        |         |        |     |
    // |   ^ |        | >       |        |         |        | >       |        |   ^ |
    // a-----c        c---------d        a---------b        c---------d        a-----c
    [TestCase(false, false, RotationAngle.Rotation90, false, true, RotationAngle.Rotation90)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // b-----d        a---------b        a---------b        a---------b        b-----d
    // |     |        |         |        |         |        |         |        |     |
    // |   ^ |        | >       |        | >       |        | >       |        |   ^ |
    // a-----c        c---------d        c---------d        c---------d        a-----c
    [TestCase(false, true, RotationAngle.Rotation90, false, false, RotationAngle.Rotation90)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // b-----d        a---------b        a---------b        c---------d        d-----b
    // |     |        |         |        |         |        | >       |        |     |
    // |   ^ |        | >       |        | >       |        |         |        | ^   |
    // a-----c        c---------d        c---------d        a---------b        c-----a
    [TestCase(true, false, RotationAngle.Rotation90, false, false, RotationAngle.Rotation90)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // b-----d        a---------b        c---------d        c---------d        d-----b
    // |     |        |         |        | >       |        | >       |        |     |
    // |   ^ |        | >       |        |         |        |         |        | ^   |
    // a-----c        c---------d        a---------b        a---------b        c-----a
    [TestCase(true, true, RotationAngle.Rotation90, false, true, RotationAngle.Rotation90)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->     rotated
    // d---------c        a---------b        c---------d        a---------b        d---------c
    // |         |        | v       |        |         |        | v       |        |         |
    // |       ^ |        |         |        | ^       |        |         |        |       ^ |
    // b---------a        c---------d        a---------b        c---------d        b---------a
    [TestCase(false, false, RotationAngle.Rotation180, false, true, RotationAngle.Rotation180)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->     rotated
    // d---------c        a---------b        a---------b        a---------b        d---------c
    // |         |        | v       |        | v       |        | v       |        |         |
    // |       ^ |        |         |        |         |        |         |        |       ^ |
    // b---------a        c---------d        c---------d        c---------d        b---------a
    [TestCase(false, true, RotationAngle.Rotation180, false, false, RotationAngle.Rotation180)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->     rotated
    // d---------c        a---------b        d---------c        b---------a        c---------d
    // |         |        | v       |        |         |        |       v |        |         |
    // |       ^ |        |         |        |       ^ |        |         |        | ^       |
    // b---------a        c---------d        b---------a        d---------c        a---------b
    [TestCase(true, false, RotationAngle.Rotation180, true, true, RotationAngle.Rotation180)]
    //     raw       ->    unrotated    ->      flip       ->   input image   ->     rotated
    // d---------c        a---------b        b---------a        b---------a        c---------d
    // |         |        | v       |        |       v |        |       v |        |         |
    // |       ^ |        |         |        |         |        |         |        | ^       |
    // b---------a        c---------d        d---------c        d---------c        a---------b
    [TestCase(true, true, RotationAngle.Rotation180, true, false, RotationAngle.Rotation180)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // c-----a        a---------b        c---------d        a---------b        c-----a
    // |     |        |       < |        |         |        |       < |        |     |
    // |   ^ |        |         |        |       < |        |         |        |   ^ |
    // d-----b        c---------d        a---------b        c---------d        d-----b
    [TestCase(false, false, RotationAngle.Rotation270, false, true, RotationAngle.Rotation270)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // c-----a        a---------b        a---------b        a---------b        c-----a
    // |     |        |       < |        |       < |        |       < |        |     |
    // |   ^ |        |         |        |         |        |         |        |   ^ |
    // d-----b        c---------d        c---------d        c---------d        d-----b
    [TestCase(false, true, RotationAngle.Rotation270, false, false, RotationAngle.Rotation270)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // c-----a        a---------b        a---------b        c---------d        a-----c
    // |     |        |       < |        |       < |        |         |        |     |
    // |   ^ |        |         |        |         |        |       < |        | ^   |
    // d-----b        c---------d        c---------d        a---------b        b-----d
    [TestCase(true, false, RotationAngle.Rotation270, false, false, RotationAngle.Rotation270)]
    //   raw     ->    unrotated    ->      flip       ->   input image   ->   rotated
    // c-----a        a---------b        c---------d        c---------d        a-----c
    // |     |        |       < |        |         |        |         |        |     |
    // |   ^ |        |         |        |       < |        |       < |        | ^   |
    // d-----b        c---------d        a---------b        a---------b        b-----d
    [TestCase(true, true, RotationAngle.Rotation270, false, true, RotationAngle.Rotation270)]
    public void TestBuild(
      bool shouldFlipHorizontally, bool isFlippedVertically, RotationAngle rotationAngle,
      bool flipHorizontally, bool flipVertically, RotationAngle expectedRotationAngle)
    {
      var options = ImageTransformationOptions.Build(shouldFlipHorizontally, isFlippedVertically, rotationAngle);
      Assert.AreEqual(flipHorizontally, options.flipHorizontally);
      Assert.AreEqual(flipVertically, options.flipVertically);
      Assert.AreEqual(expectedRotationAngle, options.rotationAngle);
    }
  }
}
