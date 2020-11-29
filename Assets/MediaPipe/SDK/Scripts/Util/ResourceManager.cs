using System;

namespace Mediapipe {
  public abstract class ResourceManager {
    public delegate string CacheFilePathResolver(string path);
    public readonly CacheFilePathResolver cacheFilePathResolver;

    public delegate bool ReadFileHandler(string path, IntPtr dst);
    public readonly ReadFileHandler readFileHandler;

    protected ResourceManager() {
      cacheFilePathResolver = CacheFileFromAsset;
      readFileHandler = ReadFile;
    }

    protected abstract string CacheFileFromAsset(string assetPath);

    protected abstract bool ReadFile(string path, IntPtr dst);
  }
}
