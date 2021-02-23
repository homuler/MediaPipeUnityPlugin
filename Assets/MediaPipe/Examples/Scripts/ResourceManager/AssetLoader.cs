using Mediapipe;
using UnityEngine;

public abstract class AssetLoader : MonoBehaviour  {
  protected ResourceManager resourceManager;

  public abstract void PrepareAsset(string name, string uniqueKey);

  public abstract void PrepareAsset(string name);
}
