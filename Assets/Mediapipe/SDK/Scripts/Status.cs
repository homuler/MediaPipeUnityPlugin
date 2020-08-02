using MpStatus = System.IntPtr;

namespace Mediapipe {
  public class Status {
    private MpStatus mpStatus;

    public Status(MpStatus ptr) {
      mpStatus = ptr;
    }

    ~Status() {
      UnsafeNativeMethods.MpStatusDestroy(mpStatus);
    }

    public bool IsOk() {
      return UnsafeNativeMethods.MpStatusOk(mpStatus);
    }

    public int GetRawCode() {
      return UnsafeNativeMethods.GetMpStatusRawCode(mpStatus);
    }

    public override string ToString() {
      return UnsafeNativeMethods.MpStatusToString(mpStatus);
    }

    public static Status Build(int code, string message) {
      var ptr = UnsafeNativeMethods.MpStatusCreate(code, message);
      return new Status(ptr);
    }
  }
}
