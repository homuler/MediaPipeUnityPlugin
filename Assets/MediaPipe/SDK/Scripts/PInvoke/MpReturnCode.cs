namespace Mediapipe {
  public enum MpReturnCode : int {
    Success = 0,
    StandardError = 1,
    UnknownError = 2,
  }

  public static class MpReturnCodeExtension {
    public static void Assert(this MpReturnCode code) {
      if (code == MpReturnCode.Success) {
        return;
      }

      throw new MediaPipeException($"Failed to call a native method (code={code})");
    }
  }
}
