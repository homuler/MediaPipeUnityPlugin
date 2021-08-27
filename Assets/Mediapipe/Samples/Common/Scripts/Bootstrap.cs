using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Mediapipe.Unity {
  public class Bootstrap : MonoBehaviour {
    [System.Serializable]
    public enum AssetLoaderType {
      StreamingAssets,
      AssetBundle,
    }

    static readonly string TAG = typeof(Bootstrap).Name;

    [SerializeField] Image screen;
    [SerializeField] GameObject consolePrefab;
    [SerializeField] ImageSource.SourceType defaultImageSource;
    [SerializeField] InferenceMode preferableInferenceMode;
    [SerializeField] AssetLoaderType assetLoaderType;
    [SerializeField] bool enableGlog = true;

    public InferenceMode inferenceMode { get; private set; }
    public bool isFinished { get; private set; }
    bool isGlogInitialized;

    IEnumerator Start() {
      Logger.SetLogger(new MemoizedLogger(100));
      Logger.minLogLevel = Logger.LogLevel.Debug;

      Logger.LogInfo(TAG, "Starting console window...");
      Instantiate(consolePrefab, screen.transform);
      yield return new WaitForEndOfFrame();

      Logger.LogInfo(TAG, "Setting global flags...");
      GlobalConfigManager.SetFlags();

      if (enableGlog) {
        if (Glog.logDir != null) {
          if (!Directory.Exists(Glog.logDir)) {
            Directory.CreateDirectory(Glog.logDir);
          }
          Logger.LogVerbose(TAG, $"Glog will output files under {Glog.logDir}");
        }
        Glog.Initialize("MediaPipeUnityPlugin");
        isGlogInitialized = true;
      }

      Logger.LogInfo(TAG, "Initializing AssetLoader...");
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
        Logger.LogInfo(TAG, "Initializing GPU resources...");
        yield return GpuManager.Initialize();
      }

      Logger.LogInfo(TAG, "Preparing ImageSource...");
      ImageSourceProvider.SwitchSource(defaultImageSource);
      DontDestroyOnLoad(GameObject.Find("Image Source"));

      DontDestroyOnLoad(this.gameObject);
      isFinished = true;

      Logger.LogInfo(TAG, "Loading the first scene...");
      var sceneLoadReq = SceneManager.LoadSceneAsync(1);
      yield return new WaitUntil(() => sceneLoadReq.isDone);
    }

    void DecideInferenceMode() {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
      if (preferableInferenceMode == InferenceMode.GPU) {
        Logger.LogWarning(TAG, "Current platform does not support GPU inference mode, so falling back to CPU mode");
      }
      inferenceMode = InferenceMode.CPU;
#endif
      inferenceMode = preferableInferenceMode;
    }

    void OnApplicationQuit() {
      GpuManager.Shutdown();

      if (isGlogInitialized) {
        Glog.Shutdown();
      }

      Logger.SetLogger(null);
    }
  }
}
