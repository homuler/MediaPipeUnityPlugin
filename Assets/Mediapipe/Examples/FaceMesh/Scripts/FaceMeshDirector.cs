using System.Collections;

using Mediapipe;
using UnityEngine;

public class FaceMeshDirector : Director {
  protected override IEnumerator RunGraph() {
    var webCamScreenController = webCamScreen.GetComponent<WebCamScreenController>();

    graph.StartRun(new SidePacket()).AssertOk();

    while (true) {
      yield return new WaitForEndOfFrame();

      if (!webCamScreenController.isPlaying()) {
        Debug.LogWarning("WebCam is not working");
        break;
      }

      var pixelData = webCamScreenController.GetPixels32();
      var width = webCamScreenController.Width();
      var height = webCamScreenController.Height();

      graph.PushColor32(pixelData, width, height);
      graph.RenderOutput(webCamScreenController.GetScreen(), pixelData);
    }
  }
}
