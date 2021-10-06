using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class AssetBundleResourceManager : ResourceManager
  {
    static readonly string TAG = typeof(AssetBundleResourceManager).Name;

    static string assetBundlePath;
    static string cachePathRoot;

    public override PathResolver pathResolver
    {
      get { return PathToResourceAsFile; }
    }

    public override ResourceProvider resourceProvider
    {
      get { return GetResourceContents; }
    }

    public AssetBundleResourceManager(string assetBundleName, string cachePath = "Cache") : base()
    {
      // It's safe to update static members because at most one RsourceManager can be initialized.
      assetBundlePath = Path.Combine(Application.streamingAssetsPath, assetBundleName);
      cachePathRoot = Path.Combine(Application.persistentDataPath, cachePath);
    }

    public override bool IsPrepared(string name)
    {
      var path = GetCachePathFor(name);

      return File.Exists(path);
    }

    AssetBundle _assetBundle;
    AssetBundle assetBundle
    {
      get { return _assetBundle; }
      set
      {
        if (_assetBundle != null)
        {
          _assetBundle.Unload(false);
        }
        _assetBundle = value;
      }
    }

    public void ClearAllCacheFiles()
    {
      if (Directory.Exists(cachePathRoot))
      {
        Directory.Delete(cachePathRoot, true);
      }
    }

    public IEnumerator LoadAssetBundleAsync()
    {
      if (assetBundle != null)
      {
        Logger.LogWarning(TAG, "AssetBundle is already loaded");
        yield break;
      }

      var bundleLoadReq = AssetBundle.LoadFromFileAsync(assetBundlePath);
      yield return bundleLoadReq;

      if (bundleLoadReq.assetBundle == null)
      {
        throw new IOException($"Failed to load {assetBundlePath}");
      }

      assetBundle = bundleLoadReq.assetBundle;
    }

    public override IEnumerator PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true)
    {
      var destFilePath = GetCachePathFor(uniqueKey);

      if (File.Exists(destFilePath) && !overwrite)
      {
        Logger.LogInfo(TAG, $"{name} will not be copied to {destFilePath} because it already exists");
        yield break;
      }

      if (assetBundle == null)
      {
        yield return LoadAssetBundleAsync();
      }

      var assetLoadReq = assetBundle.LoadAssetAsync<TextAsset>(name);
      yield return assetLoadReq;

      if (assetLoadReq.asset == null)
      {
        throw new IOException($"Failed to load {name} from {assetBundle.name}");
      }

      Logger.LogVerbose(TAG, $"Writing {name} data to {destFilePath}...");
      if (!Directory.Exists(cachePathRoot))
      {
        Directory.CreateDirectory(cachePathRoot);
      }
      var bytes = (assetLoadReq.asset as TextAsset).bytes;
      File.WriteAllBytes(destFilePath, bytes);
      Logger.LogVerbose(TAG, $"{name} is saved to {destFilePath} (length={bytes.Length})");
    }

    [AOT.MonoPInvokeCallback(typeof(PathResolver))]
    static string PathToResourceAsFile(string assetPath)
    {
      var assetName = GetAssetNameFromPath(assetPath);
      return GetCachePathFor(assetName);
    }

    [AOT.MonoPInvokeCallback(typeof(ResourceProvider))]
    protected static bool GetResourceContents(string path, IntPtr dst)
    {
      try
      {
        Logger.LogDebug($"{path} is requested");

        var cachePath = PathToResourceAsFile(path);
        if (!File.Exists(cachePath))
        {
          Logger.LogError(TAG, $"{cachePath} is not found");
          return false;
        }

        var asset = File.ReadAllBytes(cachePath);
        using (var srcStr = new StdString(asset))
        {
          srcStr.Swap(new StdString(dst, false));
        }

        return true;
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        return false;
      }
    }

    static string GetCachePathFor(string assetName)
    {
      return Path.Combine(cachePathRoot, assetName);
    }
  }
}
