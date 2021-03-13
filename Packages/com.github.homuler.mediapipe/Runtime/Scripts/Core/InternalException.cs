using System;

namespace Mediapipe {
  public class InternalException : Exception {
    public InternalException(string message) : base(message) {}
  }
}
