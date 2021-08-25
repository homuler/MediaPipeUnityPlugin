using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class Modal : MonoBehaviour {
    [SerializeField] Solution solution;
    GameObject contents;

    public void Open(GameObject contents) {
      this.contents = Instantiate(contents, gameObject.transform);
      this.contents.transform.localScale = new Vector3(0.8f, 0.8f, 1);
      gameObject.SetActive(true);
    }

    public void OpenAndPause(GameObject contents) {
      Open(contents);
      solution?.Pause();
    }

    public void Close() {
      gameObject.SetActive(false);

      if (contents != null) {
        Destroy(contents);
      }
    }

    public void CloseAndResume(bool forceRestart) {
      Close();

      if (forceRestart) {
        solution?.Play();
      } else {
        solution?.Resume();
      }
    }
  }
}
