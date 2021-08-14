using Mediapipe;
using Mediapipe.Unity;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TextureFrame {
  public class ReleaseEvent : UnityEvent<TextureFrame> {}

  static InstanceCacheTable<Guid, TextureFrame> instanceCacheTable = new InstanceCacheTable<Guid, TextureFrame>(100);
  static Dictionary<UInt32, Guid> nameTable = new Dictionary<UInt32, Guid>();

  readonly Texture2D texture;
  IntPtr nativeTexturePtr = IntPtr.Zero;
  GlSyncPoint glSyncToken;

  readonly Guid instanceId;
  public int width { get { return texture.width; } }
  public int height { get { return texture.height; } }
  public TextureFormat format { get { return texture.format; } }

  /// <summary>
  ///   The event that will be invoked when the TextureFrame is released.
  /// </summary>
  public readonly ReleaseEvent OnRelease;

  TextureFrame(Texture2D texture) {
    this.texture = texture;
    this.OnRelease = new ReleaseEvent();

    instanceId = Guid.NewGuid();
    nameTable.Add(GetTextureName(), instanceId);
    instanceCacheTable.Add(instanceId, this);
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

  public UInt32 GetTextureName() {
    return (UInt32)GetNativeTexturePtr();
  }

  public Guid GetInstanceID() {
    return instanceId;
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
  public static void OnReleaseTextureFrame(UInt32 textureName, IntPtr syncTokenPtr) {
    var isIdFound = nameTable.TryGetValue(textureName, out var instanceId);

    if (!isIdFound) {
      Debug.LogError($"Texture (name={textureName}) is released, but the owner TextureFrame is not found");
      return;
    }

    var isTextureFrameFound = instanceCacheTable.TryGetValue(instanceId, out var textureFrame);

    if (!isTextureFrameFound) {
      Debug.LogWarning($"The owner TextureFrame of the released texture (name={textureName}) is already garbage collected");
      return;
    }

    var glSyncToken = syncTokenPtr == IntPtr.Zero ? null : new GlSyncPoint(syncTokenPtr);
    textureFrame.Release(glSyncToken);
  }
}
