using System;
using System.Runtime.InteropServices;
using System.Security;

using MpCalculatorGraph = System.IntPtr;
using MpCalculatorGraphConfig = System.IntPtr;
using MpClassificationList = System.IntPtr;
using MpDetection = System.IntPtr;
using MpDetectionVector = System.IntPtr;
using MpGlCalculatorHelper = System.IntPtr;
using MpGlContext = System.IntPtr;
using MpGpuResources = System.IntPtr;
using MpLandmarkList = System.IntPtr;
using MpLandmarkListVector = System.IntPtr;
using MpNormalizedRect = System.IntPtr;
using MpNormalizedRectVector = System.IntPtr;
using MpPacket = System.IntPtr;
using MpRect = System.IntPtr;
using MpRectVector = System.IntPtr;
using MpSerializedProto = System.IntPtr;
using MpSerializedProtoVector = System.IntPtr;
using MpSidePacket = System.IntPtr;
using MpStatus = System.IntPtr;
using MpStatusOrGpuBuffer = System.IntPtr;
using MpStatusOrGpuResources = System.IntPtr;
using MpStatusOrImageFrame = System.IntPtr;
using MpStatusOrPoller = System.IntPtr;

using CacheFilePathResolverPtr = System.IntPtr;
using GlContextPtr = System.IntPtr;
using GlTexturePtr = System.IntPtr;
using GlTextureInfoPtr = System.IntPtr;
using GlStatusFunctionPtr = System.IntPtr;
using GpuBufferPtr = System.IntPtr;
using GpuResourcesPtr = System.IntPtr;
using ImageFrameMemoryHandlerPtr = System.IntPtr;
using ImageFramePtr = System.IntPtr;
using OutputStreamPollerPtr = System.IntPtr;
using ProtobufLogHandlerPtr = System.IntPtr;
using ReadFileHandlerPtr = System.IntPtr;

namespace Mediapipe {
  [SuppressUnmanagedCodeSecurityAttribute] 
  internal static class UnsafeNativeMethods {
    #if UNITY_ANDROID
      private const string MediaPipeLibrary = "mediapipe_jni";
    #else
      private const string MediaPipeLibrary = "mediapipe_c";
    #endif

    /// CalculatorGraph API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpCalculatorGraph MpCalculatorGraphCreate();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpCalculatorGraphDestroy(MpCalculatorGraph graph);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphInitialize(MpCalculatorGraph graph, MpCalculatorGraphConfig config);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphStartRun(MpCalculatorGraph graph, MpSidePacket sidePacket);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphWaitUntilDone(MpCalculatorGraph graph);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpGpuResources MpCalculatorGraphGetGpuResources(MpCalculatorGraph graph);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphSetGpuResources(MpCalculatorGraph graph, MpGpuResources gpuResources);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatusOrPoller MpCalculatorGraphAddOutputStreamPoller(MpCalculatorGraph graph, string name);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphAddPacketToInputStream(MpCalculatorGraph graph, string name, MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpCalculatorGraphCloseInputStream(MpCalculatorGraph graph, string name);


    /// CalculatorGraphConfig API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpCalculatorGraphConfig ParseMpCalculatorGraphConfig(string input);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpCalculatorGraphConfigDestroy(MpCalculatorGraphConfig config);


    /// Classification API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpClassificationList MpPacketGetClassificationList(MpPacket packet);


    /// Detection API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpDetection MpPacketGetDetection(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpDetectionVector MpPacketGetDetectionVector(MpPacket packet);


    /// Gl API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void GlFlush();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void GlReadPixels(int x, int y, int width, int height, UInt32 glFormat, UInt32 glType, IntPtr pixels);


    /// GlCalculatorHelper API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpGlCalculatorHelper MpGlCalculatorHelperCreate();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGlCalculatorHelperDestroy(MpGlCalculatorHelper gpuHelper);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGlCalculatorHelperInitializeForTest(MpGlCalculatorHelper gpuHelper, GpuResourcesPtr gpuResources);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpGlCalculatorHelperRunInGlContext(MpGlCalculatorHelper gpuHelper, [MarshalAs(UnmanagedType.FunctionPtr)]GlStatusFunctionPtr glFunc);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GlTexturePtr MpGlCalculatorHelperCreateSourceTextureForImageFrame(MpGlCalculatorHelper gpuHelper, ImageFramePtr imageFrame);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GlTexturePtr MpGlCalculatorHelperCreateSourceTextureForGpuBuffer(MpGlCalculatorHelper gpuHelper, GpuBufferPtr gpu_buffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGlCalculatorHelperBindFramebuffer(MpGlCalculatorHelper gpuHelper, GlTexturePtr glTexture);


    /// GlContext API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpGlContext MpGlContextGetCurrent();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GlContextPtr MpGlContextGet(MpGlContext glContext);


    /// GlTexture API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGlTextureDestroy(GlTexturePtr glTexture);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpGlTextureWidth(GlTexturePtr glTexture);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpGlTextureHeight(GlTexturePtr glTexture);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGlTextureRelease(GlTexturePtr glTexture);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GpuBufferPtr MpGlTextureGetGpuBufferFrame(GlTexturePtr glTexture);


    /// GlTextureInfo API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpGlTextureInfoDestroy(GlTextureInfoPtr glTextureInfo);


