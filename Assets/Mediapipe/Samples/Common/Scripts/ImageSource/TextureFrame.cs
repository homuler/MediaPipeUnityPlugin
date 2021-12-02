// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering;

namespace Mediapipe.Unity
{
  public class TextureFrame
  {
    public class ReleaseEvent : UnityEvent<TextureFrame> { }

    private const string _TAG = nameof(TextureFrame);

    private static readonly GlobalInstanceTable<Guid, TextureFrame> _InstanceTable = new GlobalInstanceTable<Guid, TextureFrame>(100);
    private static readonly Dictionary<uint, Guid> _NameTable = new Dictionary<uint, Guid>();

    private readonly Texture2D _texture;
    private IntPtr _nativeTexturePtr = IntPtr.Zero;
    private GlSyncPoint _glSyncToken;

    // Buffers that will be used to copy texture data on CPU.
    // They won't be initialized until it's necessary.
    private Texture2D _textureBuffer;

    private Color32[] _pixelsBuffer; // for WebCamTexture
    private Color32[] pixelsBuffer
    {
      get
      {
        if (_pixelsBuffer == null)
        {
          _pixelsBuffer = new Color32[width * height];
        }
        return _pixelsBuffer;
      }
    }

    private readonly Guid _instanceId;
    // NOTE: width and height can be accessed from a thread other than Main Thread.
    public readonly int width;
    public readonly int height;
    public readonly TextureFormat format;

    private ImageFormat.Format _format = ImageFormat.Format.UNKNOWN;
    public ImageFormat.Format imageFormat
    {
      get
      {
        if (_format == ImageFormat.Format.UNKNOWN)
        {
          _format = format.ToImageFormat();
        }
        return _format;
      }
    }

    public bool isReadable => _texture.isReadable;

    // TODO: determine at runtime
    public GpuBufferFormat gpuBufferformat => GpuBufferFormat.kBGRA32;

    /// <summary>
    ///   The event that will be invoked when the TextureFrame is released.
    /// </summary>
#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public readonly ReleaseEvent OnRelease;
#pragma warning restore IDE1006

    private TextureFrame(Texture2D texture)
    {
      _texture = texture;
      width = texture.width;
      height = texture.height;
      format = texture.format;
      OnRelease = new ReleaseEvent();
      _instanceId = Guid.NewGuid();
      RegisterInstance(this);
    }

    public TextureFrame(int width, int height, TextureFormat format) : this(new Texture2D(width, height, format, false)) { }
    public TextureFrame(int width, int height) : this(width, height, TextureFormat.RGBA32) { }

    public void CopyTexture(Texture dst)
    {
      Graphics.CopyTexture(_texture, dst);
    }

    public void CopyTextureFrom(Texture src)
    {
      Graphics.CopyTexture(src, _texture);
    }

    public bool ConvertTexture(Texture dst)
    {
      return Graphics.ConvertTexture(_texture, dst);
    }

