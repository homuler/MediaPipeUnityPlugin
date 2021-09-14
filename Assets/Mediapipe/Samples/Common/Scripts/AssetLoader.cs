using System;
using System.Collections;

namespace Mediapipe.Unity {
  public static class AssetLoader {
    static ResourceManager resourceManager;

    public static void Provide(ResourceManager manager) {
      resourceManager = manager;
    }

    public static IEnumerator PrepareAssetAsync(string name, string uniqueKey, bool overwrite = false) {
      if (resourceManager == null) {
#if UNITY_EDITOR
        Logger.LogWarning("ResourceManager is not provided, so default LocalResourceManager will be used");
        resourceManager = new LocalResourceManager();
#else
        throw new InvalidOperationException("ResourceManager is not provided");
#endif
      }
      return resourceManager.PrepareAssetAsync(name, uniqueKey, overwrite);
    }

    public static IEnumerator PrepareAssetAsync(string name, bool overwrite = false) {
      return PrepareAssetAsync(name, name, overwrite);
    }
  }
}
