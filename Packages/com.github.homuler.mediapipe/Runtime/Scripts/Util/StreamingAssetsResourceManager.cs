// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Mediapipe.Unity
{
  public class StreamingAssetsResourceManager : ResourceManager
  {
    private static readonly string _TAG = nameof(StreamingAssetsResourceManager);

    private static string _RelativePath;
    private static string _AssetPathRoot;
    private static string _CachePathRoot;

    public override PathResolver pathResolver => PathToResourceAsFile;

    public override ResourceProvider resourceProvider => GetResourceContents;

    public StreamingAssetsResourceManager(string path) : base()
    {
      // It's safe to update static members because at most one RsourceManager can be initialized.
      _RelativePath = path;
      _AssetPathRoot = Path.Combine(Application.streamingAssetsPath, _RelativePath);
      _CachePathRoot = Path.Combine(Application.persistentDataPath, _RelativePath);
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
        Logger.LogInfo(_TAG, $"{name} will not be copied to {destFilePath} because it already exists");
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

      Logger.LogVerbose(_TAG, $"Copying {sourceFilePath} to {destFilePath}...");
      File.Copy(sourceFilePath, destFilePath, overwrite);
      Logger.LogVerbose(_TAG, $"{sourceFilePath} is copied to {destFilePath}");
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
          Logger.LogError(_TAG, $"{cachePath} is not found");
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

    private IEnumerator CreateCacheFile(string assetName)
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
          if (!Directory.Exists(_CachePathRoot))
          {
            var _ = Directory.CreateDirectory(_CachePathRoot);
          }
          Logger.LogVerbose(_TAG, $"Writing {assetName} data to {cacheFilePath}...");
          var bytes = webRequest.downloadHandler.data;
          File.WriteAllBytes(cacheFilePath, bytes);
          Logger.LogVerbose(_TAG, $"{assetName} is saved to {cacheFilePath} (length={bytes.Length})");
        }
        else
        {
          throw new InternalException($"Failed to load {assetName}: {webRequest.error}");
        }
      }
#endif
    }

    private static string GetAssetPathFor(string assetName)
    {
      return Path.Combine(_AssetPathRoot, assetName);
    }

    private static string GetCachePathFor(string assetName)
    {
      var assetPath = GetAssetPathFor(assetName);
      return File.Exists(assetPath) ? assetPath : Path.Combine(_CachePathRoot, assetName);
    }
  }
}
