using UnityEngine;

public sealed class LocalAssetLoader : MonoBehaviour {
  Mediapipe.LocalAssetManager resourceManager;

  void Start() {
    resourceManager = new Mediapipe.LocalAssetManager();
  }

  void PrepareAsset(string name, string uniqueKey) {
    resourceManager.PrepareAsset(name, uniqueKey);
  }

  void PrepareAsset(string name) {
    resourceManager.PrepareAsset(name, name);
  }
}
