using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Mediapipe {
  public class AssetBundleManager : ResourceManager {
    static string _CacheRootPath = Path.Combine(Application.persistentDataPath, "Cache");
    static string _AssetBundlePath = Path.Combine(Application.streamingAssetsPath, "mediapipe");
    AssetBundle _assetBundle;

    public override PathResolver pathResolver {
      get { return PathToResourceAsFile; }
    }

    public override ResourceProvider resourceProvider {
      get { return GetResourceContents; }
    }

    public AssetBundleManager() : base() {}

    public AssetBundleManager(string CacheRootPath, string AssetBundlePath) : this() {
      this.CacheRootPath = CacheRootPath;
      this.AssetBundlePath = AssetBundlePath;
    }

    public override bool IsPrepared(string name) {
      var path = GetCacheFilePathFor(name);

      return File.Exists(path);
    }

    public string CacheRootPath {
      get { return _CacheRootPath; }
      set { _CacheRootPath = value; }
    }

    public string AssetBundlePath {
      get { return _AssetBundlePath; }
      set {
        assetBundle = null;
        _AssetBundlePath = value;
      }
    }

    AssetBundle assetBundle {
      get { return _assetBundle; }
      set {
        if (_assetBundle != null) {
          _assetBundle.Unload(false);
        }
        _assetBundle = value;
      }
    }

    public void ClearAllCacheFiles() {
      if (Directory.Exists(CacheRootPath)) {
        Directory.Delete(CacheRootPath, true);
      }
    }

    public void LoadAssetBundle() {
      if (assetBundle != null) {
        Debug.LogWarning("AssetBundle is already loaded");
        return;
      }

      assetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
    }

    public async Task LoadAssetBundleAsync() {
      if (assetBundle != null) {
        Debug.LogWarning("AssetBundle is already loaded");
        return;
      }

      var assetBundlePath = AssetBundlePath;
      var bundleLoadReq = await AssetBundle.LoadFromFileAsync(assetBundlePath);

      if (bundleLoadReq.assetBundle == null) {
        throw new IOException($"Failed to load {assetBundlePath}");
      }

      assetBundle = bundleLoadReq.assetBundle;
    }

    public override void PrepareAsset(string name, string uniqueKey, bool overwrite = true) {
      if (assetBundle == null) {
        LoadAssetBundle();
      }

      var asset = assetBundle.LoadAsset<TextAsset>(name);
      if (asset == null) {
        throw new IOException($"Failed to load {name} from {assetBundle.name}");
      }

      WriteCacheFile(asset, uniqueKey, overwrite);
    }

    public override async Task PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true) {
      if (assetBundle == null) {
        await LoadAssetBundleAsync();
      }

      var assetLoadReq = await assetBundle.LoadAssetAsync<TextAsset>(name);
      if (assetLoadReq.asset == null) {
        throw new IOException($"Failed to load {name} from {assetBundle.name}");
      }

      await WriteCacheFileAsync((TextAsset)assetLoadReq.asset, uniqueKey, overwrite);
    }

    [AOT.MonoPInvokeCallback(typeof(PathResolver))]
    static string PathToResourceAsFile(string assetPath) {
      var assetName = GetAssetNameFromPath(assetPath);
      var cachePath = GetCacheFilePathFor(assetName);

      if (File.Exists(cachePath)) {
        return cachePath;
      }

      Debug.LogWarning($"{cachePath} does not exist");
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
      return Path.Combine(_CacheRootPath, assetName);
    }

    void WriteCacheFile(TextAsset asset, string uniqueKey, bool overwrite) {
      var cachePath = GetCacheFilePathFor(uniqueKey);

      if (!overwrite && File.Exists(cachePath)) {
        Debug.Log($"{cachePath} already exists");
        return;
      }

      if (!Directory.Exists(CacheRootPath)) {
        Directory.CreateDirectory(CacheRootPath);
      }

      File.WriteAllBytes(cachePath, asset.bytes);
      Debug.Log($"{asset.name} is saved to {cachePath} (length={asset.bytes.Length})");
    }

    async Task WriteCacheFileAsync(TextAsset asset, string uniqueKey, bool overwrite) {
      var cachePath = GetCacheFilePathFor(uniqueKey);

      if (!overwrite && File.Exists(cachePath)) {
        Debug.Log($"{cachePath} already exists");
        return;
      }

      if (!Directory.Exists(CacheRootPath)) {
        Directory.CreateDirectory(CacheRootPath);
      }

      var bytes = asset.bytes;
      var mode = overwrite ? FileMode.Create : FileMode.CreateNew;

      using (var sourceStream = new FileStream(cachePath, mode, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true)) {
        await sourceStream.WriteAsync(bytes, 0, bytes.Length);
        Debug.Log($"{asset.name} is saved to {cachePath} (length={bytes.Length})");
      }
    }
  }
}
