using UnityEngine;

namespace Mediapipe.Unity.UI {
  public class ModalContents : MonoBehaviour {
    protected Modal GetModal() {
      return gameObject.transform.parent.gameObject.GetComponent<Modal>();
    }

    public void Exit() {
      GetModal().Close();
    }
  }
}
