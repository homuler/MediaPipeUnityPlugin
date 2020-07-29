using System.Runtime.InteropServices;

using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpStatus mpStatus;

    public Status(MpStatus ptr) {
      mpStatus = ptr;
    }

    ~Status() {
      MpStatusDestroy(mpStatus);
    }

    public bool IsOk() {
      return MpStatusOk(mpStatus);
    }

    public int GetRawCode() {
      return GetMpStatusRawCode(mpStatus);
    }

    public override string ToString() {
      return MpStatusToString(mpStatus);
    }

    public static Status Build(int code, string message) {
      var ptr = MpStatusCreate(code, message);
      return new Status(ptr);
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpStatusCreate(int code, string message);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe bool MpStatusOk(MpStatus status);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe int GetMpStatusRawCode(MpStatus status);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe string MpStatusToString(MpStatus status);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpStatusDestroy(MpStatus status);

    #endregion
  }
}
