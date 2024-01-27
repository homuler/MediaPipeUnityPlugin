// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Unity.Collections;

namespace Mediapipe.Tests
{
  public class PacketTest
  {
    #region Bool
    [TestCase(true)]
    [TestCase(false)]
    public void CreateBool_ShouldReturnNewBoolPacket(bool value)
    {
      using var packet = Packet.CreateBool(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(true)]
    [TestCase(false)]
    public void CreateBoolAt_ShouldReturnNewBoolPacket(bool value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateBoolAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region BoolVector
    [Test]
    public void CreateBoolVector_ShouldReturnNewBoolVectorPacket()
    {
      var value = new bool[] { true, false };
      using var packet = Packet.CreateBoolVector(value);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get();
      Assert.AreEqual(value.Length, result.Count);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateBoolVectorAt_ShouldReturnNewBoolVectorPacket()
    {
      var value = new bool[] { true, false };
      var timestamp = 1;
      using var packet = Packet.CreateBoolVectorAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get();
      Assert.AreEqual(value.Length, result.Count);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Double
    [TestCase(double.MaxValue)]
    [TestCase(0d)]
    [TestCase(double.MinValue)]
    public void CreateDouble_ShouldReturnNewDoublePacket(double value)
    {
      using var packet = Packet.CreateDouble(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(double.MaxValue)]
    [TestCase(0d)]
    [TestCase(double.MinValue)]
    public void CreateDoubleAt_ShouldReturnNewDoublePacket(double value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateDoubleAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Float
    [TestCase(float.MaxValue)]
    [TestCase(0f)]
    [TestCase(float.MinValue)]
    public void CreateFloat_ShouldReturnNewFloatPacket(float value)
    {
      using var packet = Packet.CreateFloat(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(float.MaxValue)]
    [TestCase(0f)]
    [TestCase(float.MinValue)]
    public void CreateFloatAt_ShouldReturnNewFloatPacket(float value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateFloatAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region FloatArray
    [Test]
    public void CreateFloatArray_ShouldReturnNewFloatArrayPacket()
    {
      var value = new float[] { float.MinValue, 0f, float.MaxValue };
      using var packet = Packet.CreateFloatArray(value);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get(value.Length);
      Assert.AreEqual(value.Length, result.Length);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateFloatArrayAt_ShouldReturnNewFloatArrayPacket()
    {
      var value = new float[] { float.MinValue, 0f, float.MaxValue };
      var timestamp = 1;
      using var packet = Packet.CreateFloatArrayAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get(value.Length);
      Assert.AreEqual(value.Length, result.Length);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region FloatVector
    [Test]
    public void CreateFloatVector_ShouldReturnNewFloatVectorPacket()
    {
      var value = new float[] { float.MinValue, 0f, float.MaxValue };
      using var packet = Packet.CreateFloatVector(value);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get();
      Assert.AreEqual(value.Length, result.Count);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateFloatVectorAt_ShouldReturnNewFloatVectorPacket()
    {
      var value = new float[] { float.MinValue, 0f, float.MaxValue };
      var timestamp = 1;
      using var packet = Packet.CreateFloatVectorAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get();
      Assert.AreEqual(value.Length, result.Count);
      for (var i = 0; i < value.Length; i++)
      {
        Assert.AreEqual(value[i], result[i]);
      }
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Image
    [Test]
    public void CreateImage_ShouldReturnNewImagePacket()
    {
      var bytes = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
      var image = BuildSRGBAImage(bytes, 4, 2);
      using var packet = Packet.CreateImage(image);

      Assert.DoesNotThrow(packet.Validate);

      using (var result = packet.Get())
      {
        AssertImage(result, 4, 2, ImageFormat.Types.Format.Srgba, bytes);
      }

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateImageAt_ShouldReturnNewImagePacket()
    {
      var bytes = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
      var timestamp = 1;
      var image = BuildSRGBAImage(bytes, 4, 2);
      using var packet = Packet.CreateImageAt(image, timestamp);

      Assert.DoesNotThrow(packet.Validate);

      using (var result = packet.Get())
      {
        AssertImage(result, 4, 2, ImageFormat.Types.Format.Srgba, bytes);
      }

      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region ImageFrame
    [Test]
    public void CreateImageFrame_ShouldReturnNewImageFramePacket()
    {
      var bytes = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
      var imageFrame = BuildSRGBAImageFrame(bytes, 4, 2);
      using var packet = Packet.CreateImageFrame(imageFrame);

      Assert.DoesNotThrow(packet.Validate);

      using (var result = packet.Get())
      {
        AssertImageFrame(result, 4, 2, ImageFormat.Types.Format.Srgba, bytes);
      }

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateImageFrameAt_ShouldReturnNewImageFramePacket()
    {
      var bytes = Enumerable.Range(0, 32).Select(x => (byte)x).ToArray();
      var timestamp = 1;
      var imageFrame = BuildSRGBAImageFrame(bytes, 4, 2);
      using var packet = Packet.CreateImageFrameAt(imageFrame, timestamp);

      Assert.DoesNotThrow(packet.Validate);

      using (var result = packet.Get())
      {
        AssertImageFrame(result, 4, 2, ImageFormat.Types.Format.Srgba, bytes);
      }

      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Int
    [TestCase(int.MaxValue)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    public void CreateInt_ShouldReturnNewIntPacket(int value)
    {
      using var packet = Packet.CreateInt(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(int.MaxValue)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    public void CreateIntAt_ShouldReturnNewIntPacket(int value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateIntAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Matrix
    [Test]
    public void CreateColMajorMatrix_ShouldReturnNewMatrixPacket()
    {
      var value = new Matrix(new float[] { 1, 2, 3, 4, 5, 6 }, 2, 3);
      using var packet = Packet.CreateColMajorMatrix(value);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get();
      Assert.AreEqual(value.data, result.data);
      Assert.AreEqual(value.rows, result.rows);
      Assert.AreEqual(value.cols, result.cols);
      Assert.AreEqual(value.layout, result.layout);

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateColMajorMatrixAt_ShouldReturnNewMatrixPacket()
    {
      var value = new Matrix(new float[] { 1, 2, 3, 4, 5, 6 }, 2, 3);
      var timestamp = 1;
      using var packet = Packet.CreateColMajorMatrixAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);

      var result = packet.Get();
      Assert.AreEqual(value.data, result.data);
      Assert.AreEqual(value.rows, result.rows);
      Assert.AreEqual(value.cols, result.cols);
      Assert.AreEqual(value.layout, result.layout);

      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region Proto
    [Test]
    public void CreateProto_ShouldReturnNewProtoPacket()
    {
      var value = new NormalizedRect()
      {
        Rotation = 0,
        XCenter = 0.5f,
        YCenter = 0.5f,
        Width = 1,
        Height = 1,
      };
      using var packet = Packet.CreateProto(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get(NormalizedRect.Parser));

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateProtoAt_ShouldReturnNewProtoPacket()
    {
      var timestamp = 1;
      var value = new NormalizedRect()
      {
        Rotation = 0,
        XCenter = 0.5f,
        YCenter = 0.5f,
        Width = 1,
        Height = 1,
      };
      using var packet = Packet.CreateProtoAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get(NormalizedRect.Parser));
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion


    #region String
    [Test]
    public void CreateString_ShouldReturnNewStringPacket_When_ValueIsNullString()
    {
      using var packet = Packet.CreateString((string)null);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateString_ShouldReturnNewStringPacket_When_ValueIsEmptyString()
    {
      using var packet = Packet.CreateString("");

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase("hello")]
    public void CreateString_ShouldReturnNewStringPacket_When_StringIsGiven(string value)
    {
      using var packet = Packet.CreateString(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateString_ShouldReturnNewStringPacket_When_ValueIsNullArray()
    {
      using var packet = Packet.CreateString((byte[])null);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.GetBytes());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateString_ShouldReturnNewStringPacket_When_ValueIsEmptyArray()
    {
      var value = new byte[] { };
      using var packet = Packet.CreateString(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.GetBytes());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateString_ShouldReturnNewStringPacket_When_ByteArrayIsGiven()
    {
      var value = new byte[] { 1, 2, 3 };
      using var packet = Packet.CreateString(value);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.GetBytes());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateStringAt_ShouldReturnNewStringPacket_When_ValueIsNullString()
    {
      var timestamp = 1;
      using var packet = Packet.CreateStringAt((string)null, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateStringAt_ShouldReturnNewStringPacket_When_ValueIsEmptyString()
    {
      var timestamp = 1;
      using var packet = Packet.CreateStringAt("", timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }

    [TestCase("hello")]
    public void CreateStringAt_ShouldReturnNewStringPacket_When_StringIsGiven(string value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateStringAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.Get());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateStringAt_ShouldReturnNewStringPacket_When_ValueIsNullArray()
    {
      var timestamp = 1;
      using var packet = Packet.CreateStringAt((byte[])null, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.GetBytes());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateStringAt_ShouldReturnNewStringPacket_When_ValueIsEmptyArray()
    {
      var timestamp = 1;
      var value = new byte[] { };
      using var packet = Packet.CreateStringAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.IsEmpty(packet.GetBytes());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }

    [Test]
    public void CreateStringAt_ShouldReturnNewStringPacket_When_ByteArrayIsGiven()
    {
      var timestamp = 1;
      var value = new byte[] { 1, 2, 3 };
      using var packet = Packet.CreateStringAt(value, timestamp);

      Assert.DoesNotThrow(packet.Validate);
      Assert.AreEqual(value, packet.GetBytes());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region #Validate
    [Test]
    public void ValidateAsBool_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<bool>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }

    [Test]
    public void ValidateAsBoolVector_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<List<bool>>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }

    [Test]
    public void ValidateAsDouble_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<double>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }

    [Test]
    public void ValidateAsFloat_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<float>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }

    [Test]
    public void ValidateAsFloatArray_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<float[]>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }

    [Test]
    public void ValidateAsFloatVector_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<List<float>>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }

    [Test]
    public void ValidateAsImage_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = new Packet<Image>();
      _ = Assert.Throws<BadStatusException>(packet.Validate);
    }
    #endregion

    private Image BuildSRGBAImage(byte[] bytes, int width, int height)
    {
      Assert.AreEqual(bytes.Length, width * height * 4);

      var pixelData = new NativeArray<byte>(bytes.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      pixelData.CopyFrom(bytes);

      return new Image(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData);
    }

    private ImageFrame BuildSRGBAImageFrame(byte[] bytes, int width, int height)
    {
      Assert.AreEqual(bytes.Length, width * height * 4);

      var pixelData = new NativeArray<byte>(bytes.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      pixelData.CopyFrom(bytes);

      return new ImageFrame(ImageFormat.Types.Format.Srgba, width, height, width * 4, pixelData);
    }

    private void AssertImage(Image image, int width, int height, ImageFormat.Types.Format format, byte[] expectedBytes)
    {
      Assert.AreEqual(width, image.Width());
      Assert.AreEqual(height, image.Height());
      Assert.AreEqual(format, image.ImageFormat());

      using (var pixelLock = new PixelWriteLock(image))
      {
        var pixelData = new byte[width * height * format.NumberOfChannels()];
        Marshal.Copy(pixelLock.Pixels(), pixelData, 0, pixelData.Length);

        Assert.AreEqual(expectedBytes, pixelData);
      }
    }

    private void AssertImageFrame(ImageFrame imageFrame, int width, int height, ImageFormat.Types.Format format, byte[] expectedBytes)
    {
      Assert.AreEqual(width, imageFrame.Width());
      Assert.AreEqual(height, imageFrame.Height());
      Assert.AreEqual(format, imageFrame.Format());

      var pixelData = new byte[width * height * format.NumberOfChannels()];
      Marshal.Copy(imageFrame.MutablePixelData(), pixelData, 0, pixelData.Length);

      Assert.AreEqual(expectedBytes, pixelData);
    }
  }
}
