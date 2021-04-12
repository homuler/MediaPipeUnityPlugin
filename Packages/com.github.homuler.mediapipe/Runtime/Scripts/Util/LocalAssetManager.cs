using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Mediapipe {
  public class LocalAssetManager : ResourceManager {
    readonly static string ResourceRootPath = Path.Combine(Application.dataPath, "..", "Packages", "com.github.homuler.mediapipe", "Runtime", "Resources");

    public override PathResolver pathResolver {
      get { return PathToResourceAsFile; }
    }

    public override ResourceProvider resourceProvider {
      get { return GetResourceContents; }
    }

    public override bool IsPrepared(string name) {
      var path = GetCacheFilePathFor(name);

      return File.Exists(path);
    }

    public string RootPath {
      get { return ResourceRootPath; }
    }

    public override void PrepareAsset(string name, string uniqueKey, bool overwrite = true) {
      var sourceFilePath = GetCacheFilePathFor(name);

      if (!File.Exists(sourceFilePath)) {
        throw new FileNotFoundException($"{sourceFilePath} is not found");
      }

      var destFilePath = GetCacheFilePathFor(uniqueKey);

      if (sourceFilePath == destFilePath) {
        return;
      }

      if (!overwrite && File.Exists(destFilePath)) {
        Debug.Log($"{destFilePath} already exists");
        return;
      }

      using (var sourceStream = File.OpenRead(sourceFilePath)) {
        using (var destStream = File.Open(destFilePath, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write)) {
          sourceStream.CopyTo(destStream);
          Debug.Log($"{sourceFilePath} is copied to {destFilePath}");
        }
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

      using (var sourceStream = File.OpenRead(sourceFilePath)) {
        using (var destStream = File.Open(destFilePath, overwrite ? FileMode.Create : FileMode.CreateNew)) {
          await sourceStream.CopyToAsync(destStream);
        }
      }
    }

    [AOT.MonoPInvokeCallback(typeof(PathResolver))]
    protected static string PathToResourceAsFile(string assetPath) {
      var assetName = GetAssetNameFromPath(assetPath);
      var cachePath = GetCacheFilePathFor(assetName);

      if (File.Exists(cachePath)) {
        return cachePath;
      }

      Debug.LogError($"{cachePath} does not exist");
      return null;
    }

    [AOT.MonoPInvokeCallback(typeof(ResourceProvider))]
    protected static bool GetResourceContents(string path, IntPtr dst) {
      try {
        var cachePath = PathToResourceAsFile(path);
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

    static string GetCacheFilePathFor(string assetName) {
      return Path.Combine(ResourceRootPath, assetName);
    }
  }
}
