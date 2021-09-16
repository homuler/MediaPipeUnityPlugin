using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;

namespace Mediapipe.Unity {
  public class TextureFrame {
    public class ReleaseEvent : UnityEvent<TextureFrame> {}

    static readonly string TAG = typeof(TextureFrame).Name;

    static readonly GlobalInstanceTable<Guid, TextureFrame> instanceTable = new GlobalInstanceTable<Guid, TextureFrame>(100);
    static readonly Dictionary<UInt32, Guid> nameTable = new Dictionary<UInt32, Guid>();

    readonly Texture2D texture;
    IntPtr nativeTexturePtr = IntPtr.Zero;
    GlSyncPoint glSyncToken;

    // Buffers that will be used to copy texture data on CPU.
    // They won't be initialized until it's necessary.
    Texture2D _textureBuffer;
    Texture2D textureBuffer {
      get {
        if (_textureBuffer == null) {
          _textureBuffer = new Texture2D(texture.width, texture.height, texture.format, false);
        }
        return _textureBuffer;
      }
    }

    Color32[] _pixelsBuffer; // for WebCamTexture
    Color32[] pixelsBuffer {
      get {
        if (_pixelsBuffer == null) {
          _pixelsBuffer = new Color32[width * height];
        }
        return _pixelsBuffer;
      }
    }

    readonly Guid instanceId;
    // NOTE: width and height can be accessed from a thread other than Main Thread.
    public readonly int width;
    public readonly int height;
    public readonly TextureFormat format;

    ImageFormat.Format _format = ImageFormat.Format.UNKNOWN;
    public ImageFormat.Format imageFormat {
      get {
        if (_format == ImageFormat.Format.UNKNOWN) {
          _format = format.ToImageFormat();
        }
        return _format;
      }
    }

    public bool isReadable { get { return texture.isReadable; } }

    // TODO: determine at runtime
    public GpuBufferFormat gpuBufferformat { get { return GpuBufferFormat.kBGRA32; } }

    /// <summary>
    ///   The event that will be invoked when the TextureFrame is released.
    /// </summary>
    public readonly ReleaseEvent OnRelease;

    TextureFrame(Texture2D texture) {
      this.texture = texture;
      this.width = texture.width;
      this.height = texture.height;
      this.format = texture.format;
      this.OnRelease = new ReleaseEvent();
      instanceId = Guid.NewGuid();
      RegisterInstance(this);
    }

    public TextureFrame(int width, int height, TextureFormat format) : this(new Texture2D(width, height, format, false)) {}
    public TextureFrame(int width, int height) : this(width, height, TextureFormat.RGBA32) {}

    public void CopyTexture(Texture dst) {
      Graphics.CopyTexture(texture, dst);
    }

    public void CopyTextureFrom(Texture src) {
      Graphics.CopyTexture(src, texture);
    }

    public bool ConvertTexture(Texture dst) {
      return Graphics.ConvertTexture(texture, dst);
    }

