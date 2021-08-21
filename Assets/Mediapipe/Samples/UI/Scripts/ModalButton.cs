using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class ModalButton : MonoBehaviour {
    [SerializeField] GameObject modal;
    [SerializeField] GameObject contents;

    public void OnClick() {
      if (contents != null) {
        modal.GetComponent<Modal>().Open(contents);
      }
    }
  }
}
