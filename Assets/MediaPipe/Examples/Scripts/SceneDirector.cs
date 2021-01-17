using Mediapipe;
using System;
using System.IO;
using System.Collections;
using UnityEngine;

#if UNITY_ANDROID
using System.Runtime.InteropServices;
#endif

using Directory = System.IO.Directory;

public class SceneDirector : MonoBehaviour {
  [SerializeField] bool useGPU = true;

  readonly object graphLock = new object();
  WebCamDevice? webCamDevice;
  GameObject webCamScreen;
  Coroutine cameraSetupCoroutine;
  GameObject graphPrefab;
  GameObject graphContainer;
  Coroutine graphRunner;

  GpuResources gpuResources;
  GlCalculatorHelper gpuHelper;
  delegate void PluginCallback(int eventId);
  static IntPtr currentContext = IntPtr.Zero;

  const int MAX_WAIT_FRAME = 1000;
  bool IsAssetLoaded = false;
  bool IsAssetLoadFailed = false;

  void OnEnable() {
    var nameForGlog = Path.Combine(Application.dataPath, "MediaPipePlugin");
    var logDir = Path.Combine(Application.persistentDataPath, "Logs", "MediaPipe");

    if (!Directory.Exists(logDir)) {
      Directory.CreateDirectory(logDir);
    }

    Glog.Initialize(nameForGlog, logDir);
  }

#if UNITY_ANDROID
  [AOT.MonoPInvokeCallback(typeof(PluginCallback))]
  static void GetCurrentContext(int eventId) {
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
    StopCamera();
    Glog.Shutdown();
  }

  public void ChangeWebCamDevice(WebCamDevice? webCamDevice) {
    lock (graphLock) {
      ResetCamera(webCamDevice);

      if (graphPrefab != null) {
        StopGraph();
        StartGraph();
      }
    }
  }

  void ResetCamera(WebCamDevice? webCamDevice) {
    StopCamera();
    cameraSetupCoroutine = StartCoroutine(webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice));
    this.webCamDevice = webCamDevice;
  }

  void StopCamera() {
    if (cameraSetupCoroutine != null) {
      StopCoroutine(cameraSetupCoroutine);
      cameraSetupCoroutine = null;
    }
  }

  public void ChangeGraph(GameObject graphPrefab) {
    lock (graphLock) {
      StopGraph();
      this.graphPrefab = graphPrefab;

      if (webCamDevice != null) {
        StartGraph();
      }
    }
  }

  void StartGraph() {
    if (graphRunner != null) {
      Debug.Log("The graph is already running");
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
      Destroy(graphContainer);
    }
  }

  IEnumerator RunGraph() {
    yield return WaitForAssets();

    if (IsAssetLoadFailed) {
      Debug.LogError("Failed to load assets. Stopping...");
      yield break;
    }

    yield return WaitForGraph();

    if (graphPrefab == null) {
      Debug.LogWarning("No graph is set. Stopping...");
      yield break;
    }

    var webCamScreenController = webCamScreen.GetComponent<WebCamScreenController>();
    yield return WaitForCamera(webCamScreenController);

    if (!webCamScreenController.isPlaying) {
      Debug.LogWarning("WebCamDevice is not working. Stopping...");
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

      var nextFrameRequest = webCamScreenController.RequestNextFrame();
      yield return nextFrameRequest;

      var nextFrame = nextFrameRequest.textureFrame;

      graph.PushInput(nextFrame).AssertOk();
      graph.RenderOutput(webCamScreenController, nextFrame);
    }
  }

  IEnumerator WaitForAssets() {
    if (!IsAssetLoaded && !IsAssetLoadFailed) {
      Debug.Log("Waiting for assets to be loaded...");
    }

    yield return new WaitUntil(() => IsAssetLoaded || IsAssetLoadFailed);
  }

  IEnumerator WaitForGraph() {
    var waitFrame = MAX_WAIT_FRAME;

    yield return new WaitUntil(() => {
      waitFrame--;

      var isGraphPrefabPresent = graphPrefab != null;

      if (!isGraphPrefabPresent && waitFrame % 50 == 0) {
        Debug.Log($"Waiting for a graph");
      }

      return isGraphPrefabPresent || waitFrame < 0;
    });
  }

  IEnumerator WaitForCamera(WebCamScreenController webCamScreenController) {
    var waitFrame = MAX_WAIT_FRAME;

    yield return new WaitUntil(() => {
      waitFrame--;
      var isWebCamPlaying = webCamScreenController.isPlaying;

      if (!isWebCamPlaying && waitFrame % 50 == 0) {
        Debug.Log($"Waiting for a WebCamDevice");
      }

      return isWebCamPlaying || waitFrame < 0;
    });
  }
}
