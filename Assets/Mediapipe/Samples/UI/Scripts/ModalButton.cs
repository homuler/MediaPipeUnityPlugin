using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class ModalButton : MonoBehaviour {
    [SerializeField] GameObject modalPanel;
    [SerializeField] GameObject contents;

    Modal modal {
      get { return modalPanel.GetComponent<Modal>(); }
    }

    public void Open() {
      if (contents != null) {
        modal.Open(contents);
      }
    }

    public void OpenAndPause() {
      if (contents != null) {
        modal.OpenAndPause(contents);
      }
    }
  }
}
