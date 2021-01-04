using Mediapipe;
using System;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class WebCamScreenController : MonoBehaviour {
  [SerializeField] int Width = 640;
  [SerializeField] int Height = 480;
  [SerializeField] int FPS = 30;
  [SerializeField] float FocalLengthPx = 2.0f; /// TODO: calculate it from webCamDevice info if possible.
  private const int TEXTURE_SIZE_THRESHOLD = 50;

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

  /// TODO: use Coroutine and call InitScreen here
  public void ResetScreen(WebCamDevice? device) {
    if (isPlaying) {
      webCamTexture.Stop();
      webCamTexture = null;
    }

    if (device is WebCamDevice deviceValue) {
      webCamDevice = deviceValue;
    } else {
      return;
    }

    /// TODO: call Application.RequestUserAuthorization
    webCamTexture = new WebCamTexture(webCamDevice.name, Width, Height, FPS);
    WebCamTextureFramePool.Instance.SetDimension(Width, Height);

    try {
      webCamTexture.Play();
      Debug.Log($"WebCamTexture Graphics Format: {webCamTexture.graphicsFormat}");
    } catch (Exception e) {
      Debug.LogWarning(e.ToString());
      return;
    }
  }

  public void InitScreen() {
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
    // TODO: size assertion
    outputTexture.SetPixels32(colors);
    outputTexture.Apply();
  }

  public void DrawScreen(TextureFrame src) {
    // TODO: size assertion
    src.CopyTexture(outputTexture);
  }

  public void DrawScreen(ImageFrame imageFrame) {
    outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
    outputTexture.Apply();
  }

  public void DrawScreen(GpuBuffer gpuBuffer) {
#if UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || UNITY_ANDROID
    // TODO: create an external texture
    outputTexture.UpdateExternalTexture((IntPtr)gpuBuffer.GetGlTextureBuffer().Name());
#else
    throw new NotSupportedException();
#endif
  }

  public TextureFramePool.TextureFrameRequest RequestNextFrame() {
    return WebCamTextureFramePool.Instance.RequestNextTextureFrame((TextureFrame textureFrame) => {
      textureFrame.CopyTextureFrom(webCamTexture);
    });
  }

  private class WebCamTextureFramePool : TextureFramePool {}
}