    public bool ConvertTextureFrom(Texture src)
    {
      return Graphics.ConvertTexture(src, _texture);
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    ///   If <paramref name="src" /> format is different from <see cref="format" />, it converts the format.
    /// </summary>
    /// <remarks>
    ///   After calling it, pixel data won't be read from CPU safely.
    /// </remarks>
    public bool ReadTextureFromOnGPU(Texture src)
    {
      if (GetTextureFormat(src) != format)
      {
        return Graphics.ConvertTexture(src, _texture);
      }
      Graphics.CopyTexture(src, _texture);
      return true;
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    /// </summary>
    /// <remarks>
    ///   This operation is slow.
    ///   If CPU won't access the pixel data, use <see cref="ReadTextureFromOnGPU" /> instead.
    /// </remarks>
    public void ReadTextureFromOnCPU(Texture src)
    {
      var textureBuffer = LoadToTextureBuffer(src);
      SetPixels32(textureBuffer.GetPixels32());
    }

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    /// </summary>
    /// <remarks>
    ///   In most cases, it should be better to use <paramref name="src" /> directly.
    /// </remarks>
    public void ReadTextureFromOnCPU(Texture2D src)
    {
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
    public void ReadTextureFromOnCPU(WebCamTexture src)
    {
      SetPixels32(src.GetPixels32(pixelsBuffer));
    }

    public Color GetPixel(int x, int y)
    {
      return _texture.GetPixel(x, y);
    }

    public Color32[] GetPixels32()
    {
      return _texture.GetPixels32();
    }

    public void SetPixels32(Color32[] pixels)
    {
      var oldName = GetTextureName();

      _texture.SetPixels32(pixels);
      _texture.Apply();
      _nativeTexturePtr = IntPtr.Zero;

      ChangeNameFrom(oldName);
    }

    public NativeArray<T> GetRawTextureData<T>() where T : struct
    {
      return _texture.GetRawTextureData<T>();
    }

    public IntPtr GetNativeTexturePtr()
    {
      if (_nativeTexturePtr == IntPtr.Zero)
      {
        _nativeTexturePtr = _texture.GetNativeTexturePtr();
      }
      return _nativeTexturePtr;
    }

    public uint GetTextureName()
    {
      return (uint)GetNativeTexturePtr();
    }

    public Guid GetInstanceID()
    {
      return _instanceId;
    }

    public ImageFrame BuildImageFrame()
    {
      return new ImageFrame(imageFormat, width, height, 4 * width, GetRawTextureData<byte>());
    }

    public GpuBuffer BuildGpuBuffer(GlContext glContext)
    {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
      var glTextureBuffer = new GlTextureBuffer(GetTextureName(), width, height, gpuBufferformat, OnReleaseTextureFrame, glContext);
      return new GpuBuffer(glTextureBuffer);
#else
      throw new NotSupportedException("This method is only supported on Linux or Android");
#endif
    }

    public void RemoveAllReleaseListeners()
    {
      OnRelease.RemoveAllListeners();
    }

    // TODO: stop invoking OnRelease when it's already released
    public void Release(GlSyncPoint token = null)
    {
      if (_glSyncToken != null)
      {
        _glSyncToken.Dispose();
      }
      _glSyncToken = token;
      OnRelease.Invoke(this);
    }

    /// <summary>
    ///   Waits until the GPU has executed all commands up to the sync point.
    ///   This blocks the CPU, and ensures the commands are complete from the point of view of all threads and contexts.
    /// </summary>
    public void WaitUntilReleased()
    {
      if (_glSyncToken == null)
      {
        return;
      }
      _glSyncToken.Wait();
      _glSyncToken.Dispose();
      _glSyncToken = null;
    }

    [AOT.MonoPInvokeCallback(typeof(GlTextureBuffer.DeletionCallback))]
    public static void OnReleaseTextureFrame(uint textureName, IntPtr syncTokenPtr)
    {
      var isIdFound = _NameTable.TryGetValue(textureName, out var _instanceId);

      if (!isIdFound)
      {
        Logger.LogError(_TAG, $"nameof (name={textureName}) is released, but the owner TextureFrame is not found");
        return;
      }

      var isTextureFrameFound = _InstanceTable.TryGetValue(_instanceId, out var textureFrame);

      if (!isTextureFrameFound)
      {
        Logger.LogWarning(_TAG, $"nameof owner TextureFrame of the released texture (name={textureName}) is already garbage collected");
        return;
      }

      var _glSyncToken = syncTokenPtr == IntPtr.Zero ? null : new GlSyncPoint(syncTokenPtr);
      textureFrame.Release(_glSyncToken);
    }

    private static void RegisterInstance(TextureFrame textureFrame)
    {
      var name = textureFrame.GetTextureName();
      var id = textureFrame._instanceId;
      lock (((ICollection)_NameTable).SyncRoot)
      {
        if (AcquireName(name, id))
        {
          _InstanceTable.Add(id, textureFrame);
          _NameTable.Add(name, id);
          return;
        }
      }
      throw new ArgumentException("Another instance has the same name");
    }

    /// <summary>
    ///   Remove <paramref name="name" /> from <see cref="_NameTable" /> if it's stale.
    ///   If <paramref name="name" /> does not exist in <see cref="_NameTable" />, do nothing.
    /// </summary>
    /// <remarks>
    ///   If the instance whose id is <paramref name="ownerId" /> owns <paramref name="name" /> now, it still removes <paramref name="name" />.
    /// </remarks>
    /// <returns>Return if name is available</returns>
    private static bool AcquireName(uint name, Guid ownerId)
    {
      if (_NameTable.TryGetValue(name, out var id))
      {
        if (ownerId != id && _InstanceTable.TryGetValue(id, out var _))
        {
          // if instance is found, the instance is using the name.
          Logger.LogVerbose($"{id} is using {name} now");
          return false;
        }
        var _ = _NameTable.Remove(name);
      }
      return true;
    }

    private static TextureFormat GetTextureFormat(Texture texture)
    {
      return GraphicsFormatUtility.GetTextureFormat(texture.graphicsFormat);
    }

    private void ChangeNameFrom(uint oldName)
    {
      var newName = GetTextureName();
      lock (((ICollection)_NameTable).SyncRoot)
      {
        if (!AcquireName(newName, _instanceId))
        {
          throw new ArgumentException("Another instance is using the specified name now");
        }
        var _ = _NameTable.Remove(oldName);
        _NameTable.Add(newName, _instanceId);
      }
    }

    private Texture2D LoadToTextureBuffer(Texture texture)
    {
      var textureFormat = GetTextureFormat(texture);

      if (_textureBuffer == null || _textureBuffer.format != textureFormat)
      {
        _textureBuffer = new Texture2D(width, height, textureFormat, false);
      }

      var tmpRenderTexture = new RenderTexture(texture.width, texture.height, 32);
      var currentRenderTexture = RenderTexture.active;
      RenderTexture.active = tmpRenderTexture;

      Graphics.Blit(texture, tmpRenderTexture);

      var rect = new UnityEngine.Rect(0, 0, Mathf.Min(tmpRenderTexture.width, _textureBuffer.width), Mathf.Min(tmpRenderTexture.height, _textureBuffer.height));
      _textureBuffer.ReadPixels(rect, 0, 0);
      _textureBuffer.Apply();
      RenderTexture.active = currentRenderTexture;

      tmpRenderTexture.Release();

      return _textureBuffer;
    }
  }
}
