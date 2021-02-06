using Mediapipe;
using System;
using System.Collections;
using UnityEngine;

public class WebCamScreenController : MonoBehaviour {
  [SerializeField] int Width = 640;
  [SerializeField] int Height = 480;
  [SerializeField] int FPS = 30;
  [SerializeField] float FocalLengthPx = 2.0f; /// TODO: calculate it from webCamDevice info if possible.
  private const int TEXTURE_SIZE_THRESHOLD = 50;
  private const int MAX_FRAMES_TO_BE_INITIALIZED = 500;

  private WebCamDevice webCamDevice;
  private WebCamTexture webCamTexture;
  private Texture2D outputTexture;
  private Color32[] pixelData;

  public bool isPlaying {
    get { return isWebCamTextureInitialized && webCamTexture.isPlaying; }
  }

  private bool isWebCamTextureInitialized {
    get {
      // Some cameras may take time to be initialized, so check the texture size.
      return webCamTexture != null && webCamTexture.width > TEXTURE_SIZE_THRESHOLD;
    }
  }

  private bool isWebCamReady {
    get {
      return isWebCamTextureInitialized && pixelData != null;
    }
  }

  public IEnumerator ResetScreen(WebCamDevice? device) {
    if (isPlaying) {
      webCamTexture.Stop();
      webCamTexture = null;
      pixelData = null;
    }

    if (device is WebCamDevice deviceValue) {
      webCamDevice = deviceValue;
    } else {
      yield break;
    }

    webCamTexture = new WebCamTexture(webCamDevice.name, Width, Height, FPS);
    WebCamTextureFramePool.Instance.SetDimension(Width, Height);

    try {
      webCamTexture.Play();
      Debug.Log($"WebCamTexture Graphics Format: {webCamTexture.graphicsFormat}");
    } catch (Exception e) {
      Debug.LogWarning(e.ToString());
      yield break;
    }

    var waitFrame = MAX_FRAMES_TO_BE_INITIALIZED;

    yield return new WaitUntil(() => {
      return isWebCamTextureInitialized || --waitFrame < 0;
    });

    if (!isWebCamTextureInitialized) {
      Debug.LogError("Failed to initialize WebCamTexture");
      yield break;
    }

    Renderer renderer = GetComponent<Renderer>();
    outputTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
    renderer.material.mainTexture = outputTexture;

    pixelData = new Color32[webCamTexture.width * webCamTexture.height];
  }

  public float GetFocalLengthPx() {
    return isPlaying ? FocalLengthPx : 0;
  }

  public Color32[] GetPixels32() {
    return isPlaying ? webCamTexture.GetPixels32(pixelData) : null;
  }

  public IntPtr GetNativeTexturePtr() {
    return webCamTexture.GetNativeTexturePtr();
  }

  public Texture2D GetScreen() {
    return outputTexture;
  }

  public void DrawScreen(Color32[] colors) {
    if (!isWebCamReady) { return; }

    // TODO: size assertion
    outputTexture.SetPixels32(colors);
    outputTexture.Apply();
  }

  public void DrawScreen(TextureFrame src) {
    if (!isWebCamReady) { return; }

    // TODO: size assertion
    src.CopyTexture(outputTexture);
  }

  public void DrawScreen(ImageFrame imageFrame) {
    if (!isWebCamReady) { return; }

    outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
    outputTexture.Apply();
  }

  public void DrawScreen(GpuBuffer gpuBuffer) {
    if (!isWebCamReady) { return; }

#if (UNITY_STANDALONE_LINUX || UNITY_ANDROID) && !UNITY_EDITOR_OSX && !UNITY_EDITOR_WIN
    // TODO: create an external texture
    outputTexture.UpdateExternalTexture((IntPtr)gpuBuffer.GetGlTextureBuffer().Name());
#else
    throw new NotSupportedException();
#endif
  }

  public TextureFramePool.TextureFrameRequest RequestNextFrame() {
    return WebCamTextureFramePool.Instance.RequestNextTextureFrame((TextureFrame textureFrame) => {
      if (isPlaying) {
        textureFrame.CopyTextureFrom(webCamTexture);
      }
    });
  }

  private class WebCamTextureFramePool : TextureFramePool {}
}
