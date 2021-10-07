using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.HelloWorld
{
  public sealed class HelloWorldGraph : MonoBehaviour
  {
    static readonly string TAG = typeof(HelloWorldGraph).Name;

    static readonly GlobalInstanceTable<int, HelloWorldGraph> instanceTable = new GlobalInstanceTable<int, HelloWorldGraph>(5);
    static readonly Dictionary<IntPtr, int> nameTable = new Dictionary<IntPtr, int>();

    Stopwatch stopwatch;
    CalculatorGraph calculatorGraph;

    const string inputStreamName = "in";
    const string outputStreamName = "out";
    const string configText = @"
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

    public UnityEvent<string> OnOutput = new UnityEvent<string>();
    OutputStreamPoller<string> outputStreamPoller;
    StringPacket outputPacket;

    void Start()
    {
      instanceTable.Add(GetInstanceID(), this);
    }

    void OnDestroy()
    {
      Stop();
    }

    public void Initialize()
    {
      calculatorGraph = new CalculatorGraph(configText);

      nameTable.Add(calculatorGraph.mpPtr, GetInstanceID());
      stopwatch = new Stopwatch();
      stopwatch.Start();
    }

    public Status StartRun()
    {
      outputStreamPoller = calculatorGraph.AddOutputStreamPoller<string>(outputStreamName, true).Value();
      outputPacket = new StringPacket();

      return calculatorGraph.StartRun();
    }

    public Status StartRunAsync()
    {
      calculatorGraph.ObserveOutputStream(outputStreamName, OutputCallback, true).AssertOk();
      return calculatorGraph.StartRun();
    }

    public Status AddTextToInputStream(string text)
    {
      return calculatorGraph.AddPacketToInputStream(inputStreamName, new StringPacket(text, GetCurrentTimestamp()));
    }

    public string FetchNextValue()
    {
      if (!outputStreamPoller.Next(outputPacket))
      {
        Logger.LogWarning(TAG, $"Failed to fetch next packet from {outputStreamName}");
        return null;
      }
      return outputPacket.IsEmpty() ? null : outputPacket.Get();
    }

    public void Stop()
    {
      if (calculatorGraph == null) { return; }

      // TODO: not to call CloseAllPacketSources if calculatorGraph has not started.
      using (var status = calculatorGraph.CloseAllPacketSources())
      {
        if (!status.Ok())
        {
          Logger.LogError(TAG, status.ToString());
        }
      }

      using (var status = calculatorGraph.WaitUntilDone())
      {
        if (!status.Ok())
        {
          Logger.LogError(TAG, status.ToString());
        }
      }

      nameTable.Remove(calculatorGraph.mpPtr);
      calculatorGraph.Dispose();
      calculatorGraph = null;

      if (stopwatch != null && stopwatch.IsRunning)
      {
        stopwatch.Stop();
      }

      OnOutput.RemoveAllListeners();
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr OutputCallback(IntPtr graphPtr, IntPtr packetPtr)
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
          (graph as HelloWorldGraph).OnOutput.Invoke(value);
        }
        return Status.Ok().mpPtr;
      }
      catch (Exception e)
      {
        return Status.FailedPrecondition(e.ToString()).mpPtr;
      }
    }

    static bool TryGetGraph(IntPtr graphPtr, out HelloWorldGraph graph)
    {
      var isInstanceIdFound = nameTable.TryGetValue(graphPtr, out var instanceId);

      if (isInstanceIdFound)
      {
        return instanceTable.TryGetValue(instanceId, out graph);
      }
      graph = null;
      return false;
    }

    Timestamp GetCurrentTimestamp()
    {
      if (stopwatch == null || !stopwatch.IsRunning)
      {
        return Timestamp.Unset();
      }
      var microseconds = (stopwatch.ElapsedTicks) / (TimeSpan.TicksPerMillisecond / 1000);
      return new Timestamp(microseconds);
    }
  }
}
