using Mediapipe;
using UnityEngine;

public class ClassificationAnnotationController : MonoBehaviour {
  private GameObject webCamScreen;

  void Awake() {
    webCamScreen = GameObject.Find("WebCamScreen");
  }

  public void Clear() {
    Destroy(gameObject);
  }

  public void Draw(ClassificationList classificationList) {
    var arr = classificationList.Classification;
    if (arr.Count == 0 || arr[0].Score < 0.5) {
      Clear();
      return;
    }

    gameObject.GetComponent<TextMesh>().text = arr[0].Label;
  }
}
