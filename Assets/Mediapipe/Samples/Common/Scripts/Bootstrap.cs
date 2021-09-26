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
      Local,
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
      switch (assetLoaderType) {
        case AssetLoaderType.AssetBundle: {
          AssetLoader.Provide(new AssetBundleResourceManager(Path.Combine(Application.streamingAssetsPath, "mediapipe")));
          break;
        }
        case AssetLoaderType.StreamingAssets: {
          AssetLoader.Provide(new StreamingAssetsResourceManager());
          break;
        }
        default: {
#if UNITY_EDITOR
          AssetLoader.Provide(new LocalResourceManager());
          break;
#else
          Logger.LogError("LocalResourceManager is only supported on UnityEditor");
          yield break;
#endif
        }
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
#else
      inferenceMode = preferableInferenceMode;
#endif
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
