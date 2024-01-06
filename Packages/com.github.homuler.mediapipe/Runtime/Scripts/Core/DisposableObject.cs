// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

/// based on [OpenCvSharp](https://github.com/shimat/opencvsharp/blob/9a5f9828a74cfa3995562a06716e177705cde038/src/OpenCvSharp/Fundamentals/DisposableObject.cs)

using System;
using System.Threading;

namespace Mediapipe
{
  public abstract class DisposableObject : IDisposable
  {
    private volatile int _disposeSignaled = 0;
    private bool _isLocked;

    public bool isDisposed { get; protected set; }
    protected bool isOwner { get; private set; }

    protected DisposableObject() : this(true) { }

    protected DisposableObject(bool isOwner)
    {
      isDisposed = false;
      this.isOwner = isOwner;
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (_isLocked)
      {
        throw new InvalidOperationException("Cannot dispose a locked object, unlock it first");
      }

      if (Interlocked.Exchange(ref _disposeSignaled, 1) != 0)
      {
        return;
      }

      isDisposed = true;

      if (disposing)
      {
        DisposeManaged();
      }
      DisposeUnmanaged();
    }

    ~DisposableObject()
    {
      Dispose(false);
    }

    protected virtual void DisposeManaged() { }

    protected virtual void DisposeUnmanaged() { }

    /// <summary>
    ///   Lock the object to prevent it from being disposed.
    /// </summary>
    internal void Lock()
    {
      _isLocked = true;
    }

    /// <summary>
    ///   Unlock the object to allow it to be disposed.
    /// </summary>
    internal void Unlock()
    {
      _isLocked = false;
    }

    /// <summary>Relinquish the ownership</summary>
    protected void TransferOwnership()
    {
      isOwner = false;
    }

    protected void ThrowIfDisposed()
    {
      if (isDisposed)
      {
        throw new ObjectDisposedException(GetType().FullName);
      }
    }
  }
}
