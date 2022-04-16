// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Mediapipe.Unity
{
  public class StartSceneController : MonoBehaviour
  {
    private const string _TAG = nameof(Bootstrap);

    [SerializeField] private Image _screen;
    [SerializeField] private GameObject _consolePrefab;

    private IEnumerator Start()
    {
      var _ = Instantiate(_consolePrefab, _screen.transform);

      var bootstrap = GetComponent<Bootstrap>();

      yield return new WaitUntil(() => bootstrap.isFinished);

      DontDestroyOnLoad(gameObject);

      Logger.LogInfo(_TAG, "Loading the first scene...");
      var sceneLoadReq = SceneManager.LoadSceneAsync(1);
      yield return new WaitUntil(() => sceneLoadReq.isDone);
    }
  }
}
