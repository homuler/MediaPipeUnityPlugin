// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using NUnit.Framework;

namespace Mediapipe.Tests.Util
{
  public class GlobalInstanceTableTest
  {
    private class Value
    {
      public readonly int value;

      public Value(int value)
      {
        this.value = value;
      }
    }

    #region Constructor
    [Test]
    public void Ctor_Throws_WhenMaxSizeIsLessThenZero() => Assert.Throws<ArgumentException>(() => new GlobalInstanceTable<int, Value>(-1));

    [Test]
    public void Ctor_InstantiateAnEmptyTable_WhenMaxSizeIsZero()
    {
      var table = new GlobalInstanceTable<int, Value>();
      Assert.AreEqual(0, table.maxSize);
    }

    [Test]
    public void Ctor_InstantiateAnEmptyTable_WhenMaxSizeIsSpecified()
    {
      var table = new GlobalInstanceTable<int, Value>(10);
      Assert.AreEqual(10, table.maxSize);
    }
    #endregion

    #region maxSize
    [Test]
    public void MaxSize_MustBeLargerThanZero()
    {
      var table = new GlobalInstanceTable<int, Value>(10);
      Assert.Throws<ArgumentException>(() => table.maxSize = -1);
    }

    [Test]
    public void MaxSize_CanBeChangedToLargerValue()
    {
      var table = new GlobalInstanceTable<int, Value>(5);
      Assert.AreEqual(5, table.maxSize);

      table.maxSize = 6;
      Assert.AreEqual(6, table.maxSize);
    }

    [Test]
    public void MaxSize_CanBeChangedToSmallerValue()
    {
      var table = new GlobalInstanceTable<int, Value>(5);
      Assert.AreEqual(5, table.maxSize);

      table.Add(1, new Value(1));
      table.Add(2, new Value(2));
      table.Add(3, new Value(3));
      table.Add(4, new Value(4));
      table.Add(5, new Value(5));

      table.maxSize = 2;
      Assert.AreEqual(2, table.maxSize);
      Assert.AreEqual(5, table.count);
    }
    #endregion

    #region Add
    [Test]
    public void CannotAdd_IfCountEqualsMaxSize()
    {
      var table = new GlobalInstanceTable<int, Value>(1);
      var v1 = new Value(1);

      table.Add(1, v1);
      Assert.Throws<InvalidOperationException>(() => table.Add(2, new Value(2)));

      GC.KeepAlive(v1);
    }

    [Test, Ignore("Skip because it's non-deterministic")]
    public void CanAdd_IfCountEqualsMaxSize_But_SomeValuesAreGCed()
    {
      var table = new GlobalInstanceTable<int, Value>(1);

      table.Add(1, new Value(1));
      GC.Collect();

      Assert.DoesNotThrow(() => table.Add(2, new Value(2)));
    }

    [Test]
    public void CannotAdd_If_KeyAlreadyExists()
    {
      var table = new GlobalInstanceTable<int, Value>(2);
      var v1 = new Value(1);

      table.Add(1, v1);
      Assert.Throws<ArgumentException>(() => table.Add(1, new Value(2)));

      GC.KeepAlive(v1);
    }

    [Test, Ignore("Skip because it's non-deterministic")]
    public void CanAdd_If_KeyAlreadyExists_But_TheReferenceIsGCed()
    {
      var table = new GlobalInstanceTable<int, Value>(2);

      table.Add(1, new Value(1));
      GC.Collect();

      Assert.DoesNotThrow(() => table.Add(1, new Value(2)));
    }
    #endregion

    #region TryGetValue
    [Test]
    public void TryGetValue_ReturnsTrue_IfTheKeyExists()
    {
      var table = new GlobalInstanceTable<int, Value>(1);
      var v1 = new Value(1);

      table.Add(1, v1);
      Assert.IsTrue(table.TryGetValue(1, out var v2));
      Assert.AreEqual(v1, v2);

      GC.KeepAlive(v1);
    }

    [Test, Ignore("Skip because it's non-deterministic")]
    public void TryGetValue_ReturnsFalse_IfTheKeyExists_But_TheValueIsGCed()
    {
      var table = new GlobalInstanceTable<int, Value>(1);

      table.Add(1, new Value(1));
      GC.Collect();

      Assert.IsFalse(table.TryGetValue(1, out var _));
    }

    [Test]
    public void TryGetValue_ReturnsFalse_IfTheKeyDoesNotExist()
    {
      var table = new GlobalInstanceTable<int, Value>(1);
      Assert.IsFalse(table.TryGetValue(1, out var _));
    }
    #endregion

    #region Clear
    [Test]
    public void Clear_ClearsTheTable()
    {
      var table = new GlobalInstanceTable<int, Value>(2);
      var v1 = new Value(1);
      var v2 = new Value(2);

      table.Add(1, v1);
      table.Add(2, v2);
      Assert.AreEqual(2, table.count);

      table.Clear();
      Assert.AreEqual(0, table.count);

      GC.KeepAlive(v1);
      GC.KeepAlive(v2);
    }
    #endregion

    #region ContainsKey
    [Test, Ignore("Skip because it's non-deterministic")]
    public void ContainsKey_ReturnsTrue_IfTheKeyExists()
    {
      var table = new GlobalInstanceTable<int, Value>(1);
      table.Add(1, new Value(1));

      GC.Collect();

      Assert.IsTrue(table.ContainsKey(1));
      Assert.False(table.TryGetValue(1, out var _));
    }

    [Test]
    public void ContainsKey_ReturnsFalse_IfTheKyeDoesNotExist()
    {
      var table = new GlobalInstanceTable<int, Value>(1);
      Assert.IsFalse(table.ContainsKey(1));
    }
    #endregion

    #region Remove
    [Test]
    public void Remove_RemovesTheKeyFromTheTable()
    {
      var table = new GlobalInstanceTable<int, Value>(1);
      var v1 = new Value(1);

      table.Add(1, v1);
      Assert.AreEqual(1, table.count);

      table.Remove(1);
      Assert.IsFalse(table.ContainsKey(1));
      Assert.AreEqual(0, table.count);

      GC.KeepAlive(v1);
    }
    #endregion
  }
}
