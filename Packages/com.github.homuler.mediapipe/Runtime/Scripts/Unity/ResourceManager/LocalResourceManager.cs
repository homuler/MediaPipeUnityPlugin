// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

#if UNITY_EDITOR
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class LocalResourceManager : IResourceManager
  {
    private static readonly string _TAG = nameof(LocalResourceManager);

    private static string _RelativePath;
    private static readonly string _AssetPathRoot = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private static string _CachePathRoot;

    public LocalResourceManager(string path)
    {
      ResourceUtil.EnableCustomResolver();
      _RelativePath = path;
      _CachePathRoot = Path.Combine(Application.persistentDataPath, _RelativePath);
    }

    public LocalResourceManager() : this("") { }

    IEnumerator IResourceManager.PrepareAssetAsync(string name, string uniqueKey, bool overwriteDestination)
    {
      var destFilePath = GetCachePathFor(uniqueKey);
      ResourceUtil.SetAssetPath(name, destFilePath);

      if (File.Exists(destFilePath) && !overwriteDestination)
      {
        Logger.LogInfo(_TAG, $"{name} will not be copied to {destFilePath} because it already exists");
        yield break;
      }

      var assetPath = GetAssetPathFor(name);
      var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

      if (asset == null)
      {
        throw new FileNotFoundException($"{assetPath} is not found. Check if {name} is included in the package");
      }

      Logger.LogVerbose(_TAG, $"Writing {name} data to {destFilePath}...");
      if (!Directory.Exists(_CachePathRoot))
      {
        var _ = Directory.CreateDirectory(_CachePathRoot);
      }
      File.WriteAllBytes(destFilePath, asset.bytes);
      Logger.LogVerbose(_TAG, $"{name} is saved to {destFilePath} (length={asset.bytes.Length})");
    }

    private static string GetAssetPathFor(string assetName)
    {
      return Path.Combine(_AssetPathRoot, assetName);
    }

    private static string GetCachePathFor(string assetName)
    {
      return Path.Combine(_CachePathRoot, assetName);
    }
  }
}
#endif
