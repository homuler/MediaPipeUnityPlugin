using Mediapipe;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public sealed class DemoAssetManager : AssetManager {
  private static readonly Lazy<DemoAssetManager> lazy = new Lazy<DemoAssetManager>(() => new DemoAssetManager());
  public static DemoAssetManager Instance { get { return lazy.Value; } }
  private readonly static string CacheRootPath = Path.Combine(Application.persistentDataPath, "Cache");
  private readonly static string ModelCacheRootPath = Path.Combine(CacheRootPath, "MediaPipe", "Models");

  private AssetBundle _mediaPipeModels;
  private AssetBundle MediaPipeModels {
    get {
      if (_mediaPipeModels != null) {
        return _mediaPipeModels;
      }

      return _mediaPipeModels = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "AssetBundles", "mediapipe", "models"));
    }
  }

  private DemoAssetManager() : base() {
    if (!Directory.Exists(ModelCacheRootPath)) {
      Directory.CreateDirectory(ModelCacheRootPath);
    }
  }

  public async void LoadAllAssetsAsync() {
    var bundle = MediaPipeModels;

    if (bundle == null) {
      Debug.LogError("Failed to load assets");
      return;
    }

    await Task.WhenAll(bundle.LoadAllAssets<TextAsset>().Select((asset) => WriteCacheFileAsync(asset)).ToArray());
  }

  public override string CacheFileFromAsset(string assetPath) {
    var assetName = Path.GetFileNameWithoutExtension(assetPath);
    var cachePath = CachePathFor(assetName);

    if (File.Exists(cachePath)) {
      return cachePath;
    }

    return null;
  }

  public override bool ReadFile(string path, IntPtr dst) {
    var cachePath = CacheFileFromAsset(path);

    if (!File.Exists(cachePath)) {
      return false;
    }

    var asset = File.ReadAllBytes(cachePath);
    ResourceUtil.CopyBytes(dst, asset);
    return true;
  }

  private string CachePathFor(string assetName) {
    return Path.Combine(ModelCacheRootPath, assetName);
  }

  private async Task WriteCacheFileAsync(TextAsset asset) {
    var path = CachePathFor(asset.name);

    using (FileStream sourceStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true)) {
      var bytes = asset.bytes;
      await sourceStream.WriteAsync(bytes, 0, bytes.Length);
    };
  }
}
