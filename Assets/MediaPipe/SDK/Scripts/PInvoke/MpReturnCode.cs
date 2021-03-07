namespace Mediapipe {
  public enum MpReturnCode : int {
    Success = 0,
    /// <summary>A standard exception is thrown</summary>
    StandardError = 1,
    /// <summary>Something other than standard exception is thrown</summary>
    UnknownError = 70,
    /// <summary>SDK failed to set status code (bug)</summary>
    Unset = 128, //
    /// <summary>Received SIGABRT</summary>
    Aborted = 134,
  }

  public static class MpReturnCodeExtension {
    public static void Assert(this MpReturnCode code) {
      switch (code) {
        case MpReturnCode.Success: return;
        case MpReturnCode.Aborted: {
          throw new MediaPipeException("MediaPipe Aborted, refer glog files for more details");
        }
        default: {
          throw new MediaPipePluginException($"Failed to call a native function (code={code})");
        }
      }
    }
  }
}
