using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct StructArray<T> where T : unmanaged
  {
    private readonly IntPtr _data;
    private readonly int _size;

    public void Dispose()
    {
      UnsafeNativeMethods.delete_array__Pf(_data);
    }

    public List<T> Copy()
    {
      var data = new List<T>(_size);

      CopyTo(data);
      return data;
    }

    public void CopyTo(List<T> data)
    {
      data.Clear();

      unsafe
      {
        var ptr = (T*)_data;

        for (var i = 0; i < _size; i++)
        {
          data.Add(*ptr++);
        }
      }
    }
  }
}
