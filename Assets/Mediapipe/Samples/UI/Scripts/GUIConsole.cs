using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.UI {
  public class GUIConsole : MonoBehaviour {
    [SerializeField] GameObject logLinePrefab;
    [SerializeField] int maxLines = 200;

    string _ContentPath = "Viewport/Content";

    Transform contentRoot;
    MemoizedLogger logger;
    Queue<MemoizedLogger.LogStruct> scheduledLogs;
    int lines = 0;

    ScrollRect scrollRect { get { return gameObject.GetComponent<ScrollRect>(); } }

    void Start() {
      scheduledLogs = new Queue<MemoizedLogger.LogStruct>();
      InitializeView();
    }

    void LateUpdate() {
      RenderScheduledLogs();
    }

    void OnDestroy() {
      logger.OnLogOutput -= ScheduleLog;
    }

    void InitializeView() {
      contentRoot = gameObject.transform.Find(_ContentPath).gameObject.transform;

      if (!(Logger.logger is MemoizedLogger)) {
        return;
      }

      logger = (MemoizedLogger)Logger.logger;
      lock (((ICollection)logger.histories).SyncRoot) {
        foreach (var log in logger.histories) {
          AppendLog(log);
        }
        logger.OnLogOutput += ScheduleLog;
      }

      StartCoroutine(ScrollToBottom());
    }

    void ScheduleLog(MemoizedLogger.LogStruct logStruct) {
      lock (((ICollection)scheduledLogs).SyncRoot) {
        scheduledLogs.Enqueue(logStruct);
      }
    }

    void RenderScheduledLogs() {
      lock (((ICollection)scheduledLogs).SyncRoot) {
        while (scheduledLogs.Count > 0) {
          AppendLog(scheduledLogs.Dequeue());
        }
      }

      if (scrollRect.verticalNormalizedPosition < 1e-6) {
        StartCoroutine(ScrollToBottom());
      }
    }

    void AppendLog(MemoizedLogger.LogStruct logStruct) {
      var logLine = Instantiate(logLinePrefab, contentRoot).GetComponent<LogLine>();
      logLine.SetLog(logStruct);

      if (++lines > maxLines) {
        Destroy(contentRoot.GetChild(0).gameObject);
        lines--;
      }
    }

    IEnumerator ScrollToBottom() {
      yield return new WaitForEndOfFrame();
      Canvas.ForceUpdateCanvases();
      scrollRect.verticalNormalizedPosition = 0f;
    }
  }
}
