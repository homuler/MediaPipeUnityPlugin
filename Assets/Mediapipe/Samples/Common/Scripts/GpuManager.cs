// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
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

    public static IEnumerator Initialize()
    {
      lock (_SetupLock)
      {
        if (IsInitialized)
        {
          Logger.LogWarning(_TAG, "Already set up");
          yield break;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        _IsContextInitialized = false;
        PluginCallback callback = GetCurrentContext;

        var fp = Marshal.GetFunctionPointerForDelegate(callback);
        GL.IssuePluginEvent(fp, 1);
#else
        _IsContextInitialized = true;
#endif

        var count = 1000;
        yield return new WaitUntil(() =>
        {
          return --count < 0 || _IsContextInitialized;
        });

        if (!_IsContextInitialized)
        {
          throw new TimeoutException("Failed to get GlContext");
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
          var statusOrGpuResources = GpuResources.Create(_CurrentContext);

          statusOrGpuResources.status.AssertOk();
          GpuResources = statusOrGpuResources.Value();

          Logger.LogInfo(_TAG, "Initializing GlCalculatorHelper...");
          GlCalculatorHelper = new GlCalculatorHelper();
          GlCalculatorHelper.InitializeForTest(GpuResources);

          IsInitialized = true;
        }
        catch (Exception e)
        {
          Logger.LogException(e);
          Logger.LogError(_TAG, "Failed to create GpuResources. If your native library is built for CPU, change 'Preferable Inference Mode' to CPU from the Inspector Window for Bootstrap");
        }
      }
    }

    /// <summary>
    ///   Dispose GPU resources.
    /// </summary>
    /// <remarks>
    ///   This has to be called once GPU resources are used by CalculatorGraph.
    ///   Otherwise, UnityEditor will freeze.
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
    }

    // Currently, it works only on Android
#if UNITY_ANDROID && !UNITY_EDITOR
    [AOT.MonoPInvokeCallback(typeof(PluginCallback))]
    static void GetCurrentContext(int eventId) {
      _CurrentContext = Egl.GetCurrentContext();
      _IsContextInitialized = true;
    }
#endif
  }
}
