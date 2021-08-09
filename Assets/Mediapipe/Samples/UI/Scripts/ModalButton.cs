using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class ModalButton : MonoBehaviour {
    [SerializeField] GameObject modal;
    [SerializeField] GameObject contents;

    public void OnClick() {
      modal.GetComponent<Modal>().Open(contents);
    }
  }
}
