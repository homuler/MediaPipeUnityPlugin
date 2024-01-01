// Copyright (c) 2023 homuler
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
using UnityEngine.Rendering;

namespace Mediapipe.Unity.Experimental
{
#pragma warning disable IDE0065
  using Color = UnityEngine.Color;
#pragma warning restore IDE0065

  public class TextureFrame : IDisposable
  {
    public class ReleaseEvent : UnityEvent<TextureFrame> { }

    private const string _TAG = nameof(TextureFrame);

    internal const int MaxTotalCount = 100;

    private static readonly GlobalInstanceTable<Guid, TextureFrame> _InstanceTable = new GlobalInstanceTable<Guid, TextureFrame>(MaxTotalCount);
    /// <summary>
    ///   A dictionary to look up which native texture belongs to which <see cref="TextureFrame" />.
    /// </summary>
    /// <remarks>
    ///   Not all the <see cref="TextureFrame" /> instances are registered.
    ///   Texture names are queried only when necessary, and the corresponding data will be saved then.
    /// </remarks>
    private static readonly Dictionary<uint, Guid> _NameTable = new Dictionary<uint, Guid>();

    private readonly Texture2D _texture;
    public Texture texture => _texture;

    private IntPtr _nativeTexturePtr = IntPtr.Zero;
    private GlSyncPoint _glSyncToken;

    private readonly Guid _instanceId;
    // NOTE: width and height can be accessed from a thread other than Main Thread.
    public readonly int width;
    public readonly int height;
    public readonly TextureFormat format;

    public ImageFormat.Types.Format imageFormat => format.ToImageFormat();

    public bool isReadable => _texture.isReadable;

    // TODO: determine at runtime
    public GpuBufferFormat gpuBufferformat => GpuBufferFormat.kBGRA32;

    /// <summary>
    ///   The event that will be invoked when the TextureFrame is released.
    /// </summary>
#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public readonly ReleaseEvent OnRelease;
#pragma warning restore IDE1006

    private RenderTexture _tmpRenderTexture;

    private TextureFrame(Texture2D texture)
    {
      _texture = texture;
      width = texture.width;
      height = texture.height;
      format = texture.format;
      OnRelease = new ReleaseEvent();
      _instanceId = Guid.NewGuid();
      _InstanceTable.Add(_instanceId, this);
      _onReadBackRenderTexture = OnReadBackRenderTexture;
    }

    public TextureFrame(int width, int height, TextureFormat format) : this(new Texture2D(width, height, format, false)) { }
    public TextureFrame(int width, int height) : this(width, height, TextureFormat.RGBA32) { }

    public void Dispose()
    {
      RemoveAllReleaseListeners();
      if (_nativeTexturePtr != IntPtr.Zero)
      {
        var name = (uint)_nativeTexturePtr;
        lock (((ICollection)_NameTable).SyncRoot)
        {
          _ = _NameTable.Remove(name);
        }
      }
      _glSyncToken?.Dispose();
      _ = _InstanceTable.Remove(_instanceId);
    }

    public void CopyTexture(Texture dst) => Graphics.CopyTexture(_texture, dst);

    public void CopyTextureFrom(Texture src) => Graphics.CopyTexture(src, _texture);

    public bool ConvertTexture(Texture dst) => Graphics.ConvertTexture(_texture, dst);

    public bool ConvertTextureFrom(Texture src) => Graphics.ConvertTexture(src, _texture);

    /// <summary>
    ///   Copy texture data from <paramref name="src" />.
    ///   If <paramref name="src" /> format is different from <see cref="format" />, it converts the format.
    /// </summary>
    /// <remarks>
    ///   After calling it, pixel data can't be read from CPU safely.
    /// </remarks>
    public bool ReadTextureOnGPU(Texture src)
    {
      if (GetTextureFormat(src) != format)
      {
        return Graphics.ConvertTexture(src, _texture);
      }
      Graphics.CopyTexture(src, _texture);
      return true;
    }

    public AsyncGPUReadbackRequest ReadTextureAsync(Texture src)
    {
      if (!ReadTextureOnGPU(src))
      {
        throw new InvalidOperationException("Failed to read texture on GPU");
      }

      return AsyncGPUReadback.Request(_texture, 0, (req) =>
      {
        if (_texture == null)
        {
          return;
        }
        _texture.LoadRawTextureData(req.GetData<byte>());
        _texture.Apply();
      });
    }

