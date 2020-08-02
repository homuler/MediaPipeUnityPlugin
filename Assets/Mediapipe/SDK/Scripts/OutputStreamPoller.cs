using OutputStreamPollerPtr = System.IntPtr;

namespace Mediapipe {
  public class OutputStreamPoller<T> {
    private OutputStreamPollerPtr outputStreamPollerPtr;

    public OutputStreamPoller(OutputStreamPollerPtr ptr) {
      outputStreamPollerPtr = ptr;
    }

    public bool Next(Packet<T> packet) {
      return UnsafeNativeMethods.MpOutputStreamPollerNext(outputStreamPollerPtr, packet.GetPtr());
    }
  }
}
