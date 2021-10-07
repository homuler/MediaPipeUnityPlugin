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
    public delegate IntPtr NativePacketCallback(IntPtr graphPtr, IntPtr packetPtr);
    public delegate Status PacketCallback<TPacket, TValue>(TPacket packet) where TPacket : Packet<TValue>;

    public CalculatorGraph() : base()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__(out var ptr).Assert();
      this.ptr = ptr;
    }

    public CalculatorGraph(string textFormatConfig) : base()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__PKc(textFormatConfig, out var ptr).Assert();
      this.ptr = ptr;
    }

    public CalculatorGraph(byte[] serializedConfig) : base()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__PKc_i(serializedConfig, serializedConfig.Length, out var ptr).Assert();
      this.ptr = ptr;
    }

    public CalculatorGraph(CalculatorGraphConfig config) : this(config.ToByteArray()) { }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__delete(ptr);
    }

    public Status Initialize(CalculatorGraphConfig config)
    {
      var bytes = config.ToByteArray();
      UnsafeNativeMethods.mp_CalculatorGraph__Initialize__PKc_i(mpPtr, bytes, bytes.Length, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status Initialize(CalculatorGraphConfig config, SidePacket sidePacket)
    {
      var bytes = config.ToByteArray();
      UnsafeNativeMethods.mp_CalculatorGraph__Initialize__PKc_i_Rsp(mpPtr, bytes, bytes.Length, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
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

    public Status ObserveOutputStream(string streamName, NativePacketCallback nativePacketCallback, bool observeTimestampBounds = false)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__ObserveOutputStream__PKc_PF_b(mpPtr, streamName, nativePacketCallback, observeTimestampBounds, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status ObserveOutputStream<TPacket, TValue>(string streamName, PacketCallback<TPacket, TValue> packetCallback, bool observeTimestampBounds, out GCHandle callbackHandle) where TPacket : Packet<TValue>
    {
      NativePacketCallback nativePacketCallback = (IntPtr _, IntPtr packetPtr) =>
      {
        Status status = null;
        try
        {
          var packet = (TPacket)Activator.CreateInstance(typeof(TPacket), packetPtr, false);
          status = packetCallback(packet);
        }
        catch (Exception e)
        {
          status = Status.FailedPrecondition(e.ToString());
        }
        return status.mpPtr;
      };
      callbackHandle = GCHandle.Alloc(nativePacketCallback, GCHandleType.Pinned);

      return ObserveOutputStream(streamName, nativePacketCallback, observeTimestampBounds);
    }

    public Status ObserveOutputStream<TPacket, TValue>(string streamName, PacketCallback<TPacket, TValue> packetCallback, out GCHandle callbackHandle) where TPacket : Packet<TValue>
    {
      return ObserveOutputStream(streamName, packetCallback, false, out callbackHandle);
    }

    public StatusOrPoller<T> AddOutputStreamPoller<T>(string streamName, bool observeTimestampBounds = false)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__AddOutputStreamPoller__PKc_b(mpPtr, streamName, observeTimestampBounds, out var statusOrPollerPtr).Assert();

      GC.KeepAlive(this);
      return new StatusOrPoller<T>(statusOrPollerPtr);
    }

    public Status Run()
    {
      return Run(new SidePacket());
    }

    public Status Run(SidePacket sidePacket)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__Run__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status StartRun()
    {
      return StartRun(new SidePacket());
    }

    public Status StartRun(SidePacket sidePacket)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__StartRun__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status WaitUntilIdle()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilIdle(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status WaitUntilDone()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__WaitUntilDone(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public bool HasError()
    {
      return SafeNativeMethods.mp_CalculatorGraph__HasError(mpPtr);
    }

    public Status AddPacketToInputStream<T>(string streamName, Packet<T> packet)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__AddPacketToInputStream__PKc_Ppacket(mpPtr, streamName, packet.mpPtr, out var statusPtr).Assert();
      packet.Dispose(); // respect move semantics

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status SetInputStreamMaxQueueSize(string streamName, int maxQueueSize)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__SetInputStreamMaxQueueSize__PKc_i(mpPtr, streamName, maxQueueSize, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status CloseInputStream(string streamName)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseInputStream__PKc(mpPtr, streamName, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }

    public Status CloseAllPacketSources()
    {
      UnsafeNativeMethods.mp_CalculatorGraph__CloseAllPacketSources(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
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

    public Status SetGpuResources(GpuResources gpuResources)
    {
      UnsafeNativeMethods.mp_CalculatorGraph__SetGpuResources__SPgpu(mpPtr, gpuResources.sharedPtr, out var statusPtr).Assert();

      GC.KeepAlive(gpuResources);
      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
