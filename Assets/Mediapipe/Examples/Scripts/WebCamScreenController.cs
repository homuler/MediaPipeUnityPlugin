using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FrameEvent : UnityEvent<Color32[], int, int>{}

public class WebCamScreenController : MonoBehaviour {
  [SerializeField] int DefaultHeight = 1920;
  [SerializeField] int DefaultWidth = 1080;
  [SerializeField] int FPS = 30;

  private WebCamTexture webCamTexture;
  private Texture2D outputTexture;
  private Color32[] pixelData;

  public void ResetScreen(WebCamDevice? device) {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      webCamTexture.Stop();
      webCamTexture = null;
    }

    if (device == null) return;

    webCamTexture = new WebCamTexture(device?.name, DefaultHeight, DefaultWidth, FPS);
    webCamTexture.Play();

    Renderer renderer = GetComponent<Renderer>();
    outputTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
    renderer.material.mainTexture = outputTexture;

    pixelData = new Color32[webCamTexture.width * webCamTexture.height];
  }

  public bool isPlaying() {
    return webCamTexture == null ? false : webCamTexture.isPlaying;
  }

  public int Height() {
    return isPlaying() ? webCamTexture.height : 0;
  }

  public int Width() {
    return isPlaying() ? webCamTexture.width : 0;
  }

  public Color32[] GetPixels32() {
    return isPlaying() ? webCamTexture.GetPixels32(pixelData) : null;
  }

  public void DrawScreen(Color32[] colors) {
    // TODO: size assertion
    outputTexture.SetPixels32(colors);
    outputTexture.Apply();
  }
}
