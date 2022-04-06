// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Unity
{
  public class GlobalInstanceTable<TKey, TValue> where TValue : class
  {
    private readonly Dictionary<TKey, WeakReference<TValue>> _table;
    private readonly int _maxSize;

    public GlobalInstanceTable(int maxSize)
    {
      _table = new Dictionary<TKey, WeakReference<TValue>>();
      _maxSize = maxSize;
    }

    public void Add(TKey key, TValue value)
    {
      if (_table.Count >= _maxSize)
      {
        ClearUnusedKeys();
      }

      lock (((ICollection)_table).SyncRoot)
      {
        if (_table.Count >= _maxSize)
        {
          throw new InvalidOperationException("The table is full");
        }

        if (_table.ContainsKey(key))
        {
          if (_table[key].TryGetTarget(out var currentValue))
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
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
      lock (((ICollection)_table).SyncRoot)
      {
        if (_table.ContainsKey(key))
        {
          return _table[key].TryGetTarget(out value);
        }
      }
      value = default;
      return false;
    }

    public void Clear()
    {
      lock (((ICollection)_table).SyncRoot)
      {
        _table.Clear();
      }
    }

    private void ClearUnusedKeys()
    {
      lock (((ICollection)_table).SyncRoot)
      {
        var deadKeys = _table.Where(x => !x.Value.TryGetTarget(out var target)).Select(x => x.Key).ToArray();

        foreach (var key in deadKeys)
        {
          var _ = _table.Remove(key);
        }
      }
    }
  }
}
