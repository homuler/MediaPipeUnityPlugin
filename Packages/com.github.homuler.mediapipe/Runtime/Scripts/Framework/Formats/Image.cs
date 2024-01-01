// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Mediapipe
{
  public class Image : MpResourceHandle
  {
    public Image(IntPtr imagePtr, bool isOwner = true) : base(imagePtr, isOwner) { }

    public Image(ImageFormat.Types.Format format, int width, int height, int widthStep, IntPtr pixelData, ImageFrame.Deleter deleter) : base()
    {
      UnsafeNativeMethods.mp_Image__ui_i_i_i_Pui8_PF(format, width, height, widthStep, pixelData, deleter, out var ptr).Assert();
      this.ptr = ptr;
    }

    public unsafe Image(ImageFormat.Types.Format format, int width, int height, int widthStep, NativeArray<byte> pixelData, ImageFrame.Deleter deleter)
      : this(format, width, height, widthStep, (IntPtr)NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(pixelData), deleter)
    { }

    /// <summary>
    ///   Initialize an <see cref="Image" />.
    /// </summary>
    /// <remarks>
    ///   <paramref name="pixelData" /> won't be released if the instance is disposed of.<br />
    ///   It's useful when:
    ///   <list type="bullet">
    ///     <item>
    ///       <description>You can reuse the memory allocated to <paramref name="pixelData" />.</description>
    ///     </item>
    ///     <item>
    ///       <description>You've not allocated the memory (e.g. <see cref="Texture2D.GetRawTextureData" />).</description>
    ///     </item>
    ///   </list>
    /// </remarks>
    public Image(ImageFormat.Types.Format format, int width, int height, int widthStep, NativeArray<byte> pixelData)
          : this(format, width, height, widthStep, pixelData, _VoidDeleter)
    { }

    // TODO: detect format from the texture
    public Image(ImageFormat.Types.Format format, Texture2D texture) :
        this(format, texture.width, texture.height, format.NumberOfChannels() * texture.width, texture.GetRawTextureData<byte>())
    { }

#if UNITY_EDITOR_LINUX || UNITY_STANDLONE_LINUX || UNITY_ANDROID
    public Image(uint target, uint name, int width, int height, GpuBufferFormat format, GlTextureBuffer.DeletionCallback callback, GlContext glContext) : base()
    {
      UnsafeNativeMethods.mp_Image__ui_ui_i_i_ui_PF_PSgc(target, name, width, height, format, callback, glContext.sharedPtr, out var ptr).Assert();
      this.ptr = ptr;
    }

    public Image(uint name, int width, int height, GpuBufferFormat format, GlTextureBuffer.DeletionCallback callback, GlContext glContext) :
        this(Gl.GL_TEXTURE_2D, name, width, height, format, callback, glContext)
    { }
#endif

    private static readonly ImageFrame.Deleter _VoidDeleter = ImageFrame.VoidDeleter;

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_Image__delete(ptr);
    }

    public int Width()
    {
      var ret = SafeNativeMethods.mp_Image__width(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public int Height()
    {
      var ret = SafeNativeMethods.mp_Image__height(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public int Channels()
    {
      var ret = SafeNativeMethods.mp_Image__channels(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public int Step()
    {
      var ret = SafeNativeMethods.mp_Image__step(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public bool UsesGpu()
    {
      var ret = SafeNativeMethods.mp_Image__UsesGpu(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public ImageFormat.Types.Format ImageFormat()
    {
      var ret = SafeNativeMethods.mp_Image__image_format(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public GpuBufferFormat Format()
    {
      var ret = SafeNativeMethods.mp_Image__format(mpPtr);

      GC.KeepAlive(this);
      return ret;
    }

    public bool ConvertToCpu()
    {
      UnsafeNativeMethods.mp_Image__ConvertToCpu(mpPtr, out var result).Assert();

      GC.KeepAlive(this);
      return result;
    }

    public bool ConvertToGpu()
    {
      UnsafeNativeMethods.mp_Image__ConvertToGpu(mpPtr, out var result).Assert();

      GC.KeepAlive(this);
      return result;
    }
  }

  public class PixelWriteLock : MpResourceHandle
  {
    public PixelWriteLock(Image image) : base()
    {
      UnsafeNativeMethods.mp_PixelWriteLock__RI(image.mpPtr, out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_PixelWriteLock__delete(ptr);
    }

    public IntPtr Pixels()
    {
      return SafeNativeMethods.mp_PixelWriteLock__Pixels(mpPtr);
    }
  }
}
