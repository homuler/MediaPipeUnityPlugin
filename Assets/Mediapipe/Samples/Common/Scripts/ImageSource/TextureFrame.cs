using Mediapipe;
using System;
using Unity.Collections;
using UnityEngine;

public class TextureFrame {
  private readonly Texture2D texture;
  private IntPtr nativeTexturePtr = IntPtr.Zero;

  public int width { get { return texture.width; } }
  public int height { get { return texture.height; } }

  public readonly GlTextureBuffer.DeletionCallback OnRelease;

  public TextureFrame(int width, int height, GlTextureBuffer.DeletionCallback OnRelease) {
    texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
    this.OnRelease = OnRelease;
  }

  public void CopyTexture(Texture dst) {
    Graphics.CopyTexture(texture, dst);
  }

  public void CopyTextureFrom(WebCamTexture src) {
    Graphics.CopyTexture(src, texture);
    nativeTexturePtr = IntPtr.Zero;
  }

  public Color32[] GetPixels32() {
    return texture.GetPixels32();
  }

  // TODO: implement generic method
  public NativeArray<byte> GetRawNativeByteArray() {
    return texture.GetRawTextureData<byte>();
  }

  public IntPtr GetNativeTexturePtr() {
    if (nativeTexturePtr == IntPtr.Zero) {
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
    OnRelease((UInt64)GetNativeTexturePtr(), IntPtr.Zero);
  }
}
