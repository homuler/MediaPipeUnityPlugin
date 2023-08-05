// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Mediapipe
{
  // TODO: make GlobalInstanceTable internal
  public class GlobalInstanceTable<TKey, TValue> where TValue : class
  {
    private readonly ReaderWriterLockSlim _tableLock = new ReaderWriterLockSlim();
    private readonly Dictionary<TKey, WeakReference<TValue>> _table;

    private int _maxSize;
    /// <summary>
    ///   The maximum number of instances that can be stored in the table.
    ///   It can be safely changed to a value less than the current number of instances,
    ///   but <see cref="Add" /> will fail till the number of instances becomes less than or equal to the new value.
    /// </summary>
    public int maxSize
    {
      get => _maxSize;
      set
      {
        if (value < 0)
        {
          throw new ArgumentException("maxSize must be greater than or equal to 0");
        }
        _maxSize = value;
      }
    }

    public int count
    {
      get
      {
        _tableLock.EnterReadLock();
        try
        {
          return _table.Count;
        }
        finally
        {
          _tableLock.ExitReadLock();
        }
      }
    }

    public GlobalInstanceTable(int maxSize = 0)
    {
      this.maxSize = maxSize;
      _table = new Dictionary<TKey, WeakReference<TValue>>(maxSize);
    }

    public void Add(TKey key, TValue value)
    {
      _tableLock.EnterWriteLock();
      try
      {
        if (_table.Count >= maxSize)
        {
          ClearUnusedKeys();
        }

        if (_table.Count >= maxSize)
        {
          throw new InvalidOperationException("The table is full");
        }

        if (_table.ContainsKey(key))
        {
          if (_table[key].TryGetTarget(out var _))
          {
            throw new ArgumentException("An instance with the same key already exists");
          }
          _table[key].SetTarget(value);
        }
        else
        {
          _table[key] = new WeakReference<TValue>(value);
        }
      }
      finally
      {
        _tableLock.ExitWriteLock();
      }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      _tableLock.EnterReadLock();
      try
      {
        if (_table.ContainsKey(key))
        {
          return _table[key].TryGetTarget(out value);
        }
        value = default;
        return false;
      }
      finally
      {
        _tableLock.ExitReadLock();
      }
    }

    public void Clear()
    {
      _tableLock.EnterWriteLock();
      try
      {
        _table.Clear();
      }
      finally
      {
        _tableLock.ExitWriteLock();
      }
    }

    public bool ContainsKey(TKey key)
    {
      _tableLock.EnterReadLock();
      try
      {
        return _table.ContainsKey(key);
      }
      finally
      {
        _tableLock.ExitReadLock();
      }
    }

    public bool Remove(TKey key)
    {
      _tableLock.EnterWriteLock();
      try
      {
        return _table.Remove(key);
      }
      finally
      {
        _tableLock.ExitWriteLock();
      }
    }

    /// <remarks>
    ///   Aquire the write lock before calling this method.
    /// </remarks>
    private void ClearUnusedKeys()
    {
      var deadKeys = _table.Where(x => !x.Value.TryGetTarget(out var target)).Select(x => x.Key).ToArray();

      foreach (var key in deadKeys)
      {
        var _ = _table.Remove(key);
      }
    }
  }
}
