using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class GUIConsole : MonoBehaviour {
    [SerializeField] GameObject logLinePrefab;

    string _ContentPath = "Viewport/Content";

    Transform parent;
    MemoizedLogger logger;

    void Start() {
      InitializeView();
    }

    void OnDestroy() {
      logger.OnHistoryUpdate.RemoveListener(AppendLog);
    }

    void InitializeView() {
      parent = gameObject.transform.Find(_ContentPath).gameObject.transform;

      if (!(Logger.logger is MemoizedLogger)) {
        return;
      }

      logger = (MemoizedLogger)Logger.logger;
      foreach (var log in logger.histories) {
        AppendLog(log);
      }
      logger.OnHistoryUpdate.AddListener(AppendLog);
    }

    void AppendLog(MemoizedLogger.LogStruct logStruct) {
      var logLine = Instantiate(logLinePrefab, parent).GetComponent<LogLine>();
      logLine.SetLog(logStruct);
    }
  }
}