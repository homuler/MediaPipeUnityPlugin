// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Runtime.InteropServices;

using Google.Protobuf;

namespace Mediapipe
{
  public class CalculatorGraph : MpResourceHandle
  {
    public delegate StatusArgs NativePacketCallback(IntPtr graphPtr, int streamId, IntPtr packetPtr);
    public delegate void PacketCallback<T>(Packet<T> packet);

    public CalculatorGraph() : base()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__(out var ptr).Assert();
      this.ptr = ptr;
    }

    private CalculatorGraph(byte[] serializedConfig) : base()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__PKc_i(serializedConfig, serializedConfig.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public CalculatorGraph(CalculatorGraphConfig config) : this(config.ToByteArray()) { }

    public CalculatorGraph(string textFormatConfig) : this(CalculatorGraphConfig.Parser.ParseFromTextFormat(textFormatConfig)) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__delete(ptr);
    }

    public void Initialize(CalculatorGraphConfig config)
    {
      var bytes = config.ToByteArray();
      UnsafeNativeMethods.mp_CalculatorGraph__Initialize__PKc_i(mpPtr, bytes, bytes.Length, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void Initialize(CalculatorGraphConfig config, PacketMap sidePacket)
    {
      var bytes = config.ToByteArray();
      UnsafeNativeMethods.mp_CalculatorGraph__Initialize__PKc_i_Rsp(mpPtr, bytes, bytes.Length, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    /// <remarks>Crashes if config is not set</remarks>
    public CalculatorGraphConfig Config()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__Config(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var config = serializedProto.Deserialize(CalculatorGraphConfig.Parser);
      serializedProto.Dispose();

      return config;
    }

    public void ObserveOutputStream(string streamName, int streamId, NativePacketCallback nativePacketCallback, bool observeTimestampBounds = false)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__ObserveOutputStream__PKc_PF_b(mpPtr, streamName, streamId, nativePacketCallback, observeTimestampBounds, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void ObserveOutputStream<T>(string streamName, PacketCallback<T> packetCallback, bool observeTimestampBounds, out GCHandle callbackHandle)
    {
      NativePacketCallback nativePacketCallback = (IntPtr graphPtr, int streamId, IntPtr packetPtr) =>
      {
        try
        {
          var packet = Packet<T>.CreateForReference(packetPtr);
          packetCallback(packet);
          return StatusArgs.Ok();
        }
        catch (Exception e)
        {
          return StatusArgs.Internal(e.ToString());
        }
      };
      callbackHandle = GCHandle.Alloc(nativePacketCallback, GCHandleType.Pinned);

      ObserveOutputStream(streamName, 0, nativePacketCallback, observeTimestampBounds);
    }

    public void ObserveOutputStream<T>(string streamName, PacketCallback<T> packetCallback, out GCHandle callbackHandle)
    {
      ObserveOutputStream(streamName, packetCallback, false, out callbackHandle);
    }

    public OutputStreamPoller<T> AddOutputStreamPoller<T>(string streamName, bool observeTimestampBounds = false)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__AddOutputStreamPoller__PKc_b(mpPtr, streamName, observeTimestampBounds, out var statusPtr, out var pollerPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
      return new OutputStreamPoller<T>(pollerPtr);
    }

    public void Run()
    {
      Run(new PacketMap());
    }

    public void Run(PacketMap sidePacket)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__Run__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void StartRun()
    {
      StartRun(new PacketMap());
    }

    public void StartRun(PacketMap sidePacket)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__StartRun__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void WaitUntilIdle()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilIdle(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void WaitUntilDone()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilDone(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public bool HasError()
    {
      return SafeNativeMethods.mp_CalculatorGraph__HasError(mpPtr);
    }

    public void AddPacketToInputStream<T>(string streamName, Packet<T> packet)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mpPtr, streamName, packet.mpPtr, out var statusPtr).Assert();
      packet.Dispose(); // respect move semantics

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void SetInputStreamMaxQueueSize(string streamName, int maxQueueSize)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__SetInputStreamMaxQueueSize__PKc_i(mpPtr, streamName, maxQueueSize, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void CloseInputStream(string streamName)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseInputStream__PKc(mpPtr, streamName, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void CloseAllPacketSources()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseAllPacketSources(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void Cancel()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__Cancel(mpPtr).Assert();
      GC.KeepAlive(this);
    }

    public bool GraphInputStreamsClosed()
    {
      return SafeNativeMethods.mp_CalculatorGraph__GraphInputStreamsClosed(mpPtr);
    }

    public bool IsNodeThrottled(int nodeId)
    {
      return SafeNativeMethods.mp_CalculatorGraph__IsNodeThrottled__i(mpPtr, nodeId);
    }

    public bool UnthrottleSources()
    {
      return SafeNativeMethods.mp_CalculatorGraph__UnthrottleSources(mpPtr);
    }

    public GpuResources GetGpuResources()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__GetGpuResources(mpPtr, out var gpuResourcesPtr).Assert();

      GC.KeepAlive(this);
      return new GpuResources(gpuResourcesPtr);
    }

    public void SetGpuResources(GpuResources gpuResources)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__SetGpuResources__SPgpu(mpPtr, gpuResources.sharedPtr, out var statusPtr).Assert();

      GC.KeepAlive(gpuResources);
      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
