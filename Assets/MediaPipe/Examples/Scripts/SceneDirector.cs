using System.IO;
using System.Collections;

using Mediapipe;
using UnityEngine;

using Directory = System.IO.Directory;

public class SceneDirector : MonoBehaviour {
  [SerializeField] bool useGPU = true;

  GameObject webCamScreen;
  WebCamDevice? webCamDevice;
  GameObject graphPrefab;
  GameObject graphContainer;
  SidePacket sidePacket;
  Coroutine graphRunner;
  GpuResources gpuResources;
  GlCalculatorHelper gpuHelper;

  const int MAX_WAIT_FRAME = 50;

  void OnEnable() {
    var nameForGlog = Path.Combine(Application.dataPath, "MediaPipePlugin");

    #if UNITY_EDITOR || UNITY_STANDALONE
      var logDir = Path.Combine(Application.dataPath.Replace("/Assets", ""), "Logs", "MediaPipe");
    #else
      var logDir = Path.Combine(Application.persistentDataPath, "Logs", "MediaPipe");
    #endif

    if (!Directory.Exists(logDir)) {
      Directory.CreateDirectory(logDir);
    }

    UnsafeNativeMethods.InitGoogleLogging(nameForGlog, logDir);
    ResourceUtil.SetResourceRootPath(Application.streamingAssetsPath);
  }

  void Start() {
    webCamScreen = GameObject.Find("WebCamScreen");

    if (useGPU) {
      gpuResources = GpuResources.Create().ConsumeValue();

      gpuHelper = new GlCalculatorHelper();
      gpuHelper.InitializeForTest(gpuResources);
    }
  }

  void OnDisable() {
    UnsafeNativeMethods.ShutdownGoogleLogging();
  }

  public void ChangeWebCamDevice(WebCamDevice? webCamDevice) {
    this.webCamDevice = webCamDevice;

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
      var isWebCamPlaying = webCamScreenController.IsPlaying();

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
    
    if (!webCamScreenController.IsPlaying()) {
      Debug.LogWarning("WebCamDevice is not working. Stopping...");
      yield break;
    }

    graphContainer = Instantiate(graphPrefab);
    var graph = graphContainer.GetComponent<IDemoGraph<PixelData>>();

    if (useGPU) {
      graph.Initialize(gpuResources, gpuHelper);
    } else {
      graph.Initialize();
    }

    sidePacket = new SidePacket();
    graph.StartRun(sidePacket).AssertOk();

    while (true) {
      yield return new WaitForEndOfFrame();

      if (!webCamScreenController.IsPlaying()) {
        Debug.LogWarning("WebCam is not working");
        break;
      }

      var colors = webCamScreenController.GetPixels32();
      var width = webCamScreenController.Width();
      var height = webCamScreenController.Height();
      var pixelData = new PixelData(colors, width, height);

      graph.PushInput(pixelData);
      graph.RenderOutput(webCamScreenController, pixelData);
    }
  }
}
