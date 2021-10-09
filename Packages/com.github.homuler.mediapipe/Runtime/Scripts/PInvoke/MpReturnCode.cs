// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe
{
  public enum MpReturnCode : int
  {
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

  public static class MpReturnCodeExtension
  {
    public static void Assert(this MpReturnCode code)
    {
      switch (code)
      {
        case MpReturnCode.Success: return;
        case MpReturnCode.Aborted:
          {
            throw new MediaPipeException("MediaPipe Aborted, refer glog files for more details");
          }
        case MpReturnCode.StandardError:
          {
            throw new MediaPipePluginException($"Exception is thrown in Unmanaged Code");
          }
        case MpReturnCode.UnknownError:
          {
            throw new MediaPipePluginException($"Unknown exception is thrown in Unmanaged Code");
          }
        case MpReturnCode.Unset:
          {
            // Bug
            throw new MediaPipePluginException($"Failed to call a native function, but the reason is unknown");
          }
        default:
          {
            throw new MediaPipePluginException($"Failed to call a native function, but the reason is undefined");
          }
      }
    }
  }
}
