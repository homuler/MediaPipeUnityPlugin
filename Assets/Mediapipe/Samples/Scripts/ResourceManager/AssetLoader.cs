using Mediapipe;
using UnityEngine;

public abstract class AssetLoader : MonoBehaviour  {
  protected ResourceManager resourceManager;

  public abstract void PrepareAsset(string name, string uniqueKey, bool overwrite = false);

  public abstract void PrepareAsset(string name, bool overwrite = false);
}
