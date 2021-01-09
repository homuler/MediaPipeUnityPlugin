using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

using Mediapipe;
using UnityEngine;

using Directory = System.IO.Directory;

public class SceneDirector : MonoBehaviour {
  [SerializeField] bool useGPU = true;

  GameObject webCamScreen;
  GameObject graphPrefab;
  GameObject graphContainer;
  Coroutine graphRunner;
  GpuResources gpuResources;
  GlCalculatorHelper gpuHelper;

  const int MAX_WAIT_FRAME = 500;

  bool IsAssetLoaded = false;
  bool IsAssetLoadFailed = false;

  delegate void PluginCallback(int eventId);
  IntPtr currentContext = IntPtr.Zero;

  void OnEnable() {
    var nameForGlog = Path.Combine(Application.dataPath, "MediaPipePlugin");
    var logDir = Path.Combine(Application.persistentDataPath, "Logs", "MediaPipe");

    if (!Directory.Exists(logDir)) {
      Directory.CreateDirectory(logDir);
    }

    Glog.Initialize(nameForGlog, logDir);
  }

#if UNITY_ANDROID
  void GetCurrentContext(int eventId) {
    currentContext = Egl.getCurrentContext();
  }
#endif

  async void Start() {
    webCamScreen = GameObject.Find("WebCamScreen");

#if UNITY_ANDROID
    if (useGPU) {
      PluginCallback callback = GetCurrentContext;

      var fp = Marshal.GetFunctionPointerForDelegate(callback);
      GL.IssuePluginEvent(fp, 1);
    }
#endif

#if UNITY_EDITOR
    var resourceManager = LocalAssetManager.Instance;
#else
    var resourceManager = AssetBundleManager.Instance;
#endif

    ResourceUtil.InitializeResourceManager(resourceManager);

    try {
      await resourceManager.LoadAllAssetsAsync();
      IsAssetLoaded = true;
    } catch (Exception e) {
      Debug.LogError(e);
      IsAssetLoadFailed = true;
    }
  }

  void OnDisable() {
    StopGraph();
    Glog.Shutdown();
  }

  public void ChangeWebCamDevice(WebCamDevice? webCamDevice) {
    webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice);
  }

  public void ChangeGraph(GameObject graphPrefab) {
    StopGraph();
    this.graphPrefab = graphPrefab;
    StartGraph();
  }

  void StartGraph() {
    if (graphRunner != null) {
      return;
    }

    graphRunner = StartCoroutine(RunGraph());
  }

  void StopGraph() {
    if (graphRunner != null) {
      StopCoroutine(graphRunner);
      graphRunner = null;
    }

    if (graphContainer != null) {
      Destroy(graphContainer);;
    }
  }

  IEnumerator RunGraph() {
    var webCamScreenController = webCamScreen.GetComponent<WebCamScreenController>();
    var waitFrame = MAX_WAIT_FRAME;

    yield return new WaitWhile(() => {
      waitFrame--;

      var isGraphPrefabPresent = graphPrefab != null;
      var isWebCamPlaying = webCamScreenController.isPlaying;

      if (!isGraphPrefabPresent && waitFrame % 10 == 0) {
        Debug.Log($"Waiting for a graph");
      }

      if (!isWebCamPlaying && waitFrame % 10 == 0) {
        Debug.Log($"Waiting for a WebCamDevice");
      }

      return (!isGraphPrefabPresent || !isWebCamPlaying) && waitFrame > 0;
    });

    if (graphPrefab == null) {
      Debug.LogWarning("No graph is set. Stopping...");
      yield break;
    }

    if (!webCamScreenController.isPlaying) {
      Debug.LogWarning("WebCamDevice is not working. Stopping...");
      yield break;
    }

    webCamScreenController.InitScreen();

    if (!IsAssetLoaded && !IsAssetLoadFailed) {
      Debug.Log("Waiting for assets to be loaded...");
    }

    yield return new WaitUntil(() => IsAssetLoaded || IsAssetLoadFailed);

    if (IsAssetLoadFailed) {
      Debug.LogError("Failed to load assets. Stopping...");
      yield break;
    }

    graphContainer = Instantiate(graphPrefab);
    var graph = graphContainer.GetComponent<IDemoGraph<TextureFrame>>();

    if (useGPU) {
      // TODO: have to wait for currentContext to be initialized.
      if (currentContext == IntPtr.Zero) {
        Debug.LogWarning("No EGL Context Found");
      } else {
        Debug.Log($"EGL Context Found ({currentContext})");
      }

      gpuResources = GpuResources.Create(currentContext).ConsumeValueOrDie();
      gpuHelper = new GlCalculatorHelper();
      gpuHelper.InitializeForTest(gpuResources);

      graph.Initialize(gpuResources, gpuHelper);
    } else {
      graph.Initialize();
    }

    graph.StartRun(webCamScreenController.GetScreen()).AssertOk();

    while (true) {
      yield return new WaitForEndOfFrame();

      if (!webCamScreenController.isPlaying) {
        Debug.LogWarning("WebCam is not working");
        break;
      }

      var nextFrameRequest = webCamScreenController.RequestNextFrame();
      yield return nextFrameRequest;

      var nextFrame = nextFrameRequest.textureFrame;

      graph.PushInput(nextFrame).AssertOk();
      graph.RenderOutput(webCamScreenController, nextFrame);
    }
  }
}
