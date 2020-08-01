using UnityEngine;

public class WebCamScreenController : MonoBehaviour {
  private WebCamTexture webCamTexture;
  public int Height = 1920;
  public int Width = 1080;
  public int FPS = 30;

  public void ResetScreen(WebCamDevice? device) {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      webCamTexture.Stop();
      webCamTexture = null;
    }

    if (device == null) return;

    webCamTexture = new WebCamTexture(device?.name, Height, Width, FPS);
    Renderer renderer = GetComponent<Renderer>();
    renderer.material.mainTexture = webCamTexture;
    webCamTexture.Play();
  }
}
