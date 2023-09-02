// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mediapipe
{
  public class ImageVectorPacket : Packet<List<Image>>
  {
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct ImageVector
    {
      private readonly IntPtr _data;
      private readonly int _size;

      public void Dispose()
      {
        UnsafeNativeMethods.mp_api_ImageArray__delete(_data);
      }

      public List<Image> Copy()
      {
        var images = new List<Image>(_size);

        unsafe
        {
          var imagePtr = (IntPtr*)_data;

          for (var i = 0; i < _size; i++)
          {
            var image = new Image(*imagePtr++, true);
            images.Add(image);
          }
        }

        return images;
      }
    }

    /// <summary>
    ///   Creates an empty <see cref="ImageVectorPacket" /> instance.
    /// </summary>
    public ImageVectorPacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public ImageVectorPacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public ImageVectorPacket At(Timestamp timestamp)
    {
      return At<ImageVectorPacket>(timestamp);
    }

    public override List<Image> Get()
    {
      UnsafeNativeMethods.mp_Packet__GetImageVector(mpPtr, out var imageVector).Assert();
      GC.KeepAlive(this);

      var images = imageVector.Copy();
      imageVector.Dispose();

      return images;
    }

    public override List<Image> Consume()
    {
      throw new NotSupportedException();
    }
  }
}
