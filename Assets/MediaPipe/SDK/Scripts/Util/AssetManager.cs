using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public abstract class AssetManager {
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate string CacheFilePathResolver(string path);
    private readonly CacheFilePathResolver cacheFilePathResolver;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate bool ReadFileHandler(string path, IntPtr dst);
    private readonly ReadFileHandler readFileHandler;

    protected AssetManager() {
      cacheFilePathResolver = CacheFileFromAsset;
      readFileHandler = ReadFile;
    }

    public IntPtr GetCacheFilePathResolverPtr() {
      return Marshal.GetFunctionPointerForDelegate(cacheFilePathResolver);
    }

    public IntPtr GetReadFileHandlerPtr() {
      return Marshal.GetFunctionPointerForDelegate(readFileHandler);
    }

    public abstract string CacheFileFromAsset(string assetPath);

    public abstract bool ReadFile(string path, IntPtr dst);
  }
}
