// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.IO;

namespace Mediapipe
{
  public static class ResourceUtil
  {
    internal delegate string PathResolver(string path);
    internal delegate bool NativeResourceProvider(string path, IntPtr dest);

    private static readonly string _TAG = nameof(ResourceUtil);

    private static bool _IsInitialized;
    private static readonly Dictionary<string, string> _AssetPathMap = new Dictionary<string, string>();

    public static void EnableCustomResolver()
    {
      if (_IsInitialized)
      {
        return;
      }
      SafeNativeMethods.mp__SetCustomGlobalPathResolver__P(PathToResourceAsFile);
      SafeNativeMethods.mp__SetCustomGlobalResourceProvider__P(GetResourceContents);
      _IsInitialized = true;
    }

    /// <summary>
    ///   Registers the asset path to the resource manager.
    /// </summary>
    /// <param name="assetKey">
    ///   The key to register the asset path.
    ///   It is usually the file path of the asset hard-coded in the native code (e.g. `path/to/model.tflite`)
    ///   or the asset name (e.g. `model.bytes`).
    /// </param>
    /// <param name="assetPath">
    ///   The file path of the asset.
    /// </param>
    public static void SetAssetPath(string assetKey, string assetPath) => _AssetPathMap[assetKey] = assetPath;

    /// <summary>
    ///   Registers the asset path to the resource manager.
    /// </summary>
    /// <exception cref="ArgumentException">
    ///   Thrown when the asset key is already registered
    /// </exception>
    /// <param name="assetKey">
    ///   The key to register the asset path.
    ///   It is usually the file path of the asset hard-coded in the native code (e.g. `path/to/model.tflite`)
    ///   or the asset name (e.g. `model.bytes`).
    /// </param>
    /// <param name="assetPath">
    ///   The file path of the asset.
    /// </param>
    public static void AddAssetPath(string assetKey, string assetPath) => _AssetPathMap.Add(assetKey, assetPath);

    /// <summary>
    ///   Removes the asset key from the resource manager.
    /// </summary>
    /// <param name="assetKey"></param>
    public static bool RemoveAssetPath(string assetKey) => _AssetPathMap.Remove(assetKey);

    public static bool TryGetFilePath(string assetPath, out string filePath)
    {
      // try to find the file path by the requested asset path
      if (_AssetPathMap.TryGetValue(assetPath, out filePath))
      {
        return true;
      }
      // try to find the file path by the asset name
      if (_AssetPathMap.TryGetValue(GetAssetNameFromPath(assetPath), out filePath))
      {
        return true;
      }
      return false;
    }

    [AOT.MonoPInvokeCallback(typeof(PathResolver))]
    private static string PathToResourceAsFile(string assetPath)
    {
      UnityEngine.Debug.Log($"{assetPath} is requested");
      try
      {
        Logger.LogDebug(_TAG, $"{assetPath} is requested");
        if (TryGetFilePath(assetPath, out var filePath))
        {
          return filePath;
        }
        throw new KeyNotFoundException($"Failed to find the file path for `{assetPath}`");
      }
      catch (Exception e)
      {
        Logger.LogException(e);
        return "";
      }
    }

    [AOT.MonoPInvokeCallback(typeof(NativeResourceProvider))]
    private static bool GetResourceContents(string path, IntPtr dst)
    {
      UnityEngine.Debug.Log($"{path} is requested");

      try
      {
        Logger.LogDebug(_TAG, $"{path} is requested");

        if (!TryGetFilePath(path, out var filePath))
        {
          throw new KeyNotFoundException($"Failed to find the file path for `{path}`");
        }

        var asset = File.ReadAllBytes(filePath);
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

    private static string GetAssetNameFromPath(string assetPath)
    {
      var assetName = Path.GetFileNameWithoutExtension(assetPath);
      var extension = Path.GetExtension(assetPath);

      switch (extension)
      {
        case ".binarypb":
        case ".tflite":
          {
            return $"{assetName}.bytes";
          }
        case ".pbtxt":
          {
            return $"{assetName}.txt";
          }
        default:
          {
            return $"{assetName}{extension}";
          }
      }
    }
  }
}
