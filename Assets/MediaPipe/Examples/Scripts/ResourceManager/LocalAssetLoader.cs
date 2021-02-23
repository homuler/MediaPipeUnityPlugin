using Mediapipe;

public sealed class LocalAssetLoader : AssetLoader {
  void Start() {
    resourceManager = new Mediapipe.LocalAssetManager();
  }

  public override void PrepareAsset(string name, string uniqueKey, bool overwrite = false) {
    resourceManager.PrepareAsset(name, uniqueKey, overwrite);
  }

  public override void PrepareAsset(string name, bool overwrite = false) {
    resourceManager.PrepareAsset(name, name, overwrite);
  }
}
