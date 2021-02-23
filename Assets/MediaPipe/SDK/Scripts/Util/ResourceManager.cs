using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mediapipe {
  /// <summary>
  ///   Class to manage assets that MediaPipe accesses.
  /// </summary>
  /// <remarks>
  ///   There must not be more than one instance at the same time.
  /// </remarks>
  public abstract class ResourceManager : IDisposable {
    volatile int disposeSignaled = 0;
    bool isDisposed = false;

    public delegate string CacheFilePathResolver(string path);
    public abstract CacheFilePathResolver cacheFilePathResolver { get; }

    public delegate bool ReadFileHandler(string path, IntPtr dst);
    public abstract ReadFileHandler readFileHandler { get; }

    public ResourceManager() {
      SafeNativeMethods.mp_api__ResetResourceManager(cacheFilePathResolver, readFileHandler);
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (Interlocked.Exchange(ref disposeSignaled, 1) != 0) {
        return;
      }

      isDisposed = true;

      if (disposing) {
        DisposeManaged();
      }
      DisposeUnmanaged();
    }

    ~ResourceManager() {
      Dispose(false);
    }

    protected virtual void DisposeManaged() {}

    protected virtual void DisposeUnmanaged() {
      SafeNativeMethods.mp_api__ResetResourceManager(IntPtr.Zero, IntPtr.Zero);
    }



    /// <param name="name">Asset name</param>
    /// <returns>
    ///   Returns true if <paramref name="name" /> is already prepared (saved locally on the device).
    /// </returns>
    public abstract bool IsPrepared(string name);

    /// <summary>
    ///   Saves all MediaPipe assets (e.g. *.tflite) locally so that MediaPipe can read them.
    /// </summary>
    public abstract Task PrepareAllAssetsAsync(bool overwrite = true);

    /// <summary>
    ///   Saves <paramref name="name" /> as <paramref name="uniqueKey" /> to the device.
    /// </summary>
    /// <param name="uniqueKey">
    ///   A unique key used to specify the asset.
    ///   It will be the file name on the device storage.
    /// </param>
    /// <param name="overwrite">
    ///   Specifies whether <paramref name="uniqueKey" /> will be overwritten if it already exists.
    /// </param>
    public abstract void PrepareAsset(string name, string uniqueKey, bool overwrite = true);

    public void PrepareAsset(string name, bool overwrite = true) {
      PrepareAsset(name, name, overwrite);
    }

    /// <summary>
    ///   Saves <paramref name="name" /> as <paramref name="uniqueKey" /> asynchronously.
    /// </summary>
    /// <param name="overwrite">
    ///   Specifies whether <paramref name="uniqueKey" /> will be overwritten if it already exists.
    /// </param>
    public abstract Task PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true);

    public Task PrepareAssetAsync(string name, bool overwrite = true) {
      return PrepareAssetAsync(name, name, overwrite);
    }
  }
}
