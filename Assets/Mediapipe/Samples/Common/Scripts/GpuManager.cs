using System;
using System.Collections;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Mediapipe.Unity {
  public static class GpuManager {
    static readonly string TAG = typeof(GpuManager).Name;

    delegate void PluginCallback(int eventId);

    static readonly object setupLock = new object();
    static IntPtr currentContext = IntPtr.Zero;
    static bool isContextInitialized = false;

    public static GpuResources gpuResources { get; private set; }
    public static GlCalculatorHelper glCalculatorHelper { get; private set; }

    public static bool isInitialized { get; private set; }

    public static IEnumerator Initialize() {
      lock(setupLock) {
        if (isInitialized) {
          Logger.LogWarning(TAG, "Already set up");
          yield break;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        isContextInitialized = false;
        PluginCallback callback = GetCurrentContext;

        var fp = Marshal.GetFunctionPointerForDelegate(callback);
        GL.IssuePluginEvent(fp, 1);
#else
        isContextInitialized = true;
#endif

        var count = 1000;
        yield return new WaitUntil(() => {
          return --count < 0 || isContextInitialized;
        });

        if (!isContextInitialized) {
          throw new TimeoutException("Failed to get GlContext");
        }

#if UNITY_ANDROID
        if (currentContext == IntPtr.Zero) {
          Logger.LogWarning(TAG, "EGL context is not found, so MediaPipe won't share their EGL contexts with Unity");
        } else {
          Logger.LogVerbose(TAG, $"EGL context is found: {currentContext}");
        }
#endif

        Logger.LogInfo(TAG, "Initializing GpuResources...");
        gpuResources = GpuResources.Create(currentContext).Value();

        Logger.LogInfo(TAG, "Initializing GlCalculatorHelper...");
        glCalculatorHelper = new GlCalculatorHelper();
        glCalculatorHelper.InitializeForTest(gpuResources);

        isInitialized = true;
      }
    }

    /// <summary>
    ///   Dispose GPU resources.
    /// </summary>
    /// <remarks>
    ///   This has to be called once GPU resources are used by CalculatorGraph.
    ///   Otherwise, UnityEditor will freeze.
    /// </remarks>
    public static void Shutdown() {
      if (gpuResources != null) {
        gpuResources.Dispose();
        gpuResources = null;
      }

      if (glCalculatorHelper != null) {
        glCalculatorHelper.Dispose();
        glCalculatorHelper = null;
      }
    }

// Currently, it works only on Android
#if UNITY_ANDROID && !UNITY_EDITOR
    [AOT.MonoPInvokeCallback(typeof(PluginCallback))]
    static void GetCurrentContext(int eventId) {
      currentContext = Egl.getCurrentContext();
      isContextInitialized = true;
    }
#endif
  }
}