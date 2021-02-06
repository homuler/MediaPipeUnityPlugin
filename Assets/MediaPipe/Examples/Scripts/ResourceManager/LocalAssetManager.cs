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
  private readonly static string ResourceRootPath = Path.Combine(Application.dataPath, "MediaPipe", "SDK", "Resources");

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

  [AOT.MonoPInvokeCallback(typeof(CacheFilePathResolver))]
  static string CacheFileFromAsset(string assetPath) {
    var assetName = GetAssetName(assetPath);
    var localPath = GetLocalFilePath(assetName);


    if (File.Exists(localPath)) {
      return localPath;
    }

    Debug.LogWarning($"{localPath} does not exist");
    return null;
  }

  [AOT.MonoPInvokeCallback(typeof(ReadFileHandler))]
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

    return extension == ".tflite" ? $"{assetName}.bytes" : $"{assetName}{extension}";
  }

  static string GetLocalFilePath(string assetName) {
    return Path.Combine(ResourceRootPath, assetName);
  }
}
