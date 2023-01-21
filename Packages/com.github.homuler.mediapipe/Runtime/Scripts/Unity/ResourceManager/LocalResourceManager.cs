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
  public class LocalResourceManager : ResourceManager
  {
    private static readonly string _TAG = nameof(LocalResourceManager);

    private static string _RelativePath;
    private static readonly string _AssetPathRoot = "Packages/com.github.homuler.mediapipe/PackageResources/MediaPipe";
    private static string _CachePathRoot;

    public LocalResourceManager(string path) : base(PathToResourceAsFile, GetResourceContents)
    {
      // It's safe to update static members because at most one RsourceManager can be initialized.
      _RelativePath = path;
      _CachePathRoot = Path.Combine(Application.persistentDataPath, _RelativePath);
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

    protected static string PathToResourceAsFile(string assetPath)
    {
      var assetName = GetAssetNameFromPath(assetPath);
      return GetCachePathFor(assetName);
    }

    protected static byte[] GetResourceContents(string path)
    {
      // TODO: try AsyncReadManager
      Logger.LogDebug($"{path} is requested");

      var cachePath = PathToResourceAsFile(path);
      return File.ReadAllBytes(cachePath);
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
