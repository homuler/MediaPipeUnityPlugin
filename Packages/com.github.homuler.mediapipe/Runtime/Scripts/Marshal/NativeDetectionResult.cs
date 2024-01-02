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
  internal readonly struct NativeDetection
  {
    private readonly IntPtr _categories;

    public readonly uint categoriesCount;

    public readonly NativeRect boundingBox;

    private readonly IntPtr _keypoints;

    public readonly uint keypointsCount;

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

    public ReadOnlySpan<NativeNormalizedKeypoint> keypoints
    {
      get
      {
        unsafe
        {
          return new ReadOnlySpan<NativeNormalizedKeypoint>((NativeNormalizedKeypoint*)_keypoints, (int)keypointsCount);
        }
      }
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct NativeDetectionResult
  {
    private readonly IntPtr _detections;
    public readonly uint detectionsCount;

    public ReadOnlySpan<NativeDetection> AsReadOnlySpan()
    {
      unsafe
      {
        return new ReadOnlySpan<NativeDetection>((NativeDetection*)_detections, (int)detectionsCount);
      }
    }

    public void Dispose()
    {
      UnsafeNativeMethods.mp_tasks_c_components_containers_CppCloseDetectionResult(this);
    }
  }
}
