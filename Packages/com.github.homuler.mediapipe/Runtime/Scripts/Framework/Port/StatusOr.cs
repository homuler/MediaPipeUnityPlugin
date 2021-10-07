// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public abstract class StatusOr<T> : MpResourceHandle
  {
    public StatusOr(IntPtr ptr) : base(ptr) { }

    public abstract Status status { get; }
    public virtual bool Ok()
    {
      return status.Ok();
    }

    public virtual T ValueOr(T defaultValue = default)
    {
      return Ok() ? Value() : defaultValue;
    }

    /// <exception cref="MediaPipePluginException">Thrown when status is not ok</exception>
    public abstract T Value();
  }
}
