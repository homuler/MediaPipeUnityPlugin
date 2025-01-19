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
  internal readonly struct NativeClassifications
  {
    private readonly IntPtr _categories;
    public readonly uint categoriesCount;
    public readonly int headIndex;
    private readonly IntPtr _headName;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_tasks_c_components_containers_CppCloseClassifications(this);
    }

    public ReadOnlySpan<NativeCategory> categories
    {
      get
      {
        unsafe
        {
          return new ReadOnlySpan<NativeCategory>((NativeCategory*)_categories, (int)categoriesCount);
        }
      }
    }

    public string headName => Marshal.PtrToStringAnsi(_headName);
  }

  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeClassificationResult
  {
    private readonly IntPtr _classifications;
    public readonly uint classificationsCount;
    public readonly long timestampMs;
    public readonly bool hasTimestampMs;

    public ReadOnlySpan<NativeClassifications> classifications
    {
      get
      {
        unsafe
        {
          return new ReadOnlySpan<NativeClassifications>((NativeClassifications*)_classifications, (int)classificationsCount);
        }
      }
    }

    public void Dispose()
    {
      UnsafeNativeMethods.mp_tasks_c_components_containers_CppCloseClassificationResult(this);
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeClassificationResultArray
  {
    private readonly IntPtr _data;
    public readonly int size;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_api_ClassificationResultArray__delete(this);
    }

    public ReadOnlySpan<NativeClassificationResult> AsReadOnlySpan()
    {
      unsafe
      {
        return new ReadOnlySpan<NativeClassificationResult>((NativeClassificationResult*)_data, size);
      }
    }
  }

}
