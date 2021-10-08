// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.UI
{
  public class GUIConsole : MonoBehaviour
  {
    [SerializeField] private GameObject _logLinePrefab;
    [SerializeField] private int _maxLines = 200;

    private const string _ContentPath = "Viewport/Content";

    private Transform _contentRoot;
    private MemoizedLogger _logger;
    private Queue<MemoizedLogger.LogStruct> _scheduledLogs;
    private int _lines = 0;

    private ScrollRect scrollRect => gameObject.GetComponent<ScrollRect>();

    private void Start()
    {
      _scheduledLogs = new Queue<MemoizedLogger.LogStruct>();
      InitializeView();
    }

    private void LateUpdate()
    {
      RenderScheduledLogs();
    }

    private void OnDestroy()
    {
      _logger.OnLogOutput -= ScheduleLog;
    }

    private void InitializeView()
    {
      _contentRoot = gameObject.transform.Find(_ContentPath).gameObject.transform;

      if (!(Logger.logger is MemoizedLogger))
      {
        return;
      }

      _logger = (MemoizedLogger)Logger.logger;
      lock (((ICollection)_logger.histories).SyncRoot)
      {
        foreach (var log in _logger.histories)
        {
          AppendLog(log);
        }
        _logger.OnLogOutput += ScheduleLog;
      }

      var _ = StartCoroutine(ScrollToBottom());
    }

    private void ScheduleLog(MemoizedLogger.LogStruct logStruct)
    {
      lock (((ICollection)_scheduledLogs).SyncRoot)
      {
        _scheduledLogs.Enqueue(logStruct);
      }
    }

    private void RenderScheduledLogs()
    {
      lock (((ICollection)_scheduledLogs).SyncRoot)
      {
        while (_scheduledLogs.Count > 0)
        {
          AppendLog(_scheduledLogs.Dequeue());
        }
      }

      if (scrollRect.verticalNormalizedPosition < 1e-6)
      {
        var _ = StartCoroutine(ScrollToBottom());
      }
    }

    private void AppendLog(MemoizedLogger.LogStruct logStruct)
    {
      var logLine = Instantiate(_logLinePrefab, _contentRoot).GetComponent<LogLine>();
      logLine.SetLog(logStruct);

      if (++_lines > _maxLines)
      {
        Destroy(_contentRoot.GetChild(0).gameObject);
        _lines--;
      }
    }

    private IEnumerator ScrollToBottom()
    {
      yield return new WaitForEndOfFrame();
      Canvas.ForceUpdateCanvases();
      scrollRect.verticalNormalizedPosition = 0f;
    }
  }
}
