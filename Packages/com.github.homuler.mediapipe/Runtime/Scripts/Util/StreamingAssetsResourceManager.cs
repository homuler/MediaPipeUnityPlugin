using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Mediapipe.Unity
{
  public class StreamingAssetsResourceManager : ResourceManager
  {
    static readonly string TAG = typeof(StreamingAssetsResourceManager).Name;

    static string relativePath;
    static string assetPathRoot;
    static string cachePathRoot;

    public override PathResolver pathResolver
    {
      get { return PathToResourceAsFile; }
    }

    public override ResourceProvider resourceProvider
    {
      get { return GetResourceContents; }
    }

    public StreamingAssetsResourceManager(string path) : base()
    {
      // It's safe to update static members because at most one RsourceManager can be initialized.
      relativePath = path;
      assetPathRoot = Path.Combine(Application.streamingAssetsPath, relativePath);
      cachePathRoot = Path.Combine(Application.persistentDataPath, relativePath);
    }

    public StreamingAssetsResourceManager() : this("") { }

    public override bool IsPrepared(string name)
    {
      var path = GetCachePathFor(name);

      return File.Exists(path);
    }

    public override IEnumerator PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true)
    {
      var destFilePath = GetCachePathFor(uniqueKey);

      if (File.Exists(destFilePath) && !overwrite)
      {
        Logger.LogInfo(TAG, $"{name} will not be copied to {destFilePath} because it already exists");
        yield break;
      }

      var sourceFilePath = GetCachePathFor(name);
      if (!File.Exists(sourceFilePath))
      {
        yield return CreateCacheFile(name);
      }

      if (sourceFilePath == destFilePath)
      {
        yield break;
      }

      Logger.LogVerbose(TAG, $"Copying {sourceFilePath} to {destFilePath}...");
      File.Copy(sourceFilePath, destFilePath, overwrite);
      Logger.LogVerbose(TAG, $"{sourceFilePath} is copied to {destFilePath}");
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

    IEnumerator CreateCacheFile(string assetName)
    {
      var cacheFilePath = GetCachePathFor(assetName);

      if (File.Exists(cacheFilePath))
      {
        yield break;
      }

#if !UNITY_ANDROID && !UNITY_WEBGL
      throw new FileNotFoundException($"{cacheFilePath} is not found");
#else
      var assetPath = GetAssetPathFor(assetName);
      using (var webRequest = UnityWebRequest.Get(assetPath))
      {
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
          if (!Directory.Exists(cachePathRoot))
          {
            Directory.CreateDirectory(cachePathRoot);
          }
          Logger.LogVerbose(TAG, $"Writing {assetName} data to {cacheFilePath}...");
          var bytes = webRequest.downloadHandler.data;
          File.WriteAllBytes(cacheFilePath, bytes);
          Logger.LogVerbose(TAG, $"{assetName} is saved to {cacheFilePath} (length={bytes.Length})");
        }
        else
        {
          throw new InternalException($"Failed to load {assetName}: {webRequest.error}");
        }
      }
#endif
    }

    static string GetAssetPathFor(string assetName)
    {
      return Path.Combine(assetPathRoot, assetName);
    }

    static string GetCachePathFor(string assetName)
    {
      var assetPath = GetAssetPathFor(assetName);
      if (File.Exists(assetPath))
      {
        return assetPath;
      }
      return Path.Combine(cachePathRoot, assetName);
    }
  }
}
