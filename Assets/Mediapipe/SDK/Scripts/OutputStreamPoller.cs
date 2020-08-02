using OutputStreamPollerPtr = System.IntPtr;

namespace Mediapipe {
  public class OutputStreamPoller<S, T> where S : Packet<T>, new() {
    private OutputStreamPollerPtr outputStreamPollerPtr;
    private S packet;

    public OutputStreamPoller(OutputStreamPollerPtr ptr) {
      outputStreamPollerPtr = ptr;
      this.packet = new S();
    }

    private bool HasNext() {
      return UnsafeNativeMethods.MpOutputStreamPollerNext(outputStreamPollerPtr, packet.GetPtr());
    }

    public (bool, T) GetNextValue() {
      if (!HasNext()) { return (false, default(T)); }

      return (true, packet.GetValue());
    }
  }
}
