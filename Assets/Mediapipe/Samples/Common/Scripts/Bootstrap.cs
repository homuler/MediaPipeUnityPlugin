using Mediapipe;
using System;
using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity {
  public class Bootstrap : MonoBehaviour {
    [System.Serializable]
    public enum AssetLoaderType {
      StreamingAssets,
      AssetBundle,
    }

    [System.Serializable]
    public enum InferenceMode {
      GPU,
      CPU,
    }

    [SerializeField] InferenceMode preferableInferenceMode;
    [SerializeField] AssetLoaderType assetLoaderType;

    public InferenceMode inferenceMode { get; private set; }
    public bool isFinished { get; private set; }

    IEnumerator Start() {
      // GlobalConfigManager must be initialized before loading MediaPipe libraries.
      Debug.Log("Setting environment variables...");
      GlobalConfigManager.SetEnvs();

      Debug.Log("Initializing AssetLoader...");
      if (assetLoaderType == AssetLoaderType.AssetBundle) {
        AssetLoader.Provide(new AssetBundleManager());
      } else {
        AssetLoader.Provide(new LocalAssetManager());
      }

      DecideInferenceMode();
      if (inferenceMode == InferenceMode.GPU) {
        Debug.Log("Initializing GPU resources...");
        yield return GpuManager.Initialize();
      }

      DontDestroyOnLoad(this.gameObject);
      isFinished = true;
    }

    void DecideInferenceMode() {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
      if (preferableInferenceMode == InferenceMode.GPU) {
        Debug.LogWarning("Current platform does not support GPU inference mode, so falling back to CPU mode");
      }
      inferenceMode = InferenceMode.CPU;
#endif
      inferenceMode = preferableInferenceMode;
    }
  }
}