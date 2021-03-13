using Mediapipe;
using System;
using Unity.Collections;
using UnityEngine;

public class TextureFrame {
  private Texture2D texture;
  private IntPtr nativeTexturePtr = IntPtr.Zero;

  public int width { get; private set; }
  public int height { get; private set; }

  public GlTextureBuffer.DeletionCallback OnRelease;

  public TextureFrame(int width, int height, GlTextureBuffer.DeletionCallback OnRelease) {
    texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
    this.width = width;
    this.height = height;
    this.OnRelease = OnRelease;
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

  public IntPtr GetNativeTexturePtr(bool update = true) {
    if (update || nativeTexturePtr == IntPtr.Zero) {
      nativeTexturePtr = texture.GetNativeTexturePtr();
    }

    return nativeTexturePtr;
  }

  public GpuBufferFormat gpuBufferformat {
    get {
      return GpuBufferFormat.kBGRA32;
    }
  }

  public void Release() {
    OnRelease((UInt64)GetNativeTexturePtr(false), IntPtr.Zero);
  }
}
