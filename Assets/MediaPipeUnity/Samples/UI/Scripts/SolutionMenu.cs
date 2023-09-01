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

namespace Mediapipe.Unity.Sample.UI
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class SolutionMenu : ModalContents
  {
    [SerializeField] private GameObject _solutionRowPrefab;

    private const string _GridPath = "Scroll View/Viewport/Contents/Solution Grid";

    private Transform _solutionGrid;

    private void Start()
    {
      _solutionGrid = transform.Find(_GridPath);

      var solutionCount = SceneManager.sceneCountInBuildSettings;
      Transform currentRow = null;

      for (var i = 0; i < solutionCount; i++)
      {
        if (i % 2 == 0)
        {
          // 2 buttons in a row
          currentRow = InitializeRow();
        }
        var button = GetButtonInRow(currentRow, i % 2);

        var buildIndex = i;
        button.transform.GetComponentInChildren<Text>().text = GetSceneNameByBuildIndex(buildIndex);
        button.onClick.AddListener(() => { var _ = StartCoroutine(LoadSceneAsync(buildIndex)); });
      }

      if (solutionCount % 2 == 1)
      {
        var unusedButton = GetButtonInRow(currentRow, 1);
        HideButton(unusedButton);
      }
    }

    public override void Exit()
    {
      GetModal().CloseAndResume();
    }

    private Transform InitializeRow()
    {
      return Instantiate(_solutionRowPrefab, _solutionGrid).transform;
    }

    private Button GetButtonInRow(Transform row, int index)
    {
      var child = row.GetChild(index);
      return (child == null || child.gameObject == null) ? null : child.gameObject.GetComponent<Button>();
    }

    private void HideButton(Button button)
    {
      var image = button.gameObject.GetComponent<UnityEngine.UI.Image>();
      image.color = new Color(0, 0, 0, 0);
      button.transform.GetComponentInChildren<Text>().text = null;
    }

    private string GetSceneNameByBuildIndex(int buildIndex)
    {
      var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
      return Path.GetFileNameWithoutExtension(path);
    }

    private IEnumerator LoadSceneAsync(int buildIndex)
    {
      var sceneLoadReq = SceneManager.LoadSceneAsync(buildIndex);
      yield return new WaitUntil(() => sceneLoadReq.isDone);
    }
  }
}
