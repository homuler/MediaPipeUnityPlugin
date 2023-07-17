// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.
using System;

namespace Mediapipe
{
  public class BadStatusException : Exception
  {
    public Status.StatusCode statusCode { get; private set; }

    public BadStatusException(string message) : base(message) { }

    public BadStatusException(Status.StatusCode statusCode, string message) : base(message)
    {
      this.statusCode = statusCode;
    }
  }
}
