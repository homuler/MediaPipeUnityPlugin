// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

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

      Assert.DoesNotThrow(packet.ValidateAsBool);
      Assert.AreEqual(value, packet.GetBool());

      using var unsetTimestamp = Timestamp.Unset();
      Assert.AreEqual(unsetTimestamp.Microseconds(), packet.TimestampMicroseconds());
    }

    [TestCase(true)]
    [TestCase(false)]
    public void CreateBoolAt_ShouldReturnNewBoolPacket(bool value)
    {
      var timestamp = 1;
      using var packet = Packet.CreateBoolAt(value, timestamp);

      Assert.DoesNotThrow(packet.ValidateAsBool);
      Assert.AreEqual(value, packet.GetBool());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region BoolVector
    [Test]
    public void CreateBoolVector_ShouldReturnNewBoolVectorPacket()
    {
      var value = new bool[] { true, false };
      using var packet = Packet.CreateBoolVector(value);

      Assert.DoesNotThrow(packet.ValidateAsBoolVector);

      var result = packet.GetBoolList();
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

      Assert.DoesNotThrow(packet.ValidateAsBoolVector);

      var result = packet.GetBoolList();
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

      Assert.DoesNotThrow(packet.ValidateAsDouble);
      Assert.AreEqual(value, packet.GetDouble());

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

      Assert.DoesNotThrow(packet.ValidateAsDouble);
      Assert.AreEqual(value, packet.GetDouble());
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

      Assert.DoesNotThrow(packet.ValidateAsFloat);
      Assert.AreEqual(value, packet.GetFloat());

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

      Assert.DoesNotThrow(packet.ValidateAsFloat);
      Assert.AreEqual(value, packet.GetFloat());
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region FloatArray
    [Test]
    public void CreateFloatArray_ShouldReturnNewFloatArrayPacket()
    {
      var value = new float[] { float.MinValue, 0f, float.MaxValue };
      using var packet = Packet.CreateFloatArray(value);

      Assert.DoesNotThrow(packet.ValidateAsFloatArray);

      var result = packet.GetFloatArray(value.Length);
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

      Assert.DoesNotThrow(packet.ValidateAsFloatArray);

      var result = packet.GetFloatArray(value.Length);
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

      Assert.DoesNotThrow(packet.ValidateAsFloatVector);

      var result = packet.GetFloatList();
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

      Assert.DoesNotThrow(packet.ValidateAsFloatVector);

      var result = packet.GetFloatList();
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

      Assert.DoesNotThrow(packet.ValidateAsImage);

      using (var result = packet.GetImage())
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

      Assert.DoesNotThrow(packet.ValidateAsImage);

      using (var result = packet.GetImage())
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

      Assert.DoesNotThrow(packet.ValidateAsImageFrame);

      using (var result = packet.GetImageFrame())
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

      Assert.DoesNotThrow(packet.ValidateAsImageFrame);

      using (var result = packet.GetImageFrame())
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

      Assert.DoesNotThrow(packet.ValidateAsInt);
      Assert.AreEqual(value, packet.GetInt());

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

      Assert.DoesNotThrow(packet.ValidateAsInt);
      Assert.AreEqual(value, packet.GetInt());
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

      Assert.DoesNotThrow(packet.ValidateAsProtoMessageLite);
      Assert.AreEqual(value, packet.GetProto(NormalizedRect.Parser));

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

      Assert.DoesNotThrow(packet.ValidateAsProtoMessageLite);
      Assert.AreEqual(value, packet.GetProto(NormalizedRect.Parser));
      Assert.AreEqual(timestamp, packet.TimestampMicroseconds());
    }
    #endregion

    #region #Validate
    [Test]
    public void ValidateAsBool_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsBool);
    }

    [Test]
    public void ValidateAsBoolVector_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsBoolVector);
    }

    [Test]
    public void ValidateAsDouble_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsDouble);
    }

    [Test]
    public void ValidateAsFloat_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsFloat);
    }

    [Test]
    public void ValidateAsFloatArray_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsFloatArray);
    }

    [Test]
    public void ValidateAsFloatVector_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsFloatVector);
    }

    [Test]
    public void ValidateAsImage_ShouldThrow_When_ValueIsNotSet()
    {
      using var packet = Packet.CreateEmpty();
      _ = Assert.Throws<BadStatusException>(packet.ValidateAsImage);
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
