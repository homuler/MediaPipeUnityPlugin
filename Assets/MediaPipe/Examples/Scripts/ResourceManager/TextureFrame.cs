using Mediapipe;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

using MpGlSyncToken = System.IntPtr;

public class TextureFrame {
  private Texture2D texture;
  private GCHandle releaseCallbackHandle;

  public int width {
    get { return texture.width; }
  }

  public int height {
    get { return texture.height; }
  }

  public TextureFrame(int width, int height) {
    this.texture = new Texture2D(width, height, TextureFormat.BGRA32, false);
    releaseCallbackHandle = GCHandle.Alloc((GlTextureBuffer.DeletionCallback)this.OnRelease, GCHandleType.Pinned);
  }

  ~TextureFrame() {
    if (releaseCallbackHandle != null && releaseCallbackHandle.IsAllocated) {
      releaseCallbackHandle.Free();
    }
  }

  public void CopyTexture(Texture dst) {
    Graphics.CopyTexture(texture, dst);
  }

  public void CopyTextureFrom(WebCamTexture src) {
    // TODO: Convert format on GPU
    texture.SetPixels32(src.GetPixels32());
    texture.Apply();
  }

  public Color32[] GetPixels32() {
    return texture.GetPixels32();
  }

  public IntPtr GetNativeTexturePtr() {
    return texture.GetNativeTexturePtr();
  }

  public GpuBufferFormat gpuBufferformat {
    get {
      return GpuBufferFormat.kBGRA32;
    }
  }

  public void OnRelease(MpGlSyncToken ptr) {
    Debug.Log("OnRelease");
    var glSyncToken = new GlSyncToken(ptr);

    glSyncToken.Wait();
  }
}
