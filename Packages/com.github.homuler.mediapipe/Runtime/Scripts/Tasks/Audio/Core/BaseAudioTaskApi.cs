// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe.Tasks.Audio.Core
{
  /// <summary>
  ///   The base class of the user-facing mediapipe audio task api classes.
  /// </summary>
  public class BaseAudioTaskApi : IDisposable
  {
    private readonly Tasks.Core.TaskRunner _taskRunner;
    public RunningMode runningMode { get; }
    private bool _isClosed = false;

    /// <summary>
    ///   Initializes the `BaseAudioTaskApi` object.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///   The packet callback is not properly set based on the task's running mode.
    /// </exception>
    protected BaseAudioTaskApi(
      CalculatorGraphConfig graphConfig,
      RunningMode runningMode,
      Tasks.Core.TaskRunner.PacketsCallback packetsCallback)
    {
      if (runningMode == RunningMode.AUDIO_STREAM)
      {
        if (packetsCallback == null)
        {
          throw new ArgumentException("The audio task is in audio stream mode, a user-defined result callback must be provided.");
        }
      }
      else if (packetsCallback != null)
      {
        throw new ArgumentException("The audio task is in audio clips mode, a user-defined result callback should not be provided.");
      }

      var (callbackId, nativePacketsCallback) = Tasks.Core.PacketsCallbackTable.Add(packetsCallback);
      _taskRunner = Tasks.Core.TaskRunner.Create(graphConfig, callbackId, nativePacketsCallback);
      this.runningMode = runningMode;
    }

    /// <summary>
    ///   A synchronous method to process independent audio clips.
    ///
    ///   The call blocks the current thread until a failure status or a successful result is returned.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   If the task's running mode is not set to audio clips mode.
    /// </exception>
    protected PacketMap ProcessAudioClip(PacketMap inputs)
    {
      if (runningMode != RunningMode.AUDIO_CLIPS)
      {
        throw new InvalidOperationException($"Task is not initialized with the audio clips mode. Current running mode: {runningMode}");
      }
      return _taskRunner.Process(inputs);
    }

    /// <summary>
    ///   An asynchronous method to set audio sample rate in the audio stream mode.
    /// </summary>
    /// <param name="sampleRateStreamName">The audio sample rate stream name.</param>
    /// <param name="sampleRate">The audio sample rate.</param>
    /// <exception cref="InvalidOperationException">
    ///   If the task's running mode is not set to audio stream mode.
    /// </exception>
    protected void SetSampleRate(string sampleRateStreamName, double sampleRate)
    {
      if (runningMode != RunningMode.AUDIO_STREAM)
      {
        throw new InvalidOperationException($"Task is not initialized with the audio stream mode. Current running mode: {runningMode}");
      }

      using var timestamp = Timestamp.PreStream();
      var packetMap = new PacketMap();
      // NOTE: to set special timestamp, we cannot use CreateDoubleAt
      packetMap.Emplace(sampleRateStreamName, Packet.CreateDouble(sampleRate).At(timestamp));
      _taskRunner.Send(packetMap);
    }

    /// <summary>
    ///   An asynchronous method to send audio stream data to the runner.
    ///
    ///   The results will be available in the user-defined results callback.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   If the task's running mode is not set to audio stream mode.
    /// </exception>
    protected void SendAudioStreamData(PacketMap inputs)
    {
      if (runningMode != RunningMode.AUDIO_STREAM)
      {
        throw new InvalidOperationException($"Task is not initialized with the audio stream mode. Current running mode: {runningMode}");
      }
      _taskRunner.Send(inputs);
    }

    /// <summary>
    ///   Shuts down the mediapipe audio task instance.
    /// </summary>
    /// <exception cref="BadStatusException">
    ///   If the mediapipe audio task failed to close.
    /// </exception>
    public void Close()
    {
      _taskRunner.Close();
      _isClosed = true;
    }

    void IDisposable.Dispose()
    {
      if (!_isClosed)
      {
        Close();
      }
      _taskRunner.Dispose();
    }
  }
}
