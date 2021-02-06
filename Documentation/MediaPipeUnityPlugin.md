# MediaPipeUnityPlugin

## Platform dependent compilation
```cs
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID || UNITY_IOS
// When GPU is available (SDK)
#elif (UNITY_STANDALONE_LINUX || UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR_OSX && !UNITY_EDITOR_WIN
// (Application Code)
#endif

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
// When GpuBuffer uses GlTextureBuffer (SDK)
#elif (UNITY_STANDALONE_LINUX || UNITY_ANDROID) && !UNITY_EDITOR_OSX && !UNITY_EDITOR_WIN
// (Application Code)
#endif

#if UNITY_IOS
// When GpuBuffer uses CVPixelBuffer (SDK)
#elif UNITY_IOS && !UNITY_EDITOR
// (Application Code)
#endif

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
// When EGL is available (SDK)
#elif (UNITY_STANDALONE_LINUX || UNITY_ANDROID) && !UNITY_EDITOR_OSX && !UNITY_EDITOR_WIN
// (Application Code)
#endif

#if UNITY_IOS
// When EAGL is available (SDK)
#if UNITY_IOS && !UNITY_EDITOR
// (Application Code)
#endif

#if UNITY_STANDALONE_OSX
// When NSGL is available (SDK)
// NOTE: On macOS, native libs cannot be built with GPU enabled, so NSGL isn't available now.
#endif
```
