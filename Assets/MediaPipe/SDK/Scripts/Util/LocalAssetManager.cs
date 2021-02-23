using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Mediapipe {
  public class LocalAssetManager : ResourceManager {
    readonly static string ResourceRootPath = Path.Combine(Application.dataPath, "MediaPipe", "SDK", "Resources");

    public override CacheFilePathResolver cacheFilePathResolver {
      get { return CacheFileFromAsset; }
    }

    public override ReadFileHandler readFileHandler {
      get { return ReadFile; }
    }

    public override bool IsPrepared(string name) {
      var path = GetCacheFilePathFor(name);

      return File.Exists(path);
    }

    public string RootPath {
      get { return ResourceRootPath; }
    }

    /// <summary>dummy method</summary>
    public override Task PrepareAllAssetsAsync(bool overwrite = true) {
      return Task.CompletedTask;
    }

    public override void PrepareAsset(string name, string uniqueKey, bool overwrite = true) {
      var sourceFilePath = GetCacheFilePathFor(name);

      if (!File.Exists(sourceFilePath)) {
        throw new FileNotFoundException($"{sourceFilePath} is not found");
      }

      var destFilePath = GetCacheFilePathFor(uniqueKey);

      if (sourceFilePath != destFilePath) {
        File.Copy(sourceFilePath, destFilePath, overwrite);
      }
    }

    public override async Task PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true) {
      var sourceFilePath = GetCacheFilePathFor(name);

      if (!File.Exists(sourceFilePath)) {
        throw new FileNotFoundException($"{sourceFilePath} is not found");
      }

      var destFilePath = GetCacheFilePathFor(uniqueKey);

      if (sourceFilePath == destFilePath) {
        return;
      }

      FileStream sourceStream = null;
      FileStream destStream = null;
  
      try {
        sourceStream = File.OpenRead(sourceFilePath);
        destStream = File.Open(destFilePath, overwrite ? FileMode.Create : FileMode.CreateNew);

        await sourceStream.CopyToAsync(destStream);
      } finally {
        if (sourceStream != null) {
          sourceStream.Close();
        }
        if (destStream != null) {
          destStream.Close();
        }
      }
    }

    [AOT.MonoPInvokeCallback(typeof(CacheFilePathResolver))]
    protected static string CacheFileFromAsset(string assetPath) {
      var assetName = GetAssetName(assetPath);
      var cachePath = GetCacheFilePathFor(assetName);

      if (File.Exists(cachePath)) {
        return cachePath;
      }

      Debug.LogError($"{cachePath} does not exist");
      return null;
    }

    [AOT.MonoPInvokeCallback(typeof(ReadFileHandler))]
    protected static bool ReadFile(string path, IntPtr dst) {
      try {
        var cachePath = CacheFileFromAsset(path);
        var asset = File.ReadAllBytes(cachePath);

        using (var srcStr = new StdString(asset)) {
          srcStr.Swap(new StdString(dst, false));
        }

        return true;
      } catch (Exception e) {
        Debug.LogError($"Failed to read file `{path}`: ${e.ToString()}");
        return false;
      }
    }

    static string GetAssetName(string assetPath) {
      // assume that each file name is unique if its extension is ignored.
      var assetName = Path.GetFileNameWithoutExtension(assetPath);
      var extension = Path.GetExtension(assetPath);

      return extension == ".tflite" ? $"{assetName}.bytes" : $"{assetName}{extension}";
    }

    static string GetCacheFilePathFor(string assetName) {
      return Path.Combine(ResourceRootPath, assetName);
    }
  }
}
