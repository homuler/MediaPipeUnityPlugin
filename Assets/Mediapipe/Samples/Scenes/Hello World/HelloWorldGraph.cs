// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.HelloWorld
{
  public sealed class HelloWorldGraph : MonoBehaviour
  {
    private const string _TAG = nameof(HelloWorldGraph);

    private static readonly GlobalInstanceTable<int, HelloWorldGraph> _InstanceTable = new GlobalInstanceTable<int, HelloWorldGraph>(5);
    private static readonly Dictionary<IntPtr, int> _NameTable = new Dictionary<IntPtr, int>();

    private Stopwatch _stopwatch;
    private CalculatorGraph _calculatorGraph;

    private const string _InputStreamName = "in";
    private const string _OutputStreamName = "out";
    private const string _ConfigText = @"
input_stream: ""in""
output_stream: ""out""
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
";

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<string> OnOutput = new UnityEvent<string>();
#pragma warning restore IDE1006
    private OutputStreamPoller<string> _outputStreamPoller;
    private StringPacket _outputPacket;

    private void Start()
    {
      _InstanceTable.Add(GetInstanceID(), this);
    }

    private void OnDestroy()
    {
      Stop();
    }

    public void Initialize()
    {
      _calculatorGraph = new CalculatorGraph(_ConfigText);

      _NameTable.Add(_calculatorGraph.mpPtr, GetInstanceID());
      _stopwatch = new Stopwatch();
      _stopwatch.Start();
    }

    public Status StartRun()
    {
      _outputStreamPoller = _calculatorGraph.AddOutputStreamPoller<string>(_OutputStreamName, true).Value();
      _outputPacket = new StringPacket();

      return _calculatorGraph.StartRun();
    }

    public Status StartRunAsync()
    {
      _calculatorGraph.ObserveOutputStream(_OutputStreamName, OutputCallback, true).AssertOk();
      return _calculatorGraph.StartRun();
    }

    public Status AddTextToInputStream(string text)
    {
      return _calculatorGraph.AddPacketToInputStream(_InputStreamName, new StringPacket(text, GetCurrentTimestamp()));
    }

    public string FetchNextValue()
    {
      if (!_outputStreamPoller.Next(_outputPacket))
      {
        Logger.LogWarning(_TAG, $"Failed to fetch next packet from {_OutputStreamName}");
        return null;
      }
      return _outputPacket.IsEmpty() ? null : _outputPacket.Get();
    }

    public void Stop()
    {
      if (_calculatorGraph == null) { return; }

      // TODO: not to call CloseAllPacketSources if _calculatorGraph has not started.
      using (var status = _calculatorGraph.CloseAllPacketSources())
      {
        if (!status.Ok())
        {
          Logger.LogError(_TAG, status.ToString());
        }
      }

      using (var status = _calculatorGraph.WaitUntilDone())
      {
        if (!status.Ok())
        {
          Logger.LogError(_TAG, status.ToString());
        }
      }

      var _ = _NameTable.Remove(_calculatorGraph.mpPtr);
      _calculatorGraph.Dispose();
      _calculatorGraph = null;

      if (_stopwatch != null && _stopwatch.IsRunning)
      {
        _stopwatch.Stop();
      }

      OnOutput.RemoveAllListeners();
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr OutputCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      try
      {
        var isFound = TryGetGraph(graphPtr, out var graph);
        if (!isFound)
        {
          return Status.FailedPrecondition("Graph runner is not found").mpPtr;
        }
        using (var packet = new StringPacket(packetPtr, false))
        {
          var value = packet.IsEmpty() ? null : packet.Get();
          graph.OnOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      }
      catch (Exception e)
      {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    private static bool TryGetGraph(IntPtr graphPtr, out HelloWorldGraph graph)
    {
      var isInstanceIdFound = _NameTable.TryGetValue(graphPtr, out var instanceId);

      if (isInstanceIdFound)
      {
        return _InstanceTable.TryGetValue(instanceId, out graph);
      }
      graph = null;
      return false;
    }

    private Timestamp GetCurrentTimestamp()
    {
      if (_stopwatch == null || !_stopwatch.IsRunning)
      {
        return Timestamp.Unset();
      }
      var microseconds = _stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
      return new Timestamp(microseconds);
    }
  }
}
