using System.Runtime.InteropServices;

using MpOutputStreamPoller = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe
{
  public class StatusOrPoller
  {
    private const string MediapipeLibrary = "mediapipe_c";

    private MpStatusOrPoller mpStatusOrPoller;

    public StatusOrPoller(MpStatusOrPoller ptr)
    {
      mpStatusOrPoller = ptr;
    }

    ~StatusOrPoller()
    {
      MpStatusOrPollerDestroy(mpStatusOrPoller);
    }

    public bool IsOk()
    {
      return MpStatusOrPollerOk(mpStatusOrPoller);
    }

    public OutputStreamPoller GetPoller()
    {
      if (!MpStatusOrPollerOk(mpStatusOrPoller))
      {
        return null;
      }

      var mpOutputStreamPoller = MpStatusOrPollerValue(mpStatusOrPoller);

      return new OutputStreamPoller(mpOutputStreamPoller);
    }

    #region Externs

    [DllImport (MediapipeLibrary)]
    private static extern unsafe bool MpStatusOrPollerOk(MpStatusOrPoller statusOrPoller);

    [DllImport (MediapipeLibrary)]
    private static extern unsafe MpOutputStreamPoller MpStatusOrPollerValue(MpStatusOrPoller statusOrPoller);


    [DllImport (MediapipeLibrary)]
    private static extern unsafe void MpStatusOrPollerDestroy(MpStatusOrPoller statusOrPoller);

    #endregion
  }
}
