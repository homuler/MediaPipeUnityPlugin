using System;
using System.IO;
using System.Threading.Tasks;

namespace Mediapipe.Unity {
  public class FileSystemResourceManager : ResourceManager {
    static readonly string TAG = typeof(FileSystemResourceManager).Name;

    static string _RootPath;

    public override PathResolver pathResolver {
      get { return PathToResourceAsFile; }
    }

    public override ResourceProvider resourceProvider {
      get { return GetResourceContents; }
    }

    public FileSystemResourceManager(string RootPath) : base() {
      // It's safe to update static members because at most one RsourceManager can be initialized.
      _RootPath = RootPath;
    }

    public override bool IsPrepared(string name) {
      var path = GetCacheFilePathFor(name);

      return File.Exists(path);
    }

    public string RootPath { get { return _RootPath; } }

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
        Logger.LogVerbose(TAG, $"{destFilePath} already exists");
        return;
      }

      using (var sourceStream = File.OpenRead(sourceFilePath)) {
        using (var destStream = File.Open(destFilePath, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write)) {
          sourceStream.CopyTo(destStream);
          Logger.LogVerbose(TAG, $"{sourceFilePath} is copied to {destFilePath}");
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

      Logger.LogError(TAG, $"{cachePath} does not exist");
      return null;
    }

    [AOT.MonoPInvokeCallback(typeof(ResourceProvider))]
    protected static bool GetResourceContents(string path, IntPtr dst) {
      try {
        var cachePath = PathToResourceAsFile(path);
        if (cachePath == null) {
          Logger.LogError($"Failed to resolve path to {path}");
          return false;
        }

        var asset = File.ReadAllBytes(cachePath);
        using (var srcStr = new StdString(asset)) {
          srcStr.Swap(new StdString(dst, false));
        }

        return true;
      } catch (Exception e) {
        Logger.LogException(e);
        return false;
      }
    }

    static string GetCacheFilePathFor(string assetName) {
      return Path.Combine(_RootPath, assetName);
    }
  }
}
