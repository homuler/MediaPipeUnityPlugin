using UnityEngine;

namespace Mediapipe.Unity {
  public static class AssetLoader {
    static ResourceManager resourceManager;

    public static void Provide(ResourceManager manager) {
      resourceManager = manager;
    }

    public static void PrepareAsset(string name, string uniqueKey, bool overwrite = false) {
      if (resourceManager == null) {
        Logger.LogWarning("ResourceManager is not provided, so default LocalResourceManager will be used");
        resourceManager = new LocalResourceManager();
      }
      resourceManager.PrepareAsset(name, uniqueKey, overwrite);
    }

    public static void PrepareAsset(string name, bool overwrite = false) {
      PrepareAsset(name, name, overwrite);
    }
  }
}
