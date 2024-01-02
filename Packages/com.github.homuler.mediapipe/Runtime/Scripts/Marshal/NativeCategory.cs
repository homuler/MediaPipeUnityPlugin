// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeCategory
  {
    public readonly int index;
    public readonly float score;
    private readonly IntPtr _categoryName;
    private readonly IntPtr _displayName;

    public string categoryName => Marshal.PtrToStringAnsi(_categoryName);
    public string displayName => Marshal.PtrToStringAnsi(_displayName);
  }

  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeCategories
  {
    private readonly IntPtr _categories;
    public readonly uint categoriesCount;

    public ReadOnlySpan<NativeCategory> AsReadOnlySpan()
    {
      unsafe
      {
        return new ReadOnlySpan<NativeCategory>((NativeCategory*)_categories, (int)categoriesCount);
      }
    }
  }
}
