// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Mediapipe.Unity
{
  public class Bootstrap : MonoBehaviour
  {
    [System.Serializable]
    public enum AssetLoaderType
    {
      StreamingAssets,
      AssetBundle,
      Local,
    }

    private const string _TAG = nameof(Bootstrap);

    [SerializeField] private Image _screen;
    [SerializeField] private GameObject _consolePrefab;
    [SerializeField] private ImageSource.SourceType _defaultImageSource;
    [SerializeField] private InferenceMode _preferableInferenceMode;
    [SerializeField] private AssetLoaderType _assetLoaderType;
    [SerializeField] private bool _enableGlog = true;

    public InferenceMode inferenceMode { get; private set; }
    public bool isFinished { get; private set; }
    private bool _isGlogInitialized;

    private IEnumerator Start()
    {
      Logger.SetLogger(new MemoizedLogger(100));
      Logger.minLogLevel = Logger.LogLevel.Debug;

      Logger.LogInfo(_TAG, "Starting console window...");
      Instantiate(_consolePrefab, _screen.transform);
      yield return new WaitForEndOfFrame();

      Logger.LogInfo(_TAG, "Setting global flags...");
      GlobalConfigManager.SetFlags();

      if (_enableGlog)
      {
        if (Glog.LogDir != null)
        {
          if (!Directory.Exists(Glog.LogDir))
          {
            Directory.CreateDirectory(Glog.LogDir);
          }
          Logger.LogVerbose(_TAG, $"Glog will output files under {Glog.LogDir}");
        }
        Glog.Initialize("MediaPipeUnityPlugin");
        _isGlogInitialized = true;
      }

      Logger.LogInfo(_TAG, "Initializing AssetLoader...");
      switch (_assetLoaderType)
      {
        case AssetLoaderType.AssetBundle:
          {
            AssetLoader.Provide(new AssetBundleResourceManager("mediapipe"));
            break;
          }
        case AssetLoaderType.StreamingAssets:
          {
            AssetLoader.Provide(new StreamingAssetsResourceManager());
            break;
          }
        case AssetLoaderType.Local:
          {
#if UNITY_EDITOR
            AssetLoader.Provide(new LocalResourceManager());
            break;
#else
          Logger.LogError("LocalResourceManager is only supported on UnityEditor");
          yield break;
#endif
          }
        default:
          {
            Logger.LogError($"AssetLoaderType is unknown: {_assetLoaderType}");
            yield break;
          }
      }

      DecideInferenceMode();
      if (inferenceMode == InferenceMode.GPU)
      {
        Logger.LogInfo(_TAG, "Initializing GPU resources...");
        yield return GpuManager.Initialize();
      }

      Logger.LogInfo(_TAG, "Preparing ImageSource...");
      ImageSourceProvider.SwitchSource(_defaultImageSource);
      DontDestroyOnLoad(GameObject.Find("Image Source"));

      DontDestroyOnLoad(gameObject);
      isFinished = true;

      Logger.LogInfo(_TAG, "Loading the first scene...");
      var sceneLoadReq = SceneManager.LoadSceneAsync(1);
      yield return new WaitUntil(() => sceneLoadReq.isDone);
    }

    private void DecideInferenceMode()
    {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
      if (_preferableInferenceMode == InferenceMode.GPU) {
        Logger.LogWarning(_TAG, "Current platform does not support GPU inference mode, so falling back to CPU mode");
      }
      inferenceMode = InferenceMode.CPU;
#else
      inferenceMode = _preferableInferenceMode;
#endif
    }

    private void OnApplicationQuit()
    {
      GpuManager.Shutdown();

      if (_isGlogInitialized)
      {
        Glog.Shutdown();
      }

      Logger.SetLogger(null);
    }
  }
}
