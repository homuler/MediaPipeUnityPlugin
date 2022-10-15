// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class MatrixPacketTest
  {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValue()
    {
      var matrix = CreateMatrixInputData();
      using (var packet = new MatrixPacket(matrix))
      {
        Assert.True(packet.ValidateAsType().Ok());
        Assert.AreEqual(matrix, packet.Get());
        Assert.AreEqual(Timestamp.Unset(), packet.Timestamp());
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var packet = new MatrixPacket())
      {
        Assert.False(packet.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var packet = new MatrixPacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #At
    [Test]
    public void At_ShouldReturnNewPacketWithTimestamp()
    {
      using (var timestamp = new Timestamp(1))
      {
        var matrix = CreateMatrixInputData();
        var packet = new MatrixPacket(matrix).At(timestamp);
        Assert.AreEqual(matrix, packet.Get());
        Assert.AreEqual(timestamp, packet.Timestamp());

        using (var newTimestamp = new Timestamp(2))
        {
          var newPacket = packet.At(newTimestamp);
          Assert.AreEqual(matrix, newPacket.Get());
          Assert.AreEqual(newTimestamp, newPacket.Timestamp());
        }

        Assert.AreEqual(timestamp, packet.Timestamp());
      }
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldThrowNotSupportedException()
    {
      using (var packet = new MatrixPacket())
      {
#pragma warning disable IDE0058
        Assert.Throws<NotSupportedException>(() => { packet.Consume(); });
#pragma warning restore IDE0058
      }
    }
    #endregion

    #region #ValidateAsType
    [Test]
    public void ValidateAsType_ShouldReturnOk_When_ValueIsSet()
    {
      using (var packet = new MatrixPacket(CreateMatrixInputData()))
      {
        Assert.True(packet.ValidateAsType().Ok());
      }
    }
    #endregion

    private static MatrixData CreateMatrixInputData()
    {
      var matrix = new MatrixData();
      matrix.PackedData.Add(0);
      matrix.PackedData.Add(1);
      matrix.PackedData.Add(2);
      matrix.PackedData.Add(3);
      matrix.PackedData.Add(4);
      matrix.PackedData.Add(5);

      matrix.Rows = 2;
      matrix.Cols = 3;
      return matrix;
    }
  }
}
