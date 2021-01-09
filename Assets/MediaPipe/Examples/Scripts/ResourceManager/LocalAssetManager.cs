using Mediapipe;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
///   Sample implementation of ResourceManager, that reads files from local filesystem.
///   This class is used in UnityEditor environment.
/// </summary>
public sealed class LocalAssetManager : ResourceManager {
  private static readonly Lazy<LocalAssetManager> lazy = new Lazy<LocalAssetManager>(() => new LocalAssetManager());
  public static LocalAssetManager Instance { get { return lazy.Value; } }
  private readonly static string ModelRootPath = Path.Combine(Application.dataPath, "MediaPipe", "SDK", "Models");

  public override CacheFilePathResolver cacheFilePathResolver {
    get { return CacheFileFromAsset; }
  }

  public override ReadFileHandler readFileHandler {
    get { return ReadFile; }
  }

  private LocalAssetManager() {}

  /// <summary>dummy method</summary>
  public async Task LoadAllAssetsAsync() {
    await Task.CompletedTask;
  }

  static string CacheFileFromAsset(string assetPath) {
    var assetName = GetAssetName(assetPath);
    var localPath = GetLocalFilePath(assetName);

    if (File.Exists(localPath)) {
      return localPath;
    }

    return null;
  }

  static bool ReadFile(string path, IntPtr dst) {
    try {
      Debug.Log(path);
      var localPath = CacheFileFromAsset(path);
      var asset = File.ReadAllBytes(localPath);

      using (var srcStr = new StdString(asset)) {
        srcStr.Swap(new StdString(dst, false));
      }

      return true;
    } catch (Exception e) {
      Debug.Log($"Failed to read file `{path}`: ${e.ToString()}");
      return false;
    }
  }

  static string GetAssetName(string assetPath) {
    var assetName = Path.GetFileNameWithoutExtension(assetPath);
    var extension = Path.GetExtension(assetPath);

    return (extension == ".tflite" || extension == ".bytes") ? $"{assetName}.bytes" : $"{assetName}.txt";
  }

  static string GetLocalFilePath(string assetName) {
    return Path.Combine(ModelRootPath, assetName);
  }
}
