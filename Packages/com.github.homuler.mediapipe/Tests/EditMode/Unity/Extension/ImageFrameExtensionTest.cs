// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using Mediapipe.Unity;

using NUnit.Framework;
using Unity.Collections;

namespace Tests
{
  public class ImageFrameExtensionTest
  {
    #region TryReadChannel(byte)
    [Test]
    public void TryReadChannelByte_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(-1, true, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReturnFalse_When_TheChannelDataIsNotStoredInBytes()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(1, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, true, new byte[] { }));
      }
    }

    [Test]
    public void TryReadChannelByte_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(2, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(3, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(3, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(0, true, new byte[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(2, true, new byte[] { }));
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new byte[] { 9, 41, 73, 1, 33, 65 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new byte[] { 73, 41, 9, 65, 33, 1 });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new byte[] { 10, 42, 74, 2, 34, 66 });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new byte[] { 74, 42, 10, 66, 34, 2 });

        Assert.True(imageFrame.TryReadChannel(2, false, result));
        Assert.AreEqual(result, new byte[] { 11, 43, 75, 3, 35, 67 });

        Assert.True(imageFrame.TryReadChannel(2, true, result));
        Assert.AreEqual(result, new byte[] { 75, 43, 11, 67, 35, 3 });
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new byte[] { 9, 41, 73, 1, 33, 65 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new byte[] { 73, 41, 9, 65, 33, 1 });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new byte[] { 10, 42, 74, 2, 34, 66 });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new byte[] { 74, 42, 10, 66, 34, 2 });

        Assert.True(imageFrame.TryReadChannel(2, false, result));
        Assert.AreEqual(result, new byte[] { 11, 43, 75, 3, 35, 67 });

        Assert.True(imageFrame.TryReadChannel(2, true, result));
        Assert.AreEqual(result, new byte[] { 75, 43, 11, 67, 35, 3 });

        Assert.True(imageFrame.TryReadChannel(3, false, result));
        Assert.AreEqual(result, new byte[] { 12, 44, 76, 4, 36, 68 });

        Assert.True(imageFrame.TryReadChannel(3, true, result));
        Assert.AreEqual(result, new byte[] { 76, 44, 12, 68, 36, 4 });
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new byte[] { 9, 41, 73, 1, 33, 65 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new byte[] { 73, 41, 9, 65, 33, 1 });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new byte[] { 10, 42, 74, 2, 34, 66 });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new byte[] { 74, 42, 10, 66, 34, 2 });

        Assert.True(imageFrame.TryReadChannel(2, false, result));
        Assert.AreEqual(result, new byte[] { 11, 43, 75, 3, 35, 67 });

        Assert.True(imageFrame.TryReadChannel(2, true, result));
        Assert.AreEqual(result, new byte[] { 75, 43, 11, 67, 35, 3 });

        Assert.True(imageFrame.TryReadChannel(3, false, result));
        Assert.AreEqual(result, new byte[] { 12, 44, 76, 4, 36, 68 });

        Assert.True(imageFrame.TryReadChannel(3, true, result));
        Assert.AreEqual(result, new byte[] { 76, 44, 12, 68, 36, 4 });
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new byte[] { 4, 5, 6, 1, 2, 3 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new byte[] { 6, 5, 4, 3, 2, 1 });
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new byte[] { 9, 41, 73, 1, 33, 65 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new byte[] { 73, 41, 9, 65, 33, 1 });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new byte[] { 10, 42, 74, 2, 34, 66 });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new byte[] { 74, 42, 10, 66, 34, 2 });

        Assert.True(imageFrame.TryReadChannel(2, false, result));
        Assert.AreEqual(result, new byte[] { 11, 43, 75, 3, 35, 67 });

        Assert.True(imageFrame.TryReadChannel(2, true, result));
        Assert.AreEqual(result, new byte[] { 75, 43, 11, 67, 35, 3 });
      }
    }
    #endregion

    #region TryReadChannel(ushort)
    [Test]
    public void TryReadChannelUshort_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(-1, true, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReturnFalse_When_TheChannelDataIsNotStoredInUshorts()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(1, true, new ushort[] { }));
      }
    }

    [Test]
    public void TryReadChannelUshort_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(2, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(3, true, new ushort[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(0, true, new ushort[] { }));
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new ushort[] { 9, 41, 73, 1, 33, 65 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new ushort[] { 73, 41, 9, 65, 33, 1 });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new ushort[] { 10, 42, 74, 2, 34, 66 });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new ushort[] { 74, 42, 10, 66, 34, 2 });

        Assert.True(imageFrame.TryReadChannel(2, false, result));
        Assert.AreEqual(result, new ushort[] { 11, 43, 75, 3, 35, 67 });

        Assert.True(imageFrame.TryReadChannel(2, true, result));
        Assert.AreEqual(result, new ushort[] { 75, 43, 11, 67, 35, 3 });
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new ushort[] { 9, 41, 73, 1, 33, 65 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new ushort[] { 73, 41, 9, 65, 33, 1 });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new ushort[] { 10, 42, 74, 2, 34, 66 });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new ushort[] { 74, 42, 10, 66, 34, 2 });

        Assert.True(imageFrame.TryReadChannel(2, false, result));
        Assert.AreEqual(result, new ushort[] { 11, 43, 75, 3, 35, 67 });

        Assert.True(imageFrame.TryReadChannel(2, true, result));
        Assert.AreEqual(result, new ushort[] { 75, 43, 11, 67, 35, 3 });

        Assert.True(imageFrame.TryReadChannel(3, false, result));
        Assert.AreEqual(result, new ushort[] { 12, 44, 76, 4, 36, 68 });

        Assert.True(imageFrame.TryReadChannel(3, true, result));
        Assert.AreEqual(result, new ushort[] { 76, 44, 12, 68, 36, 4 });
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
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new ushort[] { 4, 5, 6, 1, 2, 3 });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new ushort[] { 6, 5, 4, 3, 2, 1 });
      }
    }
    #endregion

    #region TryReadChannel(float)
    [Test]
    public void TryReadChannelFloat_ShouldReturnFalse_When_ChannelNumberIsNegative()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(-1, true, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReturnFalse_When_TheChannelDataIsNotStoredInFloats()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Sbgra, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgb48, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(3, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Srgba64, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(4, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Gray16, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Lab8, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(0, true, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReturnFalse_When_ChannelNumberEqualsTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(1, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.False(imageFrame.TryReadChannel(2, true, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReturnTrue_When_ChannelNumberIsLessThanTheNumberOfChannels()
    {
      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(0, true, new float[] { }));
      }

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 0, 0, 0, new byte[] { }))
      {
        Assert.True(imageFrame.TryReadChannel(1, true, new float[] { }));
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReadTheSpecifiedChannelData_When_TheFormatIsVec32F1()
    {
      var floats = new float[] {
        // padding is 16 - 3 * 4 = 4
        1.0f, 2.0f, 3.0f, 0,
        4.0f, 5.0f, 6.0f, 0,
      };
      var bytes = FloatsToBytes(floats);
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F1, 3, 2, 16, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new float[] { 4.0f, 5.0f, 6.0f, 1.0f, 2.0f, 3.0f });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new float[] { 6.0f, 5.0f, 4.0f, 3.0f, 2.0f, 1.0f });
      }
    }

    [Test]
    public void TryReadChannelFloat_ShouldReadTheSpecifiedChannelData_When_TheFormatIsVec32F2()
    {
      // (w, h, c) -> w << 5 + h << 3 + (c + 1)
      var floats = new float[] {
        // padding is 32 - 2 * 3 * 4 = 8
        1.0f,  2.0f, 33.0f, 34.0f, 65.0f, 66.0f, 0, 0,
        9.0f, 10.0f, 41.0f, 42.0f, 73.0f, 74.0f, 0, 0,
      };
      var bytes = FloatsToBytes(floats);
      var result = new float[6];

      using (var imageFrame = BuildImageFrame(ImageFormat.Types.Format.Vec32F2, 3, 2, 32, bytes))
      {
        Assert.True(imageFrame.TryReadChannel(0, false, result));
        Assert.AreEqual(result, new float[] { 9.0f, 41.0f, 73.0f, 1.0f, 33.0f, 65.0f });

        Assert.True(imageFrame.TryReadChannel(0, true, result));
        Assert.AreEqual(result, new float[] { 73.0f, 41.0f, 9.0f, 65.0f, 33.0f, 1.0f });

        Assert.True(imageFrame.TryReadChannel(1, false, result));
        Assert.AreEqual(result, new float[] { 10.0f, 42.0f, 74.0f, 2.0f, 34.0f, 66.0f });

        Assert.True(imageFrame.TryReadChannel(1, true, result));
        Assert.AreEqual(result, new float[] { 74.0f, 42.0f, 10.0f, 66.0f, 34.0f, 2.0f });
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
  }
}
