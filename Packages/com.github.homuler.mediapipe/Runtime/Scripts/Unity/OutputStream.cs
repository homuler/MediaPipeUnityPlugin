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
  public sealed class OutputStream<T> : IDisposable
  {
    public readonly struct OutputEventArgs
    {
      /// <summary>
      ///   <see cref="Packet"/> that contains the output value.
      ///   As long as it's not <see langword="null"/>, it's guaranteed that <see cref="Packet.IsEmpty"/> is <see langword="false"/>.
      /// </summary>
      public readonly Packet<T> packet;
      public readonly long timestampMicrosecond;

      internal OutputEventArgs(Packet<T> packet, long timestampMicrosecond)
      {
        this.packet = packet;
        this.timestampMicrosecond = timestampMicrosecond;
      }
    }

    public readonly struct NextResult
    {
      /// <summary>
      ///   <see cref="Packet"/> that contains the output value.
      ///   As long as it's not <see langword="null"/>, it's guaranteed that <see cref="Packet.IsEmpty"/> is <see langword="false"/>.
      /// </summary>
      public readonly Packet<T> packet;
      /// <summary>
      ///   <see langword="true"/> if the next packet is retrieved successfully; otherwise <see langword="false"/>.
      /// </summary>
      public readonly bool ok;

      public NextResult(Packet<T> packet, bool ok)
      {
        this.packet = packet;
        this.ok = ok;
      }
    }

    private static int _Counter = 0;
    private static readonly GlobalInstanceTable<int, OutputStream<T>> _InstanceTable = new GlobalInstanceTable<int, OutputStream<T>>(20);

    private CalculatorGraph _calculatorGraph;

    private readonly int _id;
    public readonly string streamName;
    public readonly bool observeTimestampBounds;

    private OutputStreamPoller<T> _poller;
    private Packet<T> _outputPacket;
    private Packet<T> outputPacket
    {
      get
      {
        if (_outputPacket == null)
        {
          _outputPacket = new Packet<T>();
          _outputPacket.Lock();
        }
        return _outputPacket;
      }
    }

    private readonly ReaderWriterLockSlim _waitTaskLock = new ReaderWriterLockSlim();
    /// <remarks>
    ///   Only the <see cref="_waitTask"/> can modify <see cref="_outputPacket"/>.
    /// </remarks>
    private Task<NextResult> _waitTask;

    private event EventHandler<OutputEventArgs> OnReceived;
    private long _lastTimestampMicrosec;

    private Packet<T> _referencePacket;
    private Packet<T> referencePacket
    {
      get
      {
        if (_referencePacket == null)
        {
          _referencePacket = Packet<T>.CreateForReference(IntPtr.Zero);
          _referencePacket.Lock();
        }
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

      if (_outputPacket != null)
      {
        _outputPacket.Unlock();
        _outputPacket.Dispose();
        _outputPacket = null;
      }

      if (_referencePacket != null)
      {
        _referencePacket.Unlock();
        _referencePacket.Dispose();
        _referencePacket = null;
      }
    }

    ~OutputStream()
    {
      Dispose(false);
    }

    public void StartPolling()
    {
      ThrowIfDisposed();

      _poller = _calculatorGraph.AddOutputStreamPoller<T>(streamName, observeTimestampBounds);
    }

    /// <summary>
    /// </summary>
    /// <remarks>
    ///   When <see cref="observeTimestampBounds" /> is set to <see langword="true"/>, <see cref="callback"/> can be invoked with an empty packet.
    ///   However, in some cases, most of the output packets can be empty even if the output must be present.<br/>
    ///   In that case, specify <paramref name="emptyPacketThresholdMicrosecond" /> to mitigate the problem.
    /// </remarks>
    /// <param name="callback"></param>
    /// <param name="emptyPacketThresholdMicrosecond">
    ///   When <see cref="observeTimestampBounds" /> is set to <see langword="true"/>, <see cref="callback"/> can be invoked with an empty packet.
    ///   However, in some cases, most of the output packets can be empty even if the output must be present.<br/>
    ///
    ///   This parameter specifies the duration for ignoring empty packets.
    ///   That is, when receiving an empty packet, <paramref name="callback"/> will not be invoked
    ///   unless the specified time has elapsed since the last non-empty output was received.
    /// </param>
    public void AddListener(EventHandler<OutputEventArgs> callback, long emptyPacketThresholdMicrosecond = 0)
    {
      ThrowIfDisposed();

      if (OnReceived == null)
      {
        _calculatorGraph.ObserveOutputStream(streamName, _id, InvokeIfOutputStreamFound, observeTimestampBounds);
      }

      if (emptyPacketThresholdMicrosecond <= 0)
      {
        OnReceived += callback;
        return;
      }

      OnReceived += (sender, eventArgs) =>
      {
        var stream = (OutputStream<T>)sender;
        if (eventArgs.packet == null && eventArgs.timestampMicrosecond - stream._lastTimestampMicrosec <= emptyPacketThresholdMicrosecond)
        {
          return;
        }
        callback(stream, eventArgs);
      };
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
        var stream = (OutputStream<T>)state;
        if (stream.Next(out var packet)) // this blocks the thread
        {
          if (packet.IsEmpty())
          {
            return new NextResult(null, true);
          }
          return new NextResult(packet, true);
        }
        return new NextResult(null, false);
      }, this);
    }

    private bool Next(out Packet<T> packet)
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

    private void InvokeOnReceived(Packet<T> nextPacket)
    {
      var isEmpty = nextPacket.IsEmpty();
      var timestampMicrosec = nextPacket.TimestampMicroseconds();
      if (!isEmpty)
      {
        // not thread-safe
        _lastTimestampMicrosec = Math.Max(timestampMicrosec, _lastTimestampMicrosec);
      }
      OnReceived?.Invoke(this, new OutputEventArgs(isEmpty ? null : nextPacket, timestampMicrosec));
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
}