    public bool ConvertTextureFrom(Texture src) {
      return Graphics.ConvertTexture(src, texture);
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    ///   If <paramref name="src" /> format is different from <see cref="format" />, it converts the format.
    /// </summary>
    /// <remarks>
    ///   After calling it, pixel data won't be read from CPU safely.
    /// </remarks>
    public bool ReadTextureFromOnGPU(Texture src) {
      if (GetTextureFormat(src) != format) {
        return Graphics.ConvertTexture(src, texture);
      }
      Graphics.CopyTexture(src, texture);
      return true;
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    /// </summary>
    /// <remarks>
    ///   This operation is slow.
    ///   If CPU won't access the pixel data, use <see cref="ReadTextureFromOnGPU" /> instead.
    /// </remarks>
    public void ReadTextureFromOnCPU(Texture src) {
      var currentRenderTexture = RenderTexture.active;
      var tmpRenderTexture = new RenderTexture(src.width, src.height, 32);
      Graphics.Blit(src, tmpRenderTexture);
      RenderTexture.active = tmpRenderTexture;

      var rect = new UnityEngine.Rect(0, 0, Mathf.Min(tmpRenderTexture.width, textureBuffer.width), Mathf.Min(tmpRenderTexture.height, textureBuffer.height));
      textureBuffer.ReadPixels(rect, 0, 0);
      textureBuffer.Apply();
      RenderTexture.active = currentRenderTexture;

      SetPixels32(textureBuffer.GetPixels32());
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    /// </summary>
    /// <remarks>
    ///   In most cases, it should be better to use <paramref name="src" /> directly.
    /// </remarks>
    public void ReadTextureFromOnCPU(Texture2D src) {
      SetPixels32(src.GetPixels32());
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    /// </summary>
    /// <param name="src">
    ///   The texture from which the pixels are read.
    ///   Its width and height must match that of the TextureFrame.
    /// </param>
    /// <remarks>
    ///   This operation is slow.
    ///   If CPU won't access the pixel data, use <see cref="ReadTextureFromOnGPU" /> instead.
    /// </remarks>
    public void ReadTextureFromOnCPU(WebCamTexture src) {
      SetPixels32(src.GetPixels32(pixelsBuffer));
    }

    public Color GetPixel(int x, int y) {
      return texture.GetPixel(x, y);
    }

    public Color32[] GetPixels32() {
      return texture.GetPixels32();
    }

    public void SetPixels32(Color32[] pixels) {
      var oldName = GetTextureName();

      texture.SetPixels32(pixels);
      texture.Apply();
      nativeTexturePtr = IntPtr.Zero;

      ChangeNameFrom(oldName);
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

    public ImageFrame BuildImageFrame() {
      var bytes = GetRawTextureData<byte>();
      return new ImageFrame(imageFormat, width, height, 4 * width, GetRawTextureData<byte>());
    }

    public GpuBuffer BuildGpuBuffer(GlContext glContext) {
      var glTextureBuffer = new GlTextureBuffer(GetTextureName(), width, height, gpuBufferformat, OnReleaseTextureFrame, glContext);
      return new GpuBuffer(glTextureBuffer);
    }

    public void RemoveAllReleaseListeners() {
      OnRelease.RemoveAllListeners();
    }

    // TODO: stop invoking OnRelease when it's already released
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
        Logger.LogError(TAG, $"Texture (name={textureName}) is released, but the owner TextureFrame is not found");
        return;
      }

      var isTextureFrameFound = instanceTable.TryGetValue(instanceId, out var textureFrame);

      if (!isTextureFrameFound) {
        Logger.LogWarning(TAG, $"The owner TextureFrame of the released texture (name={textureName}) is already garbage collected");
        return;
      }

      var glSyncToken = syncTokenPtr == IntPtr.Zero ? null : new GlSyncPoint(syncTokenPtr);
      textureFrame.Release(glSyncToken);
    }

    static void RegisterInstance(TextureFrame textureFrame) {
      var name = textureFrame.GetTextureName();
      var id = textureFrame.instanceId;
      lock (((ICollection)nameTable).SyncRoot) {
        if (AcquireName(name, id)) {
          instanceTable.Add(id, textureFrame);
          nameTable.Add(name, id);
          return;
        }
      }
      throw new ArgumentException("Another instance has the same name");
    }

    /// <summary>
    ///   Remove <paramref name="name" /> from <see cref="nameTable" /> if it's stale.
    ///   If <paramref name="name" /> does not exist in <see cref="nameTable" />, do nothing.
    /// </summary>
    /// <remarks>
    ///   If the instance whose id is <paramref name="ownerId" /> owns <paramref name="name" /> now, it still removes <paramref name="name" />.
    /// </remarks>
    /// <returns>Return if name is available</returns>
    static bool AcquireName(UInt32 name, Guid ownerId) {
      if (nameTable.TryGetValue(name, out var id)) {
        if (ownerId != id && instanceTable.TryGetValue(id, out var instance)) {
          // if instance is found, the instance is using the name.
          Logger.LogVerbose($"{id} is using {name} now");
          return false;
        }
        nameTable.Remove(name);
      }
      return true;
    }

    static TextureFormat GetTextureFormat(Texture texture) {
      return GraphicsFormatUtility.GetTextureFormat(texture.graphicsFormat);
    }

    void ChangeNameFrom(UInt32 oldName) {
      var newName = GetTextureName();
      lock (((ICollection)nameTable).SyncRoot) {
        if (!AcquireName(newName, instanceId)) {
          throw new ArgumentException("Another instance is using the specified name now");
        }
        nameTable.Remove(oldName);
        nameTable.Add(newName, instanceId);
      }
    }

    Texture2D GetTextureBufferFor(Texture texture) {
      var textureFormat = GetTextureFormat(texture);

      if (_textureBuffer == null || _textureBuffer.format != textureFormat) {
        _textureBuffer = new Texture2D(texture.width, texture.height, textureFormat, false);
      }

      var currentRenderTexture = RenderTexture.active;
      var tmpRenderTexture = new RenderTexture(texture.width, texture.height, 32);
      Graphics.Blit(texture, tmpRenderTexture);
      RenderTexture.active = tmpRenderTexture;

      _textureBuffer.ReadPixels(new UnityEngine.Rect(0, 0, tmpRenderTexture.width, tmpRenderTexture.height), 0, 0);
      _textureBuffer.Apply();
      RenderTexture.active = currentRenderTexture;

      return _textureBuffer;
    }
  }
}
