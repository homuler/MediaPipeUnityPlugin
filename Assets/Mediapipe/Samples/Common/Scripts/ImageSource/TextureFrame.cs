using Mediapipe;
using Mediapipe.Unity;
using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TextureFrame {
  public class ReleaseEvent : UnityEvent<TextureFrame> {}

  static InstanceCacheTable<UInt64, TextureFrame> instanceCacheTable = new InstanceCacheTable<UInt64, TextureFrame>(100);

  readonly Texture2D texture;
  IntPtr nativeTexturePtr = IntPtr.Zero;
  GlSyncPoint glSyncToken;

  public int width { get { return texture.width; } }
  public int height { get { return texture.height; } }

  /// <summary>
  ///   The event that will be invoked when the TextureFrame is released.
  /// </summary>
  public readonly ReleaseEvent OnRelease;

  TextureFrame(Texture2D texture) {
    this.texture = texture;
    this.OnRelease = new ReleaseEvent();

    instanceCacheTable.Add(GetId(), this);
  }

  public TextureFrame(int width, int height, TextureFormat format) : this(new Texture2D(width, height, format, false)) {}
  public TextureFrame(int width, int height) : this(width, height, TextureFormat.RGBA32) {}

  public void CopyTexture(Texture dst) {
    Graphics.CopyTexture(texture, dst);
  }

  public void CopyTextureFrom(Texture src) {
    Graphics.CopyTexture(src, texture);
  }

  public Color32[] GetPixels32() {
    return texture.GetPixels32();
  }

  public NativeArray<T> GetRawTextureData<T>() where T : struct {
    return texture.GetRawTextureData<T>();
  }

  public IntPtr GetNativeTexturePtr() {
    if (nativeTexturePtr == IntPtr.Zero) {
      nativeTexturePtr = texture.GetNativeTexturePtr();
    }
    return nativeTexturePtr;
  }

  public UInt64 GetId() {
    return (UInt64)GetNativeTexturePtr();
  }

  public GpuBufferFormat gpuBufferformat {
    get {
      return GpuBufferFormat.kBGRA32;
    }
  }

  public void Release(GlSyncPoint token = null) {
    if (glSyncToken != null) {
      glSyncToken.Dispose();
    }
    glSyncToken = token;
    OnRelease.Invoke(this);
  }

  /// <summary>
  ///   Waits until the GPU has executed all commands up to the sync point.
  ///   This blocks the CPU, and ensures the commands are complete from the point of view of all threads and contexts.
  /// </summary>
  public void WaitUntilReleased() {
    if (glSyncToken == null) {
      return;
    }
    glSyncToken.Wait();
    glSyncToken.Dispose();
    glSyncToken = null;
  }

  [AOT.MonoPInvokeCallback(typeof(GlTextureBuffer.DeletionCallback))]
  public static void OnReleaseTextureFrame(UInt64 textureName, IntPtr syncTokenPtr) {
    var isTextureFrameFound = instanceCacheTable.TryGetValue(textureName, out var textureFrame);

    if (!isTextureFrameFound) {
      Debug.LogWarning($"The released texture is not found or already garbage collected: {textureName}");
      return;
    }

    var glSyncToken = syncTokenPtr == IntPtr.Zero ? null : new GlSyncPoint(syncTokenPtr);
    textureFrame.Release(glSyncToken);
  }
}
