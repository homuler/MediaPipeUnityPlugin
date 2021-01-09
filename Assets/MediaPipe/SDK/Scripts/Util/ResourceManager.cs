using System;

namespace Mediapipe {
  public abstract class ResourceManager {
    public delegate string CacheFilePathResolver(string path);
    public abstract CacheFilePathResolver cacheFilePathResolver { get; }

    public delegate bool ReadFileHandler(string path, IntPtr dst);
    public abstract ReadFileHandler readFileHandler { get; }
  }
}
