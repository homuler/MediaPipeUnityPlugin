using System;
using UnityEngine;

public class WebCamScreenController : MonoBehaviour {
  [SerializeField] int DefaultHeight = 640;
  [SerializeField] int DefaultWidth = 480;
  [SerializeField] int FPS = 30;
  [SerializeField] float focalLengthPx = 2.0f;

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

    try {
      webCamTexture.Play();
    } catch (Exception e) {
      Debug.LogWarning(e.Message);
      return;
    }

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

  public float FocalLengthPx() {
    return isPlaying() ? focalLengthPx : 0;
  }

  public Color32[] GetPixels32() {
    return isPlaying() ? webCamTexture.GetPixels32(pixelData) : null;
  }

  public Texture2D GetScreen() {
    return outputTexture;
  }

  public void DrawScreen(Color32[] colors) {
    // TODO: size assertion
    outputTexture.SetPixels32(colors);
    outputTexture.Apply();
  }
}
