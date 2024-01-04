// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediapipe.Unity
{
  public class OutputEventArgs<T> : EventArgs
  {
    public readonly T value;

    public OutputEventArgs(T value)
    {
      this.value = value;
    }
  }

  public sealed class OutputStream : IDisposable
  {
    public readonly struct OutputEventArgs
    {
      public readonly Packet packet;

      public OutputEventArgs(Packet packet)
      {
        this.packet = packet;
      }
    }

    public readonly struct NextResult
    {
      public readonly Packet packet;
      /// <summary>
      ///   <see langword="true"/> if the next packet is retrieved successfully; otherwise <see langword="false"/>.
      /// </summary>
      public readonly bool ok;

      public bool isDropped => ok && packet == null;

      public NextResult(Packet packet, bool ok)
      {
        this.packet = packet;
        this.ok = ok;
      }
    }

    private static int _Counter = 0;
    private static readonly GlobalInstanceTable<int, OutputStream> _InstanceTable = new GlobalInstanceTable<int, OutputStream>(20);

    private CalculatorGraph _calculatorGraph;

    private readonly int _id;
    public readonly string streamName;
    public readonly bool observeTimestampBounds;

    private OutputStreamPoller _poller;
    private Packet _outputPacket;
    private Packet outputPacket
    {
      get
      {
        _outputPacket ??= Packet.CreateEmpty();
        return _outputPacket;
      }
    }

    private readonly ReaderWriterLockSlim _waitTaskLock = new ReaderWriterLockSlim();
    /// <remarks>
    ///   Only the <see cref="_waitTask"/> can modify <see cref="_outputPacket"/>.
    /// </remarks>
    private Task<NextResult> _waitTask;

    private event EventHandler<OutputEventArgs> OnReceived;

    private Packet _referencePacket;
    private Packet referencePacket
    {
      get
      {
        _referencePacket ??= Packet.CreateForReference(IntPtr.Zero);
        return _referencePacket;
      }
    }

    private volatile int _disposeSignaled = 0;
    private bool _isDisposed;

    /// <summary>
    ///  Initialize a new instance of the <see cref="OutputStream" /> class.
    /// </summary>
    /// <remarks>
    ///   When <paramref name="observeTimestampBounds" /> is set to <see langword="false"/>, while there's not output present in the stream,
    ///   <list type="bullet">
    ///     <item>
    ///       <description>
    ///         <see cref="WaitNextAsync"/> won't return the result.
    ///       </description>
    ///     </item>
    ///     <item>
    ///       <description>
    ///         Callbacks, which can be registered by <see cref="AddListener"/>, won't be called.
    ///       </description>
    ///     </item>
    ///   </list>
    /// </remarks>
    /// <param name="calculatorGraph">The owner of the stream</param>
    /// <param name="streamName">The name of the stream</param>
    /// <param name="observeTimestampBounds">
    ///   This parameter controlls the behaviour when there's no output.<br/>
    ///   If it's set to <see langword="true"/>, the stream outputs an empty packet when there's not output,
    ///   but if it's <see langword="false"/>, the stream does not output packets.
    /// </param>
    public OutputStream(CalculatorGraph calculatorGraph, string streamName, bool observeTimestampBounds = true)
    {
      _id = Interlocked.Increment(ref _Counter);
      _calculatorGraph = calculatorGraph;
      this.streamName = streamName;
      this.observeTimestampBounds = observeTimestampBounds;

      _InstanceTable.Add(_id, this);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
      if (Interlocked.Exchange(ref _disposeSignaled, 1) != 0)
      {
        return;
      }

      _isDisposed = true;

      if (disposing)
      {
        DisposeManaged();
      }
      DisposeUnmanaged();
    }

    private void DisposeManaged()
    {
      OnReceived = null;
      _ = _InstanceTable.Remove(_id);
    }

    private void DisposeUnmanaged()
    {
      // _calculatorGraph is not managed by this class.
      _calculatorGraph = null;

      _poller?.Dispose();
      _poller = null;
      _outputPacket?.Dispose();
      _outputPacket = null;
      _referencePacket?.Dispose();
      _referencePacket = null;
    }

    ~OutputStream()
    {
      Dispose(false);
    }

    public void StartPolling()
    {
      ThrowIfDisposed();

      _poller = _calculatorGraph.AddOutputStreamPoller(streamName, observeTimestampBounds);
    }

    public void AddListener(EventHandler<OutputEventArgs> callback)
    {
      ThrowIfDisposed();

      if (OnReceived == null)
      {
        _calculatorGraph.ObserveOutputStream(streamName, _id, InvokeIfOutputStreamFound, observeTimestampBounds);
      }
      OnReceived += callback;
    }

    public void RemoveListener(EventHandler<OutputEventArgs> eventHandler)
    {
      OnReceived -= eventHandler;
    }

    public void RemoveAllListeners()
    {
      OnReceived = null;
    }

    /// <summary>
    ///   Wait the next packet from the stream.
    /// </summary>
    /// <remarks>
    ///   Only if <see cref="observeTimestampBounds" /> is set to true, MediaPipe outputs an empty packet when no output is present.
    /// </remarks>
    public async Task<NextResult> WaitNextAsync()
    {
      ThrowIfDisposed();

      try
      {
        var result = await WaitNextInternal().ConfigureAwait(false);
        return result;
      }
      finally
      {
        ClearWaitTask();
      }
    }

    /// <summary>
    ///   Wait the next packet from the stream.
    /// </summary>
    /// <remarks>
    ///   Only if <see cref="observeTimestampBounds" /> is set to true, MediaPipe outputs an empty packet when no output is present.
    /// </remarks>
    public async Task<NextResult> WaitNextAsync(CancellationToken cancellationToken)
    {
      ThrowIfDisposed();

      var waitTask = WaitNextInternal();
      try
      {
        var result = await waitTask.WaitAsync(cancellationToken).ConfigureAwait(false);
        ClearWaitTask();
        return result;
      }
      catch (OperationCanceledException)
      {
        throw;
      }
      catch (Exception)
      {
        ClearWaitTask();
        throw;
      }
    }

    private Task<NextResult> WaitNextInternal()
    {
      ThrowIfDisposed();

      _waitTaskLock.EnterUpgradeableReadLock();
      try
      {
        if (_waitTask != null)
        {
          return _waitTask;
        }

        _waitTaskLock.EnterWriteLock();
        try
        {
          _waitTask = StartWaitNextTask();
          return _waitTask;
        }
        finally
        {
          _waitTaskLock.ExitWriteLock();
        }
      }
      finally
      {
        _waitTaskLock.ExitUpgradeableReadLock();
      }
    }

    private Task<NextResult> StartWaitNextTask()
    {
      if (_poller == null)
      {
        throw new InvalidOperationException("OutputStreamPoller is not initialized. Call StartPolling before running the CalculatorGraph");
      }

      return Task<NextResult>.Factory.StartNew((state) =>
      {
        var stream = (OutputStream)state;
        if (stream.Next(out var packet)) // this blocks the thread
        {
          return new NextResult(packet, true);
        }
        return new NextResult(null, false);
      }, this);
    }

    private bool Next(out Packet packet)
    {
      var result = _poller.Next(outputPacket);
      packet = outputPacket;
      return result;
    }

    /// <remarks>
    ///   This method will aquire the write lock of <see cref="_waitTaskLock" />.
    /// </remarks>
    private void ClearWaitTask()
    {
      _waitTaskLock.EnterWriteLock();
      try
      {
        _waitTask = null;
      }
      finally
      {
        _waitTaskLock.ExitWriteLock();
      }
    }

    private void InvokeOnReceived(Packet nextPacket)
    {
      OnReceived?.Invoke(this, new OutputEventArgs(nextPacket));
    }

    private void ThrowIfDisposed()
    {
      if (_isDisposed)
      {
        throw new ObjectDisposedException(GetType().FullName);
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static StatusArgs InvokeIfOutputStreamFound(IntPtr graphPtr, int streamId, IntPtr packetPtr)
    {
      try
      {
        var isFound = _InstanceTable.TryGetValue(streamId, out var outputStream);
        if (!isFound)
        {
          return StatusArgs.NotFound($"OutputStream with id {streamId} is not found, maybe already GCed");
        }
        if (outputStream._calculatorGraph.mpPtr != graphPtr)
        {
          return StatusArgs.InvalidArgument($"OutputStream is found, but is not linked to the specified CalclatorGraph");
        }

        var packet = outputStream.referencePacket;
        packet.SwitchNativePtr(packetPtr);

        outputStream.InvokeOnReceived(packet);

        packet.ReleaseMpResource();

        return StatusArgs.Ok();
      }
      catch (Exception e)
      {
        return StatusArgs.Internal(e.ToString());
      }
    }
  }

  public class OutputStream<TPacket, TValue> where TPacket : Packet<TValue>, new()
  {
    private static int _Counter = 0;
    private static readonly GlobalInstanceTable<int, OutputStream<TPacket, TValue>> _InstanceTable = new GlobalInstanceTable<int, OutputStream<TPacket, TValue>>(20);

    protected readonly CalculatorGraph calculatorGraph;

    private readonly int _id;
    public readonly string streamName;
    public readonly string presenceStreamName;
    public readonly bool observeTimestampBounds;

    private OutputStreamPoller<TValue> _poller;
    private TPacket _outputPacket;

    private OutputStreamPoller<bool> _presencePoller;
    private BoolPacket _presencePacket;

    private long _lastTimestampMicrosec;
    private long _timeoutMicrosec;
    public long timeoutMicrosec
    {
      get => _timeoutMicrosec;
      set => _timeoutMicrosec = Math.Max(0, value);
    }

    protected event EventHandler<OutputEventArgs<TValue>> OnReceived;

    private TPacket _referencePacket;
    protected TPacket referencePacket
    {
      get
      {
        if (_referencePacket == null)
        {
          _referencePacket = Packet<TValue>.Create<TPacket>(IntPtr.Zero, false);
        }
        return _referencePacket;
      }
    }

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
    /// <param name="timeoutMicrosec">
    ///   If the output packet is empty, the <see cref="OutputStream" /> instance drops the packet until the period specified here elapses.
    /// </param>
    public OutputStream(CalculatorGraph calculatorGraph, string streamName, bool observeTimestampBounds = true, long timeoutMicrosec = 0)
    {
      _id = Interlocked.Increment(ref _Counter);
      this.calculatorGraph = calculatorGraph;
      this.streamName = streamName;
      this.observeTimestampBounds = observeTimestampBounds;
      this.timeoutMicrosec = timeoutMicrosec;

      _InstanceTable.Add(_id, this);
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
    /// <param name="timeoutMicrosec">
    ///   If the output packet is empty, the <see cref="OutputStream" /> instance drops the packet until the period specified here elapses.
    /// </param>
    public OutputStream(CalculatorGraph calculatorGraph, string streamName, string presenceStreamName, long timeoutMicrosec = 0) : this(calculatorGraph, streamName, false, timeoutMicrosec)
    {
      this.presenceStreamName = presenceStreamName;
    }

    public void StartPolling()
    {
      _outputPacket = new TPacket();
      _poller = calculatorGraph.AddOutputStreamPoller<TValue>(streamName, observeTimestampBounds);

      if (presenceStreamName == null)
      {
        return;
      }

      _presencePacket = new BoolPacket();
      _presencePoller = calculatorGraph.AddOutputStreamPoller<bool>(presenceStreamName, false);
    }

    public void AddListener(EventHandler<OutputEventArgs<TValue>> callback)
    {
      if (OnReceived == null)
      {
        calculatorGraph.ObserveOutputStream(streamName, _id, InvokeIfOutputStreamFound, observeTimestampBounds);
      }
      OnReceived += callback;
    }

    public void RemoveListener(EventHandler<OutputEventArgs<TValue>> eventHandler)
    {
      OnReceived -= eventHandler;
    }

    public void RemoveAllListeners()
    {
      OnReceived = null;
    }

    public void Close()
    {
      RemoveAllListeners();

      _poller?.Dispose();
      _poller = null;
      _outputPacket?.Dispose();
      _outputPacket = null;

      _presencePoller?.Dispose();
      _presencePoller = null;
      _presencePacket?.Dispose();
      _presencePacket = null;

      _referencePacket?.Dispose();
      _referencePacket = null;
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

      try
      {
        value = _outputPacket.Consume();
      }
      catch
      {
        value = default;
      }
      return true;
    }

    public bool TryConsumeNext(out TValue value, bool allowBlock = true)
    {
      return TryConsumeNext(out value, 0, allowBlock);
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
      if (_poller == null)
      {
        Logger.LogWarning("OutputStreamPoller is not initialized. Call StartPolling before running the CalculatorGraph");
        return false;
      }

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

    protected bool TryGetPacketValue(Packet<TValue> packet, out TValue value, long timeoutMicrosec = 0)
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

    protected bool TryConsumePacketValue(Packet<TValue> packet, out TValue value, long timeoutMicrosec = 0)
    {
      using (var timestamp = packet.Timestamp())
      {
        var currentMicrosec = timestamp.Microseconds();

        if (!packet.IsEmpty())
        {
          _lastTimestampMicrosec = currentMicrosec;

          try
          {
            value = packet.Consume();
          }
          catch
          {
            value = default;
          }
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

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    protected static StatusArgs InvokeIfOutputStreamFound(IntPtr graphPtr, int streamId, IntPtr packetPtr)
    {
      try
      {
        var isFound = _InstanceTable.TryGetValue(streamId, out var outputStream);
        if (!isFound)
        {
          return StatusArgs.NotFound($"OutputStream with id {streamId} is not found, maybe already GCed");
        }
        if (outputStream.calculatorGraph.mpPtr != graphPtr)
        {
          return StatusArgs.InvalidArgument($"OutputStream is found, but is not linked to the specified CalclatorGraph");
        }

        outputStream.referencePacket.SwitchNativePtr(packetPtr);
        if (outputStream.TryGetPacketValue(outputStream.referencePacket, out var value, outputStream.timeoutMicrosec))
        {
          outputStream.OnReceived?.Invoke(outputStream, new OutputEventArgs<TValue>(value));
        }
        outputStream.referencePacket.ReleaseMpResource();

        return StatusArgs.Ok();
      }
      catch (Exception e)
      {
        return StatusArgs.Internal(e.ToString());
      }
    }
  }
}
