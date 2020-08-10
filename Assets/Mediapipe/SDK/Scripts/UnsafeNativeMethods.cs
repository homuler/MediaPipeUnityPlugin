using System;
using System.Runtime.InteropServices;
using System.Security;

using MpCalculatorGraph = System.IntPtr;
using MpCalculatorGraphConfig = System.IntPtr;
using MpGlCalculatorHelper = System.IntPtr;
using MpGpuBuffer = System.IntPtr;
using MpGpuResources = System.IntPtr;
using MpPacket = System.IntPtr;
using MpSidePacket = System.IntPtr;
using MpStatus = System.IntPtr;
using MpStatusOrGpuBuffer = System.IntPtr;
using MpStatusOrGpuResources = System.IntPtr;
using MpStatusOrImageFrame = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

using GlTexturePtr = System.IntPtr;
using GlTextureInfoPtr = System.IntPtr;
using GlStatusFunctionPtr = System.IntPtr;
using GpuResourcesPtr = System.IntPtr;
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
    public static extern unsafe MpStatus MpCalculatorGraphSetGpuResources(MpGpuResources gpuResources);

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


    /// Gl API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe void GlFlush();

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void GlReadPixels(int x, int y, int width, int height, UInt32 glFormat, UInt32 glType, byte[] pixels);


    /// GlCalculatorHelper API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpGlCalculatorHelper MpGlCalculatorHelperCreate();

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGlCalculatorHelperDestroy(MpGlCalculatorHelper gpuHelper);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGlCalculatorHelperInitializeForTest(MpGlCalculatorHelper gpuHelper, MpGpuResources gpuResources);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpGlCalculatorHelperRunInGlContext(MpGlCalculatorHelper gpuHelper, [MarshalAs(UnmanagedType.FunctionPtr)]GlStatusFunctionPtr glFunc);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe GlTexturePtr MpGlCalculatorHelperCreateSourceTexture(MpGlCalculatorHelper gpuHelper, ImageFramePtr imageFrame);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGlCalculatorHelperBindFramebuffer(MpGlCalculatorHelper gpuHelper, GlTexturePtr glTexture);


    /// GlTexture API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGlTextureDestroy(GlTexturePtr glTexture);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGlTextureRelease(GlTexturePtr glTexture);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpGpuBuffer MpGlTextureGetGpuBufferFrame(GlTexturePtr glTexture);


    /// GlTextureInfo API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpGlTextureInfoDestroy(GlTextureInfoPtr glTextureInfo);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpGlTextureInfoGlInternalFormat(GlTextureInfoPtr glTextureInfo);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe UInt32 MpGlTextureInfoGlFormat(GlTextureInfoPtr glTextureInfo);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe UInt32 MpGlTextureInfoGlType(GlTextureInfoPtr glTextureInfo);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpGlTextureInfoDownscale(GlTextureInfoPtr glTextureInfo);


    /// GpuBuffer API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGpuBufferDestroy(MpGpuBuffer gpuBuffer);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe UInt32 MpGpuBufferFormat(MpGpuBuffer gpuBuffer);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpGpuBufferWidth(MpGpuBuffer gpuBuffer);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpGpuBufferHeight(MpGpuBuffer gpuBuffer);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpPacket MpMakeGpuBufferPacketAt(MpGpuBuffer gpuBuffer, int timestamp);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatusOrGpuBuffer MpPacketConsumeGpuBuffer(MpPacket packet);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpStatusOrGpuBufferDestroy(MpStatusOrGpuBuffer gpuBuffer);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrGpuBufferStatus(MpStatusOrGpuBuffer gpuBuffer);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpGpuBuffer MpStatusOrGpuBufferConsumeValue(MpStatusOrGpuBuffer gpuBuffer);


    /// GpuBufferFormat API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe int MpImageFormatForGpuBufferFormat(UInt32 gpuFormatCode);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe GlTextureInfoPtr MpGlTextureInfoForGpuBufferFormat(UInt32 gpuFormatCode, int plane);


    /// GpuResources API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpGpuResourcesDestroy(MpGpuResources gpuResources);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe GpuResourcesPtr MpGpuResourcesGet(MpGpuResources gpuResources);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatusOrGpuResources MpGpuResourcesCreate();

    [DllImport (MediapipeLibrary)]
    public static extern unsafe void MpStatusOrGpuResourcesDestroy(MpStatusOrGpuResources statusOrGpuResources);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrGpuResourcesStatus(MpStatusOrGpuResources statusOrGpuResources);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe MpGpuResources MpStatusOrGpuResourcesConsumeValue(MpStatusOrGpuResources statusOrGpuResources);


    /// ImageFrame API
    [DllImport (MediapipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreate(int formatCode, int width, int height, UInt32 alignmentBoundary);

    [DllImport (MediapipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreateWithPixelData(int formatCode, int width, int height, int widthStep, byte[] pixelData, [MarshalAs(UnmanagedType.FunctionPtr)]ImageFrameMemoryHandlerPtr deleter);

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
