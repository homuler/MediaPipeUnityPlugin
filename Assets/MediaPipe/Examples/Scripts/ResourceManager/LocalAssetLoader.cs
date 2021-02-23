using Mediapipe;

public sealed class LocalAssetLoader : AssetLoader {
  void Start() {
    resourceManager = new Mediapipe.LocalAssetManager();
  }

  public override void PrepareAsset(string name, string uniqueKey) {
    resourceManager.PrepareAsset(name, uniqueKey);
  }

  public override void PrepareAsset(string name) {
    resourceManager.PrepareAsset(name, name);
  }
}
