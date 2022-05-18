// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using Unity.Collections;
using UnityEngine;

using System.Linq;

namespace Mediapipe.Unity.Tests
{
  public class ImageFrameExtensionTest
  {
    #region TryReadChannel(byte)
    [Test]
    public void TryReadChannelByte_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(-1, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReturnFalse_When_TheChannelDataIsNotStoredInBytes()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(1, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(2, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(3, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(3, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(0, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(2, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSrgb()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 3 = 7
        1,  2,  3, 33, 34, 35, 65, 66, 67, 0, 0, 0, 0, 0, 0, 0,
        9, 10, 11, 41, 42, 43, 73, 74, 75, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new byte[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new byte[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new byte[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new byte[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new byte[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new byte[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new byte[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new byte[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new byte[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, false));
        Assert.AreEqual(new byte[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, true));
        Assert.AreEqual(new byte[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, false));
        Assert.AreEqual(new byte[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, true));
        Assert.AreEqual(new byte[] { 67, 35, 3, 75, 43, 11 }, result);

      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSrgba()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 4 = 4
        1,  2,  3,  4, 33, 34, 35, 36, 65, 66, 67, 68, 0, 0, 0, 0,
        9, 10, 11, 12, 41, 42, 43, 44, 73, 74, 75, 76, 0, 0, 0, 0,
      };
      var result = new byte[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new byte[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new byte[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new byte[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new byte[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new byte[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new byte[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new byte[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new byte[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, false));
        Assert.AreEqual(new byte[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, true));
        Assert.AreEqual(new byte[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, false));
        Assert.AreEqual(new byte[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, true));
        Assert.AreEqual(new byte[] { 67, 35, 3, 75, 43, 11 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, false, false));
        Assert.AreEqual(new byte[] { 12, 44, 76, 4, 36, 68 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, false, true));
        Assert.AreEqual(new byte[] { 4, 36, 68, 12, 44, 76 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, true, false));
        Assert.AreEqual(new byte[] { 76, 44, 12, 68, 36, 4 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, true, true));
        Assert.AreEqual(new byte[] { 68, 36, 4, 76, 44, 12 }, result);
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSbgra()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 4 * 3 = 4
        1,  2,  3,  4, 33, 34, 35, 36, 65, 66, 67, 68, 0, 0, 0, 0,
        9, 10, 11, 12, 41, 42, 43, 44, 73, 74, 75, 76, 0, 0, 0, 0,
      };
      var result = new byte[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new byte[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new byte[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new byte[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new byte[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new byte[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new byte[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new byte[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new byte[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, false));
        Assert.AreEqual(new byte[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, true));
        Assert.AreEqual(new byte[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, false));
        Assert.AreEqual(new byte[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, true));
        Assert.AreEqual(new byte[] { 67, 35, 3, 75, 43, 11 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, false, false));
        Assert.AreEqual(new byte[] { 12, 44, 76, 4, 36, 68 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, false, true));
        Assert.AreEqual(new byte[] { 4, 36, 68, 12, 44, 76 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, true, false));
        Assert.AreEqual(new byte[] { 76, 44, 12, 68, 36, 4 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, true, true));
        Assert.AreEqual(new byte[] { 68, 36, 4, 76, 44, 12 }, result);
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReadTheSpecifiedChannelData_When_TheFormatIsGray8()
    {
      var bytes = new byte[] {
        // padding is 16 - 3 * 1 = 13
        1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new byte[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new byte[] { 4, 5, 6, 1, 2, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new byte[] { 6, 5, 4, 3, 2, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new byte[] { 3, 2, 1, 6, 5, 4 }, result);
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReadTheSpecifiedChannelData_When_TheFormatIsLab8()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 3 = 7
        1,  2,  3, 33, 34, 35, 65, 66, 67, 0, 0, 0, 0, 0, 0, 0,
        9, 10, 11, 41, 42, 43, 73, 74, 75, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new byte[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new byte[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new byte[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new byte[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new byte[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new byte[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new byte[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new byte[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new byte[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, false));
        Assert.AreEqual(new byte[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, true));
        Assert.AreEqual(new byte[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, false));
        Assert.AreEqual(new byte[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, true));
        Assert.AreEqual(new byte[] { 67, 35, 3, 75, 43, 11 }, result);
      }
    }
    #endregion

    #region TryReadChannel(ushort)
    [Test]
    public void TryReadChannelUshort_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(-1, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReturnFalse_When_TheChannelDataIsNotStoredInUshorts()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(1, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(2, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(3, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(0, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSrgb48()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 24 - 2 * 3 * 3 = 6
        1, 0,  2, 0,  3, 0, 33, 0, 34, 0, 35, 0, 65, 0, 66, 0, 67, 0, 0, 0, 0, 0, 0, 0,
        9, 0, 10, 0, 11, 0, 41, 0, 42, 0, 43, 0, 73, 0, 74, 0, 75, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new ushort[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 3, 2, 24, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new ushort[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new ushort[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new ushort[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new ushort[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new ushort[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new ushort[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new ushort[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new ushort[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, false));
        Assert.AreEqual(new ushort[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, true));
        Assert.AreEqual(new ushort[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, false));
        Assert.AreEqual(new ushort[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, true));
        Assert.AreEqual(new ushort[] { 67, 35, 3, 75, 43, 11 }, result);
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSrgba64()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 24 - 2 * 3 * 4 = 0
        1, 0,  2, 0,  3, 0,  4, 0, 33, 0, 34, 0, 35, 0, 36, 0, 65, 0, 66, 0, 67, 0, 68, 0,
        9, 0, 10, 0, 11, 0, 12, 0, 41, 0, 42, 0, 43, 0, 44, 0, 73, 0, 74, 0, 75, 0, 76, 0,
      };
      var result = new ushort[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 3, 2, 24, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new ushort[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new ushort[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new ushort[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new ushort[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new ushort[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new ushort[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new ushort[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new ushort[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, false));
        Assert.AreEqual(new ushort[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, false, true));
        Assert.AreEqual(new ushort[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, false));
        Assert.AreEqual(new ushort[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(2, result, true, true));
        Assert.AreEqual(new ushort[] { 67, 35, 3, 75, 43, 11 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, false, false));
        Assert.AreEqual(new ushort[] { 12, 44, 76, 4, 36, 68 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, false, true));
        Assert.AreEqual(new ushort[] { 4, 36, 68, 12, 44, 76 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, true, false));
        Assert.AreEqual(new ushort[] { 76, 44, 12, 68, 36, 4 }, result);

        Assert.True(imageFrame.TryReadChannel(3, result, true, true));
        Assert.AreEqual(new ushort[] { 68, 36, 4, 76, 44, 12 }, result);
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReadTheSpecifiedChannelData_When_TheFormatIsGray16()
    {
      var bytes = new byte[] {
        // padding is 16 - 2 * 3 = 10
        1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        4, 0, 5, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new ushort[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new ushort[] { 4, 5, 6, 1, 2, 3 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new ushort[] { 1, 2, 3, 4, 5, 6 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new ushort[] { 6, 5, 4, 3, 2, 1 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new ushort[] { 3, 2, 1, 6, 5, 4 }, result);
      }
    }
    #endregion

    #region TryReadChannel(float)
    [Test]
    public void TryReadChannelFloat_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(-1, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReturnFalse_When_TheChannelDataIsNotStoredInFloats()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(1, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(2, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(1, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReadTheSpecifiedChannelData_When_TheFormatIsVec32F1()
    {
      var floats = new float[] {
        // padding is 16 - 3 * 4 = 4
        1.0f / 255, 2.0f / 255, 3.0f / 255, 0,
        4.0f / 255, 5.0f / 255, 6.0f / 255, 0,
      };
      var bytes = FloatsToBytes(floats);
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new float[] { 4.0f / 255, 5.0f / 255, 6.0f / 255, 1.0f / 255, 2.0f / 255, 3.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new float[] { 1.0f / 255, 2.0f / 255, 3.0f / 255, 4.0f / 255, 5.0f / 255, 6.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new float[] { 6.0f / 255, 5.0f / 255, 4.0f / 255, 3.0f / 255, 2.0f / 255, 1.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new float[] { 3.0f / 255, 2.0f / 255, 1.0f / 255, 6.0f / 255, 5.0f / 255, 4.0f / 255 }, result);
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReadTheSpecifiedChannelData_When_TheFormatIsVec32F2()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var floats = new float[] {
        // padding is 32 - 2 * 3 * 4 = 8
        1.0f / 255,  2.0f / 255, 33.0f / 255, 34.0f / 255, 65.0f / 255, 66.0f / 255, 0, 0,
        9.0f / 255, 10.0f / 255, 41.0f / 255, 42.0f / 255, 73.0f / 255, 74.0f / 255, 0, 0,
      };
      var bytes = FloatsToBytes(floats);
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 3, 2, 32, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, result, false, false));
        Assert.AreEqual(new float[] { 9.0f / 255, 41.0f / 255, 73.0f / 255, 1.0f / 255, 33.0f / 255, 65.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, false, true));
        Assert.AreEqual(new float[] { 1.0f / 255, 33.0f / 255, 65.0f / 255, 9.0f / 255, 41.0f / 255, 73.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, false));
        Assert.AreEqual(new float[] { 73.0f / 255, 41.0f / 255, 9.0f / 255, 65.0f / 255, 33.0f / 255, 1.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(0, result, true, true));
        Assert.AreEqual(new float[] { 65.0f / 255, 33.0f / 255, 1.0f / 255, 73.0f / 255, 41.0f / 255, 9.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, false));
        Assert.AreEqual(new float[] { 10.0f / 255, 42.0f / 255, 74.0f / 255, 2.0f / 255, 34.0f / 255, 66.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, false, true));
        Assert.AreEqual(new float[] { 2.0f / 255, 34.0f / 255, 66.0f / 255, 10.0f / 255, 42.0f / 255, 74.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, false));
        Assert.AreEqual(new float[] { 74.0f / 255, 42.0f / 255, 10.0f / 255, 66.0f / 255, 34.0f / 255, 2.0f / 255 }, result);

        Assert.True(imageFrame.TryReadChannel(1, result, true, true));
        Assert.AreEqual(new float[] { 66.0f / 255, 34.0f / 255, 2.0f / 255, 74.0f / 255, 42.0f / 255, 10.0f / 255 }, result);
      }
    }
    #endregion

    #region TryReadChannelNormalized
    [Test]
    public void TryReadChannelNormalized_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(-1, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelNormalized_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(3, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(4, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(4, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(3, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(4, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(1, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(1, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(1, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(2, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannelNormalized(3, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelNormalized_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(2, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(3, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(3, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(2, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(3, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(1, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(2, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelNormalized_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSrgba()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 4 = 4
        1,  2,  3,  4, 33, 34, 35, 36, 65, 66, 67, 68, 0, 0, 0, 0,
        9, 10, 11, 12, 41, 42, 43, 44, 73, 74, 75, 76, 0, 0, 0, 0,
      };
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, false));
        AssertNormalized(new byte[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, true));
        AssertNormalized(new byte[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, false));
        AssertNormalized(new byte[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, true));
        AssertNormalized(new byte[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, false, false));
        AssertNormalized(new byte[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, false, true));
        AssertNormalized(new byte[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, true, false));
        AssertNormalized(new byte[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, true, true));
        AssertNormalized(new byte[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, false, false));
        AssertNormalized(new byte[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, false, true));
        AssertNormalized(new byte[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, true, false));
        AssertNormalized(new byte[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, true, true));
        AssertNormalized(new byte[] { 67, 35, 3, 75, 43, 11 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, false, false));
        AssertNormalized(new byte[] { 12, 44, 76, 4, 36, 68 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, false, true));
        AssertNormalized(new byte[] { 4, 36, 68, 12, 44, 76 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, true, false));
        AssertNormalized(new byte[] { 76, 44, 12, 68, 36, 4 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, true, true));
        AssertNormalized(new byte[] { 68, 36, 4, 76, 44, 12 }, result);
      }
    }

    [Test]
    public void TryReadChannelNormalized_ShouldReadTheSpecifiedChannelData_When_TheFormatIsGray8()
    {
      var bytes = new byte[] {
        // padding is 16 - 3 * 1 = 13
        1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, false));
        AssertNormalized(new byte[] { 4, 5, 6, 1, 2, 3 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, true));
        AssertNormalized(new byte[] { 1, 2, 3, 4, 5, 6 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, false));
        AssertNormalized(new byte[] { 6, 5, 4, 3, 2, 1 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, true));
        AssertNormalized(new byte[] { 3, 2, 1, 6, 5, 4 }, result);
      }
    }

    [Test]
    public void TryReadChannelNormalized_ShouldReadTheSpecifiedChannelData_When_TheFormatIsSrgba64()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 24 - 2 * 3 * 4 = 0
        1, 0,  2, 0,  3, 0,  4, 0, 33, 0, 34, 0, 35, 0, 36, 0, 65, 0, 66, 0, 67, 0, 68, 0,
        9, 0, 10, 0, 11, 0, 12, 0, 41, 0, 42, 0, 43, 0, 44, 0, 73, 0, 74, 0, 75, 0, 76, 0,
      };
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 3, 2, 24, bytes))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, false));
        AssertNormalized(new ushort[] { 9, 41, 73, 1, 33, 65 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, true));
        AssertNormalized(new ushort[] { 1, 33, 65, 9, 41, 73 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, false));
        AssertNormalized(new ushort[] { 73, 41, 9, 65, 33, 1 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, true));
        AssertNormalized(new ushort[] { 65, 33, 1, 73, 41, 9 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, false, false));
        AssertNormalized(new ushort[] { 10, 42, 74, 2, 34, 66 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, false, true));
        AssertNormalized(new ushort[] { 2, 34, 66, 10, 42, 74 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, true, false));
        AssertNormalized(new ushort[] { 74, 42, 10, 66, 34, 2 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(1, result, true, true));
        AssertNormalized(new ushort[] { 66, 34, 2, 74, 42, 10 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, false, false));
        AssertNormalized(new ushort[] { 11, 43, 75, 3, 35, 67 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, false, true));
        AssertNormalized(new ushort[] { 3, 35, 67, 11, 43, 75 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, true, false));
        AssertNormalized(new ushort[] { 75, 43, 11, 67, 35, 3 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(2, result, true, true));
        AssertNormalized(new ushort[] { 67, 35, 3, 75, 43, 11 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, false, false));
        AssertNormalized(new ushort[] { 12, 44, 76, 4, 36, 68 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, false, true));
        AssertNormalized(new ushort[] { 4, 36, 68, 12, 44, 76 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, true, false));
        AssertNormalized(new ushort[] { 76, 44, 12, 68, 36, 4 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(3, result, true, true));
        AssertNormalized(new ushort[] { 68, 36, 4, 76, 44, 12 }, result);
      }
    }

    [Test]
    public void TryReadChannelNormalized_ShouldReadTheSpecifiedChannelData_When_TheFormatIsGray16()
    {
      var bytes = new byte[] {
        // padding is 16 - 2 * 3 = 10
        1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        4, 0, 5, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
      };
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, false));
        AssertNormalized(new ushort[] { 4, 5, 6, 1, 2, 3 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, false, true));
        AssertNormalized(new ushort[] { 1, 2, 3, 4, 5, 6 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, false));
        AssertNormalized(new ushort[] { 6, 5, 4, 3, 2, 1 }, result);

        Assert.True(imageFrame.TryReadChannelNormalized(0, result, true, true));
        AssertNormalized(new ushort[] { 3, 2, 1, 6, 5, 4 }, result);
      }
    }
    #endregion

    #region TryReadPixelData
    [Test]
    public void TryReadPixelData_ShouldReturnFalse_When_TheFormatIsInvalid()
    {
      using (var imageFrame = new ImageFrame())
      {
        Assert.False(imageFrame.TryReadPixelData(new Color32[] { }));
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnFalse_When_ColorsLengthIsWrong()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadPixelData(new Color32[1] { new Color32() }));
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsSrgb()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 3 = 7
        1,  2,  3, 33, 34, 35, 65, 66, 67, 0, 0, 0, 0, 0, 0, 0,
        9, 10, 11, 41, 42, 43, 73, 74, 75, 0, 0, 0, 0, 0, 0, 0,
      };
      var expected = new Color32[] {
        new Color32(9, 10, 11, 255), new Color32(41, 42, 43, 255), new Color32(73, 74, 75, 255),
        new Color32(1, 2, 3, 255), new Color32(33, 34, 35, 255), new Color32(65, 66, 67, 255),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsSrgba()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 4 = 4
        1,  2,  3,  4, 33, 34, 35, 36, 65, 66, 67, 68, 0, 0, 0, 0,
        9, 10, 11, 12, 41, 42, 43, 44, 73, 74, 75, 76, 0, 0, 0, 0,
      };
      var expected = new Color32[] {
        new Color32(9, 10, 11, 12), new Color32(41, 42, 43, 44), new Color32(73, 74, 75, 76),
        new Color32(1, 2, 3, 4), new Color32(33, 34, 35, 36), new Color32(65, 66, 67, 68),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsSbgra()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 16 - 3 * 4 = 4
        1,  2,  3,  4, 33, 34, 35, 36, 65, 66, 67, 68, 0, 0, 0, 0,
        9, 10, 11, 12, 41, 42, 43, 44, 73, 74, 75, 76, 0, 0, 0, 0,
      };
      var expected = new Color32[] {
        new Color32(11, 10, 9, 12), new Color32(43, 42, 41, 44), new Color32(75, 74, 73, 76),
        new Color32(3, 2, 1, 4), new Color32(35, 34, 33, 36), new Color32(67, 66, 65, 68),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsGray8()
    {
      var bytes = new byte[] {
        // padding is 16 - 3 * 1 = 13
        1, 2, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
      };
      var expected = new Color32[] {
        new Color32(4, 4, 4, 255), new Color32(5, 5, 5, 255), new Color32(6, 6, 6, 255),
        new Color32(1, 1, 1, 255), new Color32(2, 2, 2, 255), new Color32(3, 3, 3, 255),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsLab8()
    {
      var bytes = new byte[] {
        // padding is 16 - 3 * 1 = 13
        0, 0, 0, 0, 128, 128, 0, 128, 127, 0, 127, 128, 0, 127, 127, 0,
        50, 0, 0, 50, 128, 128, 50, 128, 127, 50, 127, 128, 50, 127, 127, 0,
        100, 0, 0, 100, 128, 128, 100, 128, 127, 100, 127, 128, 100, 127, 127, 0,
        69, 10, 30, 62, 207, 87, 27, 241, 100, 12, 79, 78, 36, 70, 2, 0, // random
      };
      var expected = new Color32[] {
        new Color32(204, 161, 115, 255), new Color32(93, 169, 0, 255), new Color32(71, 68, 0, 255), new Color32(122, 0, 0, 255), new Color32(178, 0, 84, 255),
        new Color32(255, 255, 255, 255), new Color32(0, 255, 255, 255), new Color32(0, 255, 0, 255), new Color32(255, 139, 255, 255), new Color32(255, 70, 0, 255),
        new Color32(119, 119, 119, 255), new Color32(0, 169, 255, 255), new Color32(0, 152, 0, 255), new Color32(183, 0, 255, 255), new Color32(255, 0, 0, 255),
        new Color32(0, 0, 0, 255), new Color32(0, 64, 194, 255), new Color32(0, 45, 0, 255), new Color32(0, 0, 195, 255), new Color32(132, 0, 0, 255),
      };
      var result = new Color32[20];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 5, 4, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsSrgb48()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 24 - 2 * 3 * 3 = 6
        0, 1, 0,  2, 0,  3, 0, 33, 0, 34, 0, 35, 0, 65, 0, 66, 0, 67, 0, 0, 0, 0, 0, 0,
        0, 9, 0, 10, 0, 11, 0, 41, 0, 42, 0, 43, 0, 73, 0, 74, 0, 75, 0, 0, 0, 0, 0, 0,
      };
      var expected = new Color32[] {
        new Color32(9, 10, 11, 255), new Color32(41, 42, 43, 255), new Color32(73, 74, 75, 255),
        new Color32(1, 2, 3, 255), new Color32(33, 34, 35, 255), new Color32(65, 66, 67, 255),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 3, 2, 24, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsSrgba64()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var bytes = new byte[] {
        // padding is 24 - 2 * 3 * 4 = 0
        0, 1, 0,  2, 0,  3, 0,  4, 0, 33, 0, 34, 0, 35, 0, 36, 0, 65, 0, 66, 0, 67, 0, 68,
        0, 9, 0, 10, 0, 11, 0, 12, 0, 41, 0, 42, 0, 43, 0, 44, 0, 73, 0, 74, 0, 75, 0, 76,
      };
      var expected = new Color32[] {
        new Color32(9, 10, 11, 12), new Color32(41, 42, 43, 44), new Color32(73, 74, 75, 76),
        new Color32(1, 2, 3, 4), new Color32(33, 34, 35, 36), new Color32(65, 66, 67, 68),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 3, 2, 24, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsGray16()
    {
      var bytes = new byte[] {
        // padding is 16 - 2 * 3 = 10
        0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 4, 0, 5, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
      };
      var expected = new Color32[] {
        new Color32(4, 4, 4, 255), new Color32(5, 5, 5, 255), new Color32(6, 6, 6, 255),
        new Color32(1, 1, 1, 255), new Color32(2, 2, 2, 255), new Color32(3, 3, 3, 255),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }

    [Test]
    public void TryReadPixelData_ShouldReturnTrue_When_TheFormatIsVec32f1()
    {
      var floats = new float[] {
        // padding is 16 - 3 * 4 = 4
        1.0f / 255, 2.0f / 255, 3.0f / 255, 0,
        4.0f / 255, 5.0f / 255, 6.0f / 255, 0,
      };
      var bytes = FloatsToBytes(floats);
      var expected = new Color32[] {
        new Color32(4, 4, 4, 255), new Color32(5, 5, 5, 255), new Color32(6, 6, 6, 255),
        new Color32(1, 1, 1, 255), new Color32(2, 2, 2, 255), new Color32(3, 3, 3, 255),
      };
      var result = new Color32[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadPixelData(result));
        Assert.AreEqual(expected, result);
      }
    }
    #endregion

    private ImageFrame BuildImageFrame(ImageFormat.Types.Format format, int width, int height, int widthStep, byte[] pixelData)
    {
      var array = new NativeArray<byte>(pixelData.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      array.CopyFrom(pixelData);

      return new ImageFrame(format, width, height, widthStep, array);
    }

    private byte[] FloatsToBytes(float[] array)
    {
      var bytes = new byte[array.Length * 4];

      unsafe
      {
        fixed (float* pArray = array)
        {
          var pByte = (byte*)pArray;
          for (var i = 0; i < 4 * array.Length; i++)
          {
            bytes[i] = *pByte++;
          }
        }
      }
      return bytes;
    }

    private void AssertNormalized(byte[] expectedUnnormalized, float[] result)
    {
      Assert.True(result.All((v) => v >= 0.0f && v <= 1.0f));
      AreAlmostEqual(expectedUnnormalized.Select((v) => (float)v / 255).ToArray(), result, 1e-6);
    }

    private void AssertNormalized(ushort[] expectedUnnormalized, float[] result)
    {
      Assert.True(result.All((v) => v >= 0.0f && v <= 1.0f));
      AreAlmostEqual(expectedUnnormalized.Select((v) => (float)v / 65525).ToArray(), result, 1e-6);
    }

    private void AreAlmostEqual(float[] expected, float[] actual, double threshold)
    {
      Assert.AreEqual(expected.Length, actual.Length);
      Assert.True(expected.Zip(actual, (x, y) => x - y).All((diff) => Mathf.Abs(diff) < threshold));
    }
  }
}
