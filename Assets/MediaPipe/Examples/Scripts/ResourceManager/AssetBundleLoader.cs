using Mediapipe;

public sealed class AssetBundleLoader : AssetLoader {
  void Start() {
    resourceManager = new AssetBundleManager();
  }

  void OnDestroy() {
    ((AssetBundleManager)resourceManager).ClearAllCacheFiles();
  }

  public override void PrepareAsset(string name, string uniqueKey) {
    if (!resourceManager.IsPrepared(uniqueKey)) {
      resourceManager.PrepareAsset(name, uniqueKey);
    }
  }

  public override void PrepareAsset(string name) {
    PrepareAsset(name, name);
  }
}
