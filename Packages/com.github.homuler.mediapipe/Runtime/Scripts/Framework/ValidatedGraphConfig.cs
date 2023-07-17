// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Google.Protobuf;

namespace Mediapipe
{
  public enum NodeType : int
  {
    Unknown = 0,
    Calculator = 1,
    PacketGenerator = 2,
    GraphInputStream = 3,
    StatusHandler = 4,
  };

  [StructLayout(LayoutKind.Sequential)]
  public readonly struct NodeRef
  {
    public readonly NodeType type;
    public readonly int index;
  }

  [StructLayout(LayoutKind.Sequential)]
  public readonly struct EdgeInfo
  {
    public readonly int upstream;
    public readonly NodeRef parentNode;
    public readonly string name;
    public readonly bool backEdge;

    internal EdgeInfo(int upstream, NodeRef parentNode, string name, bool backEdge)
    {
      this.upstream = upstream;
      this.parentNode = parentNode;
      this.name = name;
      this.backEdge = backEdge;
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct EdgeInfoVector
  {
    private readonly IntPtr _data;
    private readonly int _size;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_api_EdgeInfoArray__delete(_data, _size);
    }

    public List<EdgeInfo> Copy()
    {
      var edgeInfos = new List<EdgeInfo>(_size);

      unsafe
      {
        var edgeInfoPtr = (EdgeInfoTmp*)_data;

        for (var i = 0; i < _size; i++)
        {
          var edgeInfoTmp = Marshal.PtrToStructure<EdgeInfoTmp>((IntPtr)edgeInfoPtr++);
          edgeInfos.Add(edgeInfoTmp.Copy());
        }
      }

      return edgeInfos;
    }

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct EdgeInfoTmp
    {
      private readonly int _upstream;
      private readonly NodeRef _parentNode;
      private readonly IntPtr _name;

      [MarshalAs(UnmanagedType.U1)]
      private readonly bool _backEdge;

      public EdgeInfo Copy()
      {
        var name = Marshal.PtrToStringAnsi(_name);
        return new EdgeInfo(_upstream, _parentNode, name, _backEdge);
      }
    }
  }

  public class ValidatedGraphConfig : MpResourceHandle
  {
    public ValidatedGraphConfig() : base()
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__(out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__delete(ptr);
    }

    public void Initialize(CalculatorGraphConfig config)
    {
      var bytes = config.ToByteArray();
      UnsafeNativeMethods.mp_ValidatedGraphConfig__Initialize__Rcgc(mpPtr, bytes, bytes.Length, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public void Initialize(string graphType)
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__Initialize__PKc(mpPtr, graphType, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public bool Initialized()
    {
      return SafeNativeMethods.mp_ValidatedGraphConfig__Initialized(mpPtr);
    }

    public void ValidateRequiredSidePackets(PacketMap sidePacket)
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__ValidateRequiredSidePackets__Rsp(mpPtr, sidePacket.mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(sidePacket);
      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }

    public CalculatorGraphConfig Config(ExtensionRegistry extensionRegistry = null)
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__Config(mpPtr, out var serializedProto).Assert();
      GC.KeepAlive(this);

      var parser = extensionRegistry == null ? CalculatorGraphConfig.Parser : CalculatorGraphConfig.Parser.WithExtensionRegistry(extensionRegistry);
      var config = serializedProto.Deserialize(parser);
      serializedProto.Dispose();

      return config;
    }

    public List<EdgeInfo> InputStreamInfos()
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__InputStreamInfos(mpPtr, out var edgeInfoVector).Assert();
      GC.KeepAlive(this);

      var edgeInfos = edgeInfoVector.Copy();
      edgeInfoVector.Dispose();
      return edgeInfos;
    }

    public List<EdgeInfo> OutputStreamInfos()
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__OutputStreamInfos(mpPtr, out var edgeInfoVector).Assert();
      GC.KeepAlive(this);

      var edgeInfos = edgeInfoVector.Copy();
      edgeInfoVector.Dispose();
      return edgeInfos;
    }

    public List<EdgeInfo> InputSidePacketInfos()
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__InputSidePacketInfos(mpPtr, out var edgeInfoVector).Assert();
      GC.KeepAlive(this);

      var edgeInfos = edgeInfoVector.Copy();
      edgeInfoVector.Dispose();
      return edgeInfos;
    }

    public List<EdgeInfo> OutputSidePacketInfos()
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__OutputSidePacketInfos(mpPtr, out var edgeInfoVector).Assert();
      GC.KeepAlive(this);

      var edgeInfos = edgeInfoVector.Copy();
      edgeInfoVector.Dispose();
      return edgeInfos;
    }

    public int OutputStreamIndex(string name)
    {
      return SafeNativeMethods.mp_ValidatedGraphConfig__OutputStreamIndex__PKc(mpPtr, name);
    }

    public int OutputSidePacketIndex(string name)
    {
      return SafeNativeMethods.mp_ValidatedGraphConfig__OutputSidePacketIndex__PKc(mpPtr, name);
    }

    public int OutputStreamToNode(string name)
    {
      return SafeNativeMethods.mp_ValidatedGraphConfig__OutputStreamToNode__PKc(mpPtr, name);
    }

    public string RegisteredSidePacketTypeName(string name)
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__RegisteredSidePacketTypeName(mpPtr, name, out var statusPtr, out var strPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
      return MarshalStringFromNative(strPtr);
    }

    public string RegisteredStreamTypeName(string name)
    {
      UnsafeNativeMethods.mp_ValidatedGraphConfig__RegisteredStreamTypeName(mpPtr, name, out var statusPtr, out var strPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
      return MarshalStringFromNative(strPtr);
    }

    public string Package()
    {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_ValidatedGraphConfig__Package);
    }

    public static bool IsReservedExecutorName(string name)
    {
      return SafeNativeMethods.mp_ValidatedGraphConfig_IsReservedExecutorName(name);
    }

    public bool IsExternalSidePacket(string name)
    {
      return SafeNativeMethods.mp_ValidatedGraphConfig__IsExternalSidePacket__PKc(mpPtr, name);
    }
  }
}
