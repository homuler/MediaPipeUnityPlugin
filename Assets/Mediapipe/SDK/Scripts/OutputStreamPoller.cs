using System.Runtime.InteropServices;

using MpPacket = System.IntPtr;
using MpOutputStreamPoller = System.IntPtr;

namespace Mediapipe
{
  public class OutputStreamPoller
  {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpOutputStreamPoller mpOutputStreamPoller;
    private StringPacket packet;

    public OutputStreamPoller(MpOutputStreamPoller ptr)
    {
      mpOutputStreamPoller = ptr;
      packet = new StringPacket();
    }

    ~OutputStreamPoller()
    {
      MpOutputStreamPollerDestroy(mpOutputStreamPoller);
    }

    public bool HasNextPacket()
    {
      return MpOutputStreamPollerNext(mpOutputStreamPoller, packet.GetPtr());
    }

    public string GetPacketValue()
    {
      return packet.GetValue();
    }

    #region Externs

    // CalculatorGraph API
    [DllImport (MediapipeLibrary)]
    private static extern unsafe bool MpOutputStreamPollerNext(MpOutputStreamPoller poller, MpPacket packet);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpOutputStreamPollerDestroy(MpOutputStreamPoller poller);

    #endregion
  }
}