    /// GpuBuffer API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGpuBufferDestroy(GpuBufferPtr gpuBuffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe UInt32 MpGpuBufferFormat(GpuBufferPtr gpuBuffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpGpuBufferWidth(GpuBufferPtr gpuBuffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpGpuBufferHeight(GpuBufferPtr gpuBuffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpPacket MpMakeGpuBufferPacketAt(GpuBufferPtr gpuBuffer, int timestamp);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GpuBufferPtr MpPacketGetGpuBuffer(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatusOrGpuBuffer MpPacketConsumeGpuBuffer(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpStatusOrGpuBufferDestroy(MpStatusOrGpuBuffer gpuBuffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrGpuBufferStatus(MpStatusOrGpuBuffer gpuBuffer);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GpuBufferPtr MpStatusOrGpuBufferConsumeValue(MpStatusOrGpuBuffer gpuBuffer);


    /// GpuBufferFormat API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFormatForGpuBufferFormat(UInt32 gpuFormatCode);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GlTextureInfoPtr MpGlTextureInfoForGpuBufferFormat(UInt32 gpuFormatCode, int plane);


    /// GpuResources API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpGpuResourcesDestroy(MpGpuResources gpuResources);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe GpuResourcesPtr MpGpuResourcesGet(MpGpuResources gpuResources);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatusOrGpuResources MpGpuResourcesCreate();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpStatusOrGpuResourcesDestroy(MpStatusOrGpuResources statusOrGpuResources);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrGpuResourcesStatus(MpStatusOrGpuResources statusOrGpuResources);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpGpuResources MpStatusOrGpuResourcesConsumeValue(MpStatusOrGpuResources statusOrGpuResources);


    /// ImageFrame API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreate(int formatCode, int width, int height, uint alignmentBoundary);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreateDefault();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe ImageFramePtr MpImageFrameCreateWithPixelData(int formatCode, int width, int height, int widthStep, IntPtr pixelData, [MarshalAs(UnmanagedType.FunctionPtr)]ImageFrameMemoryHandlerPtr deleter);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpImageFrameDestroy(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe bool MpImageFrameIsEmpty(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameFormat(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameWidth(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameHeight(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameChannelSize(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameNumberOfChannels(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameByteDepth(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int MpImageFrameWidthStep(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe IntPtr MpImageFramePixelData(ImageFramePtr imageFramePtr);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpPacket MpMakeImageFramePacketAt(ImageFramePtr imageFramePtr, int timestamp);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe ImageFramePtr MpPacketGetImageFrame(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatusOrImageFrame MpPacketConsumeImageFrame(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpStatusOrImageFrameDestroy(MpStatusOrImageFrame statusOrImageFrame);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrImageFrameStatus(MpStatusOrImageFrame statusOrImageFrame);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe ImageFramePtr MpStatusOrImageFrameConsumeValue(MpStatusOrImageFrame statusOrImageFrame);


    /// Landmark API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpLandmarkList MpPacketGetNormalizedLandmarkList(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpLandmarkListVector MpPacketGetNormalizedLandmarkListVector(MpPacket packet);


    /// OutputStreamPoller API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpStatusOrPollerDestroy(MpStatusOrPoller statusOrPoller);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpStatusOrPollerStatus(MpStatusOrPoller statusOrPoller);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe OutputStreamPollerPtr MpStatusOrPollerConsumeValue(MpStatusOrPoller statusOrPoller);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe bool MpOutputStreamPollerNext(OutputStreamPollerPtr outputStreamPollerPtr, MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpOutputStreamPollerDestroy(OutputStreamPollerPtr outputStreamPollerPtr);


    /// Packet API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpPacket MpPacketCreate();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpPacketDestroy(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpPacket MpMakeBoolPacket(bool value);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe bool MpPacketGetBool(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpPacket MpMakeFloatPacket(float value);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe float MpPacketGetFloat(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpPacket MpMakeStringPacketAt(string text, int timestamp);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe string MpPacketGetString(MpPacket packet);


    /// Rect API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpRect MpPacketGetRect(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpNormalizedRect MpPacketGetNormalizedRect(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpRectVector MpPacketGetRectVector(MpPacket packet);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpNormalizedRectVector MpPacketGetNormalizedRectVector(MpPacket packet);


  #if UNITY_EDITOR || UNITY_STANDALONE
    /// Resource Util API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpAssetManagerInitialize([MarshalAs(UnmanagedType.FunctionPtr)]CacheFilePathResolverPtr resolver,[MarshalAs(UnmanagedType.FunctionPtr)]ReadFileHandlerPtr handler);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpStringCopy(IntPtr dest, byte[] src, int size);
  #endif

    /// SidePacket API

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpSidePacket MpSidePacketCreate();

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpSidePacketDestroy(MpSidePacket sidePacket);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpSidePacketInsert(MpSidePacket sidePacket, string key, MpPacket packet);


    /// Status API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe MpStatus MpStatusCreate(int code, string message);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe bool MpStatusOk(MpStatus status);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe int GetMpStatusRawCode(MpStatus status);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe string MpStatusToString(MpStatus status);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpStatusDestroy(MpStatus status);


    /// Protobuf API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpSerializedProtoDestroy(MpSerializedProto proto);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void MpSerializedProtoVectorDestroy(MpSerializedProtoVector protoVec);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe ProtobufLogHandlerPtr SetProtobufLogHandler([MarshalAs(UnmanagedType.FunctionPtr)]ProtobufLogHandlerPtr logHandler);


    /// Glog API
    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void InitGoogleLogging(string program, string logDir);

    [DllImport (MediaPipeLibrary)]
    public static extern unsafe void ShutdownGoogleLogging();
  }
}
