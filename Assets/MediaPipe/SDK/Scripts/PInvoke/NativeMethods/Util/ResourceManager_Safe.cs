using System.Runtime.InteropServices;

namespace Mediapipe {
  internal static partial class SafeNativeMethods {
    [DllImport (MediaPipeLibrary, ExactSpelling = true)]
    public static extern void mp_api__PrepareResourceManager(
        [MarshalAs(UnmanagedType.FunctionPtr)]ResourceManager.CacheFilePathResolver resolver,
        [MarshalAs(UnmanagedType.FunctionPtr)]ResourceManager.ReadFileHandler handler);
  }
}
