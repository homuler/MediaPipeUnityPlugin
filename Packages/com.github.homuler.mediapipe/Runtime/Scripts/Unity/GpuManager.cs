// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_ANDROID
using System.Runtime.InteropServices;
#endif

namespace Mediapipe.Unity
{
  public static class GpuManager
  {
    private const string _TAG = nameof(GpuManager);

    private delegate void PluginCallback(int eventId);

    private static readonly object _SetupLock = new object();
#pragma warning disable IDE0044
    private static IntPtr _CurrentContext = IntPtr.Zero;
#pragma warning restore IDE0044
    private static bool _IsContextInitialized = false;

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

#if UNITY_ANDROID
        _IsContextInitialized = SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3;
        if (!_IsContextInitialized)
        {
          PluginCallback callback = GetCurrentContext;

          var fp = Marshal.GetFunctionPointerForDelegate(callback);
          GL.IssuePluginEvent(fp, 1);
        }
#else
        _IsContextInitialized = true;
#endif

        var count = 100;
        yield return new WaitUntil(() =>
        {
          return --count < 0 || _IsContextInitialized;
        });

        if (!_IsContextInitialized)
        {
          Logger.LogError(_TAG, "Failed to get GlContext");
          yield break;
        }

#if UNITY_ANDROID
        if (_CurrentContext == IntPtr.Zero)
        {
          Logger.LogWarning(_TAG, "EGL context is not found, so MediaPipe won't share their EGL contexts with Unity");
        }
        else
        {
          Logger.LogVerbose(_TAG, $"EGL context is found: {_CurrentContext}");
        }
#endif

        try
        {
          Logger.LogInfo(_TAG, "Initializing GpuResources...");
          GpuResources = GpuResources.Create(_CurrentContext);

          Logger.LogInfo(_TAG, "Initializing GlCalculatorHelper...");
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

    // Currently, it works only on Android
#if UNITY_ANDROID
    [AOT.MonoPInvokeCallback(typeof(PluginCallback))]
    private static void GetCurrentContext(int eventId) {
      _CurrentContext = Egl.GetCurrentContext();
      _IsContextInitialized = true;
    }
#endif
  }
}
