using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Unity
{
  public class GlobalInstanceTable<T, U> where U : class
  {
    Dictionary<T, WeakReference<U>> table;
    int maxSize;

    public GlobalInstanceTable(int maxSize)
    {
      table = new Dictionary<T, WeakReference<U>>();
      this.maxSize = maxSize;
    }

    public void Add(T key, U value)
    {
      if (table.Count >= maxSize)
      {
        ClearUnusedKeys();
      }

      lock (((ICollection)table).SyncRoot)
      {
        if (table.Count >= maxSize)
        {
          throw new InvalidOperationException("The table is full");
        }

        if (table.ContainsKey(key))
        {
          if (table[key].TryGetTarget(out var currentValue))
          {
            throw new ArgumentException("An instance with the same key already exists");
          }
          table[key].SetTarget(value);
        }
        else
        {
          table[key] = new WeakReference<U>(value);
        }
      }
    }

    public bool TryGetValue(T key, out U value)
    {
      lock (((ICollection)table).SyncRoot)
      {
        if (table.ContainsKey(key))
        {
          return table[key].TryGetTarget(out value);
        }
      }
      value = default(U);
      return false;
    }

    public void Clear()
    {
      lock (((ICollection)table).SyncRoot)
      {
        table.Clear();
      }
    }

    void ClearUnusedKeys()
    {
      lock (((ICollection)table).SyncRoot)
      {
        var deadKeys = table.Where(x => !x.Value.TryGetTarget(out var target)).Select(x => x.Key).ToArray();

        foreach (var key in deadKeys)
        {
          table.Remove(key);
        }
      }
    }
  }
}
