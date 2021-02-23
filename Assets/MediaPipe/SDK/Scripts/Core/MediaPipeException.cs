using System;

namespace Mediapipe {
  public class MediaPipeException : Exception {
    public MediaPipeException(string message) : base(message) {}
  }
}
