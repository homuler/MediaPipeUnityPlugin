using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Mediapipe {
  /// <summary>
  ///   Class to manage assets that MediaPipe accesses.
  /// </summary>
  /// <remarks>
  ///   There must not be more than one instance at the same time.
  /// </remarks>
  public abstract class ResourceManager {
    public delegate string PathResolver(string path);
    public abstract PathResolver pathResolver { get; }
    public delegate bool ResourceProvider(string path, IntPtr output);
    public abstract ResourceProvider resourceProvider { get; }

    public ResourceManager() {
      SafeNativeMethods.mp__SetCustomGlobalPathResolver__P(pathResolver);
      SafeNativeMethods.mp__SetCustomGlobalResourceProvider__P(resourceProvider);
    }

    /// <param name="name">Asset name</param>
    /// <returns>
    ///   Returns true if <paramref name="name" /> is already prepared (saved locally on the device).
    /// </returns>
    public abstract bool IsPrepared(string name);

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

    protected static string GetAssetNameFromPath(string assetPath) {
      var assetName = Path.GetFileNameWithoutExtension(assetPath);
      var extension = Path.GetExtension(assetPath);

      switch (extension) {
        case ".binarypb":
        case ".tflite": {
          return $"{assetName}.bytes";
        }
        case ".pbtxt": {
          return $"{assetName}.txt";
        }
        default: {
          return $"{assetName}{extension}";
        }
      }
    }
  }
}
