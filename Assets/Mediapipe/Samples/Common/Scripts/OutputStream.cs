// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity
{
  public class OutputStream<TPacket, TValue> where TPacket : Packet<TValue>, new()
  {
    protected readonly CalculatorGraph calculatorGraph;

    public readonly string streamName;
    public readonly string presenceStreamName;
    public readonly bool observeTimestampBounds;

    private OutputStreamPoller<TValue> _poller;
    private TPacket _outputPacket;

    private OutputStreamPoller<bool> _presencePoller;
    private BoolPacket _presencePacket;

    private long _lastTimestampMicrosec;

    protected bool canTestPresence => presenceStreamName != null;

    /// <summary>
    ///  Initialize a new instance of the <see cref="OutputStream" /> class.
    /// </summary>
    /// <remarks>
    ///   If <paramref name="observeTimestampBounds" /> is set to <c>false</c>, there are no ways to know whether the output is present or not.<br/>
    ///   This can be especially problematic <br/>
    ///      - when trying to get the output synchronously, because the thread will hang forever if no value is output.<br />
    ///      - when trying to get the output using callbacks, because they won't be called while no value is output.<br />
    /// </remarks>
    /// <param name="calculatorGraph">The owner of the stream</param>
    /// <param name="streamName">The name of the stream</param>
    /// <param name="observeTimestampBounds">
    ///   This parameter controlls the behaviour when no output is present. <br/>
    ///   When no output is present, if it's set to <c>true</c>, the stream outputs an empty packet, but if it's <c>false</c>, the stream does not output packets.
    /// </param>
    public OutputStream(CalculatorGraph calculatorGraph, string streamName, bool observeTimestampBounds = true)
    {
      this.calculatorGraph = calculatorGraph;
      this.streamName = streamName;
      this.observeTimestampBounds = observeTimestampBounds;
    }

    /// <summary>
    ///   Initialize a new instance of the <see cref="OutputStream" /> class.<br />
    ///   It's necessary for the graph to have <c>PacketPresenceCalculator</c> node that calculates if the stream has output or not.
    /// </summary>
    /// <remarks>
    ///   This is useful when you want to get the output synchronously but don't want to block the thread while waiting for the output.
    /// </remarks>
    /// <param name="calculatorGraph">The owner of the stream</param>
    /// <param name="streamName">The name of the stream</param>
    /// <param name="presenceStreamName">
    ///   The name of the stream that outputs true iff the output is present.
    /// </param>
    public OutputStream(CalculatorGraph calculatorGraph, string streamName, string presenceStreamName) : this(calculatorGraph, streamName, false)
    {
      this.presenceStreamName = presenceStreamName;
    }

    public Status StartPolling()
    {
      _outputPacket = new TPacket();

      var statusOrPoller = calculatorGraph.AddOutputStreamPoller<TValue>(streamName, observeTimestampBounds);
      var status = statusOrPoller.status;
      if (status.Ok())
      {
        _poller = statusOrPoller.Value();
      }

      if (presenceStreamName == null)
      {
        return status;
      }

      _presencePacket = new BoolPacket();

      var statusOrPresencePoller = calculatorGraph.AddOutputStreamPoller<bool>(presenceStreamName, false);
      status = statusOrPresencePoller.status;
      if (status.Ok())
      {
        _presencePoller = statusOrPresencePoller.Value();
      }
      return status;
    }

    public Status AddListener(CalculatorGraph.NativePacketCallback callback)
    {
      return calculatorGraph.ObserveOutputStream(streamName, callback, observeTimestampBounds);
    }

    /// <summary>
    ///   Gets the next value from the stream.
    ///   This method drops a packet whose timestamp is less than <paramref name="timestampThreshold" />.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     If <see cref="observeTimestampBounds" /> is set to true, MediaPipe outputs an empty packet when no output is present, but this method ignores those packets.
    ///     This behavior is useful to avoid outputting an empty value when the detection fails for a particular frame.
    ///   </para>
    ///   <para>
    ///     When <paramref name="value" /> is set to the default value, you cannot tell whether it's because the output was empty or not.
    ///   </para>
    /// </remarks>
    /// <param name="value">
    ///   When this method returns, it contains the next output value if it's present and retrieved successfully; otherwise, the default value for the type of the value parameter.
    ///   This parameter is passed uninitialized.
    /// </param>
    /// <param name="timestampThreshold">
    ///   Drops outputs whose timestamp is less than this value.
    /// </param>
    /// <param name="allowBlock">
    ///   If <c>true</c>, this method can block the thread until the value is retrieved.<br />
    ///   It can be set to <c>false</c> only if <see cref="presenceStreamName" /> is set.
    /// </param>
    /// <returns>
    ///   <c>true</c> if <paramref name="value" /> is successfully retrieved; otherwise <c>false</c>.
    /// </returns>
    public bool TryGetNext(out TValue value, long timestampThreshold, bool allowBlock = true)
    {
      var timestampMicrosec = long.MinValue;

      while (timestampMicrosec < timestampThreshold)
      {
        if (!CanCallNext(allowBlock) || !Next())
        {
          value = default;
          return false;
        }
        using (var timestamp = _outputPacket.Timestamp())
        {
          timestampMicrosec = timestamp.Microseconds();
        }
      }

      if (_outputPacket.IsEmpty())
      {
        value = default; // TODO: distinguish when the output is empty and when it's not (retrieved value can be the default value).
        return false;
      }

      _lastTimestampMicrosec = timestampMicrosec;
      value = _outputPacket.Get();
      return true;
    }

    public bool TryGetNext(out TValue value, bool allowBlock = true)
    {
      return TryGetNext(out value, 0, allowBlock);
    }

    public bool TryConsumeNext(out TValue value, long timestampThreshold, bool allowBlock = true)
    {
      long timestampMicrosec = 0;

      while (timestampMicrosec <= timestampThreshold)
      {
        if (!CanCallNext(allowBlock) || !Next())
        {
          value = default;
          return true;
        }
        using (var timestamp = _outputPacket.Timestamp())
        {
          timestampMicrosec = timestamp.Microseconds();
        }
      }

      if (_outputPacket.IsEmpty())
      {
        value = default; // TODO: distinguish when the output is empty and when it's not (retrieved value can be the default value).
        return false;
      }

      _lastTimestampMicrosec = timestampMicrosec;
      var statusOrValue = _outputPacket.Consume();

      value = statusOrValue.ValueOr();
      return true;
    }

    public bool TryConsumeNext(out TValue value, bool allowBlock = true)
    {
      return TryConsumeNext(out value, 0, allowBlock);
    }

    public bool TryGetPacketValue(Packet<TValue> packet, out TValue value, long timeoutMicrosec = 0)
    {
      using (var timestamp = packet.Timestamp())
      {
        var currentMicrosec = timestamp.Microseconds();

        if (!packet.IsEmpty())
        {
          _lastTimestampMicrosec = currentMicrosec;
          value = packet.Get();
          return true;
        }

        value = default; // TODO: distinguish when the output is empty and when it's not (retrieved value can be the default value).
        var hasTimedOut = currentMicrosec - _lastTimestampMicrosec >= timeoutMicrosec;

        if (hasTimedOut)
        {
          _lastTimestampMicrosec = currentMicrosec;
        }
        return hasTimedOut;
      }
    }

    public bool TryConsumePacketValue(Packet<TValue> packet, out TValue value, long timeoutMicrosec = 0)
    {
      using (var timestamp = packet.Timestamp())
      {
        var currentMicrosec = timestamp.Microseconds();

        if (!packet.IsEmpty())
        {
          _lastTimestampMicrosec = currentMicrosec;
          var statusOrValue = packet.Consume();

          value = statusOrValue.ValueOr();
          return true;
        }

        value = default; // TODO: distinguish when the output is empty and when it's not (retrieved value can be the default value).
        var hasTimedOut = currentMicrosec - _lastTimestampMicrosec >= timeoutMicrosec;

        if (hasTimedOut)
        {
          _lastTimestampMicrosec = currentMicrosec;
        }
        return hasTimedOut;
      }
    }

    public bool ResetTimestampIfTimedOut(long timestampMicrosec, long timeoutMicrosec)
    {
      if (timestampMicrosec - _lastTimestampMicrosec <= timeoutMicrosec)
      {
        return false;
      }
      _lastTimestampMicrosec = timestampMicrosec;
      return true;
    }

    protected bool CanCallNext(bool allowBlock)
    {
      if (canTestPresence)
      {
        if (!allowBlock)
        {
          if (_presencePoller.QueueSize() <= 0)
          {
            return false;
          }
        }
        if (!NextPresence() || !_presencePacket.Get())
        {
          // NOTE: _presencePacket.IsEmpty() always returns false
          return false;
        }
      }
      else if (!allowBlock)
      {
        Logger.LogWarning("Cannot avoid thread being blocked when `presenceStreamName` is not set");
        return false;
      }
      return true;
    }

    protected bool NextPresence()
    {
      return Next(_presencePoller, _presencePacket, presenceStreamName);
    }

    protected bool Next()
    {
      return Next(_poller, _outputPacket, streamName);
    }

    protected static bool Next<T>(OutputStreamPoller<T> poller, Packet<T> packet, string stream)
    {
      if (!poller.Next(packet))
      {
        Logger.LogWarning($"Failed to get next value from {stream}. See logs for more details");
        return false;
      }
      return true;
    }
  }
}