    /// <remarks>
    ///   This method is not thread-safe.
    ///   Avoid calling this method again before the returned <see cref="AsyncGPUReadbackRequest.done"/> is <see langword="true"/>.
    /// </remarks>
    public AsyncGPUReadbackRequest ReadTextureAsync(Texture src, bool flipHorizontally, bool flipVertically)
    {
      var graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(format, true);
      _tmpRenderTexture = RenderTexture.GetTemporary(src.width, src.height, 32, graphicsFormat);
      var currentRenderTexture = RenderTexture.active;
      RenderTexture.active = _tmpRenderTexture;

      var scale = new Vector2(1.0f, 1.0f);
      var offset = new Vector2(0.0f, 0.0f);
      if (flipHorizontally)
      {
        scale.x = -1.0f;
        offset.x = 1.0f;
      }
      if (flipVertically)
      {
        scale.y = -1.0f;
        offset.y = 1.0f;
      }
      Graphics.Blit(src, _tmpRenderTexture, scale, offset);
      RenderTexture.active = currentRenderTexture;

      return AsyncGPUReadback.Request(_tmpRenderTexture, 0, _onReadBackRenderTexture);
    }

    private readonly Action<AsyncGPUReadbackRequest> _onReadBackRenderTexture;
    private void OnReadBackRenderTexture(AsyncGPUReadbackRequest req)
    {
      if (_texture == null)
      {
        return;
      }
      _texture.LoadRawTextureData(req.GetData<byte>());
      _texture.Apply();
      _ = RevokeNativeTexturePtr();
      RenderTexture.ReleaseTemporary(_tmpRenderTexture);
    }

    public Color GetPixel(int x, int y) => _texture.GetPixel(x, y);

    public Color32[] GetPixels32() => _texture.GetPixels32();

    public void SetPixels32(Color32[] pixels)
    {
      _texture.SetPixels32(pixels);
      _texture.Apply();

      if (!RevokeNativeTexturePtr())
      {
        // If this line was executed, there must be a bug.
        Logger.LogError("Failed to revoke the native texture.");
      }
    }

    public NativeArray<T> GetRawTextureData<T>() where T : struct => _texture.GetRawTextureData<T>();

    /// <returns>The texture's native pointer</returns>
    public IntPtr GetNativeTexturePtr()
    {
      if (_nativeTexturePtr == IntPtr.Zero)
      {
        _nativeTexturePtr = _texture.GetNativeTexturePtr();
        var name = (uint)_nativeTexturePtr;

        lock (((ICollection)_NameTable).SyncRoot)
        {
          if (!AcquireName(name, _instanceId))
          {
            throw new InvalidProgramException($"Another instance (id={_instanceId}) is using the specified name ({name}) now");
          }
          _NameTable.Add(name, _instanceId);
        }
      }
      return _nativeTexturePtr;
    }

    public uint GetTextureName() => (uint)GetNativeTexturePtr();

    public Guid GetInstanceID() => _instanceId;

    public ImageFrame BuildImageFrame() => new ImageFrame(imageFormat, _texture);

    public Image BuildCPUImage() => new Image(imageFormat, _texture);

    public GpuBuffer BuildGpuBuffer(GlContext glContext)
    {
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
      var glTextureBuffer = new GlTextureBuffer(GetTextureName(), width, height, gpuBufferformat, OnReleaseTextureFrame, glContext);
      return new GpuBuffer(glTextureBuffer);
#else
      throw new NotSupportedException("This method is only supported on Linux or Android");
#endif
    }

    public void RemoveAllReleaseListeners() => OnRelease.RemoveAllListeners();

    // TODO: stop invoking OnRelease when it's already released
    public void Release(GlSyncPoint token = null)
    {
      _glSyncToken?.Dispose();
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

    private static TextureFormat GetTextureFormat(Texture texture) => GraphicsFormatUtility.GetTextureFormat(texture.graphicsFormat);

    /// <summary>
    ///   Remove the texture name from <see cref="_NameTable" /> and empty <see cref="_nativeTexturePtr" />.
    ///   This method needs to be called when an operation is performed that may change the internal texture.
    /// </summary>
    private bool RevokeNativeTexturePtr()
    {
      if (_nativeTexturePtr == IntPtr.Zero)
      {
        return true;
      }

      var currentName = GetTextureName();
      if (!_NameTable.Remove(currentName))
      {
        return false;
      }
      _nativeTexturePtr = IntPtr.Zero;
      return true;
    }
  }
}
