using Mediapipe;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class TextureFrame {
  private Texture2D texture;
  private GCHandle releaseCallbackHandle;

  public int width {
    get { return texture.width; }
  }

  public int height {
    get { return texture.height; }
  }

  public GraphicsFormat graphicsFormat {
    get { return texture.graphicsFormat; }
  }

  public TextureFormat format {
    get { return texture.format; }
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

  // TODO: implement generic method
  public NativeArray<byte> GetRawNativeByteArray() {
    return texture.GetRawTextureData<byte>();
  }

  public IntPtr GetNativeTexturePtr() {
    return texture.GetNativeTexturePtr();
  }

  public GpuBufferFormat gpuBufferformat {
    get {
      return GpuBufferFormat.kBGRA32;
    }
  }

  public void OnRelease(IntPtr ptr) {
    var token = new GlSyncPoint(ptr);
    Debug.Log("OnRelease");
    token.Wait();
  }
}
