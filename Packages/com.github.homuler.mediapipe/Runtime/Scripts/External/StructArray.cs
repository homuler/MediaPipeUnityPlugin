using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct StructArray
  {
    private readonly IntPtr _data;
    private readonly int _size;

    public void Dispose()
    {
      UnsafeNativeMethods.delete_array__Pf(_data);
    }

    public List<T> Copy<T>() where T : unmanaged
    {
      var data = new List<T>(_size);

      CopyTo(data);
      return data;
    }

    public void CopyTo<T>(List<T> data) where T : unmanaged
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
