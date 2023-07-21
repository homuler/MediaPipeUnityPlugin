// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

namespace Mediapipe.Tests
{
  public class ImageTest
  {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiateCpuImage()
    {
      var pixelData = BuildPixelData();

      using (var image = new Image(ImageFormat.Types.Format.Srgba, 4, 2, 16, pixelData))
      {
        Assert.AreEqual(ImageFormat.Types.Format.Srgba, image.ImageFormat());
        Assert.AreEqual(GpuBufferFormat.kBGRA32, image.Format());
        Assert.AreEqual(4, image.Width());
        Assert.AreEqual(2, image.Height());
        Assert.AreEqual(4, image.Channels());
        Assert.AreEqual(16, image.Step());
        Assert.False(image.UsesGpu());
      }
    }

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [Test, GpuOnly]
    public void Ctor_ShouldInstantiateGpuImage()
    {
      var texture = new Texture2D(4, 2, TextureFormat.RGBA32, false);
      using (var image = new Image((uint)texture.GetNativeTexturePtr(), 4, 2, GpuBufferFormat.kBGRA32, OnRelease, GetGlContext()))
      {
        Assert.AreEqual(ImageFormat.Types.Format.Srgba, image.ImageFormat());
        Assert.AreEqual(GpuBufferFormat.kBGRA32, image.Format());
        Assert.AreEqual(4, image.Width());
        Assert.AreEqual(2, image.Height());
        Assert.AreEqual(4, image.Channels());
        Assert.AreEqual(16, image.Step());
        Assert.True(image.UsesGpu());
      }
    }
#endif
    #endregion

    #region ConvertToCpu
    [Test]
    public void ConvertToCpu_ShouldReturnTrue_When_ImageIsOnCpu()
    {
      var pixelData = BuildPixelData();

      using (var image = new Image(ImageFormat.Types.Format.Srgba, 4, 2, 16, pixelData))
      {
        Assert.False(image.UsesGpu());
        Assert.True(image.ConvertToCpu());
      }
    }

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [Test, GpuOnly]
    public void ConvertToCpu_ShouldReturnTrue_When_ImageIsOnGpu()
    {
      var texture = new Texture2D(4, 2, TextureFormat.RGBA32, false);
      using (var image = new Image((uint)texture.GetNativeTexturePtr(), 4, 2, GpuBufferFormat.kBGRA32, OnRelease, GetGlContext()))
      {
        Assert.True(image.UsesGpu());
        Assert.True(image.ConvertToCpu());
      }
    }
#endif
    #endregion

    #region ConvertToGpu
    [Test, GpuOnly]
    public void ConvertToGpu_ShouldReturnTrue_If_GpuIsSuppored()
    {
      var pixelData = BuildPixelData();

      using (var image = new Image(ImageFormat.Types.Format.Srgba, 4, 2, 16, pixelData))
      {
        Assert.False(image.UsesGpu());

        RunInGlContext(() => { Assert.True(image.ConvertToGpu()); });
      }
    }

#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX || UNITY_ANDROID
    [Test, GpuOnly]
    public void ConvertToGpu_ShouldReturnTrue_When_ImageIsOnGpu()
    {
      var texture = new Texture2D(4, 2, TextureFormat.RGBA32, false);
      var name = (uint)texture.GetNativeTexturePtr();
      RunInGlContext(() =>
      {
        var glContext = GlContext.GetCurrent();
        using (var image = new Image(name, 4, 2, GpuBufferFormat.kBGRA32, OnRelease, glContext))
        {
          Assert.True(image.UsesGpu());
          Assert.True(image.ConvertToGpu());
        }
      });
    }
#endif
    #endregion

    #region PixelWriteLock
    [Test]
    public void PixelWriteLock_CanBeInitializedRepeatedly()
    {
      var pixelData = BuildPixelData();

      using (var image = new Image(ImageFormat.Types.Format.Srgba, 2, 2, 8, pixelData))
      {
        var readLock = new PixelWriteLock(image);
        readLock.Dispose();
        readLock = new PixelWriteLock(image);
        readLock.Dispose();
      }
    }

    [Test]
    public void Pixels_ShouldReturnPixelData_When_ImageIsOnCpu()
    {
      var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
      var pixelData = BuildPixelData(bytes);

      using (var image = new Image(ImageFormat.Types.Format.Srgba, 2, 2, 8, pixelData))
      {
        Assert.False(image.UsesGpu());

        using (var readLock = new PixelWriteLock(image))
        {
          var ptr = readLock.Pixels();
          var outs = new byte[16];
          Marshal.Copy(ptr, outs, 0, 16);
          Assert.AreEqual(bytes, outs);
        }
      }
    }

    [Test, GpuOnly]
    public void Pixels_ShouldReturnPixelData_When_ImageIsOnGpu()
    {
      var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
      var pixelData = BuildPixelData(bytes);

      using (var image = new Image(ImageFormat.Types.Format.Srgba, 4, 2, 16, pixelData))
      {
        Assert.False(image.UsesGpu());

        RunInGlContext(() =>
        {
          Assert.True(image.ConvertToGpu());
          Assert.True(image.UsesGpu());

          using (var readLock = new PixelWriteLock(image))
          {
            var ptr = readLock.Pixels();
            var outs = new byte[16];
            Marshal.Copy(ptr, outs, 0, 16);
            Assert.AreEqual(bytes, outs);
          }
        });
      }
    }
    #endregion

    private NativeArray<byte> BuildPixelData()
    {
      var srcBytes = new byte[] {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
        16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
      };
      return BuildPixelData(srcBytes);
    }

    private NativeArray<byte> BuildPixelData(byte[] bytes)
    {
      var pixelData = new NativeArray<byte>(bytes.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      pixelData.CopyFrom(bytes);

      return pixelData;
    }

    private GlContext GetGlContext()
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create());

        return glCalculatorHelper.GetGlContext();
      }
    }

    private static void RunInGlContext(Action action)
    {
      using (var glCalculatorHelper = new GlCalculatorHelper())
      {
        glCalculatorHelper.InitializeForTest(GpuResources.Create());
        glCalculatorHelper.RunInGlContext(() =>
        {
          action();
        });
      }
    }

    [AOT.MonoPInvokeCallback(typeof(GlTextureBuffer.DeletionCallback))]
    private static void OnRelease(uint _, IntPtr syncTokenPtr)
    {
      var _glSyncToken = syncTokenPtr == IntPtr.Zero ? null : new GlSyncPoint(syncTokenPtr);
      _glSyncToken?.Dispose();
    }
  }
}
