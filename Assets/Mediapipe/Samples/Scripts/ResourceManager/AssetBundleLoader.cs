using Mediapipe;

public sealed class AssetBundleLoader : AssetLoader {
  void Start() {
    resourceManager = new AssetBundleManager();
  }

  void OnDestroy() {
    ((AssetBundleManager)resourceManager).ClearAllCacheFiles();
  }

  public override void PrepareAsset(string name, string uniqueKey, bool overwrite = false) {
    resourceManager.PrepareAsset(name, uniqueKey, overwrite);
  }

  public override void PrepareAsset(string name, bool overwrite = false) {
    PrepareAsset(name, name, overwrite);
  }
}
