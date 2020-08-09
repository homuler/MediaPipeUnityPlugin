using System;
using System.Runtime.InteropServices;
using System.Security;

using MpCalculatorGraph = System.IntPtr;
using MpCalculatorGraphConfig = System.IntPtr;
using MpPacket = System.IntPtr;
using MpSidePacket = System.IntPtr;
using MpStatus = System.IntPtr;
using MpStatusOrImageFrame = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

using ImageFrameMemoryHandlerPtr = System.IntPtr;
using ImageFramePtr = System.IntPtr;
using OutputStreamPollerPtr = System.IntPtr;
using ProtobufLogHandlerPtr = System.IntPtr;

namespace Mediapipe {
  [SuppressUnmanagedCodeSecurityAttribute] 
  internal static class UnsafeNativeMethods {
    private const string MediapipeLibrary = "mediapipe_c";

    /// CalculatorGraph API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpCalculatorGraph MpCalculatorGraphCreate();

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpCalculatorGraphDestroy(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphInitialize(MpCalculatorGraph graph, MpCalculatorGraphConfig config);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphStartRun(MpCalculatorGraph graph, MpSidePacket sidePacket);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphWaitUntilDone(MpCalculatorGraph graph);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatusOrPoller MpCalculatorGraphAddOutputStreamPoller(MpCalculatorGraph graph, string name);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphAddPacketToInputStream(MpCalculatorGraph graph, string name, MpPacket packet);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphCloseInputStream(MpCalculatorGraph graph, string name);


    /// CalculatorGraphConfig API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpCalculatorGraphConfig ParseMpCalculatorGraphConfig(string input);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpCalculatorGraphConfigDestroy(MpCalculatorGraphConfig config);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe ProtobufLogHandlerPtr SetProtobufLogHandler([MarshalAs(UnmanagedType.FunctionPtr)]ProtobufLogHandlerPtr logHandler);


    /// ImageFrame API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreate(int formatCode, int width, int height, UInt32 alignmentBoundary);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreateWithPixelData(
      int formatCode, int width, int height, int widthStep, byte[] pixelData,
      [MarshalAs(UnmanagedType.FunctionPtr)]ImageFrameMemoryHandlerPtr deleter
    );

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpImageFrameDestroy(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe bool MpImageFrameIsEmpty(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFrameWidth(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFrameHeight(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFrameChannelSize(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFrameNumberOfChannels(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFrameByteDepth(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFrameWidthStep(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe IntPtr MpImageFramePixelData(ImageFramePtr imageFramePtr);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpStatusOrImageFrameDestroy(MpStatusOrImageFrame statusOrImageFrame);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrImageFrameStatus(MpStatusOrImageFrame statusOrImageFrame);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe ImageFramePtr MpStatusOrImageFrameConsumeValue(MpStatusOrImageFrame statusOrImageFrame);


    /// OutputStreamPoller API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpStatusOrPollerDestroy(MpStatusOrPoller statusOrPoller);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrPollerStatus(MpStatusOrPoller statusOrPoller);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe OutputStreamPollerPtr MpStatusOrPollerConsumeValue(MpStatusOrPoller statusOrPoller);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe bool MpOutputStreamPollerNext(OutputStreamPollerPtr outputStreamPollerPtr, MpPacket packet);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpOutputStreamPollerDestroy(OutputStreamPollerPtr outputStreamPollerPtr);


    /// Packet API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpPacket MpPacketCreate();

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpPacketDestroy(MpPacket packet);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpPacket MpMakeStringPacketAt(string text, int timestamp);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe string MpPacketGetString(MpPacket packet);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpPacket MpMakeImageFramePacketAt(ImageFramePtr imageFramePtr, int timestamp);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatusOrImageFrame MpPacketConsumeImageFrame(MpPacket packet);


    /// SidePacket API

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpSidePacket MpSidePacketCreate();

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpSidePacketDestroy(MpSidePacket packet);


    /// Status API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpStatusCreate(int code, string message);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe bool MpStatusOk(MpStatus status);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int GetMpStatusRawCode(MpStatus status);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe string MpStatusToString(MpStatus status);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpStatusDestroy(MpStatus status);
  }
}
