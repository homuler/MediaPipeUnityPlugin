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
    public abstract PathResolver pathResolver { get; }
    public delegate bool ResourceProvider(string path, IntPtr output);
    public abstract ResourceProvider resourceProvider { get; }

    private static readonly object _InitLock = new object();
    private static bool _IsInitialized = false;

    public ResourceManager()
    {
      lock (_InitLock)
      {
        if (_IsInitialized)
        {
          throw new InvalidOperationException("ResourceManager can be initialized only once");
        }
        SafeNativeMethods.mp__SetCustomGlobalPathResolver__P(pathResolver);
        SafeNativeMethods.mp__SetCustomGlobalResourceProvider__P(resourceProvider);
        _IsInitialized = true;
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
