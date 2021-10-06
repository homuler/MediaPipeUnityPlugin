#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class LocalResourceManager : ResourceManager
  {
    static readonly string TAG = typeof(LocalResourceManager).Name;

    static string relativePath;
    static readonly string assetPathRoot = "Packages/com.github.homuler.mediapipe/Runtime/Resources";
    static string cachePathRoot;

    public override PathResolver pathResolver
    {
      get { return PathToResourceAsFile; }
    }

    public override ResourceProvider resourceProvider
    {
      get { return GetResourceContents; }
    }

    public LocalResourceManager(string path) : base()
    {
      // It's safe to update static members because at most one RsourceManager can be initialized.
      relativePath = path;
      cachePathRoot = Path.Combine(Application.persistentDataPath, relativePath);
    }

    public LocalResourceManager() : this("") { }

    public override bool IsPrepared(string assetName)
    {
      return File.Exists(GetCachePathFor(assetName));
    }

    public override IEnumerator PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true)
    {
      var destFilePath = GetCachePathFor(uniqueKey);

      if (File.Exists(destFilePath) && !overwrite)
      {
        Logger.LogInfo(TAG, $"{name} will not be copied to {destFilePath} because it already exists");
        yield break;
      }

      var assetPath = GetAssetPathFor(name);
      var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

      Logger.LogVerbose(TAG, $"Writing {name} data to {destFilePath}...");
      if (!Directory.Exists(cachePathRoot))
      {
        Directory.CreateDirectory(cachePathRoot);
      }
      File.WriteAllBytes(destFilePath, asset.bytes);
      Logger.LogVerbose(TAG, $"{name} is saved to {destFilePath} (length={asset.bytes.Length})");
    }

    [AOT.MonoPInvokeCallback(typeof(PathResolver))]
    protected static string PathToResourceAsFile(string assetPath)
    {
      var assetName = GetAssetNameFromPath(assetPath);
      return GetCachePathFor(assetName);
    }

    [AOT.MonoPInvokeCallback(typeof(ResourceProvider))]
    protected static bool GetResourceContents(string path, IntPtr dst)
    {
      // TODO: try AsyncReadManager
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

    static string GetAssetPathFor(string assetName)
    {
      return Path.Combine(assetPathRoot, assetName);
    }

    static string GetCachePathFor(string assetName)
    {
      return Path.Combine(cachePathRoot, assetName);
    }
  }
}
#endif
