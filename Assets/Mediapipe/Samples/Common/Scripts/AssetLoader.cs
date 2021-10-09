// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;

namespace Mediapipe.Unity
{
  public static class AssetLoader
  {
    private static ResourceManager _ResourceManager;

    public static void Provide(ResourceManager manager)
    {
      _ResourceManager = manager;
    }

    public static IEnumerator PrepareAssetAsync(string name, string uniqueKey, bool overwrite = false)
    {
      if (_ResourceManager == null)
      {
#if UNITY_EDITOR
        Logger.LogWarning("ResourceManager is not provided, so default LocalResourceManager will be used");
        _ResourceManager = new LocalResourceManager();
#else
        throw new System.InvalidOperationException("ResourceManager is not provided");
#endif
      }
      return _ResourceManager.PrepareAssetAsync(name, uniqueKey, overwrite);
    }

    public static IEnumerator PrepareAssetAsync(string name, bool overwrite = false)
    {
      return PrepareAssetAsync(name, name, overwrite);
    }
  }
}
