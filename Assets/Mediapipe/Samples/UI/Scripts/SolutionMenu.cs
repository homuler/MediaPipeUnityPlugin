using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Mediapipe.Unity.UI {
  public class SolutionMenu : ModalContents {
    [SerializeField] GameObject solutionRowPrefab;

    const string _GridPath = "Scroll View/Viewport/Contents/Solution Grid";

    Transform solutionGrid;

    void Start() {
      solutionGrid = transform.Find(_GridPath);

      var solutionCount = SceneManager.sceneCountInBuildSettings;
      Transform currentRow = null;

      for (var i = 1; i < solutionCount; i++) { // skip the first scene (i.e. Start Scene)
        if (i % 2 == 1) {
          // 2 buttons in a row
          currentRow = InitializeRow();
        }
        var button = GetButtonInRow(currentRow, (i - 1) % 2);

        var buildIndex = i;
        button.transform.GetComponentInChildren<Text>().text = GetSceneNameByBuildIndex(buildIndex);
        button.onClick.AddListener(() => { StartCoroutine(LoadSceneAsync(buildIndex)); });
      }

      if (solutionCount % 2 == 0) { // (solutionCount - 1) % 2 == 1
        var unusedButton = GetButtonInRow(currentRow, 1);
        HideButton(unusedButton);
      }
    }

    public override void Exit() {
      GetModal().CloseAndResume();
    }

    Transform InitializeRow() {
      return Instantiate(solutionRowPrefab, solutionGrid).transform;
    }

    Button GetButtonInRow(Transform row, int index) {
      return row.GetChild(index)?.gameObject?.GetComponent<Button>();
    }

    void HideButton(Button button) {
      var image = button.gameObject.GetComponent<Image>();
      image.color = new Color(0, 0, 0, 0);
      button.transform.GetComponentInChildren<Text>().text = null;
    }

    string GetSceneNameByBuildIndex(int buildIndex) {
      var path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
      return Path.GetFileNameWithoutExtension(path);
    }

    IEnumerator LoadSceneAsync(int buildIndex) {
      var sceneLoadReq = SceneManager.LoadSceneAsync(buildIndex);
      yield return new WaitUntil(() => sceneLoadReq.isDone);
    }
  }
}