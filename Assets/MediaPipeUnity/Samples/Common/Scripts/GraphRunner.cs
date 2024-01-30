// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Rendering;
#endif

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Sample
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

    protected RunningMode runningMode { get; private set; } = RunningMode.Async;
    private bool _isRunning = false;

    public InferenceMode inferenceMode => configType == ConfigType.CPU ? InferenceMode.CPU : InferenceMode.GPU;
    public virtual ConfigType configType
    {
      get
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
    }
    public TextAsset textConfig
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
      set => _timeoutMicrosec = (long)Mathf.Max(0, value);
    }
    public long timeoutMillisec
    {
      get => timeoutMicrosec / 1000;
      set => timeoutMicrosec = value * 1000;
    }

    public RotationAngle rotation { get; private set; } = 0;

    private Stopwatch _stopwatch;
    protected CalculatorGraph calculatorGraph { get; private set; }
    protected long latestTimestamp;

    protected virtual void Start()
    {
      _InstanceTable.Add(GetInstanceID(), this);
    }

    protected virtual void OnDestroy()
    {
      Stop();
    }

    public WaitForResult WaitForInit(RunningMode runningMode)
    {
      return new WaitForResult(this, Initialize(runningMode));
    }

    public virtual IEnumerator Initialize(RunningMode runningMode)
    {
      this.runningMode = runningMode;

      Debug.Log($"Config Type = {configType}");
      Debug.Log($"Running Mode = {runningMode}");

      InitializeCalculatorGraph();
      _stopwatch = new Stopwatch();
      _stopwatch.Start();

      Debug.Log("Loading dependent assets...");
      var assetRequests = RequestDependentAssets();
      yield return new WaitWhile(() => assetRequests.Any((request) => request.keepWaiting));

      var errors = assetRequests.Where((request) => request.isError).Select((request) => request.error).ToList();
      if (errors.Count > 0)
      {
        foreach (var error in errors)
        {
          Debug.LogError(error);
        }
        throw new InternalException("Failed to prepare dependent assets");
      }
    }

    public abstract void StartRun(ImageSource imageSource);

    protected void StartRun(PacketMap sidePacket)
    {
      calculatorGraph.StartRun(sidePacket);
      _isRunning = true;
    }

    public virtual void Stop()
    {
      if (calculatorGraph != null)
      {
        if (_isRunning)
        {
          try
          {
            calculatorGraph.CloseAllPacketSources();
          }
          catch (BadStatusException exception)
          {
            Debug.LogError(exception);
          }

          try
          {
            calculatorGraph.WaitUntilDone();
          }
          catch (BadStatusException exception)
          {
            Debug.LogError(exception);
          }
        }

        _isRunning = false;
        var _ = _NameTable.Remove(calculatorGraph.mpPtr);
        calculatorGraph.Dispose();
        calculatorGraph = null;
      }

      if (_stopwatch != null && _stopwatch.IsRunning)
      {
        _stopwatch.Stop();
      }
    }

    protected void AddPacketToInputStream<T>(string streamName, Packet<T> packet)
    {
      calculatorGraph.AddPacketToInputStream(streamName, packet);
    }

    protected void AddTextureFrameToInputStream(string streamName, TextureFrame textureFrame)
    {
      latestTimestamp = GetCurrentTimestampMicrosec();

      if (configType == ConfigType.OpenGLES)
      {
        var gpuBuffer = textureFrame.BuildGpuBuffer(GpuManager.GlCalculatorHelper.GetGlContext());
        AddPacketToInputStream(streamName, Packet.CreateGpuBufferAt(gpuBuffer, latestTimestamp));
        return;
      }

      var imageFrame = textureFrame.BuildImageFrame();
      textureFrame.Release();

      AddPacketToInputStream(streamName, Packet.CreateImageFrameAt(imageFrame, latestTimestamp));
    }

    protected bool TryGetValue<T>(Packet<T> packet, out T value, Func<Packet<T>, T> getter)
    {
      if (packet == null)
      {
        value = default;
        return false;
      }
      value = getter(packet);
      return true;
    }

    protected void AssertResult<T>(OutputStream<T>.NextResult result)
    {
      if (!result.ok)
      {
        throw new Exception("Failed to get the next packet");
      }
    }

    protected void AssertResult<T1, T2>((OutputStream<T1>.NextResult, OutputStream<T2>.NextResult) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
    }

    protected void AssertResult<T1, T2, T3>((OutputStream<T1>.NextResult, OutputStream<T2>.NextResult, OutputStream<T3>.NextResult) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
      AssertResult(result.Item3);
    }

    protected void AssertResult<T1, T2, T3, T4>((OutputStream<T1>.NextResult, OutputStream<T2>.NextResult, OutputStream<T3>.NextResult, OutputStream<T4>.NextResult) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
      AssertResult(result.Item3);
      AssertResult(result.Item4);
    }

    protected void AssertResult<T1, T2, T3, T4, T5>(
      (
        OutputStream<T1>.NextResult,
        OutputStream<T2>.NextResult,
        OutputStream<T3>.NextResult,
        OutputStream<T4>.NextResult,
        OutputStream<T5>.NextResult
      ) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
      AssertResult(result.Item3);
      AssertResult(result.Item4);
      AssertResult(result.Item5);
    }

    protected void AssertResult<T1, T2, T3, T4, T5, T6>(
      (
        OutputStream<T1>.NextResult,
        OutputStream<T2>.NextResult,
        OutputStream<T3>.NextResult,
        OutputStream<T4>.NextResult,
        OutputStream<T5>.NextResult,
        OutputStream<T6>.NextResult
      ) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
      AssertResult(result.Item3);
      AssertResult(result.Item4);
      AssertResult(result.Item5);
      AssertResult(result.Item6);
    }

    protected void AssertResult<T1, T2, T3, T4, T5, T6, T7>(
      (
        OutputStream<T1>.NextResult,
        OutputStream<T2>.NextResult,
        OutputStream<T3>.NextResult,
        OutputStream<T4>.NextResult,
        OutputStream<T5>.NextResult,
        OutputStream<T6>.NextResult,
        OutputStream<T7>.NextResult
      ) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
      AssertResult(result.Item3);
      AssertResult(result.Item4);
      AssertResult(result.Item5);
      AssertResult(result.Item6);
      AssertResult(result.Item7);
    }

    protected void AssertResult<T1, T2, T3, T4, T5, T6, T7, T8>(
      (
        OutputStream<T1>.NextResult,
        OutputStream<T2>.NextResult,
        OutputStream<T3>.NextResult,
        OutputStream<T4>.NextResult,
        OutputStream<T5>.NextResult,
        OutputStream<T6>.NextResult,
        OutputStream<T7>.NextResult,
        OutputStream<T8>.NextResult
      ) result)
    {
      AssertResult(result.Item1);
      AssertResult(result.Item2);
      AssertResult(result.Item3);
      AssertResult(result.Item4);
      AssertResult(result.Item5);
      AssertResult(result.Item6);
      AssertResult(result.Item7);
      AssertResult(result.Item8);
    }

    protected async Task<(T1, T2)> WhenAll<T1, T2>(Task<T1> task1, Task<T2> task2)
    {
      await Task.WhenAll(task1, task2);
      return (task1.Result, task2.Result);
    }

    protected async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
    {
      await Task.WhenAll(task1, task2, task3);
      return (task1.Result, task2.Result, task3.Result);
    }

    protected async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4)
    {
      await Task.WhenAll(task1, task2, task3, task4);
      return (task1.Result, task2.Result, task3.Result, task4.Result);
    }

    protected async Task<(T1, T2, T3, T4, T5)> WhenAll<T1, T2, T3, T4, T5>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5)
    {
      await Task.WhenAll(task1, task2, task3, task4, task5);
      return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
    }

    protected async Task<(T1, T2, T3, T4, T5, T6)> WhenAll<T1, T2, T3, T4, T5, T6>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6)
    {
      await Task.WhenAll(task1, task2, task3, task4, task5, task6);
      return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result);
    }

    protected async Task<(T1, T2, T3, T4, T5, T6, T7)> WhenAll<T1, T2, T3, T4, T5, T6, T7>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7)
    {
      await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7);
      return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result);
    }

    protected async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> WhenAll<T1, T2, T3, T4, T5, T6, T7, T8>(Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5, Task<T6> task6, Task<T7> task7, Task<T8> task8)
    {
      await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8);
      return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result, task6.Result, task7.Result, task8.Result);
    }

    protected long GetCurrentTimestampMicrosec()
    {
      return _stopwatch == null || !_stopwatch.IsRunning ? -1 : _stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
    }

    protected void InitializeCalculatorGraph()
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
      var baseConfig = textConfig == null ? null : CalculatorGraphConfig.Parser.ParseFromTextFormat(textConfig.text);
      if (baseConfig == null)
      {
        throw new InvalidOperationException("Failed to get the text config. Check if the config is set to GraphRunner");
      }
      ConfigureCalculatorGraph(baseConfig);

      if (inferenceMode != InferenceMode.CPU)
      {
        calculatorGraph.SetGpuResources(GpuManager.GpuResources);
      }
    }

    /// <summary>
    ///   Configure and initialize the <see cref="CalculatorGraph" />.
    /// </summary>
    /// <remarks>
    ///   This is the main process in <see cref="InitializeCalculatorGraph" />.<br />
    ///   At least, <c>calculatorGraph.Initialize</c> must be called here.
    ///   In addition to that, <see cref="OutputStream" /> instances should be initialized.
    /// </remarks>
    /// <param name="config">
    ///   A <see cref="CalculatorGraphConfig" /> instance corresponding to <see cref="textConfig" />.<br />
    ///   It can be dynamically modified here.
    /// </param>
    protected virtual void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      calculatorGraph.Initialize(config);
    }

    protected void SetImageTransformationOptions(PacketMap sidePacket, ImageSource imageSource, bool expectedToBeMirrored = false)
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

      Debug.Log($"input_rotation = {inputRotation}, input_horizontally_flipped = {inputHorizontallyFlipped}, input_vertically_flipped = {inputVerticallyFlipped}");

      sidePacket.Emplace("input_rotation", Packet.CreateInt((int)inputRotation));
      sidePacket.Emplace("input_horizontally_flipped", Packet.CreateBool(inputHorizontallyFlipped));
      sidePacket.Emplace("input_vertically_flipped", Packet.CreateBool(inputVerticallyFlipped));
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
  }
}
