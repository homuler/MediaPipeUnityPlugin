// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe.Tasks.Vision.Core
{
  /// <summary>
  ///   The base class of the user-facing mediapipe vision task api classes.
  /// </summary>
  public class BaseVisionTaskApi : IDisposable
  {
    private readonly Tasks.Core.TaskRunner _taskRunner;
    public RunningMode runningMode { get; }
    private bool _isClosed = false;

    /// <summary>
    ///   Initializes the `BaseVisionTaskApi` object.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///   The packet callback is not properly set based on the task's running mode.
    /// </exception>
    protected BaseVisionTaskApi(
      CalculatorGraphConfig graphConfig,
      RunningMode runningMode,
      Tasks.Core.TaskRunner.PacketsCallback packetsCallback) : this(graphConfig, runningMode, null, packetsCallback) { }

    /// <summary>
    ///   Initializes the `BaseVisionTaskApi` object.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///   The packet callback is not properly set based on the task's running mode.
    /// </exception>
    protected BaseVisionTaskApi(
      CalculatorGraphConfig graphConfig,
      RunningMode runningMode,
      GpuResources gpuResources,
      Tasks.Core.TaskRunner.PacketsCallback packetsCallback)
    {
      if (runningMode == RunningMode.LIVE_STREAM)
      {
        if (packetsCallback == null)
        {
          throw new ArgumentException("The vision task is in live stream mode, a user-defined result callback must be provided.");
        }
      }
      else if (packetsCallback != null)
      {
        throw new ArgumentException("The vision task is in image or video mode, a user-defined result callback should not be provided.");
      }

      var (callbackId, nativePacketsCallback) = Tasks.Core.PacketsCallbackTable.Add(packetsCallback);

      if (gpuResources != null)
      {
        _taskRunner = Tasks.Core.TaskRunner.Create(graphConfig, gpuResources, callbackId, nativePacketsCallback);
      }
      else
      {
        _taskRunner = Tasks.Core.TaskRunner.Create(graphConfig, callbackId, nativePacketsCallback);
      }
      this.runningMode = runningMode;
    }

    /// <summary>
    ///   A synchronous method to process single image inputs.
    ///   The call blocks the current thread until a failure status or a successful result is returned.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   If the task's running mode is not set to the image mode.
    /// </exception>
    protected PacketMap ProcessImageData(PacketMap inputs)
    {
      if (runningMode != RunningMode.IMAGE)
      {
        throw new InvalidOperationException($"Task is not initialized with the image mode. Current running mode: {runningMode}");
      }
      return _taskRunner.Process(inputs);
    }

    /// <summary>
    ///   A synchronous method to process continuous video frames.
    ///   The call blocks the current thread until a failure status or a successful result is returned.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   If the task's running mode is not set to the video mode.
    /// </exception>
    protected PacketMap ProcessVideoData(PacketMap inputs)
    {
      if (runningMode != RunningMode.VIDEO)
      {
        throw new InvalidOperationException($"Task is not initialized with the video mode. Current running mode: {runningMode}");
      }
      return _taskRunner.Process(inputs);
    }

    /// <summary>
    ///   An asynchronous method to send live stream data to the runner.
    ///   The results will be available in the user-defined results callback.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///   If the task's running mode is not set to the live stream mode.
    /// </exception>
    protected void SendLiveStreamData(PacketMap inputs)
    {
      if (runningMode != RunningMode.LIVE_STREAM)
      {
        throw new InvalidOperationException($"Task is not initialized with the live stream mode. Current running mode: {runningMode}");
      }
      _taskRunner.Send(inputs);
    }

    private void ResetNormalizedRect(NormalizedRect normalizedRect)
    {
      normalizedRect.Rotation = 0;
      normalizedRect.XCenter = 0.5f;
      normalizedRect.YCenter = 0.5f;
      normalizedRect.Width = 1;
      normalizedRect.Height = 1;
    }

    protected void ConfigureNormalizedRect(NormalizedRect target, ImageProcessingOptions? options, Image image, bool roiAllowed = true)
    {
      ResetNormalizedRect(target);

      if (!(options is ImageProcessingOptions optionsValue))
      {
        return;
      }

      if (optionsValue.rotationDegrees % 90 != 0)
      {
        throw new ArgumentException("Expected rotation to be a multiple of 90°.");
      }

      // Convert to radians counter-clockwise.
      // TODO: use System.MathF.PI
      target.Rotation = -optionsValue.rotationDegrees * UnityEngine.Mathf.PI / 180.0f;

      if (optionsValue.regionOfInterest is Components.Containers.RectF roi)
      {
        if (!roiAllowed)
        {
          throw new ArgumentException("This task doesn't support region-of-interest.");
        }

        if (roi.left >= roi.right || roi.top >= roi.bottom)
        {
          throw new ArgumentException("Expected RectF with left < right and top < bottom.");
        }
        if (roi.left < 0 || roi.top < 0 || roi.right > 1 || roi.bottom > 1)
        {
          throw new ArgumentException("Expected RectF values to be in [0,1].");
        }

        target.XCenter = (roi.left + roi.right) / 2.0f;
        target.YCenter = (roi.top + roi.bottom) / 2.0f;
        target.Width = roi.right - roi.left;
        target.Height = roi.bottom - roi.top;
      }

      // For 90° and 270° rotations, we need to swap width and height.
      // This is due to the internal behavior of ImageToTensorCalculator, which:
      // - first denormalizes the provided rect by multiplying the rect width or
      //   height by the image width or height, respectively.
      // - then rotates this by denormalized rect by the provided rotation, and
      //   uses this for cropping,
      // - then finally rotates this back.
      // TODO: use System.MathF.Abs
      if (UnityEngine.Mathf.Abs(optionsValue.rotationDegrees % 180) != 0)
      {
        var ih = image.Height();
        var iw = image.Width();
        var w = target.Height * ih / iw;
        var h = target.Width * iw / ih;
        target.Width = w;
        target.Height = h;
      }
    }

    /// <summary>
    ///   Shuts down the mediapipe vision task instance.
    /// </summary>
    /// <exception cref="Exception">
    ///   If the mediapipe vision task failed to close.
    /// </exception>
    public void Close()
    {
      _taskRunner.Close();
      _isClosed = true;
    }

    /// <summary>
    ///   Returns the canonicalized CalculatorGraphConfig of the underlying graph.
    /// </summary>
    public CalculatorGraphConfig GetGraphConfig() => _taskRunner.GetGraphConfig();

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
