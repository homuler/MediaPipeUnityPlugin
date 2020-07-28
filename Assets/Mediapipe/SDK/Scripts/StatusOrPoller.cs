using System.Runtime.InteropServices;

using MpOutputStreamPoller = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;
using MpStatus = System.IntPtr;

namespace Mediapipe
{
  public class StatusOrPoller
  {
    private const string MediapipeLibrary = "mediapipe_c";

    public Status status;
    private MpStatusOrPoller mpStatusOrPoller;

    public StatusOrPoller(MpStatusOrPoller ptr)
    {
      mpStatusOrPoller = ptr;
      status = new Status(MpStatusOrPollerStatus(mpStatusOrPoller));
    }

    ~StatusOrPoller()
    {
      MpStatusOrPollerDestroy(mpStatusOrPoller);
    }

    public bool IsOk()
    {
      return status.IsOk();
    }

    public OutputStreamPoller GetValue()
    {
      if (!IsOk())
      {
        return null;
      }

      var mpOutputStreamPoller = MpStatusOrPollerValue(mpStatusOrPoller);

      return new OutputStreamPoller(mpOutputStreamPoller);
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpStatus MpStatusOrPollerStatus(MpStatusOrPoller statusOrPoller);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpOutputStreamPoller MpStatusOrPollerValue(MpStatusOrPoller statusOrPoller);


    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpStatusOrPollerDestroy(MpStatusOrPoller statusOrPoller);

    #endregion
  }
}
