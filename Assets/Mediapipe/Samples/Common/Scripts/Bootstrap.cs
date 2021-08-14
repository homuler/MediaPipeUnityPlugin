using System.Collections;
using System.IO;
using UnityEngine;

namespace Mediapipe.Unity {
  public class Bootstrap : MonoBehaviour {
    [System.Serializable]
    public enum AssetLoaderType {
      StreamingAssets,
      AssetBundle,
    }

    [SerializeField] ImageSource.SourceType defaultImageSource;
    [SerializeField] InferenceMode preferableInferenceMode;
    [SerializeField] AssetLoaderType assetLoaderType;
    [SerializeField] bool enableGlog = true;

    public InferenceMode inferenceMode { get; private set; }
    public bool isFinished { get; private set; }
    bool isGlogInitialized;

    IEnumerator Start() {
      // GlobalConfigManager must be initialized before loading MediaPipe libraries.
      Debug.Log("Setting environment variables...");
      GlobalConfigManager.SetEnvs();

      if (enableGlog) {
        var logDir = GlobalConfigManager.GlogLogDir;
        if (!Directory.Exists(logDir)) {
          Directory.CreateDirectory(logDir);
        }
        Glog.Initialize("MediaPipeUnityPlugin");
        isGlogInitialized = true;
      }

      Debug.Log("Initializing AssetLoader...");
      if (assetLoaderType == AssetLoaderType.AssetBundle) {
#if UNITY_EDITOR
        AssetLoader.Provide(new LocalResourceManager());
#else
        AssetLoader.Provide(new AssetBundleResourceManager(Path.Combine(Application.streamingAssetsPath, "mediapipe")));
#endif
      } else {
        AssetLoader.Provide(new StreamingAssetsResourceManager());
      }

      DecideInferenceMode();
      if (inferenceMode == InferenceMode.GPU) {
        Debug.Log("Initializing GPU resources...");
        yield return GpuManager.Initialize();
      }

      Debug.Log("Preparing ImageSource...");
      ImageSourceProvider.SwitchSource(defaultImageSource);
      DontDestroyOnLoad(GameObject.Find("Image Source"));

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

    void OnDestroy() {
      if (isGlogInitialized) {
        Glog.Shutdown();
      }
    }
  }
}