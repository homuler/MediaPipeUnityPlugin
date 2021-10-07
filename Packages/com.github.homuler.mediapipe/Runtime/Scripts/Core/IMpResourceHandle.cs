// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System;

namespace Mediapipe
{
  public interface IMpResourceHandle : IDisposable
  {
    IntPtr mpPtr { get; }

    /// <summary>
    ///   Relinquish the ownership, and release the resource it owns if necessary.
    ///   This method should be called only if the underlying native api moves the pointer.
    /// </summary>
    /// <remarks>If the object itself is no longer used, call <see cref="Dispose" /> instead.</remarks>
    void ReleaseMpResource();

    /// <summary>Relinquish the ownership</summary>
    void TransferOwnership();

    bool OwnsResource();
  }
}
