using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class Modal : MonoBehaviour {
    [SerializeField] Solution solution;
    GameObject contents;

    public void Open(GameObject contents) {
      this.contents = Instantiate(contents, gameObject.transform);
      this.contents.transform.localScale = new Vector3(0.8f, 0.8f, 1);
      gameObject.SetActive(true);
      solution?.Pause();
    }

    public void Close(bool forceRestart = false) {
      gameObject.SetActive(false);

      if (contents != null) {
        Destroy(contents);
      }

      if (forceRestart) {
        solution?.Play();
      } else {
        solution?.Resume();
      }
    }
  }
}
