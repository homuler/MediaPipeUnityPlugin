using System;

namespace Mediapipe {
  public class MediaPipeException : ApplicationException {
    public MediaPipeException(string message) : base(message) {}
  }
}
