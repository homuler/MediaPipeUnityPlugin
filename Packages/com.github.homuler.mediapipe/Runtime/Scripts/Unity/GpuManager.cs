// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mediapipe.Unity
{
  public static class GpuManager
  {
    private const string _TAG = nameof(GpuManager);

    private delegate void PluginCallback(int eventId);

    private static readonly object _SetupLock = new object();
    private static IntPtr _PlatformGlContext = IntPtr.Zero;

    public static GpuResources GpuResources { get; private set; }
    public static GlCalculatorHelper GlCalculatorHelper { get; private set; }

    public static bool IsInitialized { get; private set; }

    /// <summary>
    ///   Initialize GPU resources.
    ///   If it finishes successfully, <see cref="IsInitialized" /> will be set to <c>true</c>.
    /// </summary>
    /// <remarks>
    ///   If <see cref="IsInitialized" /> is <c>true</c>, it will do nothing.
    ///   Before the application exits, don't forget to call <see cref="Shutdown" />.
    /// </remarks>
    public static IEnumerator Initialize()
    {
      lock (_SetupLock)
      {
        if (IsInitialized)
        {
          Logger.LogInfo(_TAG, "Already initialized");
          yield break;
        }

        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3)
        {
          var req = AsyncGlContext.Request(OnGetEglContext);
          yield return new WaitUntil(() => req.done);

          if (req.error != null)
          {
            Logger.LogException(req.error);
            yield break;
          }
        }

        try
        {
          GpuResources = GpuResources.Create(_PlatformGlContext);
          GlCalculatorHelper = new GlCalculatorHelper();
          GlCalculatorHelper.InitializeForTest(GpuResources);

          IsInitialized = true;
        }
        catch (EntryPointNotFoundException e)
        {
          Logger.LogException(e);
          Logger.LogError(_TAG, "Failed to create GpuResources. Did you build libraries with GPU enabled?");
        }
        catch (Exception e)
        {
          Logger.LogException(e);
        }
      }
    }

    /// <summary>
    ///   Dispose GPU resources.
    /// </summary>
    /// <remarks>
    ///   This has to be called before the application exits.
    ///   Otherwise, UnityEditor can freeze.
    /// </remarks>
    public static void Shutdown()
    {
      if (GpuResources != null)
      {
        GpuResources.Dispose();
        GpuResources = null;
      }

      if (GlCalculatorHelper != null)
      {
        GlCalculatorHelper.Dispose();
        GlCalculatorHelper = null;
      }

      IsInitialized = false;
    }

    public static void ResetGpuResources(IntPtr platformGlContext)
    {
      if (!IsInitialized)
      {
        throw new InvalidOperationException("GpuManager is not initialized");
      }
      GpuResources?.Dispose();

      GpuResources = new GpuResources(platformGlContext);
      GlCalculatorHelper.InitializeForTest(GpuResources);
    }

    public static GlContext GetGlContext() => GlCalculatorHelper?.GetGlContext();

    private static void OnGetEglContext(AsyncGlContextRequest request)
    {
      if (request.platformGlContext == IntPtr.Zero)
      {
        Logger.LogWarning(_TAG, "EGL context is not found, so MediaPipe won't share their EGL contexts with Unity");
        return;
      }
      Logger.LogVerbose(_TAG, $"EGL context is found: {request.platformGlContext}");

      _PlatformGlContext = request.platformGlContext;
    }
  }
}
