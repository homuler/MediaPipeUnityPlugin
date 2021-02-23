using UnityEngine;

public sealed class AssetBundleLoader : MonoBehaviour {
  Mediapipe.AssetBundleManager resourceManager;

  void Start() {
    resourceManager = new Mediapipe.AssetBundleManager();
  }

  void PrepareAsset(string name, string uniqueKey) {
    resourceManager.PrepareAsset(name, uniqueKey);
  }

  void PrepareAsset(string name) {
    resourceManager.PrepareAsset(name, name);
  }
}
