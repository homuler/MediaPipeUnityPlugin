// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.IO;

namespace Mediapipe
{
  /// <summary>
  ///   Class to manage assets that MediaPipe accesses.
  /// </summary>
  /// <remarks>
  ///   There must not be more than one instance at the same time.
  /// </remarks>
  public abstract class ResourceManager
  {
    public delegate string PathResolver(string path);
    internal delegate bool NativeResourceProvider(string path, IntPtr dest);
    public delegate byte[] ResourceProvider(string path);

    private static readonly object _InitLock = new object();
    private static ResourceManager _Instance;
    private readonly PathResolver _pathResolver;
    private readonly ResourceProvider _resourceProvider;

    public ResourceManager(PathResolver pathResolver, ResourceProvider resourceProvider)
    {
      lock (_InitLock)
      {
        if (_Instance != null)
        {
          throw new InvalidOperationException("ResourceManager can be initialized only once");
        }
        _pathResolver = pathResolver;
        _resourceProvider = resourceProvider;
        SafeNativeMethods.mp__SetCustomGlobalPathResolver__P(PathToResourceAsFile);
        SafeNativeMethods.mp__SetCustomGlobalResourceProvider__P(GetResourceContents);
        _Instance = this;
      }
    }

    /// <param name="name">Asset name</param>
    /// <returns>
    ///   Returns true if <paramref name="name" /> is already prepared (saved locally on the device).
    /// </returns>
    public abstract bool IsPrepared(string name);

    /// <summary>
    ///   Saves <paramref name="name" /> as <paramref name="uniqueKey" /> asynchronously.
    /// </summary>
    /// <param name="overwrite">
    ///   Specifies whether <paramref name="uniqueKey" /> will be overwritten if it already exists.
    /// </param>
    public abstract IEnumerator PrepareAssetAsync(string name, string uniqueKey, bool overwrite = true);

    public IEnumerator PrepareAssetAsync(string name, bool overwrite = true)
    {
      return PrepareAssetAsync(name, name, overwrite);
    }

    [AOT.MonoPInvokeCallback(typeof(PathResolver))]
    private static string PathToResourceAsFile(string assetPath)
    {
      try
      {
        return _Instance._pathResolver(assetPath);
      }
      catch (Exception e)
      {
        UnityEngine.Debug.LogException(e);
        return "";
      }
    }

    [AOT.MonoPInvokeCallback(typeof(ResourceProvider))]
    private static bool GetResourceContents(string path, IntPtr dst)
    {
      try
      {
        var asset = _Instance._resourceProvider(path);
        using (var srcStr = new StdString(asset))
        {
          srcStr.Swap(new StdString(dst, false));
        }
        return true;
      }
      catch (Exception e)
      {
        UnityEngine.Debug.LogException(e);
        return false;
      }
    }

    protected static string GetAssetNameFromPath(string assetPath)
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
