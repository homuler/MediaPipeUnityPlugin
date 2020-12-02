using System;
using UnityEngine;

public class WebCamScreenController : MonoBehaviour {
  [SerializeField] int DefaultWidth = 640;
  [SerializeField] int DefaultHeight = 480;
  [SerializeField] int FPS = 30;
  [SerializeField] float focalLengthPx = 2.0f;

  private WebCamTexture webCamTexture;
  private Texture2D outputTexture;
  private Color32[] pixelData;

  private const int TEXTURE_SIZE_THRESHOLD = 50;

  public void ResetScreen(WebCamDevice? device) {
    if (webCamTexture != null && webCamTexture.isPlaying) {
      webCamTexture.Stop();
      webCamTexture = null;
    }

    if (device == null) return;

    webCamTexture = new WebCamTexture(device?.name, DefaultWidth, DefaultHeight, FPS);

    try {
      webCamTexture.Play();
    } catch (Exception e) {
      Debug.LogWarning(e.ToString());
      return;
    }
  }

  private bool IsWebCamTextureInitialized() {
    // Some cameras may take time to be initialized, so check the texture size.
    return webCamTexture != null && webCamTexture.width > TEXTURE_SIZE_THRESHOLD;
  }

  public bool IsPlaying() {
    return IsWebCamTextureInitialized() && webCamTexture.isPlaying;
  }

  public int Height() {
    return IsPlaying() ? webCamTexture.height : 0;
  }

  public int Width() {
    return IsPlaying() ? webCamTexture.width : 0;
  }

  public float FocalLengthPx() {
    return IsPlaying() ? focalLengthPx : 0;
  }

  public Color32[] GetPixels32() {
    return IsPlaying() ? webCamTexture.GetPixels32(pixelData) : null;
  }

  public void InitScreen() {
    Renderer renderer = GetComponent<Renderer>();
    outputTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
    renderer.material.mainTexture = outputTexture;

    pixelData = new Color32[webCamTexture.width * webCamTexture.height];
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
