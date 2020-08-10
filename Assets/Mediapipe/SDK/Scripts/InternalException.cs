using System;

namespace Mediapipe {
  public class InternalException : ApplicationException {
    public InternalException(string message) : base(message) {}
  }
}
