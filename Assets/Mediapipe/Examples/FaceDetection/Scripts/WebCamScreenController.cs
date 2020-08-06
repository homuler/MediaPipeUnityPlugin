using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FrameEvent : UnityEvent<Color32[], int, int>{}

public class WebCamScreenController : MonoBehaviour {
  public int Height = 1920;
  public int Width = 1080;
  public int FPS = 30;
  public FrameEvent OnFrameRender;

  private WebCamTexture webCamTexture;
  private Color32[] pixelData;

  void Start() {
    if (OnFrameRender == null) {
      OnFrameRender = new FrameEvent();
    }
  }

  void Update() {
    if (webCamTexture == null || !webCamTexture.isPlaying) return;

    webCamTexture.GetPixels32(pixelData);
    OnFrameRender.Invoke(pixelData, webCamTexture.width, webCamTexture.height);
  }

  public void ResetScreen(WebCamDevice? device) {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      webCamTexture.Stop();
      webCamTexture = null;
    }

    if (device == null) return;

    webCamTexture = new WebCamTexture(device?.name, Height, Width, FPS);
    webCamTexture.Play();

    pixelData = new Color32[webCamTexture.width * webCamTexture.height];
  }

  public void DrawScreen(Color32[] colors) {
    // TODO: size assertion
    Texture2D texture = new Texture2D(webCamTexture.width, webCamTexture.height);
    Renderer renderer = GetComponent<Renderer>();

    renderer.material.mainTexture = texture;

    texture.SetPixels32(colors);
    texture.Apply();
  }
}
