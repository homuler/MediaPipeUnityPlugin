// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity
{
  public abstract class GraphRunner : MonoBehaviour
  {
    public enum ConfigType
    {
      None,
      CPU,
      GPU,
      OpenGLES,
    }

#pragma warning disable IDE1006
    // TODO: make it static
    protected string TAG => GetType().Name;
#pragma warning restore IDE1006

    [SerializeField] private TextAsset _cpuConfig = null;
    [SerializeField] private TextAsset _gpuConfig = null;
    [SerializeField] private TextAsset _openGlEsConfig = null;
    [SerializeField] private long _timeoutMicrosec = 0;

    private static readonly GlobalInstanceTable<int, GraphRunner> _InstanceTable = new GlobalInstanceTable<int, GraphRunner>(5);
    private static readonly Dictionary<IntPtr, int> _NameTable = new Dictionary<IntPtr, int>();

    public InferenceMode inferenceMode => configType == ConfigType.CPU ? InferenceMode.CPU : InferenceMode.GPU;
    public ConfigType configType { get; private set; }
    public TextAsset config
    {
      get
      {
        switch (configType)
        {
          case ConfigType.CPU: return _cpuConfig;
          case ConfigType.GPU: return _gpuConfig;
          case ConfigType.OpenGLES: return _openGlEsConfig;
          case ConfigType.None:
          default: return null;
        }
      }
    }

    public long timeoutMicrosec
    {
      get => _timeoutMicrosec;
      private set => _timeoutMicrosec = value;
    }
    public long timeoutMillisec => timeoutMicrosec / 1000;

    public RotationAngle rotation { get; private set; } = 0;

    private Stopwatch _stopwatch;
    protected CalculatorGraph calculatorGraph { get; private set; }
    protected Timestamp currentTimestamp;

    protected virtual void Start()
    {
      _InstanceTable.Add(GetInstanceID(), this);
    }

    protected virtual void OnDestroy()
    {
      Stop();
    }

    public WaitForResult WaitForInit()
    {
      return new WaitForResult(this, Initialize());
    }

    public virtual IEnumerator Initialize()
    {
      configType = DetectConfigType();
      Logger.LogInfo(TAG, $"Using {configType} config");

      if (configType == ConfigType.None)
      {
        throw new InvalidOperationException("Failed to detect config. Check if config is set to GraphRunner");
      }

      InitializeCalculatorGraph().AssertOk();
      _stopwatch = new Stopwatch();
      _stopwatch.Start();

      Logger.LogInfo(TAG, "Loading dependent assets...");
      var assetRequests = RequestDependentAssets();
      yield return new WaitWhile(() => assetRequests.Any((request) => request.keepWaiting));

      var errors = assetRequests.Where((request) => request.isError).Select((request) => request.error).ToList();
      if (errors.Count > 0)
      {
        foreach (var error in errors)
        {
          Logger.LogError(TAG, error);
        }
        throw new InternalException("Failed to prepare dependent assets");
      }
    }

    public abstract Status StartRun(ImageSource imageSource);

    public virtual void Stop()
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

      var _ = _NameTable.Remove(calculatorGraph.mpPtr);
      calculatorGraph.Dispose();
      calculatorGraph = null;

      if (_stopwatch != null && _stopwatch.IsRunning)
      {
        _stopwatch.Stop();
      }
    }

    public Status AddPacketToInputStream<T>(string streamName, Packet<T> packet)
    {
      return calculatorGraph.AddPacketToInputStream(streamName, packet);
    }

    public Status AddTextureFrameToInputStream(string streamName, TextureFrame textureFrame)
    {
      currentTimestamp = GetCurrentTimestamp();

      if (configType == ConfigType.OpenGLES)
      {
        var gpuBuffer = textureFrame.BuildGpuBuffer(GpuManager.GlCalculatorHelper.GetGlContext());
        return calculatorGraph.AddPacketToInputStream(streamName, new GpuBufferPacket(gpuBuffer, currentTimestamp));
      }

      var imageFrame = textureFrame.BuildImageFrame();
      textureFrame.Release();

      return AddPacketToInputStream(streamName, new ImageFramePacket(imageFrame, currentTimestamp));
    }

    public void SetTimeoutMicrosec(long timeoutMicrosec)
    {
      this.timeoutMicrosec = (long)Mathf.Max(0, timeoutMicrosec);
    }

    public void SetTimeoutMillisec(long timeoutMillisec)
    {
      SetTimeoutMicrosec(1000 * timeoutMillisec);
    }

    protected static bool TryGetGraphRunner(IntPtr graphPtr, out GraphRunner graphRunner)
    {
      var isInstanceIdFound = _NameTable.TryGetValue(graphPtr, out var instanceId);

      if (isInstanceIdFound)
      {
        return _InstanceTable.TryGetValue(instanceId, out graphRunner);
      }
      graphRunner = null;
      return false;
    }

    protected static Status InvokeIfGraphRunnerFound<T>(IntPtr graphPtr, IntPtr packetPtr, Action<T, IntPtr> action) where T : GraphRunner
    {
      try
      {
        var isFound = TryGetGraphRunner(graphPtr, out var graphRunner);
        if (!isFound)
        {
          return Status.FailedPrecondition("Graph runner is not found");
        }
        var graph = (T)graphRunner;
        action(graph, packetPtr);
        return Status.Ok();
      }
      catch (Exception e)
      {
        return Status.FailedPrecondition(e.ToString());
      }
    }

    protected static Status InvokeIfGraphRunnerFound<T>(IntPtr graphPtr, Action<T> action) where T : GraphRunner
    {
      return InvokeIfGraphRunnerFound<T>(graphPtr, IntPtr.Zero, (graph, ptr) => { action(graph); });
    }

    protected bool TryGetPacketValue<T>(Packet<T> packet, ref long prevMicrosec, out T value) where T : class
    {
      long currentMicrosec = 0;
      using (var timestamp = packet.Timestamp())
      {
        currentMicrosec = timestamp.Microseconds();
      }

      if (!packet.IsEmpty())
      {
        prevMicrosec = currentMicrosec;
        value = packet.Get();
        return true;
      }

      value = null;
      return currentMicrosec - prevMicrosec > timeoutMicrosec;
    }

    protected bool TryConsumePacketValue<T>(Packet<T> packet, ref long prevMicrosec, out T value) where T : class
    {
      long currentMicrosec = 0;
      using (var timestamp = packet.Timestamp())
      {
        currentMicrosec = timestamp.Microseconds();
      }

      if (!packet.IsEmpty())
      {
        prevMicrosec = currentMicrosec;
        var statusOrValue = packet.Consume();

        value = statusOrValue.ValueOr();
        return true;
      }

      value = null;
      return currentMicrosec - prevMicrosec > timeoutMicrosec;
    }

    protected Timestamp GetCurrentTimestamp()
    {
      if (_stopwatch == null || !_stopwatch.IsRunning)
      {
        return Timestamp.Unset();
      }
      var microseconds = _stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
      return new Timestamp(microseconds);
    }

    protected Status InitializeCalculatorGraph()
    {
      calculatorGraph = new CalculatorGraph();
      _NameTable.Add(calculatorGraph.mpPtr, GetInstanceID());

      // NOTE: There's a simpler way to initialize CalculatorGraph.
      //
      //     calculatorGraph = new CalculatorGraph(config.text);
      //
      //   However, if the config format is invalid, this code does not initialize CalculatorGraph and does not throw exceptions either.
      //   The problem is that if you call ObserveStreamOutput in this state, the program will crash.
      //   The following code is not very efficient, but it will return Non-OK status when an invalid configuration is given.
      try
      {
        var calculatorGraphConfig = GetCalculatorGraphConfig();
        var status = calculatorGraph.Initialize(calculatorGraphConfig);

        return !status.Ok() || inferenceMode == InferenceMode.CPU ? status : calculatorGraph.SetGpuResources(GpuManager.GpuResources);
      }
      catch (Exception e)
      {
        return Status.FailedPrecondition(e.ToString());
      }
    }

    protected virtual CalculatorGraphConfig GetCalculatorGraphConfig()
    {
      return CalculatorGraphConfig.Parser.ParseFromTextFormat(config.text);
    }

    protected void SetImageTransformationOptions(SidePacket sidePacket, ImageSource imageSource, bool expectedToBeMirrored = false)
    {
      // NOTE: The origin is left-bottom corner in Unity, and right-top corner in MediaPipe.
      rotation = imageSource.rotation.Reverse();
      var inputRotation = rotation;
      var isInverted = CoordinateSystem.ImageCoordinate.IsInverted(rotation);
      var shouldBeMirrored = imageSource.isHorizontallyFlipped ^ expectedToBeMirrored;
      var inputHorizontallyFlipped = isInverted ^ shouldBeMirrored;
      var inputVerticallyFlipped = !isInverted;

      if ((inputHorizontallyFlipped && inputVerticallyFlipped) || rotation == RotationAngle.Rotation180)
      {
        inputRotation = inputRotation.Add(RotationAngle.Rotation180);
        inputHorizontallyFlipped = !inputHorizontallyFlipped;
        inputVerticallyFlipped = !inputVerticallyFlipped;
      }

      Logger.LogDebug($"input_rotation = {inputRotation}, input_horizontally_flipped = {inputHorizontallyFlipped}, input_vertically_flipped = {inputVerticallyFlipped}");

      sidePacket.Emplace("input_rotation", new IntPacket((int)inputRotation));
      sidePacket.Emplace("input_horizontally_flipped", new BoolPacket(inputHorizontallyFlipped));
      sidePacket.Emplace("input_vertically_flipped", new BoolPacket(inputVerticallyFlipped));
    }

    protected virtual ConfigType DetectConfigType()
    {
      if (GpuManager.IsInitialized)
      {
#if UNITY_ANDROID
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && _openGlEsConfig != null)
        {
          return ConfigType.OpenGLES;
        }
#endif
        if (_gpuConfig != null)
        {
          return ConfigType.GPU;
        }
      }
      return _cpuConfig != null ? ConfigType.CPU : ConfigType.None;
    }

    protected WaitForResult WaitForAsset(string assetName, string uniqueKey, long timeoutMillisec, bool overwrite = false)
    {
      return new WaitForResult(this, AssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite), timeoutMillisec);
    }

    protected WaitForResult WaitForAsset(string assetName, long timeoutMillisec, bool overwrite = false)
    {
      return WaitForAsset(assetName, assetName, timeoutMillisec, overwrite);
    }

    protected WaitForResult WaitForAsset(string assetName, string uniqueKey, bool overwrite = false)
    {
      return new WaitForResult(this, AssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite));
    }

    protected WaitForResult WaitForAsset(string assetName, bool overwrite = false)
    {
      return WaitForAsset(assetName, assetName, overwrite);
    }

    protected abstract IList<WaitForResult> RequestDependentAssets();

    protected class OutputStream<TPacket, TValue> where TPacket : Packet<TValue>, new()
    {
      private readonly CalculatorGraph _calculatorGraph;

      private readonly string _streamName;
      private OutputStreamPoller<TValue> _poller;
      private TPacket _outputPacket;

      private string _presenceStreamName;
      private OutputStreamPoller<bool> _presencePoller;
      private BoolPacket _presencePacket;

      private bool canFreeze => _presenceStreamName != null;

      public OutputStream(CalculatorGraph calculatorGraph, string streamName)
      {
        _calculatorGraph = calculatorGraph;
        _streamName = streamName;
      }

      public Status StartPolling(bool observeTimestampBounds = false)
      {
        _outputPacket = new TPacket();

        var statusOrPoller = _calculatorGraph.AddOutputStreamPoller<TValue>(_streamName, observeTimestampBounds);
        var status = statusOrPoller.status;
        if (status.Ok())
        {
          _poller = statusOrPoller.Value();
        }
        return status;
      }

      public Status StartPolling(string presenceStreamName)
      {
        _presenceStreamName = presenceStreamName;
        var status = StartPolling(false);

        if (status.Ok())
        {
          _presencePacket = new BoolPacket();

          var statusOrPresencePoller = _calculatorGraph.AddOutputStreamPoller<bool>(presenceStreamName);
          status = statusOrPresencePoller.status;
          if (status.Ok())
          {
            _presencePoller = statusOrPresencePoller.Value();
          }
        }
        return status;
      }

      public Status AddListener(CalculatorGraph.NativePacketCallback callback, bool observeTimestampBounds = false)
      {
        return _calculatorGraph.ObserveOutputStream(_streamName, callback, observeTimestampBounds);
      }

      public bool TryGetNext(out TValue value)
      {
        if (HasNextValue())
        {
          value = _outputPacket.Get();
          return true;
        }
        value = default;
        return false;
      }

      public bool TryGetLatest(out TValue value)
      {
        if (HasNextValue())
        {
          var queueSize = _poller.QueueSize();

          // Assume that queue size will not be reduced from another thread.
          while (queueSize-- > 0)
          {
            if (!Next())
            {
              value = default;
              return false;
            }
          }
          value = _outputPacket.Get();
          return true;
        }
        value = default;
        return false;
      }

      private bool HasNextValue()
      {
        if (canFreeze)
        {
          if (!NextPresence() || _presencePacket.IsEmpty() || !_presencePacket.Get())
          {
            // NOTE: IsEmpty() should always return false
            return false;
          }
        }
        return Next() && !_outputPacket.IsEmpty();
      }

      private bool NextPresence()
      {
        return Next(_presencePoller, _presencePacket, _presenceStreamName);
      }

      private bool Next()
      {
        return Next(_poller, _outputPacket, _streamName);
      }

      private static bool Next<T>(OutputStreamPoller<T> poller, Packet<T> packet, string streamName)
      {
        if (!poller.Next(packet))
        {
          Logger.LogWarning($"Failed to get next value from {streamName}, so there may be errors inside the calculatorGraph. See logs for more details");
          return false;
        }
        return true;
      }
    }
  }
}
