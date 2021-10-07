// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System;

namespace Mediapipe
{
  public abstract class SharedPtrHandle : MpResourceHandle
  {
    protected SharedPtrHandle(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    /// <returns>The owning pointer</returns>
    public abstract IntPtr Get();

    /// <summary>Release the owning pointer</summary>
    public abstract void Reset();
  }
}
